using E_BangApplication.Authentication;
using E_BangDomain.Entities;
using E_BangDomain.Repository;
using E_BangDomain.RequestDtos.User;
using E_BangDomain.ResponseDtos.User;
using MyCustomMediator.Interfaces;

namespace E_BangApplication.CQRS.Command.UserHandler
{
    public class UpdateUserCommand : UpdateUserDto, IRequest<UpdateUserResponseDto>
    {
    }
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateUserResponseDto>
    {
        private readonly IUserContext _userContext;
        private readonly IUserRepository _userRepository;
        public UpdateUserCommandHandler(IUserContext userContext, IUserRepository userRepository)
        {
            _userContext = userContext;
            _userRepository = userRepository;
        }
        public async Task<UpdateUserResponseDto> Handle(UpdateUserCommand request, CancellationToken token)
        {
            UpdateUserResponseDto responseDto = new();
            CurrentUser currentUser = _userContext.GetCurrentUser();
            Users? user = await _userRepository.GetUserByAccountId(currentUser.AccountID, token);
            if (user == null)
            {
                return responseDto;
            }

            //update user properties
            user.FirstName = request.FirstName!;
            user.SecondName = request.SecondName;
            user.Surname = request.Surname!;
            user.Address.City = request.City!;
            user.Address.StreetNumber = request.StreetNumber!;
            user.Address.StreetName = request.StreetName!;
            user.Address.Country = request.Country!;
            user.Address.PostalCode = request.PostalCode!;

            //update last update time
            user.Address.LastUpdateTime = DateTime.Now;
            user.LastUpdateTime = DateTime.Now;

            //update user in repository
            bool isUpdated = await _userRepository.UpdateUserAsync(user, token);
            responseDto.IsSuccess = isUpdated;
            return responseDto;
        }
    }
}
