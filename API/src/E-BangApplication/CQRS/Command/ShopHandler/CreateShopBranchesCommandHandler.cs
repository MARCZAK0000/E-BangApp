using E_BangDomain.Repository;
using E_BangDomain.RequestDtos.Shared;
using E_BangDomain.RequestDtos.Shop;
using E_BangDomain.ResponseDtos.Shop;
using Microsoft.Extensions.Logging;
using MyCustomMediator.Interfaces;
using System.Numerics;

namespace E_BangApplication.CQRS.Command.ShopHandler
{
    public class CreateShopBranchesCommandHandler : IRequestHandler<CreateShopBranchesCommand, CreateBranchesResponseDto>
    {
        private readonly ILogger<CreateShopBranchesCommandHandler> _logger; 
        private readonly IShopRepository _shopRepository;
        private readonly IRoleRepository _roleRepository;
        public Task<CreateBranchesResponseDto> Handle(CreateShopBranchesCommand request, CancellationToken token)
        {

            throw new NotImplementedException();
        }
    }

    public class CreateShopBranchesCommand : ListDto<CreateShopBranchDto>, IRequest<CreateBranchesResponseDto>
    {
    }
}
