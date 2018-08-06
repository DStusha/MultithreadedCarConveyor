using System;
using System.Threading;

namespace MultithreadedCarConveyor
{
    class AssembleCar
    {
        public Thread Thr { get; set; }
        public int numModel { get; set; }
        public int countDetailsInModel { get; set; }

        public AssembleCar(int nm, int cnt)
        {
            numModel = nm;
            countDetailsInModel = cnt;
            ThreadStart threadRun = new ThreadStart(Run);
            Thr = new Thread(threadRun);
            Thr.Start();
        }

        public void Run()
        {
            int i = 1;
            while (i <= countDetailsInModel)
            {
                try
                {
                    Program.s.WaitOne();
                    Thread.Sleep(100);
                    if (Program.conveyor.Count != 0 && !Program.overflow)
                    {
                        Detail d = Program.conveyor.Dequeue();
                        if (d.numModel == numModel && d.numPosInModel == i)
                        {
                            Console.WriteLine("Деталь № " + d.numPosInModel + " подошла для модели № " + numModel + "!!!!!!!!!!!!!!");
                            Console.WriteLine();
                            if (i == countDetailsInModel)
                            {
                                Console.WriteLine("Модель № " + numModel + " собрана" + "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                                Thr.Abort();
                            }
                            else
                            {
                                i++;
                            }
                        }
                        else
                        {
                            if (Program.conveyor.Count < Program.lengthOfConveyor)
                            {
                                Console.WriteLine("Деталь  № " + d.numPosInModel + " не подошла для модели № " + numModel);
                                Program.conveyor.Enqueue(d);
                                Console.WriteLine("Деталь  № " + d.numPosInModel + " возвращена на конвейер");
                                Console.WriteLine();
                            }
                            else
                            {
                                Console.WriteLine("Конвейер переполнен! Завод стоит!");
                                Program.overflow = true;
                            }
                        }
                    }
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
