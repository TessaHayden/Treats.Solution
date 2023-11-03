using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;

namespace FlavorsNTreats.Models
{
  public class Flavor
  {
    public int FlavorId { get; set; }
    [Required(ErrorMessage = "This field cannot be empty. Please enter a flavor")]
    public string Type { get; set; }
    [Range(1, int.MaxValue, ErrorMessage= "You must start by entering a treat flavor.")]
    public string Description { get; set; }
    public List<SweetNSavory> JoinEntities { get; }

  }
}