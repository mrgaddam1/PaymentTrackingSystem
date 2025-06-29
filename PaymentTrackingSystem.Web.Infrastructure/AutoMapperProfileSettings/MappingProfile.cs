﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PaymentTrackingSystem.Core.Data.Models;
using PaymentTrackingSystem.Shared;

namespace PaymentTrackingSystem.Web.Infrastructure.AutoMapperProfileSettings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Client, ClientViewModel>();
            CreateMap<ClientAddress, ClientViewModel>();

            CreateMap<ClientPayment, ClientPaymentViewModel>();
            CreateMap<ClientPaymentViewModel, ClientPayment>();

            CreateMap<ClientInterestPayment, ClientPaymentInterestViewModel>();
            CreateMap<ClientPaymentInterestViewModel, ClientInterestPayment>();
        }
    }
}
