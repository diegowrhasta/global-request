namespace GlobalRequest.Services;

public static class HttpContextHelper
{
    private static IHttpContextAccessor _httpContextAccessor = null!;
    private static IWebHostEnvironment _webHostEnvironment = null!;

    public static void Configure(
        IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
    {
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }

    public static HttpContext Current => _httpContextAccessor.HttpContext!;

    public static string MapPath(string relativePath) =>
        Path.Combine(_webHostEnvironment.ContentRootPath, relativePath);
}