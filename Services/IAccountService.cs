using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thor.Domain.Models;

namespace Thor.Domain.Services
{
    public interface IAccountService
    {
        Task<UserBalance> GetBalance();

        Task<List<OwnedStock>> GetCurrentOwnedStocks();

    }
}
