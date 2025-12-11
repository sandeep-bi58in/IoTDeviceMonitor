using System;
using System.Collections.Generic;
using System.Linq;
using IoTDeviceMonitor.Models;
using IoTDeviceMonitor.Enums;

namespace IoTDeviceMonitor.Services
{
    public class DeviceManager
    {
        private List<Device> devices;
        private readonly DeviceRepository repository;

        public DeviceManager()
        {
            repository = new DeviceRepository();
            devices = repository.LoadDevices();
        }

        public void AddDevice(string id, string name, string ip)
        {
            if (devices.Any(d => d.Id.Equals(id, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Device ID already exists.");

            if (devices.Any(d => d.IpAddress == ip))
                throw new InvalidOperationException("IP address already in use.");

            var device = new Device(id, name, ip) { Status = DeviceStatus.Online, LastSeen = DateTime.Now };
            devices.Add(device);
            repository.SaveDevices(devices);
            Logger.Log($"Device added: {id} ({name})");
        }

        public Device FindById(string id)
        {
            // Linear search (required)
            foreach (var device in devices)
            {
                if (device.Id.Equals(id, StringComparison.OrdinalIgnoreCase))
                    return device;
            }
            return null;
        }

        /// <summary>
        /// Searches for devices whose name contains the given text (case-insensitive).
        /// Manual implementation without LINQ – shows understanding of loops and string comparison.
        /// </summary>
        /// <param name="namePart">Text to search for in device names</param>
        /// <returns>List of matching devices</returns>
        public List<Device> SearchByName(string namePart)
        {
            if (string.IsNullOrWhiteSpace(namePart))
            {
                Logger.Log("SearchByName called with empty search term – returning empty list.");
                return new List<Device>();
            }

            var results = new List<Device>();

            foreach (var device in devices)
            {
                if (device.Name == null)
                    continue;

                if (device.Name.IndexOf(namePart, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    results.Add(device);
                }
            }

            Logger.Log($"Search by name '{namePart}' returned {results.Count} result(s).");
            return results;
        }

        public void UpdateStatus(string id, DeviceStatus newStatus)
        {
            var device = FindById(id);
            if (device == null)
                throw new KeyNotFoundException("Device not found.");

            device.Status = newStatus;
            device.LastSeen = DateTime.Now;
            repository.SaveDevices(devices);
            Logger.Log($"Status updated for {id} -> {newStatus}");
        }

        // Manual Bubble Sort by Name
        public void SortByNameManual()
        {
            for (int i = 0; i < devices.Count - 1; i++)
            {
                for (int j = 0; j < devices.Count - i - 1; j++)
                {
                    if (string.Compare(devices[j].Name, devices[j + 1].Name, StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        var temp = devices[j];
                        devices[j] = devices[j + 1];
                        devices[j + 1] = temp;
                    }
                }
            }
            Logger.Log("Devices sorted by name (manual bubble sort)");
        }

        // Using built-in sort (for comparison)
        public void SortByNameBuiltIn()
        {
            devices = devices.OrderBy(d => d.Name, StringComparer.OrdinalIgnoreCase).ToList();
            Logger.Log("Devices sorted by name (built-in)");
        }

        public List<Device> GetAllDevices() => devices;

        public void GenerateReport()
        {
            Console.WriteLine("\n=== SYSTEM HEALTH REPORT ===");
            Console.WriteLine($"Total Devices: {devices.Count}");
            Console.WriteLine($"Online: {devices.Count(d => d.Status == DeviceStatus.Online)}");
            Console.WriteLine($"Offline: {devices.Count(d => d.Status == DeviceStatus.Offline)}");
            Console.WriteLine($"In Maintenance: {devices.Count(d => d.Status == DeviceStatus.Maintenance)}");
            Console.WriteLine("========================\n");
            Logger.Log("Health report generated");
        }

        public void Save() => repository.SaveDevices(devices);
    }
}