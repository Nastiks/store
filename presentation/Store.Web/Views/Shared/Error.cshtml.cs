//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using System.Diagnostics;

//namespace Store.Web.Pages
//{
//    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//    [IgnoreAntiforgeryToken]
//    public class ErrorViewModel : PageModel
//    {
//        public string? RequestId { get; set; }

//        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);


//        public void OnGet()
//        {
//            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
//        }
//    }
//}