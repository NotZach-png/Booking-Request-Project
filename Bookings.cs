using Milestone2;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PRG261_Milestone1
{
    

    public delegate void ApprovedBooking(string message);
    public delegate void RejectedBooking(string message);
    public delegate void ConditionalBooking(string message);
    public class Bookings
    {
        // Getting our student class into our Booking class to access its values
        readonly Student ExStudent = new Student();

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
}
