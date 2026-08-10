using AutoMapper;
using MultiShop.Catalog.DTOs.CategoryDTOs;
using MultiShop.Catalog.DTOs.FeatureSliderDTOs;
using MultiShop.Catalog.DTOs.FeatureDTOs;
using MultiShop.Catalog.DTOs.ProductDetailDTOs;
using MultiShop.Catalog.DTOs.ProductDTOs;
using MultiShop.Catalog.DTOs.ProductImageDTOs;
using MultiShop.Catalog.DTOs.SpecialOfferDTOs;
using MultiShop.Catalog.DTOs.OfferDiscountDTOs;
using MultiShop.Catalog.Entities;

namespace MultiShop.Catalog.Mapping
{
    public class CatalogMappingProfile:Profile
    {
        public CatalogMappingProfile() {
            //Category
            CreateMap<CreateCategoryDto, Category>()
                .ForMember(destination => destination.CategoryId, options => options.Ignore());
            CreateMap<UpdateCategoryDto, Category>();
            CreateMap<Category, ResultCategoryDto>();
            CreateMap<Category, GetByIdCategoryDto>();

            //Product 
            CreateMap<CreateProductDto,  Product>()
                .ForMember(destination => destination.ProductId, options => options.Ignore());
            CreateMap<UpdateProductDto, Product>();
            CreateMap<Product, GetByIdProductDto>();
            CreateMap<Product, ResultProductDto>();

            //FeatureSlider
            CreateMap<CreateFeatureSliderDto, FeatureSlider>()
                .ForMember(destination => destination.FeatureSliderId, options => options.Ignore());
            CreateMap<UpdateFeatureSliderDto, FeatureSlider>();
            CreateMap<FeatureSlider, ResultFeatureSliderDto>();
            CreateMap<FeatureSlider, GetByIdFeatureSliderDto>();

            //Feature
            CreateMap<CreateFeatureDto, Feature>()
                .ForMember(destination => destination.FeatureId, options => options.Ignore());
            CreateMap<UpdateFeatureDto, Feature>();
            CreateMap<Feature, ResultFeatureDto>();
            CreateMap<Feature, GetByIdFeatureDto>();

            //SpecialOffer
            CreateMap<CreateSpecialOfferDto, SpecialOffer>()
                .ForMember(destination => destination.SpecialOfferId, options => options.Ignore());
            CreateMap<UpdateSpecialOfferDto, SpecialOffer>();
            CreateMap<SpecialOffer, ResultSpecialOfferDto>();
            CreateMap<SpecialOffer, GetByIdSpecialOfferDto>();

            //OfferDiscount
            CreateMap<CreateOfferDiscountDto, OfferDiscount>()
                .ForMember(destination => destination.OfferDiscountId, options => options.Ignore());
            CreateMap<UpdateOfferDiscountDto, OfferDiscount>();
            CreateMap<OfferDiscount, ResultOfferDiscountDto>();
            CreateMap<OfferDiscount, GetByIdOfferDiscountDto>();

            //ProductDetail
            CreateMap<ProductDetailDto , ProductDetail>();
            CreateMap<ProductDetail, ProductDetailDto>();

            //ProductImage
            CreateMap<ProductImageDto, ProductImage>();
            CreateMap<ProductImage, ProductImageDto>();
        }
    }
}
