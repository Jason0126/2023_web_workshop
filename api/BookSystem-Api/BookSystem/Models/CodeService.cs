using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Dapper;

namespace BookSystem.Models
{
    public class CodeService
    {
        private string GetDBConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
        }
        public List<Code> GetBookStatusData()
        {
            var result = new List<Code>();
            using (SqlConnection conn = new SqlConnection(GetDBConnectionString()))
            {
                string sql = "Select CODE_ID As Value,CODE_NAME As Text From BOOK_CODE Where CODE_TYPE=@CODE_TYPE";
                Dictionary<string, Object> parameter = new Dictionary<string, object>();
                parameter.Add("@CODE_TYPE", "BOOK_STATUS");
                result = conn.Query<Code>(sql,parameter).ToList();
            }
            return result;
        }

        public List<Code> GetBookClassData()
        {
            var result = new List<Code>();
            using (SqlConnection conn = new SqlConnection(GetDBConnectionString()))
            {
                string sql = "Select BOOK_CLASS_ID As Value,BOOK_CLASS_NAME As Text From BOOK_CLASS";
                result = conn.Query<Code>(sql).ToList();
            }
            return result;
        }

        public List<Member> GetMemberData()
        {
            var result = new List<Member>();
            using (SqlConnection conn = new SqlConnection(GetDBConnectionString()))
            {
                string sql = "Select USER_ID As UserId,USER_CNAME As UserCname,USER_ENAME As UserEname From MEMBER_M";
                result = conn.Query<Member>(sql).ToList();
            }
            return result;
        }
    }
}