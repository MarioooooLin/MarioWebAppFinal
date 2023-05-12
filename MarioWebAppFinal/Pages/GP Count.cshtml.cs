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
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
            var sql = "select ModelName from ProductData";
            using (var con = new SqlConnection(conn.RookieServerContext))
            {
                var result = con.Query<string>(sql).ToList();
                return new JsonResult(result);
            }
        }
        public IActionResult OnPostGetNewPrice(IFormCollection collection)
        {
            var conn = new DapperConnections.ConnectionOptions();
            Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);
            var sql = @"select Price from ProductData where ModelName=@name";
            var name = collection["Name"].ToString();
           // var results=new Dictionary<string, object>();
            using (var con = new SqlConnection(conn.RookieServerContext))
            {
                var result = con.Query<string>(sql, new
                {
                    name = name
                });
                //results.Add("price", result);
                return new JsonResult(result);
            }
           

        }

        public IActionResult OnPostGetPriceCost(IFormCollection collection)
        {
            var modelName = collection["modelName"].ToString();
            var conn = new DapperConnections.ConnectionOptions();
            Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);
            var sql = @"select Price,Cost from ProductData as a left join ProductCost as b on a.ModelName=b.ModelName where a.ModelName=@name";
            using (var con = new SqlConnection(conn.RookieServerContext))
            {
                var result = con.Query<Product>(sql, new
                {
                    name = modelName
                });
                return new JsonResult(result);
            }
        }

        public IActionResult OnPostGetModal(IFormCollection collection)
        {
            var product = collection["Product"].ToString();
            var conn = new DapperConnections.ConnectionOptions();
            Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);
            var sql = @"select * from ProductData where ModelName=@name";
            using (var con = new SqlConnection(conn.RookieServerContext))
            {
                var result = con.Query<Product>(sql, new
                {
                    name = product
                }).ToList();
                return new JsonResult(result);
            }
        }

        public async Task<IActionResult> OnPostSetPrice(IFormCollection collection)
        {
            var result=new Dictionary<string, object>();
            var price = collection["newPrice"].ToString();
            var modelName = collection["modelName"].ToString();
            var sql = @"update [ProductData] 
                    set Price=@price 
                    where ModelName=@modelName";
            try
            {
                var conn = new DapperConnections.ConnectionOptions();
                Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);
                using (var con = new SqlConnection(conn.RookieServerContext))
                {
                    var results = await con.ExecuteAsync(sql, new
                    {
                        price = price,
                        modelName = modelName
                    });
                }
                result.Add("result", "1");
            }catch (Exception ex) 
            { var msg=ex.Message;
                result.Add("result", "0");
            }

            return new JsonResult(result);
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
