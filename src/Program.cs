using System;
using System.Threading.Tasks;
using Buttplug.Client;
using Buttplug.Core;
using Buttplug.Client.Connectors.WebsocketConnector;
using System.ComponentModel;
using tf2butt.src;

namespace tf2butt // Note: actual namespace depends on the project name.
{
    class Program
    {
        private static async Task WaitForKey()
        {
            Console.WriteLine("Press any key to continue.");
            while (!Console.KeyAvailable)
            {
                await Task.Delay(1);
            }
            Console.ReadKey(true);
        }

        static void Main()
        {
            Console.WriteLine("Starting tf2butt...");
            ButtplugConnection connection = new ButtplugConnection();

            TFWatcher watcher = new TFWatcher();
            connection.ButtplugInit().Wait();
            watcher.CheckStatus();
        }
    }
}