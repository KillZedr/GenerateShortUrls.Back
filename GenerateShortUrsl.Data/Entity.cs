using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateShortUrsl.Data
{
    public class Entity<TIdentifier> : IEntity
    {
        public TIdentifier? Identifier { get; set; }
    }
}
