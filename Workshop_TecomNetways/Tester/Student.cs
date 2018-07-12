using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tester
{
    public enum Grade
    {
        F,
        D,
        C,
        B,
        A
    }

    public class Student
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<Book> Books { get; set; }      
    }
}
