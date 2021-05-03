using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroSys.Api.Customers.Profile
{
    public class CustomerProfile: AutoMapper.Profile
    {
        public CustomerProfile()
        {
            CreateMap<Db.Customer, Model.Customer>();
        }
    }
}
