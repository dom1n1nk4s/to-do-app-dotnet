using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace to_do_app_dotnet.Models
{
    [ExcludeFromCodeCoverageAttribute]
    public class Entry
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }
    }
}