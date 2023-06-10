using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Dapper;

namespace BookSystem.Models
{
    public class BookService
    {
        private string GetDBConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
        }

        public List<Book>QueryBook(BookQueryArg arg)
        {
            var result = new List<Book>();
            using (SqlConnection conn = new SqlConnection(GetDBConnectionString()))
            {
                string sql = @"
                    Select 
	                    A.BOOK_ID As BookId,A.BOOK_NAME As BookName,
	                    A.BOOK_CLASS_ID As BookClassId,B.BOOK_CLASS_NAME As BookClassName,
	                    Convert(VarChar(10),A.BOOK_BOUGHT_DATE,120) As BookBoughtDate,
	                    A.BOOK_STATUS As BookStatusId,C.CODE_NAME As BookStatusName,
	                    A.BOOK_KEEPER As BookKeeperId,D.USER_CNAME As BookKeeperCname,D.USER_ENAME As BookKeeperEname,
	                    A.BOOK_AUTHOR As BookAuthor,A.BOOK_PUBLISHER As BookPublisher,A.BOOK_NOTE As BookNote
                    From BOOK_DATA As A
	                    Inner Join BOOK_CLASS As B On A.BOOK_CLASS_ID=B.BOOK_CLASS_ID
	                    Inner Join BOOK_CODE As C On A.BOOK_STATUS=C.CODE_ID And C.CODE_TYPE='BOOK_STATUS'
	                    Left Join MEMBER_M As D On A.BOOK_KEEPER=D.USER_ID
	                    Where (A.BOOK_ID=@BOOK_ID Or @BOOK_ID=0) And
                              (A.BOOK_NAME Like @BOOK_NAME Or @BOOK_NAME='') AND
	                          (A.BOOK_CLASS_ID=@BOOK_CLASS_ID Or @BOOK_CLASS_ID='') AND
		                      (A.BOOK_KEEPER=@BOOK_KEEPER Or @BOOK_KEEPER='') AND
		                      (A.BOOK_STATUS=@BOOK_STATUS Or @BOOK_STATUS='')";
                Dictionary<string, Object> parameter = new Dictionary<string, object>();
                parameter.Add("@BOOK_ID", arg.BookId);
                parameter.Add("@BOOK_NAME", arg.BookName!=null ? "%"+arg.BookName+"%" :string.Empty);
                parameter.Add("@BOOK_CLASS_ID", arg.BookClassId != null ?arg.BookClassId:string.Empty);
                parameter.Add("@BOOK_KEEPER", arg.BookKeeperId != null ? arg.BookKeeperId : string.Empty);
                parameter.Add("@BOOK_STATUS", arg.BookStatusId != null ? arg.BookStatusId : string.Empty);
                result = conn.Query<Book>(sql, parameter).ToList();
            }
            return result;
        }

        public void AddBook(Book book)
        {

            using (SqlConnection conn = new SqlConnection(GetDBConnectionString()))
            {
                string sql = @"
                Insert Into BOOK_DATA
                (
	                BOOK_NAME,BOOK_CLASS_ID,
	                BOOK_AUTHOR,BOOK_BOUGHT_DATE,
	                BOOK_PUBLISHER,BOOK_NOTE,
	                BOOK_STATUS,BOOK_KEEPER,
	                BOOK_AMOUNT,
	                CREATE_DATE,CREATE_USER,MODIFY_DATE,MODIFY_USER
                )
                Select 
	                @BOOK_NAME,@BOOK_CLASS_ID,
	                @BOOK_AUTHOR,@BOOK_BOUGHT_DATE,
	                @BOOK_PUBLISHER,@BOOK_NOTE,
	                @BOOK_STATUS,@BOOK_KEEPER,
	                0 As BOOK_AMOUNT,
	                GetDate() As CREATE_DATE,'Admin' As CREATE_USER,GetDate() As MODIFY_DATE,'Admin' As MODIFY_USER";

                Dictionary<string, Object> parameter = new Dictionary<string, object>();
                parameter.Add("@BOOK_NAME", book.BookName);
                parameter.Add("@BOOK_CLASS_ID", book.BookClassId);
                parameter.Add("@BOOK_AUTHOR", book.BookAuthor);
                parameter.Add("@BOOK_BOUGHT_DATE", book.BookBoughtDate);
                parameter.Add("@BOOK_PUBLISHER", book.BookPublisher);
                parameter.Add("@BOOK_NOTE", book.BookNote);
                parameter.Add("@BOOK_STATUS", "A");
                parameter.Add("@BOOK_KEEPER", book.BookKeeperId);

                conn.Execute(sql, parameter);
            }
        }

