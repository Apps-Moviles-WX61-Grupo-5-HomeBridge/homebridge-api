using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using _3_Shared.Domain.Models;

namespace _2_Domain.Publication.Models.Entities;

[ComplexType]
public class LocationModel
{
    [Required]
    [StringLength(
        (int) LocationConstraints.MaxAddressStringLength, 
        MinimumLength = (int) LocationConstraints.MinAddressStringLength, 
        ErrorMessage = "'Address' must have a valid string length range."
    )]
    public string Address { get; set; }
}