namespace SatisfactoryProductionator.DataModels.Models.Old
{
    public class Generator_legacy : Building_legacy
    {
        public double PowerGenerated { get; set; }
        public List<Fuel> FuelUsed { get; set; }
    }
}
