namespace Application.Dtos;

public record UserRequestDto(string Login, string Password, string Name, string Contact, string? DefaultAddress);