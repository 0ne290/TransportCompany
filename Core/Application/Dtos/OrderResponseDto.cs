namespace Application.Dtos;

public record OrderResponseDto(string Guid, DateTime DateBegin, DateTime? DateEnd, string Address, decimal Price, decimal CargoVolume, decimal CargoWeight);