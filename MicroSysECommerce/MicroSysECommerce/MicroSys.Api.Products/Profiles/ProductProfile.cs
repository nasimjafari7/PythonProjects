using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroSys.Api.Products.Profiles
{
    public class ProductProfile:AutoMapper.Profile
    {
        public ProductProfile()
        {
            CreateMap<Db.Product, Model.Product>();
        }
    }
}
