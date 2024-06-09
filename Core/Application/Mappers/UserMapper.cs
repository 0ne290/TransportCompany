using Application.Dtos;
using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Application.Mappers;

[Mapper]
public partial class UserMapper
{
    public partial UserResponseDto UserToUserResponseDto(User user);
    
    public partial User UserRequestDtoToUser(UserRequestDto user);
}