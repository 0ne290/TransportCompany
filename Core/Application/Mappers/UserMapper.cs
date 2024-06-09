using Application.Dtos;
using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Application.Mappers;

[Mapper]
public static partial class UserMapper
{
    public static partial UserResponseDto UserToUserResponseDto(User user);
    
    public static partial User UserRequestDtoToUser(UserRequestDto user);
}