using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Data.SqlClient;

namespace SQLHelperDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        SQLHelper _sqlHelper;
        public ValuesController(SQLHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<DataTable> Get()
        {
            return _sqlHelper.ExecuteDataTable("SELECT * FROM SYSUSER", null);
        }


        // GET api/update
        [HttpGet("update")]
        public ActionResult<int> Update()
        {
            SqlParameter[] cmdParms = new SqlParameter[] { new SqlParameter("@PASSWORD", DBNull.Value) };
            return _sqlHelper.ExecuteNonQuery("UPDATE SYSUSER SET PASSWORD =@PASSWORD WHERE  LOGINNAME='admin'", cmdParms);
        }

        // GET api/update
        [HttpGet("UpdateTransaction")]
        public ActionResult<int> UpdateTransaction()
        {
            int rowsAffected = 0;
            SqlParameter[] cmdParms = new SqlParameter[] { new SqlParameter("@PASSWORD", "123456") };
            using (var conn = _sqlHelper.GetDefaultSqlConnection())
            {
                conn.Open();
                var transaction = conn.BeginTransaction();
                try
                {
                    rowsAffected = _sqlHelper.ExecuteNonQuery(conn, "UPDATE SYSUSER SET PASSWORD =@PASSWORD WHERE  LOGINNAME='admin'", cmdParms, transaction);
                    throw new Exception("test");
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }

            }
            return rowsAffected;
        }

    }
}
