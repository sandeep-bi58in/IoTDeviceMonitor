using System;
using System.Collections.Generic;
using System.IO;
using IoTDeviceMonitor.Models;
using Newtonsoft.Json;

namespace IoTDeviceMonitor.Services
{
    public class DeviceRepository
    {
        private readonly string FilePath = "Data/devices.json";

        public DeviceRepository()
        {
            Directory.CreateDirectory("Data");
        }

        public List<Device> LoadDevices()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    Logger.Log("No existing devices file found, starting with empty list.");
                    return new List<Device>();
                }

                string json = File.ReadAllText(FilePath);

                if (string.IsNullOrWhiteSpace(json))
                    return new List<Device>();

                var devices = JsonConvert.DeserializeObject<List<Device>>(json);

                Logger.Log($"Successfully loaded {devices?.Count ?? 0} device(s) from file.");
                return devices ?? new List<Device>();
            }
            catch (JsonSerializationException jex)
            {
                Logger.Log($"JSON deserialization error (possibly corrupted file): {jex.Message}");
                if (File.Exists(FilePath))
                {
                    File.Copy(FilePath, FilePath + ".corrupted.bak", true);
                }
                return new List<Device>();
            }
            catch (Exception ex)
            {
                Logger.Log($"Error loading devices: {ex.Message}");
                return new List<Device>();
            }
        }
        public void SaveDevices(List<Device> devices)
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore
                };

                string json = JsonConvert.SerializeObject(devices, settings);
                File.WriteAllText(FilePath, json);

                Logger.Log($"Devices saved successfully ({devices.Count} device(s)).");
            }
            catch (Exception ex)
            {
                Logger.Log($"Error saving devices: {ex.Message}");
                throw;
            }
        }
    }
}