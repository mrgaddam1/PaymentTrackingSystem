using PaymentTrackingSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Web.Infrastructure.Interface
{
    public interface ICountryManager
    {
        Task<List<CountryViewModel>> GetCountries();   
    }
}
