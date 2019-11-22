
using System.ComponentModel.DataAnnotations;

namespace Ardalis.Sample.Core.DTOs
{ 
    public class ForecastRequestDto
    {
        [Required]
        public string PostalCode { get; set; }
    }
}