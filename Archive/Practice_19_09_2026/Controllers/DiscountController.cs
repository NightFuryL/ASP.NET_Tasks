using Microsoft.AspNetCore.Mvc;
using Practice_19_09_2026.Models;

namespace Practice_19_09_2026.Controllers;

public class DiscountController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View(new DiscountViewModel());
    }

    [HttpPost]
    public IActionResult Index(DiscountViewModel model)
    {
        if (model.Price < 0 || model.DiscountPercent < 0 || model.DiscountPercent > 100)
        {
            model.Message = "Будь ласка, введіть коректну ціну (>= 0) та знижку (0 - 100%).";
            return View(model);
        }

        decimal discountAmount = model.Price * model.DiscountPercent / 100m;
        model.FinalPrice = model.Price - discountAmount;
        model.Message = $"Ваша економія склала {discountAmount:F2} грн";

        return View(model);
    }
}
