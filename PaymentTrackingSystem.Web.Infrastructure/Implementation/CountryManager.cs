using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentTrackingSystem.Core.Data.Models;
using PaymentTrackingSystem.Shared;
using PaymentTrackingSystem.Web.Infrastructure.Interface;

namespace PaymentTrackingSystem.Web.Infrastructure.Implementation
{
    public class CountryManager : ICountryManager
    {
        private readonly ILogger<CountryManager> logger;
        private PTSContext DbContext { get; set; }

        public CountryManager(PTSContext _DbContext, IMapper _mapper)
        {
            DbContext = _DbContext;     
        }
        public async Task<List<CountryViewModel>> GetCountries()
        {
            return await DbContext.Countries.Select(c => new CountryViewModel
            {
                CountryId = c.CountryId,
                CountryName = c.CountryName
            }).ToListAsync();

        }
    }
}
