using Stocker.Data;
using System;

namespace Stocker
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                    Console.WriteLine("Seçiminizi yapınız..");
                    Console.WriteLine("1-data oku");
                    Console.WriteLine("2-çıkış");

                bool returnBack = true;
                while (returnBack)
                {
                    string secim = Console.ReadLine();
                    switch (secim)
                    {
                        case "1":
                            var parser = new WebParser();
                            parser.GetData().Wait();
                            break;

                        case "2":
                            returnBack = false;
                            break;
                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
