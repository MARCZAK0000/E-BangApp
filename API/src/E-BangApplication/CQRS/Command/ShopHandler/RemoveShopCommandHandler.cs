using CustomLogger.Abstraction;
using E_BangApplication.Authentication;
using E_BangDomain.Entities;
using E_BangDomain.Enums;
using E_BangDomain.MaybePattern;
using E_BangDomain.Repository;
using E_BangDomain.ResponseDtos.Shop;
using Microsoft.Extensions.Logging;
using MyCustomMediator.Interfaces;

namespace E_BangApplication.CQRS.Command.ShopHandler
{
    public class RemoveShopCommandHandler : IRequestHandler<RemoveShopCommand, RemoveShopResponseDto>
    {
        private readonly ICustomLogger<UpdateBranchCommandHandler> _logger;
        private readonly IShopRepository _shopRepository;
        private readonly IUserContext _userContext;
        private readonly IActionRepository _actionRepository;

        public RemoveShopCommandHandler(ICustomLogger<UpdateBranchCommandHandler> logger, 
            IShopRepository shopRepository, IUserContext userContext, 
            IActionRepository actionRepository)
        {
            _logger = logger;
            _shopRepository = shopRepository;
            _userContext = userContext;
            _actionRepository = actionRepository;
        }

        public async Task<RemoveShopResponseDto> Handle(RemoveShopCommand request, CancellationToken token)
        {
            CurrentUser currentUser = _userContext.GetCurrentUser();
            RemoveShopResponseDto response = new();

            int permissionLevel = await _actionRepository.GetUserShopActionLevelAsync(currentUser.AccountID, request.ShopID, token);
            Dictionary<Actions, bool> keyValuePairs = _actionRepository.GetUserActions(permissionLevel);
            bool hasPermission = _actionRepository.HasPermission(keyValuePairs, EAction.Delete);
            if (!hasPermission)
            {
                string actionName = Enum.GetName(EAction.Delete) ?? "Delete";
                _logger.LogError("User has no permission to {ActionName}", actionName);
                return response;
            }

            Maybe<Shop> maybeShop = await _shopRepository.GetShopByIDAsync(request.ShopID, token);
            if (!maybeShop.HasValue)
            {
                _logger.LogWarning("Shop with ID {ShopID} not found", request.ShopID);
                return response;
            }

            // Check if the shop has branches and then remove them
            List<ShopBranchesInformations> branches = 
                await _shopRepository.GetShopBranchesByShopIdAsync(request.ShopID, token);

            if (branches.Count > 0)
            {
                bool removedBranches = await _shopRepository.RemoveAllShopBranchesAsync(request.ShopID, token);
                if (!removedBranches)
                {
                    _logger.LogError("Failed to remove branches for shop ID {ShopID}", request.ShopID);
                    return response;
                }
            }
            // Remove the shop
            bool isRemoved = await _shopRepository.RemoveShopAsync(request.ShopID, token);
            if (isRemoved)
            {
                _logger.LogTrace("Shop has been removed with ID {ShopID}", request.ShopID);
            }
            else
            {
                _logger.LogError("Failed to remove shop with ID {ShopID}", request.ShopID);
            }
            response.IsSuccess = isRemoved;
            return response; 
        }
    }

    public class RemoveShopCommand : IRequest<RemoveShopResponseDto>
    {
        public string ShopID { get; set; }
    }

}

