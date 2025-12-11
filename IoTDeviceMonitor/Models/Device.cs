using System;
using IoTDeviceMonitor.Enums;

namespace IoTDeviceMonitor.Models
{
    public class Device
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public DeviceStatus Status { get; set; }
        public DateTime LastSeen { get; set; }

        public Device() { }

        public Device(string id, string name, string ipAddress)
        {
            Id = id;
            Name = name;
            IpAddress = ipAddress;
            Status = DeviceStatus.Offline;
            LastSeen = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{Id} | {Name.PadRight(20)} | {IpAddress.PadRight(15)} | {Status} | {LastSeen:yyyy-MM-dd HH:mm}";
        }
    }
}