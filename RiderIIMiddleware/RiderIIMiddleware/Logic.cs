using System;
using System.Collections.Generic;
using System.Text;

namespace RiderIIMiddleware
{
    class Logic
    {
        public static void Udpate()
        {

            //arduino.Update();
            SendInputToServer();
            ThreadManager.UpdateMain();
        }

        private static void SendInputToServer()
        {
            bool[] _inputs = new bool[]
            {
                //arduino.Update(),
                Console.ReadKey().Key == ConsoleKey.UpArrow,
                false,
                false,
                false
            };

            if(_inputs[0] == true)
            {
                PacketSend.PlayerMovement(_inputs);
            }
        }
    }
}
