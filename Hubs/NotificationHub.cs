using Microsoft.AspNetCore.SignalR;

namespace PetAdminApi.Hubs
{
    public class NotificationHub : Hub
    {
        // Метод для отправки уведомлений об удалении пользователя или администратора
        public async Task NotifyUserDeleted(string username)
        {
            await Clients.All.SendAsync("UserDeleted", username);
        }

        public async Task NotifyAdminDeleted(string username)
        {
            await Clients.All.SendAsync("AdminDeleted", username);
        }
    }
}
