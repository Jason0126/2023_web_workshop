
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BookSystem.Models;
namespace BookSystem.Controllers
{
    [RoutePrefix("api/code")]
    public class CodeController : ApiController
    {
        [Route("bookstatus")]
        [HttpPost()]
        public IHttpActionResult GetBookStatusData()
        {
            try
            {
                Models.CodeService codeService = new Models.CodeService();
                ApiResult<List<Code>> result = new ApiResult<List<Code>>()
                {
                    Data = codeService.GetBookStatusData(),
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

        [Route("bookclass")]
        [HttpPost()]
        public IHttpActionResult GetBookClassData()
        {
            try
            {
                Models.CodeService codeService = new Models.CodeService();
                ApiResult<List<Code>> result = new ApiResult<List<Code>>()
                {
                    Data = codeService.GetBookClassData(),
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
