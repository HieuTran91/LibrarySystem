using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Patterns
{
    public interface IPaymentStrategy
    {
        double CalculateFee(int borrowedDays);
    }
}
