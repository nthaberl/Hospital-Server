using Microsoft.AspNetCore.SignalR;
using System;

namespace HospitalServer
{
    public class HospitalHub : Hub
    {

        // --- Chat --- //
        public async Task SendMessage(string senderUserId, string receiverUserId, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", senderUserId, receiverUserId, message);
        }

        public async Task NotifyAppointmentChanged(string action, int appointmentId, int patientId, int staffId, string changedByRole)
        {
            await Clients.All.SendAsync(
                "AppointmentUpdated",
                action,
                appointmentId,
                patientId,
                staffId,
                changedByRole);
        }
    }
}
