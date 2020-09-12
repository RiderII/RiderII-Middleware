﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RiderIIMiddleware
{
    class PacketSend
    {
        private static void SendTCPData(Packet _packet)
        {
            _packet.WriteLength();
            Client.instance.tcp.SendData(_packet);
        }

        private static void SendUDPData(Packet _packet)
        {
            _packet.WriteLength();
            Client.instance.udp.SendData(_packet);
        }

        #region Packets
        public static void WelcomeReceived()
        {
            using (Packet _packet = new Packet((int)ClientPackets.RequestEnterLobby))
            {
                _packet.Write(Client.instance.sendToUserId); //the server can confirm that the client claimed the correct Id.
                _packet.Write("Middleware");

                SendTCPData(_packet);
            }
        }

        public static void PlayerMovement(bool[] _inputs)
        {
            using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
            {
                _packet.Write(_inputs.Length);
                foreach (bool _input in _inputs)
                {
                    _packet.Write(_input);
                }
                _packet.Write(Client.instance.sendToUserId);

                SendUDPData(_packet);
            }
        }
        #endregion
    }
}