# SummerGUI Demo 🚀
Welcome to the official demonstration project for the **SummerGUI X-Platform Framework**. This project is designed to showcase the power, speed, and modern rendering capabilities of SummerGUI in a real-world application context.

## What is SummerGUI?
SummerGUI is a high-performance, cross-platform UI framework for .NET 8. It moves away from legacy OpenGL to a modern **Core Profile** architecture using **Vertex Buffer Objects (VBOs)** and professional text shaping via **HarfBuzz**.

## Why try this Demo?

- **Real-World Examples:** See how to build windows, dialogs, and custom widgets.
- **True Cross-Platform:** Experience the exact same UI and performance on both Windows and Ubuntu.
- **Modern Stack:** Explore a codebase that utilizes .NET 8 and OpenTK 4.x to its full potential.

## 🛠 Prerequisites & Setup
Since SummerGUI is built for cross-platform development, this project is optimized for **VS-Code**.

### Recommended VS-Code Extensions
To get the best development experience (IntelliSense, Debugging, and Project Management), please install the following extensions:

1. **C# Dev Kit** (by Microsoft) – Provides powerful tools for managing your C# projects and solutions.
2. **C#** (by Microsoft) – The essential language support for C# development in VS-Code.

## 🚀 How to Run

### 🐧 On Ubuntu (Linux)
The project is pre-configured for Linux. Simply open the folder in VS-Code and press F5 to build and run.

### 🪟 On Windows
If you want to compile and run the project on Windows, you need to adjust the Target Runtime. Open the SummerGUI.Demo.csproj file and ensure the following setting is active:

```
<RuntimeIdentifier>win-x64</RuntimeIdentifier>
```

(Note: On Linux, you can change this to linux-x64 or remove it for a portable build.)

## 🔗 Learn More
This demo relies on the **SummerGUI Core Framework**. You can find the source code, documentation, and the rendering engine here:

[👉 SummerGUI Main Repository](https://github.com/kroll-software/SummerGUI)
[👉 SummerGUI.Scheduling](https://github.com/kroll-software/SummerGUI.Scheduling)
[👉 SummerGUI.Charting](https://github.com/kroll-software/SummerGUI.Charting)
[👉 KS.Foundation](https://github.com/kroll-software/KS.Foundation)

Clone all these Repos into a new directory and set the dependencies for the VS-Code Solution according.


## 📄 License
The SummerGUI Demo is provided under the **MIT License**. Feel free to use the code as a template for your own high-performance OpenGL applications!
