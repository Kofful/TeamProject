using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;

namespace STOServer
{
    public class MyDataContext : DataContext
    {
        public MyDataContext(string connectionString)
            : base(connectionString)
        {

        }
        [Function(Name = "CheckLogin")]
        public int CheckLogin([Parameter(Name = "login", DbType = "NVarChar(100))")] string login)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), login);
            return ((int)(result.ReturnValue));
        }
        [Function(Name = "CheckPassword")]
        public int CheckPassword([Parameter(Name = "login", DbType = "NVarChar(100))")] string login,
            [Parameter(Name = "password", DbType = "NVarChar(100)")] string password)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), login, password);
            return ((int)(result.ReturnValue));
        }
        [Function(Name = "Getuserinfo")]
        public int GetUserInfo([Parameter(Name = "id", DbType = "Int")] int id,
           [Parameter(Name = "name", DbType = "NVarChar(50)")] ref string name,
           [Parameter(Name ="email",DbType ="NVarChar(100)")] ref string email,

        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), login, );
            return ((int)(result.ReturnValue));
        }
    }
}
