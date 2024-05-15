namespace Cower.Web.Models;

public class YoomoneyNotificationRequestDto
{
    public string notification_type { get; set; }
    public string operation_id { get; set; }
    public decimal amount { get; set; }
    public decimal withdraw_amount { get; set; }
    public string currency { get; set; }
    public string datetime { get; set; }
    public string sender { get; set; }
    public bool codepro { get; set; }
    public string label { get; set; }
    public string sha1_hash { get; set; }
    public bool unaccepted { get; set; }
}