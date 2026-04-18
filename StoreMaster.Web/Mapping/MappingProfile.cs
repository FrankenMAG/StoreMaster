using AutoMapper;
using StoreMaster.Core.Entities;
using StoreMaster.Web.ViewModels;

namespace StoreMaster.Web.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile() 
        {
            // Categoria <=> CategoriaViewModel
            CreateMap<Categoria, CategoriaViewModel>()
                .ForMember(dest => dest.TotalProductos,
                    opt => opt.MapFrom(src => src.Productos.Count));

            CreateMap<CategoriaViewModel, Categoria>()
                .ForMember(dest => dest.Eliminado, Opt => Opt.Ignore())
                .ForMember(dest => dest.ModificadoEn, Opt => Opt.Ignore())
                .ForMember(dest => dest.Productos, Opt => Opt.Ignore());

            // Proveedor ↔ ProveedorViewModel
            CreateMap<Proveedor, ProveedorViewModel>()
                .ForMember(dest => dest.TotalProductos,
                    opt => opt.MapFrom(src => src.Productos.Count));

            CreateMap<ProveedorViewModel, Proveedor>()
                .ForMember(dest => dest.Eliminado, opt => opt.Ignore())
                .ForMember(dest => dest.ModificadoEn, opt => opt.Ignore())
                .ForMember(dest => dest.Productos, opt => opt.Ignore());

        }
    }
}
