using System;
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

           

            Client newClient = new Client();
            newClient.sendToUserId = playerId;
            newClient.StartConnetcion();

            

            Thread mainThread = new Thread(new ThreadStart(MainThread));
            mainThread.Start();

        }

        private static void MainThread()
        {
            //Arduino
            Arduino arduino = new Arduino();
            Console.WriteLine($"Main thread started. Running at {Constants.TICKS_PER_SEC} ticks per second.");
            DateTime _nextLoop = DateTime.Now;

            while (isRunning)
            {
                while (_nextLoop < DateTime.Now)
                {
                    Logic.Udpate(arduino);

                    _nextLoop = _nextLoop.AddMilliseconds(Constants.MS_PER_TICK);
                }
            }
        }
    }
}
