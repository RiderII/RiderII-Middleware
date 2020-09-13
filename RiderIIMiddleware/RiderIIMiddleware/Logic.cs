using System;
using System.Collections.Generic;
using System.Text;

namespace RiderIIMiddleware
{
    class Logic
    {
        public static void Udpate()
        {
            SendInputToServer();
            ThreadManager.UpdateMain();
        }

        private static void SendInputToServer()
        {
            bool[] _inputs = new bool[]
            {
                Console.ReadKey().Key == ConsoleKey.UpArrow,
                false,
                false,
                false
            };

            PacketSend.PlayerMovement(_inputs);
        }
    }
}
