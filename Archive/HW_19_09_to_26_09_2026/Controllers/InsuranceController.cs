using Microsoft.AspNetCore.Mvc;
using HW_19_09_to_26_09_2026.Models;

namespace HW_19_09_to_26_09_2026.Controllers;

public class InsuranceController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var model = new InsuranceViewModel
        {
            EngineCapacity = 1600,
            CityType = "Kyiv",
            DriverExperienceYears = 3,
            HasDiscountCategory = false
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult Index(InsuranceViewModel model)
    {
        // Валідація: якщо об'єм двигуна менше або дорівнює 0, або стаж менше 0, записати помилку в ErrorMessage.
        if (model.EngineCapacity <= 0)
        {
            model.ErrorMessage = "Об'єм двигуна повинен бути більше 0 см³.";
            return View(model);
        }

        if (model.DriverExperienceYears < 0)
        {
            model.ErrorMessage = "Стаж водіння не може бути від'ємним.";
            return View(model);
        }

        model.ErrorMessage = null;

        // Базова ставка = 1000 грн.
        decimal basePrice = 1000m;

        // Коефіцієнт двигуна:
        // до 1600 см³ включно (помножити на 1.0), від 1601 до 2000 см³ (помножити на 1.3), понад 2000 см³ (помножити на 1.6).
        decimal engineCoefficient;
        if (model.EngineCapacity <= 1600)
        {
            engineCoefficient = 1.0m;
        }
        else if (model.EngineCapacity <= 2000)
        {
            engineCoefficient = 1.3m;
        }
        else
        {
            engineCoefficient = 1.6m;
        }

        // Коефіцієнт міста:
        // Київ (помножити на 1.8), Велике місто (помножити на 1.3), Інше (помножити на 1.0).
        decimal cityCoefficient;
        switch (model.CityType)
        {
            case "Kyiv":
                cityCoefficient = 1.8m;
                break;
            case "LargeCity":
                cityCoefficient = 1.3m;
                break;
            default:
                cityCoefficient = 1.0m;
                break;
        }

        decimal price = basePrice * engineCoefficient * cityCoefficient;

        // Знижка за стаж понад 3 роки = мінус 10%.
        if (model.DriverExperienceYears > 3)
        {
            price *= 0.9m;
        }

        // Знижка за пільги (HasDiscountCategory дорівнює true) = мінус 20%.
        if (model.HasDiscountCategory)
        {
            price *= 0.8m;
        }

        model.CalculatedPrice = Math.Round(price, 2);

        return View(model);
    }
}
