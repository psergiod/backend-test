using Salon.Domain.Models.Enums;

namespace Salon.Domain.Base
{
    public class CommandBase
    {
        public Operation Operation { get; internal set; }
    }
}
