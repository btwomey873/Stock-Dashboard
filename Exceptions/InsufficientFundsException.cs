using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thor.Domain.Exceptions
{
    public class InsufficientFundsException : Exception
    {
        public decimal? AccountBalance { get; set; }
        public decimal? RequiredBalance { get; set; }
        public InsufficientFundsException(decimal? accountBalance, decimal? requiredBalance)
        {
            AccountBalance = accountBalance;
            RequiredBalance = requiredBalance;

        }

        public InsufficientFundsException(decimal? accountBalance, decimal? requiredBalance, string message) : base(message)
        {
            AccountBalance = accountBalance;
            RequiredBalance = requiredBalance;

        }

        public InsufficientFundsException(decimal? accountBalance, decimal? requiredBalance, string message, Exception innerException) : base(message, innerException)
        {
            AccountBalance = accountBalance;
            RequiredBalance = requiredBalance;

        }

    }
}
