using E_BangApplication.Authentication;
using E_BangDomain.Entities;
using E_BangDomain.Enums;
using E_BangDomain.Repository;
using E_BangDomain.RequestDtos.Shared;
using E_BangDomain.RequestDtos.Shop;
using E_BangDomain.ResponseDtos.Shop;
using Microsoft.Extensions.Logging;
using MyCustomMediator.Interfaces;

namespace E_BangApplication.CQRS.Command.ShopHandler
{
    public class CreateShopBranchesCommandHandler : IRequestHandler<CreateShopBranchesCommand, CreateBranchesResponseDto>
    {
        private readonly ILogger<CreateShopBranchesCommandHandler> _logger;
        private readonly IShopRepository _shopRepository;
        private readonly IUserContext _userContext;
        private readonly IActionRepository _actionRepository;

        public CreateShopBranchesCommandHandler(ILogger<CreateShopBranchesCommandHandler> logger,
            IShopRepository shopRepository, IUserContext userContext,
            IActionRepository actionRepository)
        {
            _logger = logger;
            _shopRepository = shopRepository;
            _userContext = userContext;
            _actionRepository = actionRepository;
        }

        public async Task<CreateBranchesResponseDto> Handle(CreateShopBranchesCommand request, CancellationToken token)
        {
            CurrentUser currentUser = _userContext.GetCurrentUser();
            CreateBranchesResponseDto response = new();

            // Check if the user has permission to create shop branches
            int permissionLevel = await _actionRepository.GetUserShopActionLevelAsync(currentUser.AccountID, request.Id, token);
            Dictionary<Actions, bool> keyValuePairs = _actionRepository.GetUserActionsAsync(permissionLevel);
            bool hasPermission = _actionRepository.HasPermission(keyValuePairs, EAction.Create);
            if (!hasPermission)
            {
                _logger.LogError("{nameof} - {date}: User has no permission to {actionName}}", 
                    nameof(CreateShopBranchesCommandHandler), 
                    DateTime.Now, 
                    Enum.GetName(EAction.Create));
                return response;
            }

            List<ShopBranchesInformations> branches = [];
            request.List.ForEach((act) =>
            {
                branches.Add(new()
                {
                    ShopID = act.ShopID,
                    ShopCity = act.ShopCity,
                    ShopCountry = act.ShopCountry,
                    ShopPostalCode = act.ShopPostalCode,
                    ShopStreetName = act.ShopStreetName,
                    IsMainShop = act.IsMainShop,
                });
            });
            bool HasAdded = await _shopRepository.CreateShopBranchAsync(branches, token);
            if (HasAdded)
                _logger.LogInformation("{nameof} - {date}: User has added {count} branches to ShopID: {shopID}",
                    nameof(CreateShopBranchesCommandHandler), DateTime.Now, branches.Count, request.Id);
            else
                _logger.LogInformation("{nameof} - {date}: User has not added branches to ShopID: {shopID}",
                   nameof(CreateShopBranchesCommandHandler), DateTime.Now, request.Id);

            //Consider check if branches already exist
            response.IsSuccess = HasAdded;
            return response;
        }
    }

    public class CreateShopBranchesCommand : ListDto<CreateShopBranchDto>, IRequest<CreateBranchesResponseDto>
    {
    }
}
