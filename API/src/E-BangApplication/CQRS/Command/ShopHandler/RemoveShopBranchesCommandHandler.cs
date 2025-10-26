using CustomLogger.Abstraction;
using E_BangApplication.Authentication;
using E_BangApplication.Exceptions;
using E_BangDomain.Entities;
using E_BangDomain.Enums;
using E_BangDomain.MaybePattern;
using E_BangDomain.Repository;
using E_BangDomain.ResponseDtos.Shop;
using Microsoft.Extensions.Logging;
using MyCustomMediator.Interfaces;

namespace E_BangApplication.CQRS.Command.ShopHandler
{
    public class RemoveShopBranchesCommandHandler : IRequestHandler<RemoveShopBranchesCommand, RemoveBranchesResponseDto>
    {
        private readonly ICustomLogger<UpdateBranchCommandHandler> _logger;
        private readonly IShopRepository _shopRepository;
        private readonly IUserContext _userContext;
        private readonly IActionRepository _actionRepository;

        public RemoveShopBranchesCommandHandler(ICustomLogger<UpdateBranchCommandHandler> logger, 
            IShopRepository shopRepository, IUserContext userContext, 
            IActionRepository actionRepository)
        {
            _logger = logger;
            _shopRepository = shopRepository;
            _userContext = userContext;
            _actionRepository = actionRepository;
        }

        public async Task<RemoveBranchesResponseDto> Handle(RemoveShopBranchesCommand request, CancellationToken token)
        {
            CurrentUser currentUser = _userContext.GetCurrentUser();
            RemoveBranchesResponseDto response = new();

            int permissionLevel = await _actionRepository.GetUserShopActionLevelAsync(currentUser.AccountID, request.ShopID, token);
            Dictionary<Actions, bool> keyValuePairs = _actionRepository.GetUserActions(permissionLevel);
            bool hasPermission = _actionRepository.HasPermission(keyValuePairs, EAction.Delete);
            if (!hasPermission)
            {
                string actionName = Enum.GetName(EAction.Delete) ?? "Delete";
                _logger.LogError("User has no permission to {ActionName}",
                    actionName);
                return response;
            }
            Maybe<ShopBranchesInformations> maybeShopBranch = await _shopRepository.GetShopBranchByIdAsync(request.BranchId, token);
            if(!maybeShopBranch.HasValue || maybeShopBranch.Value is null)
            {
                _logger.LogError("Shop branch with ID {BranchId} not found",
                    request.BranchId);
                return response;
            }

            if(maybeShopBranch.Value.IsMainShop)
            {
                List<ShopBranchesInformations> branches = await 
                    _shopRepository
                    .GetShopBranchesByShopIdAsync(maybeShopBranch.Value.ShopID, token);
                if(branches.Count <= 1) { 
                    _logger.LogError("Cannot remove the last main shop branch for shop ID {ShopId}",
                        maybeShopBranch.Value.ShopID);
                    return response;
                }

                List<ShopBranchesInformations> newMainShopBranch = branches
                    .Where(pr => pr.ShopBranchId != maybeShopBranch.Value.ShopBranchId && pr.IsMainShop)
                    .ToList();

                if(newMainShopBranch.Count > 1)
                {
                    throw new InternalServerErrorException($"There is more than one MainShop for {request.ShopID}");
                }
                ShopBranchesInformations candidate = newMainShopBranch.First();
                bool hasUpdated = await _shopRepository.UpdateMainShopAsync(candidate.ShopID, candidate.ShopBranchId, token);
                if (!hasUpdated)
                {
                    _logger.LogError("Failed to update main shop for shop ID {ShopId}: Cannot change MainShop",
                        maybeShopBranch.Value.ShopID);
                    return response;
                }

            }
            bool hasDeleted = await _shopRepository.RemoveShopBranchAsync(maybeShopBranch.Value, token);
            if (hasDeleted)
            {
                _logger.LogInformation("Shop branch with ID {BranchId} has been removed",
                    request.BranchId);
                response.IsSuccess = true;
            }
            else
            {
                _logger.LogError("Failed to remove shop branch with ID {BranchId}",
                    request.BranchId);
            }
            return response;
        }
    }

    public class RemoveShopBranchesCommand : IRequest<RemoveBranchesResponseDto>
    {
        public string ShopID { get; set; }
        public string BranchId { get; set; }
    }
}
