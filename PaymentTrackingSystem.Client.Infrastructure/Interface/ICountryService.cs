using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentTrackingSystem.Shared;

namespace PaymentTrackingSystem.Client.Infrastructure.Interface
{
    public interface ICountryService
    {
        Task<List<CountryViewModel>> GetCountries();
    }
}
