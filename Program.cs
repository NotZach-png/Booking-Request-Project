using PRG261_Milestone1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milestone2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BookingManager bookings = new BookingManager();
            bookings.ShowMenu();
            Console.ReadLine();
        }
    }
}
