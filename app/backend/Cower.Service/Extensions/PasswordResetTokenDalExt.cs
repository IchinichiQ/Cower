using Cower.Data.Models;
using Cower.Domain.Models;

namespace Cower.Service.Extensions;

internal static class PasswordResetTokenDalExt
{
    public static PasswordResetToken ToPasswordResetToken(this PasswordResetTokenDal dal)
    {
        return new PasswordResetToken(
            dal.Token,
            dal.User,
            dal.ExpireAt);
    }
}