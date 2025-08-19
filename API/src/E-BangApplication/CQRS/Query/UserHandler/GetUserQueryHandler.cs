using AutoMapper;
using E_BangApplication.Authentication;
using E_BangApplication.Cache;
using E_BangApplication.Cache.Base;
using E_BangDomain.Entities;
using E_BangDomain.EntitiesDto.Role;
using E_BangDomain.EntitiesDto.User;
using E_BangDomain.Repository;
using E_BangDomain.ResponseDtos.User;
using MyCustomMediator.Interfaces;

namespace E_BangApplication.CQRS.Query.UserHandler
{
    public class GetUserQuery : IRequest<GetUserResponseDto<UserInfoDto>>, ICacheable
    {
        public string CacheKey { get; set; } = CacheConstant.UserInfoCacheKey;
        public TimeSpan CacheDuration { get; set; } = TimeSpan.FromMinutes(5);
    }
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserResponseDto<UserInfoDto>>
    {
        private readonly IUserContext _userContext;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        public GetUserQueryHandler(IUserContext userContext,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IMapper mapper)
        {
            _userContext = userContext;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<GetUserResponseDto<UserInfoDto>> Handle(GetUserQuery request, CancellationToken token)
        {
            GetUserResponseDto<UserInfoDto> responseDto = new();
            CurrentUser currentUser = _userContext.GetCurrentUser();
            Users? user = await _userRepository.GetUserByAccountId(currentUser.AccountID, token);
            if (user == null)
            {
                return responseDto;
            }
            responseDto.Data = [_mapper.Map<UserInfoDto>(user)];
            List<Roles> roles = await _roleRepository.GetRolesByAccountIdAsync(currentUser.AccountID, token);
            if (roles.Count == 0)
            {
                return responseDto;
            }
            responseDto.Data[0].Roles = _mapper.Map<List<RolesDto>>(roles);
            if (responseDto.Data[0].Roles.Count == 0 || responseDto.Data is null)
            {
                return responseDto;
            }
            responseDto.IsSuccess = true;
            return responseDto;
        }
    }
}
