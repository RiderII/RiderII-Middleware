using System;
using System.Collections.Generic;
using System.Text;

namespace RiderIIMiddleware
{
    class PacketHandle
    {
        public static void Welcome(Packet _packet) //read the value of the packets send from the server in the same order they were send
        {
            string _msg = _packet.ReadString();
            int _id = _packet.ReadInt();

            Client.instance.middlewareId = _id;

            // Send packet back to the server
            PacketSend.WelcomeReceived();

            Console.WriteLine($"Message from server: {_msg}. RiderII middleware connected with id: {Client.instance.middlewareId}");

            //Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
        }
    }
}
