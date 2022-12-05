using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace implement_with_redis.Models
{
    public class Country
    {
        public Name name { get; set; }
        public string[] capital { get; set; }
        public string region { get; set; }
        public string subregion { get; set; }
    }

    public class Name
    {
        public string common { get; set; }
        public string official { get; set; }
    }
}
