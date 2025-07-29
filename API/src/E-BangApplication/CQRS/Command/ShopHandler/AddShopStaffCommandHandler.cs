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

            List<ShopStaff> staffList = [];

            foreach (var staff in request.List)
            {
                staffList.Add(new()
                {
                    ShopId = request.Id,
                    AccountId = await _accountRepository.GetAccountIdFromEmail(staff.EmailAddress, token),
                    ActionLevel = staff.ActionLevel,
                });
            }
            
            bool hasAdded = await _shopRepository.AddStaffToShop(staffList, request.Id, token);
            if (!hasAdded)
            {
                response.Message = "Staff already exists in Db";
            }
            response.IsSuccess = hasAdded;
            return response;
        }

    }
    public class AddShopStaffCommand : ListDto<AddShopStaffDto>,IRequest<AddShopStaffResponseDto>
    {
    }
}