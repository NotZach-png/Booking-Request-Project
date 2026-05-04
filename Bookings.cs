using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PRG261_Milestone1
{
    // This class is used just for the students information
    internal class Student
    {
        // Encapsulating these values
        private string fname;
        private string lname;
        private string stunum;

        readonly TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
        public string FName
        {
            get { return fname; }
            set
            {
                if (string.IsNullOrEmpty(value) || value.Length < 2 || !value.All(char.IsLetter))
                    throw new ArgumentException("First name must be more than 2 letters and only contain letters not numbers.");
                fname = textInfo.ToTitleCase(value.ToLower());
            }
        }
        public string LName
        {
            get { return lname; }
            set
            {
                if (string.IsNullOrEmpty(value) || value.Length < 2 || !value.All(char.IsLetter))
                    throw new ArgumentException("Your last name must be more than 2 letters and only contain letters not numbers.");
                lname = textInfo.ToTitleCase(value.ToLower());
            }
        }
        public string StuNum
        {
            get { return stunum; }
            set
            {
                if (string.IsNullOrEmpty(value) || value.Length != 6 || !value.All(char.IsDigit))
                    throw new ArgumentException("Length of student number is 6 characters and must only consist of digits");
                stunum = value;
            }
        }

        public Student()
        {
        }

        public Student(string fname, string lname, string stunum)
        {
            this.FName = fname;
            this.LName = lname;
            this.StuNum = stunum;
        }

    }

    public delegate void ApprovedBooking(string message);
    public delegate void RejectedBooking(string message);
    public delegate void ConditionalBooking(string message);
    internal class Bookings
    {
        public Student Student { get; set; }
        public int YearOfStudy { get; set; }
        public string EquipType { get; set; }
        public int DurationInHours { get; set; }
        public bool Training { get; set; }
        public int ActiveBookings { get; set; }
        public string Status { get; set; }

        // Events
        public event ApprovedBooking Approve;
        public event RejectedBooking Reject;
        public event ConditionalBooking Conditional;

        // Setting up constructor
        public Bookings(Student student, int year, string equip, int duration, bool train, int activeBookings)
        {
            // Calling formatted globalization object so that we don't have to stress to much about formatting values
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

            this.Student = student;
            this.YearOfStudy = year;
            this.EquipType = textInfo.ToTitleCase(equip.ToLower());
            this.DurationInHours = duration;
            this.Training = train;
            this.ActiveBookings = activeBookings;
            this.Status = "Pending";
        }

        public void CheckStatus()
        {
            if (this.Status == "Approved")
            {
                Approve?.Invoke("Your request has been automatically APPROVED...");
            }
            else if (this.Status == "Conditional")
            {
                Conditional?.Invoke("Your request is under CONDTIONALLY APPROVED...");
            }
            else if (this.Status == "Rejected")
            {
                Reject?.Invoke("Your request has been REJECTED...");
            }
        }
    }

    internal delegate void EvaluationDelegate(Bookings booking);

    public class BookingManager
    {
        enum Menu
        {
            Request_Booking = 1,
            Booking_Eligible,
            Booking_Stats,
            Exit
        }

        readonly List<Bookings> booking = new List<Bookings>();
        readonly List<Bookings> approved = new List<Bookings>();
        readonly List<Bookings> rejected = new List<Bookings>();
        int approvedCount = 0;
        int conditionalCount = 0;
        int rejectedCount = 0;

        public void ShowMenu()
        {
            bool looping = true;
            do
            {
                DisplayMenu();
                Console.WriteLine("----------------------------------------");
                Console.Write("Please enter the operation you want to do: ");
                string option = Console.ReadLine();
                bool optionValidate = int.TryParse(option, out int validOption);

                if (optionValidate && validOption >= 1 && validOption <= 4)
                {
                    Menu menu = (Menu)validOption;
                    switch (menu)
                    {
                        case Menu.Request_Booking:
                            Console.Clear();
                            CaptureBooking();
                            Console.Clear();
                            break;
                        case Menu.Booking_Eligible:
                            EvaluateBookings();
                            Console.Clear();
                            break;
                        case Menu.Booking_Stats:
                            DisplayStatistics();
                            Console.WriteLine("\nPress any key to continue...");
                            Console.ReadKey();
                            Console.Clear();
                            break;
                        case Menu.Exit:
                            Console.Clear();
                            Console.WriteLine("Thank you for using our application. I hope we don't see you again:)");
                            for (int i = 0; i <= 67; i++)
                            {
                                Console.Write("=");
                                Thread.Sleep(25);
                            }
                            looping = false;
                            break;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("===PLEASE ENTER A VALID OPTION===");
                    Console.WriteLine("---------------------------------");
                    Console.ResetColor();
                }
            } while (looping);
        }

        static void DisplayMenu()
        {
            foreach (Menu menu in Menu.GetValues(typeof(Menu)))
            {
                Console.WriteLine($"{(int)menu}. {menu}");
            }
        }

        public void DisplayBookings()
        {
            foreach (var book in booking)
            {
                Console.WriteLine($"Name: {book.Student.FName}\nSurname: {book.Student.LName}\nStudent Number: {book.Student.StuNum}\nCurrent Student Year: {book.YearOfStudy}\nTraining finished: {book.Training}\n");
                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine();
                Console.WriteLine($"Equipment needed: {book.EquipType}\nDuration needed(Hours): {book.DurationInHours}\nBooking Status: {book.Status}");
                Console.WriteLine("======================================================");
            }
        } // Might edit this to display all bookings later on

        public void CaptureBooking()
        {
            string equipment;

            int studentYear;
            int givenDuration;
            int activeBookings;
            bool train;
            bool capture = true;

            // This is how we are able to log the values and be able to throw our exceptions when needed with method overloading.
            Student student = new Student();

            do
            {
                do
                {
                    try
                    {
                        Console.Write("Please enter your first name: ");
                        string fName = Console.ReadLine();
                        student.FName = fName;
                        break;
                    }
                    catch (ArgumentException ex)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ex.Message);
                        Console.ResetColor();
                        Console.WriteLine("----------------------------------------------------------------------------");
                    }
                } while (true);

                do
                {
                    try
                    {
                        Console.Write("Please enter your last name: ");
                        string lName = Console.ReadLine();
                        student.LName = lName;
                        break;
                    }
                    catch (ArgumentException ex)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ex.Message);
                        Console.ResetColor();
                        Console.WriteLine("------------------------------------------------------------------------");
                    }
                } while (true);

                do
                {
                    try
                    {
                        Console.Write("Ok so now we need your student number to steal your info:) ");
                        string stuNumber = Console.ReadLine();
                        student.StuNum = stuNumber;
                        break;
                    }
                    catch (ArgumentException ex)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ex.Message);
                        Console.ResetColor();
                        Console.WriteLine("-------------------------------------------------------");
                    }
                } while (true);

                do
                {
                    Console.Write("Ok what equipment do you need: ");
                    string equip = Console.ReadLine();

                    if (equip.Length >= 4)
                    {
                        equipment = equip;
                        break;
                    }
                    else
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("So can we just forget why you are using this program? We probably need to know what equipment you want!");
                        Console.ResetColor();
                        Console.WriteLine("-------------------------------------------------------------------------------------------------------");
                    }
                } while (true);

                do
                {
                    Console.Write("What year are you currently now? (1st, 2nd or 3rd): ");
                    string year = Console.ReadLine();
                    bool isYear = int.TryParse(year, out int validYear);
                    if (isYear && validYear >= 1 && validYear <= 3)
                    {
                        studentYear = validYear;
                        break;
                    }
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Where are you from, the stone age? TRY AGAIN!");
                    Console.ResetColor();
                    Console.WriteLine("---------------------------------------------");
                } while (true);

                do
                {
                    Console.Write("For how many hours will you be stealing - I mean borrow the equipment\nMin Hours 1 - Max Hours 6: ");
                    string duration = Console.ReadLine();
                    bool isDuration = int.TryParse(duration, out int validDuration);
                    if (isDuration && validDuration > 0 && validDuration < 7)
                    {
                        givenDuration = validDuration;
                        break;
                    }
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Sorry but the duration has to be more than 0 hours but less than 7\nDon't be the funny guy give our stuff back.");
                    Console.ResetColor();
                    Console.WriteLine("-----------------------------------------------------------------");
                } while (true);

                do
                {
                    Console.Write("Ok so lastly, did you finish your main module training(y/n) ");
                    string yesOrNo = Console.ReadLine().ToLower().Trim();
                    if (yesOrNo.Equals("y") && yesOrNo.Length == 1)
                    {
                        train = true;
                        break;
                    }
                    else if (yesOrNo.Equals("n") && yesOrNo.Length == 1)
                    {
                        train = false;
                        break;
                    }
                    else
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Sorry but you either have to type 'y' or 'n', nothing complicated about that");
                        Console.ResetColor();
                        Console.WriteLine("----------------------------------------------------------------------------");
                    }
                } while (true);

                do
                {
                    Console.Write("How many active bookings do you currently have? (0-5): ");
                    string activeInput = Console.ReadLine();
                    bool isValidActive = int.TryParse(activeInput, out int validActive);
                    if (isValidActive && validActive >= 0 && validActive <= 5)
                    {
                        activeBookings = validActive;
                        break;
                    }
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please enter a number between 0 and 5");
                    Console.ResetColor();
                    Console.WriteLine("-------------------------------------");
                } while (true);

                do
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Do you want to continue\nOr can we exit the program?(y/n) ");
                    Console.ResetColor();
                    string continueOrNo = Console.ReadLine().Trim().ToLower();
                    if (continueOrNo.Equals("y"))
                    {
                        Console.Clear();
                        break;
                    }
                    else if (continueOrNo.Equals("n"))
                    {
                        capture = false;
                        break;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("You only have to type 'y' or 'n'\nDon't worry let's just go back to the main menu");
                        capture = false;
                        Thread.Sleep(2200);
                        break;
                    }
                } while (true);

                Bookings newBooking = new Bookings(student, studentYear, equipment, givenDuration, train, activeBookings);
                booking.Add(newBooking);

                // This is to update instantly and fire the event off each capture.
                EvaluationDelegate evaluate = EvaluateSingleBooking;
                evaluate(newBooking);

                // Running our events and firing it off once its needed.
                Console.Clear();
                newBooking.Approve += (msg) => Console.WriteLine(msg);
                newBooking.Conditional += (msg) => Console.WriteLine(msg);
                newBooking.Reject += (msg) => Console.WriteLine(msg);
                newBooking.CheckStatus();
                Console.Write("Please press any key to continue...");
                Console.ReadKey();

            } while (capture);
        }

        // Option 2: Evaluate Booking Eligibility.
        // This is used to add the correct bookings to the correct list.
        public void EvaluateBookings()
        {
            // Clear everything at the start so that we don't have duplicate data every time we run this method.
            approved.Clear();
            rejected.Clear();

            EvaluationDelegate evaluate = EvaluateSingleBooking;

            foreach (var book in booking)
            {
                // Getting our status
                evaluate(book);
            }

            // Filetring and adding values to the correct list.
            approved.AddRange(booking.Where(status => status.Status == "Approved" || status.Status == "Conditional"));
            rejected.AddRange(booking.Where(status => status.Status == "Rejected"));

            // Using LINQ to count the books.
            approvedCount = approved.Count(status => status.Status == "Approved");
            conditionalCount = approved.Count(status => status.Status == "Conditional");
            rejectedCount = rejected.Count();

            // display result
            Console.Clear();
            Console.WriteLine("=== BOOKING ELIGIBILITY RESULTS ===");
            Console.WriteLine($"Total bookings Evaluated: {booking.Count}");
            Console.WriteLine($"Fully Approved: {approvedCount}");
            Console.WriteLine($"Conditionally Approved: {conditionalCount}");
            Console.WriteLine($"Rejected: {rejectedCount}");
            // Subscribers and will automatically run when it is one of the events.            
            Console.WriteLine("Please press enter to continue...");
            Console.ReadKey();
        }

        // Used for the delegate function
        static void EvaluateSingleBooking(Bookings booking)
        {
            bool isApproved = booking.Training == true && booking.DurationInHours <= 4 && booking.ActiveBookings <= 2;
            bool isConditional = booking.DurationInHours >= 5 && booking.DurationInHours <= 6 && booking.Training == true && booking.ActiveBookings <= 2;
            bool isRejected = booking.DurationInHours > 6 || booking.ActiveBookings >= 3 || booking.Training == false;

            if (isApproved)
                booking.Status = "Approved";
            else if (isConditional)
                booking.Status = "Conditional";
            if (isRejected)
                booking.Status = "Rejected";
        }

        // Option 3: Display Booking Statistics
        // This method displays overall booking statistics and shows approved bookings
        // sorted by priority (year, active bookings, duration)
        public void DisplayStatistics()
        {
            // Calling this method just incase a user decided to press number 3 before 2. Otherwise the data will not be added to the lists and they will still be empty
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("==========Adding values to lists===========");
            Thread.Sleep(1500);
            EvaluateBookings();

            Console.ResetColor();
            do
            {
                Console.Clear();
                Console.WriteLine("=== BOOKING STATISTICS===");
                int total = booking.Count;

                Console.WriteLine($"Total Booking Requests: {total}");
                Console.WriteLine($"Approved (including conditional): {approvedCount + conditionalCount}");
                Console.WriteLine($"Rejected: {rejectedCount}");

                Console.WriteLine("---------------------------------------------------");

                // SORT approved bookings (priority rules)
                var sortedApproved = approved
                    .OrderByDescending(b => b.YearOfStudy)
                    .ThenBy(b => b.ActiveBookings)
                    .ThenBy(b => b.DurationInHours)
                    .ToList();

                Console.Write("\nPress 1 to see All Approved Bookings.\nPress 2 to see Rejected Bookings.\nPress 3 to exit to main Menu\nChoose: ");
                string approveOrReject = Console.ReadLine();
                bool showBookValidity = int.TryParse(approveOrReject, out int validBookOption);
                if (showBookValidity && validBookOption >= 1 && validBookOption <= 3)
                {
                    if (validBookOption == 1)
                    {
                        int priorityOrder = 1;
                        Console.WriteLine("\n=== APPROVED BOOKINGS (PRIORITY ORDER) ===");
                        foreach (var approve in sortedApproved)
                        {
                            if (approve.Status == "Approved")
                                Console.ForegroundColor = ConsoleColor.Green;
                            else if (approve.Status == "Conditional")
                                Console.ForegroundColor = ConsoleColor.Yellow;

                            Console.WriteLine($"Priority: {priorityOrder}");
                            Console.WriteLine($"Name: {approve.Student.FName} || Last Name: {approve.Student.LName} || Student Number: {approve.Student.StuNum}" +
                                $"\nTraining: {(approve.Training ? "Yes" : "No")} || Active Bookings: {approve.ActiveBookings} || Hours Booked: {approve.DurationInHours}" +
                                $"\nStudent Current Year: {approve.YearOfStudy} || Equipment needed: {approve.EquipType}" +
                                $"\nStatus: {approve.Status}");
                            Console.WriteLine("-----------------------------------------------------------");
                            priorityOrder++;
                            Console.ResetColor();
                        }
                    }
                    else if (validBookOption == 2)
                    {
                        Console.WriteLine("\n=== REJECTED BOOKINGS ===");
                        foreach (var reject in rejected)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Name: {reject.Student.FName} || Last Name: {reject.Student.LName} || Student Number: {reject.Student.StuNum}" +
                                $"\nTraining: {(reject.Training ? "Yes" : "No")} || Active Bookings: {reject.ActiveBookings} || Hours Booked: {reject.DurationInHours}" +
                                $"\nStudent Current Year: {reject.YearOfStudy} || Equipment needed: {reject.EquipType}" +
                                $"\nStatus: {reject.Status}");
                            Console.WriteLine("-----------------------------------------------------------");
                            Console.ResetColor();
                        }
                    }
                    else if (validBookOption == 3)
                    {
                        break;
                    }
                }
                else
                    Console.WriteLine("Sorry but that isn't the correct option");

                Console.WriteLine("Please press any key to continue...");
                Console.ReadKey();
            } while (true);
        }
    }
}
