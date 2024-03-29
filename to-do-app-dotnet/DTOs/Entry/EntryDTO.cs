using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace to_do_app_dotnet.DTOs.Entry
{
    [ExcludeFromCodeCoverageAttribute]
    public class EntryDTO
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
    }
}