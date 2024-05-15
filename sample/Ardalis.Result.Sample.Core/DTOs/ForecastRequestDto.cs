
using System;
using System.ComponentModel.DataAnnotations;

namespace Ardalis.Result.Sample.Core.DTOs
{ 
    public class ForecastRequestDto
    {
        [Required]
        public string PostalCode { get; set; } = String.Empty;
    }
}