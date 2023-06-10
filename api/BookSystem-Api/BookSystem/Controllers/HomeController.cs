using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BookSystem.Models;

namespace BookSystem.Controllers
{
    [RoutePrefix("api/home")]
    public class HomeController : ApiController
    {
        [Route("GetColorCodeData")]
        [HttpPost()]
        public IHttpActionResult GetColorCodeData()
        {
            var result=new List<Code>
            {
                new Code()
                {
                    Text = "Red",
                    Value = "Red"
                },
                new Code()
                {
                    Text = "Orange",
                    Value = "Orange"
                },
                new Code()
                {
                    Text = "Yellow",
                    Value = "Yellow"
                }
                ,new Code()
                {
                    Text = "Green",
                    Value = "Green"
                }
                ,new Code()
                {
                    Text = "Blue",
                    Value = "Blue"
                }
                ,new Code()
                {
                    Text = "Indigo",
                    Value = "Indigo"
                }
                ,new Code()
                {
                    Text = "Purple",
                    Value = "Purple"
                }
                ,new Code()
                {
                    Text = "Black",
                    Value = "Black"
                }
                ,new Code()
                {
                    Text = "White",
                    Value = "White"
                }
            };
            return Ok(result);
        }

        [Route("SearchEmployee")]
        [HttpPost]
        public IHttpActionResult SearchEmployee([FromBody]string color)
        {
            var result = GetEmployeeSource()
                .Where(x => string.IsNullOrEmpty(color) || x.FavColor.Equals(color));
            return Ok(result);
        }
        private List<EmployeeData> GetEmployeeSource()
        {
            return new List<EmployeeData>
            {
                new EmployeeData
                {
                    Name = "Bob",
                    Age = 20,
                    FavColor = "Black"
                },
                new EmployeeData
                {
                    Name = "Alice",
                    Age = 18,
                    FavColor = "White"
                },
                new EmployeeData
                {
                    Name = "Tom",
                    Age = 16,
                    FavColor = "Black"
                },
                new EmployeeData
                {
                    Name = "Ryan",
                    Age = 24,
                    FavColor = "Red"
                }
            };
        }
    }
}
