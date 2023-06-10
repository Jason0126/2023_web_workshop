using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BookSystem.Models;
namespace BookSystem.Controllers
{
    [RoutePrefix("api/bookmatain")]
    public class BookMatainController : ApiController
    {
        [Route("Showbook")]
        [HttpGet()]
        public IHttpActionResult TestShowBook(string bookId)
        {
            var result = new Models.Book();
            result.BookId = 130;
            result.BookName = "我國銀行實施課務別與利潤分析之研究";
            result.BookClassId = "BK";
            result.BookClassName = "Bakning";
            result.BookStatusId = "B";
            result.BookStatusName = "已借出";

            //throw new Exception("Some Thing Error");

            return Ok(result);
        }

        [HttpPost()]
        [Route("querybook")]
        public IHttpActionResult QueryBook(Models.BookQueryArg arg)
        {
            try
            {
                Models.BookService bookService = new Models.BookService();
                
                ApiResult<List<Book>> result = new ApiResult<List<Book>>
                {
                    Data = bookService.QueryBook(arg),
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

        [HttpPost()]
        [Route("loadbook")]
        public IHttpActionResult GetBookById([FromBody]int bookId)
        {
            try
            {
                Models.BookService bookService = new Models.BookService();
                ApiResult<Book> result = new ApiResult<Book>
                {
                    Data = bookService.QueryBook(new Models.BookQueryArg() { BookId = bookId }).FirstOrDefault(),
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

        [HttpPost()]
        [Route("addbook")]
        public IHttpActionResult AddBook(Models.Book book)
        {
            try
            {
                ModelState.Remove("book.BookStatusId");
                ModelState.Remove("book.BookKeeperId");
                ModelState.Remove("book.BookId");
                if (ModelState.IsValid)
                {
                    Models.BookService bookService = new Models.BookService();
                    bookService.AddBook(book);
                    return Ok(
                        new ApiResult<string>()
                        {
                            Data = string.Empty,
                            Status = true,
                            Message = string.Empty
                        });
                }
                else
                {
                    return BadRequest(ModelState);
                }
                
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [HttpPost()]
        [Route("updatebook")]
        public IHttpActionResult UpdateBook(Models.Book book)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Models.BookService bookService = new Models.BookService();
                    bookService.UpdateBook(book);
                    return Ok(new ApiResult<string>()
                    {
                        Data = string.Empty,
                        Status = true,
                        Message = string.Empty
                    }
                     );
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [HttpPost()]
        [Route("deletebook")]
        public IHttpActionResult DeleteBooById([FromBody] int bookId)
        {
            try
            {
                Models.BookService bookService = new Models.BookService();
                var bookDeleteStatus = bookService.CheckBookIsDeleteable(bookId);
                ApiResult<string> result = new ApiResult<string>
                {
                    Data = string.Empty,
                    Status = true,
                    Message = string.Empty
                };

                switch (bookDeleteStatus)
                {
                    case BookService.CheckBookIsDeleteableEnum.Lended:
                        result.Status = false;
                        result.Message = "該書已借出不可刪除";
                        break;
                    case BookService.CheckBookIsDeleteableEnum.HaveLednRecord:
                        result.Status = false;
                        result.Message = "該書已有借閱紀錄不可刪除";
                        break;
                    case BookService.CheckBookIsDeleteableEnum.CanDelete:
                        bookService.DeleteBookById(bookId);
                        break;
                    default:
                        break;
                }
                
                return Ok(result);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [HttpPost()]
        [Route("booklendrecord")]
        public IHttpActionResult GetBookLendRecordByBookId([FromBody] int bookId)
        {
            try
            {
                Models.BookService bookService = new Models.BookService();
                ApiResult<List<BookLendRecord>> result = new ApiResult<List<BookLendRecord>>()
                {
                    Data = bookService.GetBookLendRecordByBookId(bookId),
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
