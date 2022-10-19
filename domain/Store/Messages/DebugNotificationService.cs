using System.Diagnostics;

namespace Store.Messages
{
    public class DebugNotificationService : INotificationService
    {
        public void SendConfirmationCode(string cellPhone, int code)
        {
            Debug.WriteLine("Cell phone: {0}, code: {1:0000}.", cellPhone, code);
        }

        public Task SendConfirmationCodeAsync(string cellPhone, int code)
        {
            Debug.WriteLine("Cell phone: {0}, code: {1:0000}.", cellPhone, code);

            return Task.CompletedTask;
        }

        public void StartProcess(Order order)
        {
            Debug.WriteLine("Order ID {0}", order.Id);            
        }

        public Task StartProcessAsync(Order order)
        {
            StartProcess(order);

            return Task.CompletedTask;
        }
    }
}
