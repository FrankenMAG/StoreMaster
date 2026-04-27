using AutoMapper;
using StoreMaster.Core.Entities;
using StoreMaster.Web.ViewModels;

namespace StoreMaster.Web.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile() 
        {
            // Categoria  CategoriaViewModel
            CreateMap<Categoria, CategoriaViewModel>()
                .ForMember(dest => dest.TotalProductos,
                    opt => opt.MapFrom(src => src.Productos.Count));

            CreateMap<CategoriaViewModel, Categoria>()
                .ForMember(dest => dest.Eliminado, Opt => Opt.Ignore())
                .ForMember(dest => dest.ModificadoEn, Opt => Opt.Ignore())
                .ForMember(dest => dest.Productos, Opt => Opt.Ignore());

            // Proveedor  ProveedorViewModel
            CreateMap<Proveedor, ProveedorViewModel>()
                .ForMember(dest => dest.TotalProductos,
                    opt => opt.MapFrom(src => src.Productos.Count));
            // Producto → ProductoViewModel
            CreateMap<Producto, ProductoViewModel>()
                .ForMember(dest => dest.CategoriaNombre,
                    opt => opt.MapFrom(src => src.Categoria != null ? src.Categoria.Nombre : ""))
                .ForMember(dest => dest.ProveedorNombre,
                    opt => opt.MapFrom(src => src.Proveedor != null ? src.Proveedor.Nombre : ""))
                .ForMember(dest => dest.Categorias, opt => opt.Ignore())
                .ForMember(dest => dest.Proveedores, opt => opt.Ignore());

            // ProductoViewModel  Producto
            CreateMap<ProductoViewModel, Producto>()
                .ForMember(dest => dest.Eliminado, opt => opt.Ignore())
                .ForMember(dest => dest.ModificadoEn, opt => opt.Ignore())
                .ForMember(dest => dest.Categoria, opt => opt.Ignore())
                .ForMember(dest => dest.Proveedor, opt => opt.Ignore())
                .ForMember(dest => dest.DetallesVenta, opt => opt.Ignore());

            CreateMap<ProveedorViewModel, Proveedor>()
                .ForMember(dest => dest.Eliminado, opt => opt.Ignore())
                .ForMember(dest => dest.ModificadoEn, opt => opt.Ignore())
                .ForMember(dest => dest.Productos, opt => opt.Ignore());

            // Cliente  ClienteViewModel
            CreateMap<Cliente, ClienteViewModel>()
                .ForMember(dest => dest.TotalCompras,
                    opt => opt.MapFrom(src => src.Ventas.Count));

            CreateMap<ClienteViewModel, Cliente>()
                .ForMember(dest => dest.Eliminado, opt => opt.Ignore())
                .ForMember(dest => dest.ModificadoEn, opt => opt.Ignore())
                .ForMember(dest => dest.Ventas, opt => opt.Ignore());

            // Factura  FacturaViewModel
            CreateMap<Factura, FacturaViewModel>()
                .ForMember(dest => dest.Conceptos,
                    opt => opt.MapFrom(src => src.Conceptos));

            CreateMap<FacturaConcepto, FacturaConceptoViewModel>();

        }
    }
}
