namespace FlavorsNTreats.Models
{
  public class SweetNSavory
  {
    public int SweetNSavoryId { get; set; }
    public int FlavorId { get; set; }
    public int TreatId { get; set; }
    public Flavor Flavor { get; set; }
    public Treat Treat { get; set; }
  }
}