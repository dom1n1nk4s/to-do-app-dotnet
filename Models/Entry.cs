using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace to_do_app_dotnet.Models
{
    public class Entry
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }
    }
}