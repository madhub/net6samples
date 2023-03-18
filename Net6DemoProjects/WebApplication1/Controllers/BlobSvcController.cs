using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobSvcController : ControllerBase
    {
        [HttpGet]
        [Route("{orgid}/{studyuid}/blob")]
        public ActionResult Get(string orgid,string studyuid,[FromQuery]SearchParameter searchParameter)

        {
            return Ok();
        }

        [HttpPost]
        [Route("{orgid}/{studyuid}/blob")]
        public ActionResult Create(string orgid, string studyuid, [FromBody] Dictionary<string,string> payload)

        {
            return Ok();
        }
        [HttpPost]
        [Route("{orgid}/{studyuid}/{seriesuid}/blob")]
        public ActionResult Create(string orgid, string studyuid,string seriesuid, [FromBody] Dictionary<string, string> payload)

        {
            return Ok();
        }
    }

   public record class SearchParameter(string identifier,string type);

}
