namespace Basket.Services;

public static class Utils
{
    private const string Cart = "cart";

    public static string GetCartKey(Guid userId)
    {
        return Cart + ":" + userId;
    }
}