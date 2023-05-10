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
    public class GPcountModel : PageModel
    {
        private readonly IConfiguration Configuration;
        public GPcountModel(IConfiguration configuration)
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
                var result = con.Query(sql).ToList();
                return new JsonResult(result);
            }
        }

        public IActionResult OnPostGetPrice(IFormCollection collection)
        {
            var modelName= collection["modelName"].ToString();
            var conn = new DapperConnections.ConnectionOptions();
            Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);
            var sql = "select Price from ProductData where ModelName=@name";
            using (var con = new SqlConnection(conn.RookieServerContext))
            {
                var result = con.Query<string>(sql, new
                {
                    name=modelName
                }).ToList();
                return new JsonResult(result);
            }
        }

    }
}
