using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GettingStartedClient.ServiceReference2;

namespace GettingStartedClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //Step 1: Create an instance of the WCF proxy.
            FridgeClient client = new FridgeClient();

            int result0 = client.countFruit();
            Console.WriteLine("The initial number of fruit is: {0}", result0);

            Console.WriteLine("Enter the number of fruit to add:");
            int int1 = Convert.ToInt32(Console.ReadLine());
            var result1 = client.addFruit(int1);
            Console.WriteLine("The total number of fruit is: {0}", result1);

            Console.WriteLine("Enter the number of fruit to remove: ");
            int int2 = Convert.ToInt32(Console.ReadLine());
            var result2 = client.subtractFruit(int2);
            Console.WriteLine("The total number of fruit is: {0}", result2); ;

            client.Close();
        }
    }
}