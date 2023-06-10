using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BookSystem.Models;
namespace BookSystem.Controllers
{
    [RoutePrefix("api/member")]
    public class MemberController : ApiController
    {
        [Route("basicinfo")]
        [HttpPost()]
        public IHttpActionResult GetMemberData()
        {
            try
            {
                Models.CodeService codeService = new Models.CodeService();
                ApiResult<List<Member>> result = new ApiResult<List<Member>>()
                {
                    Data = codeService.GetMemberData(),
                    Status = true,
                    Message = string.Empty
                };
                return Ok(result);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            
        }
    }
}
