namespace Cower.Domain.Models;

public sealed record YoomoneyNotification(
    string NotificationType,
    string OperationId,
    decimal Amount,
    decimal WithdrawAmount,
    string Currency,
    string Datetime,
    string Sender,
    bool Codepro,
    string Label,
    string Sha1Hash,
    bool Unaccepted);
