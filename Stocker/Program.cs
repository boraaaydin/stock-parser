using Stocker.Data;
using System;
using System.Collections.Generic;

namespace Stocker
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new ConsoleClient();
            client.Run().Wait();
        }       
    }
}
