using Cower.Domain.Models;
using Cower.Web.Models;

namespace Cower.Web.Extensions;

public static class YoomoneyNotificationRequestDtoExt
{
    public static YoomoneyNotification ToYoomoneyNotification(this YoomoneyNotificationRequestDto dto)
    {
        return new YoomoneyNotification(
            dto.notification_type,
            dto.operation_id,
            dto.amount,
            dto.withdraw_amount,
            dto.currency,
            dto.datetime,
            dto.sender,
            dto.codepro,
            dto.label,
            dto.sha1_hash,
            dto.unaccepted
        );
    }
}