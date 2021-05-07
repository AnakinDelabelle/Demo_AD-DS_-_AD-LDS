using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestENV;

namespace TestENVConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var consumer = new Consumer();
            consumer.ConsumeMessage();

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
