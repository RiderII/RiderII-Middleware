using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace RiderIIMiddleware
{
    class Arduino
    {
        public static Arduino instance;
        SerialPort sp = new SerialPort("COM5", 9600);
        public Arduino()
        {
            sp.Open();
            sp.ReadTimeout = 1;
            instance = this;
        }
        public float Update()
        {
            if (sp.IsOpen)
            {
                try
                {
                    if (sp.BytesToRead != 0)
                    {

                        //int direction = sp.ReadByte();
                        string data = sp.ReadLine();
                        float direction = float.Parse(data);
                        //Console.WriteLine(direction);
                       
                        return direction;
                        
                    }
                    else
                    {
                        return 0;
                    }                     
                }
                catch (System.Exception e)
                {
                    //Console.WriteLine(e.Message);
                    return 0;
                }
            }
            return 0;
        }
    }

}



