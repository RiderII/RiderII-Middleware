﻿using System;
using System.Threading;

namespace RiderIIMiddleware
{
    class Program
    {
        private static bool isRunning;

        static void Main(string[] args)
        {
            Console.WriteLine("RiderII Middleware!");
            isRunning = true;
            Console.WriteLine("Enter player id: ");

            int playerId = Convert.ToInt32(Console.ReadLine());

            Thread mainThread = new Thread(new ThreadStart(MainThread));
            mainThread.Start();

            Client newClient = new Client();
            newClient.sendToUserId = playerId;
            newClient.StartConnetcion();
            Console.ReadKey();
        }

        private static void MainThread()
        {
            Console.WriteLine($"Main thread started. Running at {Constants.TICKS_PER_SEC} ticks per second.");
            DateTime _nextLoop = DateTime.Now;

            while (isRunning)
            {
                while (_nextLoop < DateTime.Now)
                {
                    Logic.Udpate();

                    _nextLoop = _nextLoop.AddMilliseconds(Constants.MS_PER_TICK);
                }
            }
        }
    }
}