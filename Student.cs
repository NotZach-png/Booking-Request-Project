using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milestone2
{
    // This class is used just for the students information
    public class Student
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
}
