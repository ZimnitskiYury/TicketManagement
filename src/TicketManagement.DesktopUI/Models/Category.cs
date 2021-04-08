using System.ComponentModel;

namespace TicketManagement.DesktopUI.Models
{
    public enum Category
    {
        [Description("Concert")]
        Concert,

        [Description("Theater")]
        Theater,

        [Description("Cinema")]
        Cinema,
    }
}
