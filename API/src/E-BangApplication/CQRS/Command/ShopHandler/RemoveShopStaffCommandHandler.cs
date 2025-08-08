using E_BangDomain.ResponseDtos.Shop;
using MyCustomMediator.Interfaces;

namespace E_BangApplication.CQRS.Command.ShopHandler
{
    public class RemoveShopStaffCommandHandler : IRequestHandler<RemoveShopStaffCommand, RemoveShopStaffResponseDto>
    {
        public Task<RemoveShopStaffResponseDto> Handle(RemoveShopStaffCommand request, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }

    public class RemoveShopStaffCommand : IRequest<RemoveShopStaffResponseDto>
    {
        public string ShopId { get; set; }

        public string StaffId { get; set; }
    }
}
