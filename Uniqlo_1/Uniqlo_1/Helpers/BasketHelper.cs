using System.Text.Json;
using Uniqlo_1.ViewModels.Baskets;

namespace Uniqlo_1.Helpers;

public class BasketHelper
{
    public static List<BasketCookieItemVM> GetBasket(HttpRequest request)
    {
        try
        {
            string? value = request.Cookies["basket"];
            if (value is null) return new();
            return JsonSerializer.Deserialize<List<BasketCookieItemVM>>(value) ?? new();
        }
        catch (Exception)
        {
            return new();
        }
    }
}
