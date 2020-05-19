using System;
using System.ServiceModel;

namespace GettingStartedLib
{
    public class FridgeService : IFridge
    {
        int count = 0;
        public int countFruit()
        {
            Console.WriteLine("The amount of fruit is: {0}", count);
            return count;
        }

        public int addFruit(int amount)
        {
            count += amount;
            Console.WriteLine("The updated amount of fruit is: {0}", count);
            return count;
        }

        public int subtractFruit(int amount)
        {
            count -= amount;
            Console.WriteLine("The updated amount of fruit is: {0}", count);
            return count;
        }
    }
}