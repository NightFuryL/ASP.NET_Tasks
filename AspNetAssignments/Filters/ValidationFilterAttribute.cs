using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspNetAssignments.Filters;

/// <summary>
/// Фільтр для автоматичної асинхронної валідації аргументів дій контролера через FluentValidation
/// </summary>
public class ValidationFilterAttribute : ActionFilterAttribute
{
    /// <summary>
    /// Перехоплює виконання дії та валідує всі передані вхідні аргументи
    /// </summary>
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null) continue;

            var argumentType = argument.GetType();
            var validatorType = typeof(IValidator<>).MakeGenericType(argumentType);
            var validator = context.HttpContext.RequestServices.GetService(validatorType) as IValidator;

            if (validator != null)
            {
                var validationContext = new ValidationContext<object>(argument);
                var validationResult = await validator.ValidateAsync(validationContext);
                if (!validationResult.IsValid)
                {
                    context.Result = new BadRequestObjectResult(validationResult.ToDictionary());
                    return;
                }
            }
        }

        await next();
    }
}
