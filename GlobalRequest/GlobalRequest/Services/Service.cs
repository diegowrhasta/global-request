namespace GlobalRequest.Services;

public class Service : IService
{
    public bool FilesArePresent()
    {
        return HttpContextHelper.Current.Request.Form.Files.Any();
    }
}