        public void UpdateBook(Book book)
        {
            using (SqlConnection conn = new SqlConnection(GetDBConnectionString()))
            {
                try
                {
                    string sql = @"
                    Update BOOK_DATA Set
	                    BOOK_NAME=@BOOK_NAME,BOOK_CLASS_ID=@BOOK_CLASS_ID,
	                    BOOK_AUTHOR=@BOOK_AUTHOR,
	                    BOOK_BOUGHT_DATE=@BOOK_BOUGHT_DATE,
	                    BOOK_PUBLISHER=@BOOK_PUBLISHER,BOOK_NOTE=@BOOK_NOTE,
	                    BOOK_STATUS=@BOOK_STATUS,BOOK_KEEPER=@BOOK_KEEPER,
	                    MODIFY_DATE=GetDate(),MODIFY_USER='Admin'
	                    Where BOOK_ID=@BOOK_ID";

                    Dictionary<string, Object> parameter = new Dictionary<string, object>();
                    parameter.Add("@BOOK_NAME", book.BookName);
                    parameter.Add("@BOOK_CLASS_ID", book.BookClassId);
                    parameter.Add("@BOOK_AUTHOR", book.BookAuthor);
                    parameter.Add("@BOOK_BOUGHT_DATE", book.BookBoughtDate);
                    parameter.Add("@BOOK_PUBLISHER", book.BookPublisher);
                    parameter.Add("@BOOK_NOTE", book.BookNote);
                    parameter.Add("@BOOK_STATUS", book.BookStatusId);
                    parameter.Add("@BOOK_KEEPER", book.BookKeeperId);
                    parameter.Add("BOOK_ID", book.BookId);

                    conn.Execute(sql, parameter);

                    if (book.BookStatusId == "B" || book.BookStatusId == "C")
                    {
                        sql = @"
                                Insert Into BOOK_LEND_RECORD
                                (
                                    BOOK_ID,KEEPER_ID,LEND_DATE,
                                    CRE_DATE,CRE_USR,MOD_DATE,MOD_USR
                                )
                                Select 
                                       @BOOK_ID As BOOK_ID,@KEEPER_ID As KEEPER_ID,
                                       @LEND_DATE As LEND_DATE, 
                                       GetDate() As CRE_DATE,'Admin' As CRE_USR,
                                       GetDate() As MOD_DATE,'Admin' As MOD_USR";
                        parameter.Clear();
                        parameter.Add("@BOOK_ID", book.BookId);
                        parameter.Add("@KEEPER_ID", book.BookKeeperId);
                        parameter.Add("@LEND_DATE", System.DateTime.Now.ToString("yyyy-MM-dd"));

                        conn.Execute(sql, parameter);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public void DeleteBookById(int bookId)
        {
            using (SqlConnection conn = new SqlConnection(GetDBConnectionString()))
            {
                string sql = @"Delete From BOOK_DATA Where BOOK_ID=@BOOK_ID";

                Dictionary<string, Object> parameter = new Dictionary<string, object>();
                parameter.Add("BOOK_ID", bookId);

                conn.Execute(sql, parameter);
            }
        }

        public List<BookLendRecord> GetBookLendRecordByBookId(int bookId)
        {
            var result = new List<BookLendRecord>();
            using (SqlConnection conn = new SqlConnection(GetDBConnectionString()))
            {
                string sql = @"
                    Select 
                        C.BOOK_NAME As BookName,
	                    A.BOOK_ID As BookId,
	                    A.KEEPER_ID As BookKeeperId,
	                    B.USER_CNAME As BookKeeperCname,B.USER_ENAME As BookKeeperEname,
	                    Convert(VarChar(10),A.LEND_DATE,120) As LendDate
	                    From BOOK_LEND_RECORD As A 
	                    Inner Join MEMBER_M As B On A.KEEPER_ID=B.USER_ID
                        Inner Join BOOK_DATA As C On A.BOOK_ID=C.BOOK_ID
	                    Where A.BOOK_ID=@BOOK_ID";
                Dictionary<string, Object> parameter = new Dictionary<string, object>();
                parameter.Add("@BOOK_ID", bookId);

                result = conn.Query<BookLendRecord>(sql, parameter).ToList();
            }
            return result;
        }

        public enum CheckBookIsDeleteableEnum
        {
            Lended=1,
            HaveLednRecord=2,
            CanDelete=3
        }
        public CheckBookIsDeleteableEnum CheckBookIsDeleteable(int bookId)
        {
            var book = QueryBook(new BookQueryArg() { BookId = bookId }).FirstOrDefault();
            if (book.BookStatusId == "B" || book.BookStatusId == "C")
            {
                return CheckBookIsDeleteableEnum.Lended;
            }

            var lendRecord = GetBookLendRecordByBookId(bookId);
            if (lendRecord.Count > 0)
            {
                return CheckBookIsDeleteableEnum.HaveLednRecord;
            }

            return CheckBookIsDeleteableEnum.CanDelete;
        }
    }
}