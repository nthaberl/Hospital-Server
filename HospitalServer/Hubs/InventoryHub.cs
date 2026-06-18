using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace HospitalServer.Hubs
{
    public class InventoryHub : Hub
    {
        public async Task NotifyInventoryChanged(
            string itemName,
            int qtyInStock,
            int reorderThreshold,
            string changedByRole,
            bool wasLowStock)
        {
            await Clients.Others.SendAsync(
                "InventoryUpdated",
                itemName,
                qtyInStock,
                reorderThreshold,
                changedByRole);

            bool isLowStock = qtyInStock <= reorderThreshold;

            // The user who made the change gets a local MessageBox immediately.
            // Other connected inventory forms receive this SignalR alert.
            if (!wasLowStock && isLowStock)
            {
                await Clients.Others.SendAsync(
                    "LowStockAlert",
                    itemName,
                    qtyInStock,
                    reorderThreshold);
            }
        }

        public async Task NotifyInventoryRefresh(string changedByRole)
        {
            await Clients.Others.SendAsync(
                "InventoryUpdated",
                "Inventory",
                -1,
                -1,
                changedByRole);
        }
    }
}
