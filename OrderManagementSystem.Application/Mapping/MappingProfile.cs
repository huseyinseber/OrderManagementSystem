//using AutoMapper;
//using OrderManagementSystem.Application.DTOs;
//using OrderManagementSystem.Domain.Entities;

//namespace OrderManagementSystem.Application.Mapping
//{
//    public class MappingProfile : Profile
//    {
//        public MappingProfile()
//        {
//            // Order mappings
//            CreateMap<Order, OrderDto>().ReverseMap();
//            CreateMap<CreateOrderDto, Order>();
//            CreateMap<OrderDetail, OrderDetailDto>()
//                .ForMember(dest => dest.StockName, opt => opt.MapFrom(src => src.Stock.StockName))
//                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Stock.Price));
//            CreateMap<CreateOrderDetailDto, OrderDetail>();

//            // Customer mappings
//            CreateMap<Customer, CustomerDto>()
//                .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.CustomerAddresses));
//            CreateMap<CustomerAddress, CustomerAddressDto>();
//            CreateMap<CustomerDto, Customer>();
//            CreateMap<CustomerAddressDto, CustomerAddress>();


//            CreateMap<Stock, StockDto>().ReverseMap();

//        }
//    }
//}


using AutoMapper;
using OrderManagementSystem.Application.DTOs;
using OrderManagementSystem.Domain.Entities;

namespace OrderManagementSystem.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Order mappings
            CreateMap<Order, OrderDto>().ReverseMap()
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.DeliveryAddress, opt => opt.Ignore())
                .ForMember(dest => dest.InvoiceAddress, opt => opt.Ignore())
                .ForMember(dest => dest.OrderDetails, opt => opt.Ignore());

            CreateMap<CreateOrderDto, Order>()
             .ForMember(dest => dest.Customer, opt => opt.Ignore())
             .ForMember(dest => dest.DeliveryAddress, opt => opt.Ignore())
             .ForMember(dest => dest.InvoiceAddress, opt => opt.Ignore())
             .ForMember(dest => dest.OrderDetails, opt => opt.Ignore()); // Bu satır önemli!

            // OrderDetail mappings - ÖNEMLİ: ReverseMap ekle ve ignore'ları ayarla
            CreateMap<OrderDetail, OrderDetailDto>()
                .ForMember(dest => dest.StockName, opt => opt.MapFrom(src => src.Stock.StockName))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Stock.Price))
                .ReverseMap()
                .ForMember(dest => dest.Stock, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore()); // IsActive'i ignore et

            CreateMap<CreateOrderDetailDto, OrderDetail>()
                .ForMember(dest => dest.Stock, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true)); // Yeni detaylar için true

            // Customer mappings
            CreateMap<Customer, CustomerDto>()
                .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.CustomerAddresses));
            CreateMap<CustomerAddress, CustomerAddressDto>();
            CreateMap<CustomerDto, Customer>();
            CreateMap<CustomerAddressDto, CustomerAddress>();

            CreateMap<Stock, StockDto>().ReverseMap();
        }
    }
}