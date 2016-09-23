using System;
using MonoBrickFirmware.Display;
using MonoBrickFirmware.UserInput;
using MonoBrickFirmware.Sensors;
using System.Threading;
namespace LightSensorExample
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			ManualResetEvent terminateProgram = new ManualResetEvent(false);
			var lightSensor = new NXTLightSensor(SensorPort.In1);
			ButtonEvents buts = new ButtonEvents ();
			LcdConsole.WriteLine("Use light on port1");
			LcdConsole.WriteLine("Up value ");
			LcdConsole.WriteLine("Down change mode");
			LcdConsole.WriteLine("Esc. terminate");
			buts.EscapePressed += () => { 
				terminateProgram.Set();
			};
			buts.UpPressed += () => { 
				LcdConsole.WriteLine("Sensor value:" + lightSensor.ReadAsString());
			};
			buts.DownPressed += () => { 
				if(lightSensor.Mode == LightMode.Ambient){
					lightSensor.Mode = LightMode.Relection;
				}
				else{
					lightSensor.Mode = LightMode.Ambient;
				}
				LcdConsole.WriteLine("Sensor mode is now: " + lightSensor.Mode);
			};  
			terminateProgram.WaitOne();
		}
	}
}
