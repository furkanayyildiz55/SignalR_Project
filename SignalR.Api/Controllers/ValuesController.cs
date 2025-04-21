using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.Api.Hubs;

namespace SignalR.Api.Controllers
{

    //.Net 8 ile gelen primary constructor ile myHub di olarak alınmıştır
    [ApiController]
    [Route("[controller]")]
    public class ValuesController(IHubContext<MyHub> myHub) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Get(string message)
        {
            //Api içerisinde olduğumuz için, clients metodalrının hepsi bu alanda desteklenmez
            //Örneğin sadece caller client veri gönderimi yapılamaz 

            //Web uygulamasında olduğu gibi MyHub sınıfındaki metotlar client üzerinden çağırılabilir VEYA 
            //Buradaki gibi api isteği tetiklendiği durumda, client taradınfaki bir metot manuel olarak tetiklenebilir

            myHub.Clients.All.SendAsync("ReceiveMessageForAllClient", message);


            return View();
        }
    }
}
