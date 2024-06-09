using Application.Dtos;
using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Application.Mappers;

[Mapper]
public partial class OrderMapper
{
    public partial OrderResponseDto OrderToOrderResponseDto(Order order);
    
    public partial Order OrderRequestDtoToOrder(OrderRequestDto order);
}