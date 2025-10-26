using CustomLogger.Abstraction;
using E_BangApplication.Authentication;
using E_BangDomain.Comparer;
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
        private readonly ICustomLogger<CreateShopBranchesCommandHandler> _logger;
        private readonly IShopRepository _shopRepository;
        private readonly IUserContext _userContext;
        private readonly IActionRepository _actionRepository;

        public CreateShopBranchesCommandHandler(ICustomLogger<CreateShopBranchesCommandHandler> logger,
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
            int permissionLevel = await _actionRepository.GetUserShopActionLevelAsync(currentUser.AccountID, request.ShopId, token);
            Dictionary<Actions, bool> keyValuePairs = _actionRepository.GetUserActions(permissionLevel);
            bool hasPermission = _actionRepository.HasPermission(keyValuePairs, EAction.Create);
            if (!hasPermission)
            {
                string actionName = Enum.GetName(EAction.Create) ?? "Create";
                _logger.LogError("User has no permission to {actionName}", 
                   actionName);
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

            List<ShopBranchesInformations> currentShopBranches =
                await _shopRepository.GetShopBranchesByShopIdAsync(request.ShopId, token);

            List<ShopBranchesInformations> uniqueBranches = branches
                .Except(currentShopBranches, new ShopBranchesComparer())
                .ToList();

            if(uniqueBranches.Count <= 0)
            {
                response.Message = "Branches Already Exists";
                _logger.LogInformation("User has not added branches to ShopID: {shopID} - branches already exists"
                    , request.ShopId);
                return response;
            }

            bool HasAdded = await _shopRepository.CreateShopBranchAsync(uniqueBranches, token);
            if (HasAdded)
                _logger.LogInformation("User has added {count} branches to ShopID: {shopID}",
                    branches.Count, request.ShopId);
            else
                _logger.LogInformation("User has not added branches to ShopID: {shopID}",
                   request.ShopId);

            response.IsSuccess = HasAdded;
            return response;
        }
    }

    public class CreateShopBranchesCommand : ListDto<CreateShopBranchDto>, IRequest<CreateBranchesResponseDto>
    {
        public string ShopId { get; set; }
    }
}
