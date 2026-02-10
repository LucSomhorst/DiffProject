// using Godot;
// using System;
// using System.IO.Ports;
// using System.Threading;
//
// public partial class SerialManager : Node
// {
//     [Signal] public delegate void DataReceivedEventHandler(string data);
//     [Signal] public delegate void SerialConnectedEventHandler();
//     [Signal] public delegate void SerialDisconnectedEventHandler();
//
//     private SerialPort serialPort;
//     private Thread serialThread;
//     private bool running = false;
//
//     // --- CONFIG ---
//     [Export] public string PortName = "COM3";
//     [Export] public int BaudRate = 9600;
//     [Export] public int ReadTimeoutMs = 500;
//
//     public override void _Ready()
//     {
//         TryOpenPort();
//     }
//
//     public void TryOpenPort()
//     {
//         try
//         {
//             serialPort = new SerialPort(PortName, BaudRate)
//             {
//                 ReadTimeout = ReadTimeoutMs,
//                 NewLine = "\n"
//             };
//
//             serialPort.Open();
//             GD.Print("✅ Serial port opened!");
//             EmitSignal(SignalName.SerialConnected);
//
//             running = true;
//
//             // Start background reading thread
//             serialThread = new Thread(SerialThreadLoop);
//             serialThread.Start();
//         }
//         catch (Exception e)
//         {
//             GD.PrintErr("Failed to open serial port: " + e.Message);
//         }
//     }
//
//     private void SerialThreadLoop()
//     {
//         // Give Arduino time to reset
//         Thread.Sleep(2000);
//
//         while (running)
//         {
//             try
//             {
//                 if (serialPort.BytesToRead > 0)
//                 {
//                     string line = serialPort.ReadLine().Trim();
//
//                     // Send to main thread safely
//                     CallDeferred(nameof(OnDataReceived), line);
//                 }
//             }
//             catch (TimeoutException)
//             {
//                 // Normal, just continue
//             }
//             catch (Exception e)
//             {
//                 GD.PrintErr("Serial read error: " + e.Message);
//             }
//
//             Thread.Sleep(5); // Small delay so CPU isn't maxed
//         }
//     }
//
//     private void OnDataReceived(string line)
//     {
//         EmitSignal(SignalName.DataReceived, line);
//     }
//
//     public override void _ExitTree()
//     {
//         running = false;
//
//         if (serialThread != null && serialThread.IsAlive)
//         {
//             serialThread.Join(); // wait for thread to finish
//         }
//
//         if (serialPort != null && serialPort.IsOpen)
//         {
//             serialPort.Close();
//             EmitSignal(SignalName.SerialDisconnected);
//             GD.Print("🔌 Serial port closed");
//         }
//     }
// }
