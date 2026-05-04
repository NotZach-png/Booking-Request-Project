using PRG261_Milestone1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milestone2
{
    // Option 1 did by Philippus Jacobus Ludick (603763)
    // Option 2 did by Minenhle Kubheka (604029)
    // Option 3 did by Tshiamo Didintle (600968)

    // Most fixes where done by Philippus Jacobus Ludick, project was basically done with milestone 1 so only a few changes were made
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
