namespace Shared.Utils.Errors;

public static class CommonErrors
{
    public static string NotFound<T>() where T : class
    {
        return $"{typeof(T).Name} not found";
    }

    public static string AlreadyExists<T>() where T : class
    {
        return $"{typeof(T).Name} already exists";
    }
}