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
    public class AddShopStaffCommandHandler : IRequestHandler<AddShopStaffCommand, AddShopStaffResponseDto>
    {
        private readonly IShopRepository _shopRepository;
        private readonly IActionRepository _actionRepository;
        private readonly IUserContext _userContext;
        private readonly ILogger<AddShopStaffCommandHandler> _logger;
        private readonly IAccountRepository _accountRepository;
        public AddShopStaffCommandHandler(IShopRepository shopRepository,
            IActionRepository actionRepository, IUserContext userContext,
            ILogger<AddShopStaffCommandHandler> logger, IAccountRepository accountRepository)
        {
            _shopRepository = shopRepository;
            _actionRepository = actionRepository;
            _userContext = userContext;
            _logger = logger;
            _accountRepository = accountRepository;
        }

        public async Task<AddShopStaffResponseDto> Handle(AddShopStaffCommand request, CancellationToken token)
        {
            CurrentUser currentUser = _userContext.GetCurrentUser();
            AddShopStaffResponseDto response = new();

            int permissionLevel = await _actionRepository.GetUserShopActionLevelAsync(currentUser.AccountID, request.ShopId, token);
            Dictionary<Actions, bool> keyValuePairs = _actionRepository.GetUserActions(permissionLevel);
            bool hasPermission = _actionRepository.HasPermission(keyValuePairs, EAction.Create);
            if (!hasPermission)
            {
                _logger.LogError("{Handler} - {date}: User has no permission to {actionName}",
                    nameof(AddShopStaffCommandHandler),
                    DateTime.Now,
                    Enum.GetName(typeof(EAction), EAction.Create));
                return response;
            }

            List<ShopStaff> staffList = [];
            foreach (var staff in request.List)
            {
                
                Dictionary<Actions, bool> staffActions = _actionRepository.CreateUserActions(staff.CanCreate, staff.CanEdit, staff.CanDelete);
                int actionLevel = _actionRepository.SetUserShopActionLevel(staffActions);
                string accountId = await _accountRepository.GetAccountIdFromEmail(staff.EmailAddress, token);

                staffList.Add(new()
                {
                    ShopId = request.ShopId,
                    AccountId = accountId,
                    ActionLevel = actionLevel,
                });
            }

            HashSet<string> existingStaffIds = [.. await _shopRepository.ListOfStaffIdInShop(request.ShopId, token)];
            
            List<ShopStaff> uniqueStaff = staffList
                .Where(pr=>!existingStaffIds.Contains(pr.AccountId))
                .ToList();

            if (uniqueStaff.Count == 0)
            {
                response.Message = "Staff already exists in Db";
            }

            bool hasAdded = await _shopRepository.AddStaffToShop(uniqueStaff, token);
            response.IsSuccess = hasAdded;
            if (hasAdded)
            {
                _logger.LogInformation("Added {Count} new staff members to shop {ShopId}.", uniqueStaff.Count, request.ShopId);
            }
            return response;
        }

    }
    public class AddShopStaffCommand : ListDto<AddShopStaffDto>,IRequest<AddShopStaffResponseDto>
    {
        public string ShopId { get; set; }
    }
}