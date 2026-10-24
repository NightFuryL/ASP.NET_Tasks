namespace HW_19_09_to_26_09_2026.Models;

public class InsuranceViewModel
{
    public int EngineCapacity { get; set; }
    public string CityType { get; set; } = "Kyiv";
    public int DriverExperienceYears { get; set; }
    public bool HasDiscountCategory { get; set; }
    public decimal CalculatedPrice { get; set; }
    public string? ErrorMessage { get; set; }
}
