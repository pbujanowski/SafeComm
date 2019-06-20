using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SafeComm.Shared.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IPAddress Address { get; set; }
        public string AddressText { get { return Address.ToString(); } }
        public int Port { get; set; }
    }
}
