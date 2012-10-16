using System;
using System.IO.Ports;

namespace Mike.Spikes.SerialPorts
{
    public class SerialPortsSpike
    {
        public void Spike()
        {
            foreach (var portName in SerialPort.GetPortNames())
            {
                Console.Out.WriteLine(portName);
            }
        }     
    }
}