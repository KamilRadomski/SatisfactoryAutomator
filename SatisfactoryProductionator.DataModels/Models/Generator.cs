namespace SatisfactoryProductionator.DataModels.Models
{
	public class Generator : Building
	{
		public double PowerGenerated { get; set; } //generators
		public List<Fuel>? FuelUsed { get; set; }
	}
}
