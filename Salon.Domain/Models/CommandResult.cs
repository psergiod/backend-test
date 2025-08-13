using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Salon.Domain.Models
{
    public class CommandResult : ObjectResult
    {
        public bool Error { get; set; } = false;
        public List<string> Messages { get; set; }
        public CommandResult(object value)
            : base(value)
        { }
    }
}
