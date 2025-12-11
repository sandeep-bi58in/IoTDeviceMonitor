using Microsoft.VisualStudio.TestTools.UnitTesting;
using IoTDeviceMonitor.Services;
using IoTDeviceMonitor.Enums;
using System.IO;
using System.Linq;
using System;

namespace IoTDeviceMonitor.Tests
{
    [TestClass]
    public class DeviceManagerTests
    {
        private DeviceManager manager;
        private string testFilePath = "Data/devices.json";

        [TestInitialize]
        public void Setup()
        {
            manager = new DeviceManager();

            if (File.Exists(testFilePath))
                File.Delete(testFilePath);
        }

        // Runs after each test – clean up
        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(testFilePath))
                File.Delete(testFilePath);
        }

        [TestMethod]
        public void AddDevice_ShouldAddDeviceAndPersistToFile()
        {
            // Act
            manager.AddDevice("DEV001", "Front Door Sensor", "192.168.1.100");

            // Assert
            var devices = manager.GetAllDevices();
            Assert.AreEqual(1, devices.Count);

            var device = devices.First();
            Assert.AreEqual("DEV001", device.Id);
            Assert.AreEqual("Front Door Sensor", device.Name);
            Assert.AreEqual("192.168.1.100", device.IpAddress);
            Assert.AreEqual(DeviceStatus.Online, device.Status);

            Assert.IsTrue(File.Exists(testFilePath));
            string content = File.ReadAllText(testFilePath);
            Assert.IsTrue(content.Contains("DEV001"));
            Assert.IsTrue(content.Contains("Front Door Sensor"));
        }

        [TestMethod]
        public void UpdateStatus_ShouldChangeStatusAndUpdateLastSeen()
        {
            manager.AddDevice("DEV002", "Garage Gateway", "192.168.1.101");

            manager.UpdateStatus("DEV002", DeviceStatus.Maintenance);

            var device = manager.FindById("DEV002");
            Assert.IsNotNull(device);
            Assert.AreEqual(DeviceStatus.Maintenance, device.Status);

            var timeDiff = DateTime.Now - device.LastSeen;
            Assert.IsTrue(timeDiff.TotalSeconds < 5);
        }

        [TestMethod]
        public void SortByNameManual_ShouldSortDevicesAlphabeticallyIgnoringCase()
        {
            manager.AddDevice("D3", "camera living room", "10.0.0.3");
            manager.AddDevice("D1", "Zigbee Hub", "10.0.0.1");
            manager.AddDevice("D2", "access point upstairs", "10.0.0.2");

            manager.SortByNameManual();

            var sorted = manager.GetAllDevices();

            Assert.AreEqual("access point upstairs", sorted[0].Name);  
            Assert.AreEqual("camera living room", sorted[1].Name);     
            Assert.AreEqual("Zigbee Hub", sorted[2].Name);             

            Assert.AreEqual("D2", sorted[0].Id);
            Assert.AreEqual("D3", sorted[1].Id);
            Assert.AreEqual("D1", sorted[2].Id);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddDevice_DuplicateId_ShouldThrowException()
        {
            manager.AddDevice("DUPE1", "Test Device", "192.168.99.99");
            manager.AddDevice("DUPE1", "Another Device", "192.168.99.98");
        }

        [TestMethod]
        public void SearchByName_ShouldReturnMatchingDevicesCaseInsensitive()
        {
            manager.AddDevice("S1", "Kitchen Light", "192.168.1.10");
            manager.AddDevice("S2", "kitchen fan", "192.168.1.11");
            manager.AddDevice("S3", "Bedroom Lamp", "192.168.1.12");

            var results = manager.SearchByName("kitchen");

            Assert.AreEqual(2, results.Count);
            Assert.IsTrue(results.Any(d => d.Name == "Kitchen Light"));
            Assert.IsTrue(results.Any(d => d.Name == "kitchen fan"));
        }
    }
}