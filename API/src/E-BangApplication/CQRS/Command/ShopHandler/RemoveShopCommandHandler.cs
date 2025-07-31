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
        private readonly ILogger<UpdateBranchCommandHandler> _logger;
        private readonly IShopRepository _shopRepository;
        private readonly IUserContext _userContext;
        private readonly IActionRepository _actionRepository;

        public RemoveShopCommandHandler(ILogger<UpdateBranchCommandHandler> logger, 
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
                _logger.LogError("{Handler} - {Date}: User has no permission to {ActionName}",
                    nameof(RemoveShopResponseDto),
                    DateTime.Now,
                    Enum.GetName(EAction.Delete));
                return response;
            }

            Maybe<Shop> maybeShop = await _shopRepository.GetShopByIDAsync(request.ShopID, token);
            if (!maybeShop.HasValue)
            {
                _logger.LogInformation("{Handler} - {Date}: Shop with ID {ShopID} not found",
                    nameof(RemoveShopCommandHandler),
                    DateTime.Now,
                    request.ShopID);
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
                    _logger.LogError("{Handler} - {Date}: Failed to remove branches for shop ID {ShopID}",
                        nameof(RemoveShopCommandHandler),
                        DateTime.Now,
                        request.ShopID);
                    return response;
                }
            }
            // Remove the shop
            bool isRemoved = await _shopRepository.RemoveShopAsync(request.ShopID, token);
            if (isRemoved)
            {
                _logger.LogError("{Handler} - {Date}: Shop has been removed with ID {ShopID}",
                   nameof(RemoveShopCommandHandler),
                   DateTime.Now,
                   request.ShopID);
            }
            else
            {
                _logger.LogError("{Handler} - {Date}: Failed to remove shop with ID {ShopID}",
                    nameof(RemoveShopCommandHandler),
                    DateTime.Now,
                    request.ShopID);

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

