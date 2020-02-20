using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clube.Premiar.Models
{
    [Serializable]
    public class Participant : Person
    {
        public Participant() : base()
        {
            address = new Address();
        }
        
        public int clientId { get; set; }
        public int catalogId { get; set; }
        public int profileId { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public Address address { get; set; }
    }
}
