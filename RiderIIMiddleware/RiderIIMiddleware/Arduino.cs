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

        public bool Update()
        {
            if (sp.IsOpen)
            {
                try
                {
                    //Console.WriteLine(sp);
                    int direction = sp.ReadByte();
                    Console.WriteLine(direction);
                    //if (direction.Length<3)
                    //{
                    //    return false;
                    //}
                    
                    //int data = Convert.ToInt32(direction);

                    if (direction == 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }

                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }

            return false;
        }

    }

}



