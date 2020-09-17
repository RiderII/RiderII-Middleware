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
            bool[] _inputs = new bool[]
            {
                arduino.Update(),
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
