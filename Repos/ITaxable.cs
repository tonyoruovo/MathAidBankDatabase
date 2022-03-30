using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseTest.Repos
{
    public interface ITaxable
    {
        decimal VAT
        {
            get;
        }

        decimal Others
        {
            get;
        }
    }
}
