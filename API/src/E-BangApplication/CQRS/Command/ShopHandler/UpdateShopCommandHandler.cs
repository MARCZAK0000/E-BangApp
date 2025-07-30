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
        private readonly ILogger<UpdateShopCommandHandler> _logger;

        public UpdateShopCommandHandler(IShopRepository shopRepository, 
            IActionRepository actionRepository, 
            IUserContext userContext, 
            ILogger<UpdateShopCommandHandler> logger)
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
                _logger.LogError("{Handler} - {date}: User has no permission to {actionName}",
                    nameof(UpdateShopCommandHandler),
                    DateTime.Now,
                    Enum.GetName(typeof(EAction), EAction.Update));
                return response;
            }

            Maybe<Shop> shop = await _shopRepository.GetShopByIDAsync(request.ShopId, token);
            if(!shop.HasValue || shop.Value is null)
            {
                _logger.LogError("{Handler} - {Date}: Shop with ID {ShopId} not found",
                    nameof(UpdateShopCommandHandler),
                    DateTime.Now,
                    request.ShopId);
                return response;
            }
            shop.Value.ShopName = request.ShopName;
            shop.Value.ShopDescription = request.ShopDescription;
            shop.Value.ShopTypeId = request.ShopTypeID;

            bool hasUpdate = await _shopRepository.UpdateShopAsync(shop.Value, token);
            if (hasUpdate)
                _logger.LogInformation("{Handler} - {date}: Shop has been update - Id: {shopId}"
                    , nameof(UpdateShopCommandHandler), DateTime.Now, request.ShopId);
            else
                _logger.LogInformation("{Handler} - {date}: Shop has not been update - Id: {shopId}"
                   , nameof(UpdateShopCommandHandler), DateTime.Now, request.ShopId);

            response.IsSuccess = hasUpdate;
            return response;
        }
    }

    public class UpdateShopCommand :UpdateShopDto, IRequest<UpdateShopResponseDto>
    {

    }
}
