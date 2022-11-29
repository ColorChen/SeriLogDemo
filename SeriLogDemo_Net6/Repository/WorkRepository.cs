using Dapper;
// using Microsoft.Data.SqlClient;
using SeriLogDemo_Net6.Model;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using Dapper.Extensions.MSSQL;
using Dapper.Extensions;
using Serilog;

namespace SeriLogDemo_Net6.Repository
{
    public class WorkRepository
    {
        readonly IDiagnosticContext _diagnosticContext;
        public WorkRepository(string connectString, IDiagnosticContext diagnosticContext) {
            _connectString = connectString;
            _diagnosticContext = diagnosticContext;
        }

        

        /// <summary>
        /// 連線字串
        /// </summary>
        // private readonly string _connectString = @"Server=(LocalDB)\MSSQLLocalDB;Database=Newbie;Trusted_Connection=True;";
        // private readonly string _connectString = SeriLogDemo_Net6.SeriLogDemoConfig.ConnectionStrings.DefaultConnection;
        private readonly string _connectString;

        /// <summary>
        /// 查詢卡片列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Work> GetList()
        {
            var aSql = "SELECT * FROM Work";
            using (var conn = new SqlConnection(_connectString)) {
                var result = conn.Query<Work>(
                sql: aSql
                );
                _diagnosticContext.Set("sql execute", new { sql = aSql, result = result });
                return result;
            }
            
            //return  conn.Query<Work>(
            //    sql: aSql
            //    );


            
        }

        //public IEnumerable<Work> GetListDapperExtension(bool useCache = false)
        //{
        //    return _Repo.Query<Work>(
        //        sql:"SELECT * FROM Work;",
        //        commandType:CommandType.Text,
        //        enableCache:useCache,
        //        cacheKey: "GetListDapperExtension",
        //        cacheExpire:TimeSpan.FromSeconds(10)
        //        );
        //}

        /// <summary>
        /// 查詢卡片
        /// </summary>
        /// <returns></returns>
        public Work Get(int id)
        {
            using (var conn = new SqlConnection(_connectString))
            {
                var result = conn.QueryFirstOrDefault<Work>(
                    "SELECT TOP 1 * FROM Work Where Id = @id",
                    new
                    {
                        Id = id,
                    });
                return result;
            }
        }

        /// <summary>
        /// 新增卡片
        /// </summary>
        /// <param name="parameter">參數</param>
        /// <returns></returns>
        public int Create(WorkParameter parameter)
        {
            var sql =
            @"
        INSERT INTO Work 
        (
            [Name]
            ,[UpdatedTime]  
            ,[CreatedDateTime]
        ) 
        VALUES 
        (
            @Name
           ,@CreatedDateTime
           ,@UpdatedTime
        );
        
        SELECT @@IDENTITY;
    ";

            using (var conn = new SqlConnection(_connectString))
            {
                var result = conn.QueryFirstOrDefault<int>(sql, parameter);
                return result;
            }
        }

        /// <summary>
        /// 修改卡片
        /// </summary>
        /// <param name="id">卡片編號</param>
        /// <param name="parameter">參數</param>
        /// <returns></returns>
        public bool Update(int id, WorkParameter parameter)
        {
            var sql =
            @"
        UPDATE Work
        SET 
             [Name] = @Name
            ,[UpdatedTime] = @UpdatedTime    
            ,[CreatedDateTime] = @CreatedDateTime
        WHERE 
            Id = @id
    ";

            var parameters = new DynamicParameters(parameter);
            parameters.Add("Id", id, System.Data.DbType.Int32);

            using (var conn = new SqlConnection(_connectString))
            {
                var result = conn.Execute(sql, parameters);
                return result > 0;
            }
        }

        /// <summary>
        /// 刪除卡片
        /// </summary>
        /// <param name="id">卡片編號</param>
        /// <returns></returns>
        public void Delete(int id)
        {
            var sql =
            @"
        DELETE FROM Work
        WHERE Id = @Id
    ";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, System.Data.DbType.Int32);

            using (var conn = new SqlConnection(_connectString))
            {
                var result = conn.Execute(sql, parameters);
            }
        }
    }

    /// <summary>
    /// 資料表新增/更新參數
    /// </summary>
    public class WorkParameter
    {
        public string Name { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
