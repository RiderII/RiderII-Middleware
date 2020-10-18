using System;
using System.Collections.Generic;
using System.Text;

namespace RiderIIMiddleware
{
    class Logic
    {
        public static void Udpate(Arduino arduino)
        {

            SendInputToServer(arduino);
            ThreadManager.UpdateMain();
        }

        private static void SendInputToServer(Arduino arduino)
        {
            //bool[] _inputs = new bool[]
            //{
            //    arduino.Update(),
            //    false,
            //    false,
            //    false
            //};

            float input = arduino.Update();
            
            
            if (input > 0)
            {
                float speed = (input * Client.instance.wheelLong) / 60;
                //speed *= 0.6f;
                Console.WriteLine(speed);
                if(speed < 0.25)
                {
                    PacketSend.PlayerMovement(0.0f);
                }
                else
                {
                    PacketSend.PlayerMovement(speed);
                }
                
            }

        }
    }
}
