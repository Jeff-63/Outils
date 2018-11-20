using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace MultiThread
{
    class Program
    {
        public static int someInt;
        private static object myLock = new object();
        public static Mutex mutex = new Mutex();
        public static Semaphore sem = new Semaphore(2, 2);


        public delegate void delg();

        /*static void Main(string[] args)
        {
            Random rand = new Random();

            /*{
                ThreadStart ts = new ThreadStart(PrintOutOnes);     //Create a thread start given delegate
                Thread t = new Thread(ts);                          //Create thread with threadstart
                //t.Priority = ThreadPriority.Highest;
                t.Start();                                          //Start the thread
            }
            Console.WriteLine("Main Thread");
            {
                ThreadStart ts = new ThreadStart(PrintOutZeros);     //Create a thread start given delegate
                Thread t = new Thread(ts);                          //Create thread with threadstart
                t.Start();                                          //Start the thread
            }

            {
                ThreadStart ts = new ThreadStart(GoatNoises);     //Create a thread start given delegate
                Thread t = new Thread(ts);                          //Create thread with threadstart
                t.Start();                                          //Start the thread
            }

            {
                ThreadStart ts = new ThreadStart(GoatNoises);     //Create a thread start given delegate
                Thread t = new Thread(ts);                          //Create thread with threadstart
                t.Start();                                          //Start the thread
            }

            {
                ThreadStart ts = new ThreadStart(() => { someInt++; });     //Create a thread start given delegate
                Thread t = new Thread(ts);                          //Create thread with threadstart
                t.Start();                                          //Start the thread
            }

            {
                ThreadStart ts = new ThreadStart(() => { someInt++; });     //Create a thread start given delegate
                Thread t = new Thread(ts);                          //Create thread with threadstart
                t.Start();                                          //Start the thread
            }

            {
                ThreadStart ts = new ThreadStart(() => { PrintOutChar('a', 50); });     
                Thread t = new Thread(ts);                                  
                t.Start();                                                  
            }

            for (int i = 0; i < 500; i++)
            {
                {
                    ThreadStart ts = new ThreadStart(NightClub);
                    Thread t = new Thread(ts);
                    t.Name = "Thread [" + (i + 1) + "]";
                    t.Start();
                }
            }*/

            /*for (int i = 0; i < 20; i++)
            {
                int s = rand.Next(0, 500);
                Thread.Sleep(s);

                ThreadStart ts = new ThreadStart(BiggerNightClub);
                Thread t = new Thread(ts);
                t.Name = "Thread [" + (i + 1) + "]";
                t.Start();

            }

            List<delg> delgList = new List<delg>()
            {
                BiggerNightClub,
                BiggerNightClub,
                NightClub,
                () => someInt++,
                () => BiggerNightClub(),
                () => PrintOutChar('a', 50)
            };

            Stack<delg> toProcess = new Stack<delg>(delgList);

            while(toProcess.Count > 0)
            {
                StartThread(toProcess.Pop());
            }


            //Console.WriteLine("Program finished");
            Console.ReadLine();
        }*/

        public static void StartThread(delg d)
        {
            ThreadStart ts = new ThreadStart(d);
            Thread t = new Thread(ts);
            t.Start();
        }

        public static void BiggerNightClub()
        {

            Console.WriteLine(Thread.CurrentThread.Name + " : Trying to get in the club");
            if (sem.WaitOne(1000))
            {
                Console.WriteLine(Thread.CurrentThread.Name + " : Inside the bigger club");
                Thread.Sleep(2000);
                Console.WriteLine(Thread.CurrentThread.Name + " : Exiting the bigger club");
                sem.Release();
            }
            else
            {
                Console.WriteLine(Thread.CurrentThread.Name + " : Got bored and left");
            }
        }

        public static void NightClub()
        {
            Console.WriteLine(Thread.CurrentThread.Name + " : Trying to get in the club");
            if (mutex.WaitOne(1000))
            {
                Console.WriteLine(Thread.CurrentThread.Name + " : Inside the club");
                Thread.Sleep(2000);
                Console.WriteLine(Thread.CurrentThread.Name + " : Exiting club");
                mutex.ReleaseMutex();
            }
            else
            {
                Console.WriteLine(Thread.CurrentThread.Name + " : Got bored and left");
            }

        }

        public static void GoatNoises()
        {
            lock (myLock)
            {
                Thread.Sleep(3000);
                Console.WriteLine("baaah");
                Console.WriteLine("some int: " + someInt);
            }
        }

        public static void PrintOutOnes()
        {
            for (int i = 0; i < 30; ++i)
            {
                Console.WriteLine("1");
                someInt++;
            }
        }

        public static void PrintOutZeros()
        {
            for (int i = 0; i < 30; ++i)
            {
                someInt++;
                Console.WriteLine("0");
            }
        }

        public static void PrintOutChar(char toPrint, int numOfTimes)
        {
            for (int i = 0; i < numOfTimes; ++i)
            {
                Interlocked.Increment(ref someInt); //incrémentation correcte
                Console.WriteLine(toPrint);
            }
        }

    }
}
