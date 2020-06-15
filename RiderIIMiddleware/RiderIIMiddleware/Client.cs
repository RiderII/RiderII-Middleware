﻿using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace RiderIIMiddleware
{
    class Client
    {
        public static Client instance;
        public static int dataBufferSize = 4096;
        public string ip = "127.0.0.1";
        public int port = 26950;
        public int middlewareId = 0;
        public int sendToUserId = 0;
        public TCP tcp;

        private bool isConnected = false;
        private delegate void PacketHandler(Packet _packet);
        private static Dictionary<int, PacketHandler> packetHandlers;

        public Client()
        {
            instance = this;
        }

        public void StartConnetcion()
        {
            InitializeClientData();
            tcp = new TCP();
            isConnected = true;
            tcp.Connect();
        }

        public class TCP
        {
            public TcpClient socket;

            private NetworkStream stream;
            private Packet receivedData;
            private byte[] receiveBuffer;

            public void Connect()
            {
                socket = new TcpClient
                {
                    ReceiveBufferSize = dataBufferSize,
                    SendBufferSize = dataBufferSize
                };

                receiveBuffer = new byte[dataBufferSize];
                socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);
            }

            private void ConnectCallback(IAsyncResult _result)
            {
                socket.EndConnect(_result);

                if (!socket.Connected)
                {
                    return;
                }

                //if we pass we are connected to the server and can start to send and receive data
                Console.WriteLine("Connected successfully to the server!");
                Console.WriteLine($"Movement packets will be sent to user with id: { instance.sendToUserId }");
                stream = socket.GetStream();

                receivedData = new Packet();

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }

            public void SendData(Packet _packet)
            {
                try
                {
                    if (socket != null)
                    {
                        stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                    }
                }
                catch (Exception _ex)
                {
                    Console.WriteLine($"Error sending data to server via TCP {_ex}");
                }
            }

            private void ReceiveCallback(IAsyncResult _result)
            {
                try
                {
                    int _byteLength = stream.EndRead(_result);
                    if (_byteLength <= 0)
                    {
                        // Disconnect
                        instance.Disconnect();
                        return;
                    }

                    byte[] _data = new byte[_byteLength];
                    Array.Copy(receiveBuffer, _data, _byteLength);

                    // handle data
                    receivedData.Reset(HandleData(_data));
                    stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null); //continue to reading data from the stream
                }
                catch (Exception _ex)
                {
                    Console.WriteLine($"Error receiving TCP data: {_ex}");
                    // Handle disconnect
                    Disconnect();
                }
            }

            private bool HandleData(byte[] _data)
            {
                int _packetLength = 0;

                receivedData.SetBytes(_data);

                if (receivedData.UnreadLength() >= 4)
                {
                    _packetLength = receivedData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        return true;
                    }
                }

                while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
                {
                    byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
                    ThreadManager.ExecuteOnMainThread(() =>
                    {
                        using (Packet _packet = new Packet(_packetBytes))
                        {
                            int _packetId = _packet.ReadInt();
                            packetHandlers[_packetId](_packet);
                        }
                    });

                    _packetLength = 0;
                    if (receivedData.UnreadLength() >= 4)
                    {
                        _packetLength = receivedData.ReadInt();
                        if (_packetLength <= 0)
                        {
                            return true;
                        }
                    }
                }

                if (_packetLength <= 1)
                {
                    return true;
                }

                return false;
            }

            private void Disconnect()
            {
                instance.Disconnect();

                stream = null;
                receivedData = null;
                receiveBuffer = null;
                socket = null;
            }
        }

        private void InitializeClientData()
        {
            packetHandlers = new Dictionary<int, PacketHandler>()
        {
            { (int)ServerPackets.welcome, PacketHandle.Welcome },
            //{ (int)ServerPackets.spawnPlayer, PacketHandle.SpawnPlayer },
            //{ (int)ServerPackets.playerPosition, PacketHandle.PlayerPosition },
            //{ (int)ServerPackets.playerRotation, PacketHandle.PlayerRotation },
            //{ (int)ServerPackets.playerDisconnected, PacketHandle.PlayerDisconnected },
            //{ (int)ServerPackets.playerCollided, PacketHandle.PlayerCollided },
            //{ (int)ServerPackets.obstacleSpawned, PacketHandle.ObstacleSpawned },
            //{ (int)ServerPackets.playerFinishedGame, PacketHandle.PlayerFinishedGame },
            //{ (int)ServerPackets.restartPlayerPosition, PacketHandle.RestartPlayerPosition }
        };
            Console.WriteLine("Initialized packets!");
        }

        public void Disconnect()
        {
            if (isConnected)
            {
                isConnected = false;

                if (tcp != null)
                {
                    tcp.socket.Close();
                }
                //if (udp != null)
                //{
                //    udp.socket.Close();
                //}

                Console.WriteLine("Disconnected from server.");
            }
        }
    }


    
}