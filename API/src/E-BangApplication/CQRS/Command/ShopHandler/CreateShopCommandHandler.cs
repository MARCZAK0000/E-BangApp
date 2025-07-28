using E_BangApplication.Authentication;
using E_BangDomain.Entities;
using E_BangDomain.Repository;
using E_BangDomain.RequestDtos.Shop;
using E_BangDomain.ResponseDtos.Shop;
using Microsoft.Extensions.Logging;
using MyCustomMediator.Interfaces;

namespace E_BangApplication.CQRS.Command.ShopHandler
{
    public class CreateShopCommand : CreateShopDto, IRequest<CreateShopResponseDto>
    {
    }
    public class CreateShopCommandHandler : IRequestHandler<CreateShopCommand, CreateShopResponseDto>
    {
        private readonly IShopRepository _shopRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ILogger<CreateShopCommand> _logger;
        private readonly IUserContext _userContext;
        public CreateShopCommandHandler(IShopRepository shopRepository, IRoleRepository roleRepository,
            ILogger<CreateShopCommand> logger, IUserContext userContext)
        {
            _shopRepository = shopRepository;
            _roleRepository = roleRepository;
            _logger = logger;
            _userContext = userContext;
        }

        public async Task<CreateShopResponseDto> Handle(CreateShopCommand request, CancellationToken token)
        {
            CreateShopResponseDto response = new();
            CurrentUser user = _userContext.GetCurrentUser();

            //Generate shop Entity
            Shop shop = new()
            {
                ShopName = request.ShopName,
                ShopDescription = request.ShopDescription,
                ShopTypeId = request.ShopTypeID,
                ShopStaff = [],
                ShopAddressInfromations = [],
            };

            //Create owner
            ShopStaff owner = new()
            {
                ShopStaffId = user.AccountID,
                ShopId = shop.ShopId,
                ActionLevel = 7
            };
            shop.ShopStaff.Add(owner);

            bool isAdded = await _shopRepository.CreateShopAsync(shop, token);
            if(!isAdded)
                return response;

            bool isUpdatedRole = await _roleRepository.AddToRoleAsync(user.AccountID, "ShopOwner", token);
            ///SEND EMIAL LOGIC!

            response.IsSuccess = isUpdatedRole;
            if(response.IsSuccess)
            {
                _logger.LogInformation("{Instance}: {Date} - Shop has been created",
                    nameof(CreateShopCommandHandler), DateTime.Now);
            }
            return response;
        }
    }


}
