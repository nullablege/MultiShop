using AutoMapper;
using MultiShop.Catalog.DTOs.CategoryDTOs;
using MultiShop.Catalog.DTOs.ProductDetailDTOs;
using MultiShop.Catalog.DTOs.ProductDTOs;
using MultiShop.Catalog.DTOs.ProductImageDTOs;
using MultiShop.Catalog.Entities;

namespace MultiShop.Catalog.Mapping
{
    public class CatalogMappingProfile:Profile
    {
        public CatalogMappingProfile() {
            //Category
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();
            CreateMap<Category, ResultCategoryDto>();
            CreateMap<Category, GetByIdCategoryDto>();

            //Product 
            CreateMap<CreateProductDto,  Product>();
            CreateMap<UpdateProductDto, Product>();
            CreateMap<Product, GetByIdProductDto>();
            CreateMap<Product, ResultProductDto>();

            //ProductDetail
            CreateMap<ProductDetailDto , ProductDetail>();
            CreateMap<ProductDetail, ProductDetailDto>();

            //ProductImage
            CreateMap<ProductImageDto, ProductImage>();
            CreateMap<ProductImage, ProductImageDto>();
        }
    }
}
