using MarioWebAPP.Datas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using MarioWebAppFinal.Datas;

namespace MarioWebAppFinal.Pages
{
    public class GP_CountModel : PageModel
    {
        private readonly IConfiguration Configuration;
        public GP_CountModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void OnGet()
        {
        }

        [HttpPost]
        public IActionResult OnPostGetData()
        {
            var conn = new DapperConnections.ConnectionOptions();
            Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);
            var sql = "select ModelGroup,a.ModelName,Watt,Price,Cost from ProductData as a left join ProductCost as b on a.ModelName=b.ModelName";
            using (var con = new SqlConnection(conn.RookieServerContext))
            {
                var result = con.Query<string>(sql).ToList();
                return new JsonResult(result);
            }
        }

        //public IActionResult OnPostGetCost()
        //{
        //    var conn = new DapperConnections.ConnectionOptions();
        //    Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);
        //    var sql = "select * from ProductCost";
        //    using (var con = new SqlConnection(conn.RookieServerContext))
        //    {
        //        var result = con.Query<string>(sql).ToList();
        //        return new JsonResult(result);
        //    }
        //}

    }
}
