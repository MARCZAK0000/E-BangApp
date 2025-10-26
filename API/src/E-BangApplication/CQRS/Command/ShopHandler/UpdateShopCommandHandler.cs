using CustomLogger.Abstraction;
using E_BangApplication.Authentication;
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
    public class UpdateShopCommandHandler : IRequestHandler<UpdateShopCommand, UpdateShopResponseDto>
    {
        private readonly IShopRepository _shopRepository;
        private readonly IActionRepository _actionRepository;
        private readonly IUserContext _userContext;
        private readonly ICustomLogger<UpdateShopCommandHandler> _logger;

        public UpdateShopCommandHandler(IShopRepository shopRepository, 
            IActionRepository actionRepository, 
            IUserContext userContext, 
            ICustomLogger<UpdateShopCommandHandler> logger)
        {
            _shopRepository = shopRepository;
            _actionRepository = actionRepository;
            _userContext = userContext;
            _logger = logger;
        }

        public async Task<UpdateShopResponseDto> Handle(UpdateShopCommand request, CancellationToken token)
        {
            UpdateShopResponseDto response = new();
            CurrentUser currentUser = _userContext.GetCurrentUser();
            int permissionLevel = await _actionRepository.GetUserShopActionLevelAsync(currentUser.AccountID, request.ShopId, token);
            Dictionary<Actions, bool> keyValuePairs = _actionRepository.GetUserActions(permissionLevel);
            bool hasPermission = _actionRepository.HasPermission(keyValuePairs, EAction.Update);
            if (!hasPermission)
            {
                string actionName = EAction.Update.ToString();
                _logger.LogError("User has no permission to {ActionName}", actionName);
                return response;
            }

            Maybe<Shop> shop = await _shopRepository.GetShopByIDAsync(request.ShopId, token);
            if(!shop.HasValue || shop.Value is null)
            {
                _logger.LogWarning("Shop with ID {ShopId} not found", request.ShopId);
                return response;
            }
            shop.Value.ShopName = request.ShopName;
            shop.Value.ShopDescription = request.ShopDescription;
            shop.Value.ShopTypeId = request.ShopTypeID;

            bool hasUpdate = await _shopRepository.UpdateShopAsync(shop.Value, token);
            if (hasUpdate)
                _logger.LogTrace("Shop has been updated - Id: {shopId}", request.ShopId);
            else
                _logger.LogWarning("Shop has not been updated - Id: {shopId}", request.ShopId);

            response.IsSuccess = hasUpdate;
            return response;
        }
    }

    public class UpdateShopCommand :UpdateShopDto, IRequest<UpdateShopResponseDto>
    {

    }
}
