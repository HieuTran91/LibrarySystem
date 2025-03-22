using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Patterns
{
    public class LateFee : IPaymentStrategy
    {

        public double CalculateFee(int borrowedDays)
        {
            return borrowedDays <= 7 ? borrowedDays * 1.0 : (7 * 1.0) + ((borrowedDays - 7) * 2.0);
		}
    }
}
