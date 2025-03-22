using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Patterns
{
    public class NormalFee : IPaymentStrategy
    {

        public double CalculateFee(int borrowedDays)
        {
			return borrowedDays * 1.0;
		}
    }
}
