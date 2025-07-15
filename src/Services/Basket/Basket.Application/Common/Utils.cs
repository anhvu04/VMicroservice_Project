namespace Basket.Application.Common;

public static class Utils
{
    private const string Cart = "cart";

    public static string GetCartKey(Guid userId)
    {
        return Cart + ":" + userId;
    }
    
    
}