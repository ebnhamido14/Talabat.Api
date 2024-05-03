﻿using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                     .ForMember(D=>D.ProductType,O=>O.MapFrom(S=>S.ProductType.Name))
                     .ForMember(D=>D.ProductBrand,O=>O.MapFrom(S=>S.ProductBrand.Name))
                     .ForMember(D=>D.PictureUrl,O=>O.MapFrom<ProductPictureUrlResolver>());

            CreateMap<Address, AddressDto>();
        }
    }
}
