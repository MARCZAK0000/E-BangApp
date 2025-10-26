using CustomLogger.Abstraction;
using E_BangApplication.Authentication;
using E_BangDomain.Comparer;
using E_BangDomain.Entities;
using E_BangDomain.Enums;
using E_BangDomain.MaybePattern;
using E_BangDomain.Repository;
using E_BangDomain.RequestDtos.Shop;
using E_BangDomain.ResponseDtos.Shop;
using Microsoft.Extensions.Logging;
using MyCustomMediator.Interfaces;

namespace E_BangApplication.CQRS.Command.ShopHandler
{
    public class UpdateBranchCommandHandler : IRequestHandler<UpdateBranchCommand, UpdateBranchesResponseDto>
    {
        private readonly ICustomLogger<UpdateBranchCommandHandler> _logger;
        private readonly IShopRepository _shopRepository;
        private readonly IUserContext _userContext;
        private readonly IActionRepository _actionRepository;

        public UpdateBranchCommandHandler(ICustomLogger<UpdateBranchCommandHandler> logger, 
            IShopRepository shopRepository, IUserContext userContext, 
            IActionRepository actionRepository)
        {
            _logger = logger;
            _shopRepository = shopRepository;
            _userContext = userContext;
            _actionRepository = actionRepository;
        }

        public async Task<UpdateBranchesResponseDto> Handle(UpdateBranchCommand request, CancellationToken token)
        {

            CurrentUser currentUser = _userContext.GetCurrentUser();
            UpdateBranchesResponseDto response = new();

            int permissionLevel = await _actionRepository.GetUserShopActionLevelAsync(currentUser.AccountID, request.ShopID, token);
            Dictionary<Actions, bool> keyValuePairs = _actionRepository.GetUserActions(permissionLevel);
            bool hasPermission = _actionRepository.HasPermission(keyValuePairs, EAction.Update);
            if (!hasPermission)
            {
                string actionName = EAction.Update.ToString();
                _logger.LogError("User has no permission to {ActionName}", actionName);
                return response;
            }

            Maybe<ShopBranchesInformations> maybeShopBranch = await _shopRepository.GetShopBranchByIdAsync(request.BranchId, token);
            if(!maybeShopBranch.HasValue || maybeShopBranch.Value is null)
            {
                _logger.LogWarning("Shop branch with ID {BranchId} not found", request.BranchId);
                response.IsSuccess = false;
                return response;
            }

            maybeShopBranch.Value.ShopCity = request.ShopCity;
            maybeShopBranch.Value.ShopCountry = request.ShopCountry;
            maybeShopBranch.Value.ShopPostalCode = request.ShopPostalCode;
            maybeShopBranch.Value.ShopStreetName = request.ShopStreetName;  
            maybeShopBranch.Value.IsMainShop = request.IsMainShop;
            maybeShopBranch.Value.LastModifiedTime = DateTime.Now;

            // Validate if the main shop is already set
            List<ShopBranchesInformations> shopBranches = await _shopRepository.GetShopBranchesByShopIdAsync(request.ShopID, token);

            bool hasDuplicate = shopBranches.Except(new[] { maybeShopBranch.Value }, new ShopBranchesComparer()).Count() != shopBranches.Count;
            if(hasDuplicate)
            {
                _logger.LogError("Duplicate branch information found for shop ID {ShopId}", request.ShopID);
                response.IsSuccess = false;
                return response;
            }
            // Check if the branch is already the main shop
            if (request.IsMainShop)
            {
                var getCurrentMainShop = shopBranches.FirstOrDefault(branch => branch.IsMainShop && branch.ShopBranchId != request.BranchId);
                if (getCurrentMainShop is not null)
                {
                   await _shopRepository.RemoveMainShopAsync(request.ShopID, getCurrentMainShop.ShopBranchId, token);
                }
            }

            bool hasUpdate = await _shopRepository.UpdateShopBranchAsync(maybeShopBranch.Value, token);
            if (hasUpdate)
                _logger.LogTrace("Shop branch with ID {BranchId} updated successfully", request.BranchId);
            else
                _logger.LogError("Failed to update shop branch with ID {BranchId}", request.BranchId);

            response.IsSuccess = hasUpdate;
            return response;
        }
    }

    public class UpdateBranchCommand : UpdateShopBranchDto, IRequest<UpdateBranchesResponseDto>
    {
    }
}
