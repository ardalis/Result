using System;

namespace Ardalis.Result.Sample.Core.DTOs;

public class CreatePersonRequestDto
{
    public string FirstName { get; set; } = String.Empty; 
    public string LastName { get; set; } = String.Empty;  
}