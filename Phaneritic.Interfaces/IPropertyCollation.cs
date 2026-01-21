using System;
using System.Collections.Generic;
using System.Text;

namespace Phaneritic.Interfaces;

public interface IPropertyCollation : IPropertyConfigurator
{
    bool IsCaseSensitive { get; }
    string Collation { get; set; }
}
