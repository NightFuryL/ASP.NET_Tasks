using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace AspNetAssignments.Filters;

public class OnlyAllowedControllersConvention : IApplicationModelConvention
{
    private readonly HashSet<string> _allowedControllers;

    public OnlyAllowedControllersConvention(params string[] allowedControllers)
    {
        _allowedControllers = new HashSet<string>(allowedControllers, StringComparer.OrdinalIgnoreCase);
    }

    public void Apply(ApplicationModel application)
    {
        // Якщо список дозволених контролерів порожній, то не фільтруємо нічого
        if (_allowedControllers.Count == 0)
        {
            return; 
        }

        // Якщо список дозволених контролерів не порожній, то видаляємо всі контролери, які не входять до цього списку
        var toRemove = application.Controllers
            .Where(c => !_allowedControllers.Contains(c.ControllerName))
            .ToList();

        foreach (var controller in toRemove)
        {
            application.Controllers.Remove(controller);
        }
    }
}
