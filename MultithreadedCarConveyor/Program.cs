using System;
using System.Collections.Generic;
using System.Threading;

namespace MultithreadedCarConveyor
{
    class Program
    {
        public static Semaphore s = new Semaphore(1, 1, "mySemaphor");
        public static Queue<Detail> conveyor = new Queue<Detail>();
        public static int lengthOfConveyor { get; set; }
        public static int cntModel { get; set; }
        public static int cntDetails { get; set; }
        public static bool overflow { get; set; }
        public static Random r = new Random();

        static void Main(string[] args)
        {
            // задаем количество разных моделей
            cntModel = 2;
            // задаем количество деталей в автомобиле
            cntDetails = 8;

            lengthOfConveyor = 250;
            overflow = false;

            AddToConveyor add = new AddToConveyor();

            for (int numModel = 1; numModel <= cntModel; numModel++)
            {
                AssembleCar model = new AssembleCar(numModel, cntDetails);
            }

            Console.ReadLine();
        }
    }
}
