using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;

namespace FlavorsNTreats.Models
{
  public class Treat
  {
    public int TreatId { get; set; }
    [Required(ErrorMessage = "This field is required. Please enter a treat.")]
    public string Name { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "You must start by entering a treat.")]
    public List<SweetNSavory> JoinEntities { get; }
  }
}