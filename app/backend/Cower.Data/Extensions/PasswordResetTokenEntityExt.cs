using Cower.Data.Models;
using Cower.Data.Models.Entities;

namespace Cower.Data.Extensions;

internal static class PasswordResetTokenEntityExt
{
    public static PasswordResetTokenDal ToPasswordResetTokenDal(this PasswordResetTokenEntity entity)
    {
        return new PasswordResetTokenDal(
            entity.Token,
            entity.User!.ToUser(),
            entity.ExpireAt);
    }
}