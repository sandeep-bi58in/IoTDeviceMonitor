using System;
using IoTDeviceMonitor.Services;
using IoTDeviceMonitor.Enums;

namespace IoTDeviceMonitor
{
    class Program
    {
        static DeviceManager manager = new DeviceManager();

        static void Main(string[] args)
        {
            Console.WriteLine("IoT Device Monitoring System");
            LoadData();

            while (true)
            {
                ShowMenu();
                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1": AddDevice(); break;
                        case "2": UpdateStatus(); break;
                        case "3": SearchById(); break;
                        case "4": ListAll(); break;
                        case "5": manager.SortByNameManual(); Console.WriteLine("Sorted manually!"); ListAll(); break;
                        case "6": manager.GenerateReport(); break;
                        case "7": Environment.Exit(0); break;
                        default: Console.WriteLine("Invalid option."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("\n1. Add Device");
            Console.WriteLine("2. Update Device Status");
            Console.WriteLine("3. Search by ID");
            Console.WriteLine("4. List All Devices");
            Console.WriteLine("5. Sort by Name (Manual)");
            Console.WriteLine("6. Generate Report");
            Console.WriteLine("7. Exit");
            Console.Write("Choose: ");
        }

        static void AddDevice()
        {
            Console.Write("ID: "); string id = Console.ReadLine();
            Console.Write("Name: "); string name = Console.ReadLine();
            Console.Write("IP: "); string ip = Console.ReadLine();
            manager.AddDevice(id, name, ip);
            Console.WriteLine("Device added!");
        }

        static void UpdateStatus()
        {
            Console.Write("Device ID: ");
            string id = Console.ReadLine();

            Console.WriteLine("Select new status:");
            Console.WriteLine("1. Online");
            Console.WriteLine("2. Offline");
            Console.WriteLine("3. Maintenance");
            Console.Write("Choose (1‑3): ");
            string s = Console.ReadLine();

            DeviceStatus status;

            if (s == "1")
                status = DeviceStatus.Online;
            else if (s == "2")
                status = DeviceStatus.Offline;
            else if (s == "3")
                status = DeviceStatus.Maintenance;
            else
            {
                Console.WriteLine("Invalid choice – defaulting to Offline.");
                status = DeviceStatus.Offline;
            }

            try
            {
                manager.UpdateStatus(id, status);
                manager.UpdateStatus(id, status);
                Console.WriteLine("Status updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void SearchById()
        {
            Console.Write("Device ID: "); string id = Console.ReadLine();
            var device = manager.FindById(id);
            if (device != null) Console.WriteLine(device);
            else Console.WriteLine("Not found.");
        }

        static void ListAll()
        {
            var devices = manager.GetAllDevices();
            if (devices.Count == 0) Console.WriteLine("No devices.");
            else foreach (var d in devices) Console.WriteLine(d);
        }

        static void LoadData()
        {
            Console.WriteLine($"Loaded {manager.GetAllDevices().Count} devices.");
        }
    }
}