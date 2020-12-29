using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;

namespace ProjectDesignPatern.Exercice2
{
    class ThreadWork
    {


        public static void Work_Sum(object parameter)
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId } started.");
            object[] param = (object[])parameter;
            int[] inputData = (int[])param[0];
            using (NamedPipeClientStream resultspipe = (NamedPipeClientStream)param[1])
            {
                resultspipe.Connect();
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId } : Pipe connected.");
                int res = 0;
                for (int i = 0; i < inputData.Length; i++)
                {
                    res += inputData[i];
                }

                byte[] result = BitConverter.GetBytes(res);
                byte[] header = new byte[] { (byte)result.Length, (byte)Thread.CurrentThread.ManagedThreadId };

                if (header[0] != result.Length) throw new Exception("1 byte to small !!");


                resultspipe.Write(header.Concat(result).ToArray(), 0, 0);
                resultspipe.WaitForPipeDrain();

            }
        }
        public static void Work_Sum2(object param)
        {
        }
        public static void Work_Sum3()
        {
        }
    }
}
