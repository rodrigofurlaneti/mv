using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.Models
{
    public class RegisterTerminalModel
    {
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public string Senha { get; set; }
        public string FacebookId { get; set; }
        public bool PrimeiroLogin { get; set; }
    }
}