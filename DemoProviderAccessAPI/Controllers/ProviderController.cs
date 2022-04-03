using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DemoProviderAccessAPI.Entitiy;
using System.Data;

namespace DemoProviderAccessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderController : ControllerBase
    {

        private static readonly string sqlConnectionString = "Server=tcp:sqldbservervinayak.database.windows.net,1433;Initial Catalog=kagglecmsdata;Persist Security Info=False;User ID=demoadmin1;Password=DemoPassword1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        // GET: api/Provider
        [HttpGet]
        public IEnumerable<Provider> Get()
        {
            //return new Provider[]; { new Provider(), new Provider() };
            var ProviderList = new List<Provider>();

            DataTable dt = new DataTable();
            var objADO = new ADODataAccess.SqlDataAccessLayer(sqlConnectionString);

            var cmdtext = "select * from Provider";

            dt = objADO.GetDataByAdapter(cmdtext, null, CommandType.Text);

            foreach (DataRow dr in dt.Rows)
            {
                var p = new Provider();
                p.ProviderFirstName = dr[0].ToString();
                p.ProviderLastName = dr[1].ToString();
                ProviderList.Add(p);
            }
            return ProviderList;
        }

        // GET: api/Provider/5
        [HttpGet("{id}", Name = "Get")]
        public Provider Get(int id)
        {
            return new Provider();
        }

        //// POST: api/Provider
        //[HttpPost]
        //public void Post([FromBody] Provider value)
        //{
        //}

        //// PUT: api/Provider/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] Provider value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
