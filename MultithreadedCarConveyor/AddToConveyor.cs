using System;
using System.Threading;

namespace MultithreadedCarConveyor
{
    class AddToConveyor
    {
        public Thread Thr { get; set; }

        public AddToConveyor()
        {
            ThreadStart threadRun = new ThreadStart(Run);
            Thr = new Thread(threadRun);
            Thr.Start();
        }

        private void Run()
        {
            Random r2 = new Random();
            int working = 10;
            while (!Program.overflow)
            {
                try
                {
                    Program.s.WaitOne();
                    Thread.Sleep(100);
                    int nm = Program.r.Next(1, Program.cntModel);
                    int np = r2.Next(1, Program.cntDetails);
                    Detail d = new Detail(nm, np);
                    if (Program.conveyor.Count < Program.lengthOfConveyor)
                    {
                        Program.conveyor.Enqueue(d);
                        Console.WriteLine("На конвейер добавлена деталь № " + np + " для модели № " + nm);
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("Конвейер переполнен! Завод стоит!!!");
                        Program.overflow = true;
                    }
                    working = working + 10;
                    Program.s.Release();
                }
                catch (ObjectDisposedException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Текущий экземпляр семафора уже был удален");
                }
                catch (AbandonedMutexException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Ожидание завершено, поскольку поток завершил работу, не освободив семафор.");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Текущий экземпляр является прозрачный прокси для WaitHandle в другом домене приложения.");
                }
            }

        }
    }
}
