namespace DotnetSWF
{
    public class TestController : Controller
    {
        [RouteMethod("/Home")]
        public IHttpResponseResult HomePage()
        {
            var result = HttpResponse.OK;
            result.Content += "This is home page";
            return result;
        }
    }
}