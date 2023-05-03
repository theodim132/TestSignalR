using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace TestSignalR.Services
{
    public class ServiceBusListener
    {
        private readonly string _serviceBusConnectionString;
        private readonly string _queueName;
        private readonly QueueClient _queueClient;

        public ServiceBusListener(string serviceBusConnectionString, string queueName)
        {
            _serviceBusConnectionString = serviceBusConnectionString;
            _queueName = queueName;
            _queueClient = new QueueClient(_serviceBusConnectionString, _queueName);
        }

        public void RegisterOnMessageHandlerAndReceiveMessages(Func<string, Task> onMessageCallback)
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            _queueClient.RegisterMessageHandler(async (message, token) =>
            {
                var messageBody = Encoding.UTF8.GetString(message.Body);
                await onMessageCallback(messageBody);
                await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
            }, messageHandlerOptions);
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            return Task.CompletedTask;
        }

        public async Task Close()
        {
            await _queueClient.CloseAsync();
        }
    }
}
