using Buttplug.Client.Connectors.WebsocketConnector;
using Buttplug.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tf2butt
{
     class ButtplugConnection
    {
        // Usual embedded connector setup.
        
        public async Task ButtplugVibrate()
        {
            ButtplugClient client = new ButtplugClient("TF2 Client");
            // Set up our DeviceAdded/DeviceRemoved event handlers before connecting.
            client.DeviceAdded += (aObj, aDeviceEventArgs) =>
                Console.WriteLine($"Device {aDeviceEventArgs.Device.Name} Connected!");

            client.DeviceRemoved += (aObj, aDeviceEventArgs) =>
                Console.WriteLine($"Device {aDeviceEventArgs.Device.Name} Removed!");

            // Now that everything is set up, we can connect.
            try
            {
                await client.ConnectAsync(new ButtplugWebsocketConnector(new Uri("ws://127.0.0.1:12345")));
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Can't connect, exiting! Message: {ex?.InnerException?.Message}");
                return;
            }

            // We're connected, yay!
            Console.WriteLine("Connected!");
            // Now we can start scanning for devices, and any time a device is
            // found, we should see the device name printed out.
            await client.StartScanningAsync();

            // Some Subtype Managers will scan until we still them to stop, so
            // let's stop them now.
            await client.StopScanningAsync();

            // Since we've scanned, the client holds information about devices it
            // knows about for us. These devices can be accessed with the Devices
            // getter on the client.

            Console.WriteLine("Client currently knows about these devices:");
            foreach (var device in client.Devices)
            {
                Console.WriteLine($"- {device.Name}");
            }
            // Set up our scanning finished function to print whenever scanning is done.

            client.ScanningFinished += (aObj, aScanningFinishedArgs) =>
                Console.WriteLine("Device scanning is finished!");



            foreach (var device in client.Devices)
            {
                Console.WriteLine($"{device.Name} supports vibration: ${device.VibrateAttributes.Count > 0}");
                if (device.VibrateAttributes.Count > 0)
                {
                    Console.WriteLine($" - Number of Vibrators: {device.VibrateAttributes.Count}");
                }
            }

            Console.WriteLine("Sending commands");

            // Now that we know the message types for our connected device, we
            // can send a message over! Seeing as we want to stick with the
            // modern generic messages, we'll go with VibrateCmd.
            //
            // There's a couple of ways to send this message.
            if (client.Devices.Length > 0)
            {
                var testClientDevice = client.Devices[0];
                // We can use the convenience functions on ButtplugClientDevice to
                // send the message. This version sets all of the motors on a
                // vibrating device to the same speed.
                //await testClientDevice.VibrateAsync(1.0);

                // If we wanted to just set one motor on and the other off, we could
                // try this version that uses an array. It'll throw an exception if
                // the array isn't the same size as the number of motors available as
                // denoted by FeatureCount, though.
                //
                // You can get the vibrator count using the following code, though we
                // know it's 2 so we don't really have to use it.
                //
                // This vibrateType variable is just used to keep us under 80 
                // characters for the dev guide, so don't feel that you have to reassign 
                // types like this. I'm just trying to make it so you don't have to
                // horizontally scroll in the manual. :)
                var vibratorCount = testClientDevice.VibrateAttributes.Count;
                //await testClientDevice.VibrateAsync(new[] { 1.0, 0.0 });

                await testClientDevice.VibrateAsync(1.0);
            }
            else await Console.Out.WriteLineAsync("No devices, not vibrating");

            await ContinueVibration();
            // And now we disconnect as usual.
            await client.DisconnectAsync();
        }
        public async Task ButtplugInit()
        {
            ButtplugClient client = new ButtplugClient("TF2 Client");
            // Set up our DeviceAdded/DeviceRemoved event handlers before connecting.
            client.DeviceAdded += (aObj, aDeviceEventArgs) =>
                Console.WriteLine($"Device {aDeviceEventArgs.Device.Name} Connected!");

            client.DeviceRemoved += (aObj, aDeviceEventArgs) =>
                Console.WriteLine($"Device {aDeviceEventArgs.Device.Name} Removed!");

            // Now that everything is set up, we can connect.
            try
            {
                await client.ConnectAsync(new ButtplugWebsocketConnector(new Uri("ws://127.0.0.1:12345")));
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Can't connect, exiting! Message: {ex?.InnerException?.Message}");
                return;
            }

            // We're connected, yay!
            Console.WriteLine("Connected!");
            // Now we can start scanning for devices, and any time a device is
            // found, we should see the device name printed out.
            await client.StartScanningAsync();

            // Some Subtype Managers will scan until we still them to stop, so
            // let's stop them now.
            await client.StopScanningAsync();

            // Since we've scanned, the client holds information about devices it
            // knows about for us. These devices can be accessed with the Devices
            // getter on the client.

            Console.WriteLine("Client currently knows about these devices:");
            foreach (var device in client.Devices)
            {
                Console.WriteLine($"- {device.Name}");
            }
            // Set up our scanning finished function to print whenever scanning is done.

            client.ScanningFinished += (aObj, aScanningFinishedArgs) =>
                Console.WriteLine("Device scanning is finished!");



            foreach (var device in client.Devices)
            {
                Console.WriteLine($"{device.Name} supports vibration: ${device.VibrateAttributes.Count > 0}");
                if (device.VibrateAttributes.Count > 0)
                {
                    Console.WriteLine($" - Number of Vibrators: {device.VibrateAttributes.Count}");
                }
            }

            Console.WriteLine("Sending commands");



            // And now we disconnect as usual.
            await client.DisconnectAsync();
        }
        private static async Task WaitForKey()
        {
            Console.WriteLine("Press any key to continue.");
            while (!Console.KeyAvailable)
            {
                await Task.Delay(1);
            }
            Console.ReadKey(true);
        }
        private async Task ContinueVibration()
        {
            await Task.Delay(3000);
        }
    }
}
