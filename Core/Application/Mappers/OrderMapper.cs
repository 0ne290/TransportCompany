using Application.Dtos;
using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Application.Mappers;

[Mapper]
public static partial class OrderMapper
{
    public static partial OrderResponseDto OrderToOrderResponseDto(Order order);
    
    public static partial Order OrderRequestDtoToOrder(OrderRequestDto order);
}