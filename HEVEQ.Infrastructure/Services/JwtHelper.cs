using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Infrastructure.Services
{
    public class JwtHelper
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double DaurationInMinuts { get; set; }
    }
}
