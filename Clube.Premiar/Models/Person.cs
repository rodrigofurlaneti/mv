using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clube.Premiar.Models
{
    public partial class Person
    {
        public Person()
        {
            phones = new List<Phone>();
            emails = new List<Email>();
        }

        public int id { get; set; }
        public string name { get; set; }
        public string documentNumber { get; set; }
        public string email { get; set; }
        public string rg { get; set; }
        public string status { get; set; }
        public string maritalStatus { get; set; }
        public string personType { get; set; }
        public string genderType { get; set; }
        public DateTime? birthDate { get; set; }
        public List<Email> emails { get; set; }
        public List<Phone> phones { get; set; }
    }
}
