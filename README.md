# IoT Device Monitoring System

### Project Overview
This is a **console-based IoT Device Monitoring System** developed in **C#** as part of the CET2007 module assignment. The system allows users to manage a network of IoT devices by performing the following operations:

- Add new devices (with unique ID, name, and IP address)
- Prevent duplicate IDs and IP addresses
- Update device status (Online, Offline, Maintenance)
- Search devices by ID or partial name (case-insensitive)
- Sort devices by name using a **manual bubble sort algorithm**
- Generate a system health report
- Persist all device data to a JSON file (`Data/devices.json`)
- Log all important actions to a text file (`Data/logs.txt`)
- Robust error handling and input validation

The application follows **Object-Oriented Programming (OOP)** principles and professional software engineering practices.

### Features Implemented
- **OOP Design**: Classes, encapsulation, separation of concerns
- **Data Persistence**: JSON serialization using **Newtonsoft.Json**
- **Algorithms**:
  - Linear search (by ID and name – manual implementation)
  - Manual bubble sort (by device name, case-insensitive)
- **File I/O**: Safe loading/saving with error handling and logging
- **Logging**: All key actions logged to `logs.txt` with timestamps
- **Exception Handling**: Graceful handling of invalid input, duplicates, and file errors
- **Unit Testing**: 4+ comprehensive MSTest unit tests

### Project Structure
