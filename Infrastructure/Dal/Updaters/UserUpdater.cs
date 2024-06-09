using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Dal.Updaters;

[Mapper]
public static partial class UserUpdater
{
    public static partial void UpdateUser(User newUser, User oldUser);
}