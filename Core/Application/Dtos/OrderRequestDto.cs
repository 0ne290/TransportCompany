namespace Application.Dtos;

public record OrderRequestDto(string Address, decimal LengthInKilometers, decimal ClassAdr, decimal CargoVolume, decimal CargoWeight, string UserLogin);