using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ISB42603Final001.Models;

namespace ISB42603Final001.Controllers
{
    public class Final001Controller : Controller
    {
        private readonly IConfiguration configuration;
        public Final001Controller(IConfiguration config)
        {
            this.configuration = config;
        }

        // Method to get the database list
        IList<HousingLoan> GetDbList()
        {
            IList<HousingLoan> dbList = new List<HousingLoan>();
            SqlConnection conn = new SqlConnection(configuration.
                GetConnectionString("ConnLoan"));

            string sql = @"SELECT * FROM Loan001";

            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dbList.Add(new HousingLoan()
                    {
                        Id = reader.GetString(0),
                        Name = reader.GetString(1),
                        Gender = reader.GetString(2),
                        CreditRisk = reader.GetChar(3),
                        Principal = reader.GetDouble(4),
                        InterestRate = reader.GetDouble(5),
                        NumberOfYears = reader.GetInt32(6),
                        MonthlyPayment = reader.GetDouble(7),
                    });
                }
            }
            catch
            {
                RedirectToAction("Error");
            }
            finally
            {
                conn.Close();
            }
            return dbList;
        }
        public IActionResult Index()
        {
            IList<HousingLoan> dbList = GetDbList();
            ViewBag.HighPrincipal = dbList.Max(x => x.Principal);
            ViewBag.LowPrincipal = dbList.Min(x => x.Principal);
            ViewBag.AverageMonthlyFemale = dbList.Where(x => x.Gender == "F")
                .Average(x => x.MonthlyPayment);
            ViewBag.AverageMonthlyMale = dbList.Where(x => x.Gender == "M")
                .Average(x => x.MonthlyPayment);
            return View(dbList);  
        }

        [HttpGet]
        public IActionResult LoanApplication001()
        {
            HousingLoan houseloan = new HousingLoan();
            return View(houseloan);
        }

        [HttpPost]
        public IActionResult LoanApplication001(HousingLoan houseloan)
        {
            if (ModelState.IsValid)
            {
                SqlConnection conn = new SqlConnection(configuration.GetConnectionString("ConnLoan"));
                SqlCommand cmd = new SqlCommand("spInsertData001", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", houseloan.Id);
                cmd.Parameters.AddWithValue("@name", houseloan.Name);
                cmd.Parameters.AddWithValue("@gender", houseloan.Gender);
                cmd.Parameters.AddWithValue("@creditrisk", houseloan.CreditRisk);
                cmd.Parameters.AddWithValue("@principal", houseloan.Principal);
                cmd.Parameters.AddWithValue("@interestrate", houseloan.InterestRate);
                cmd.Parameters.AddWithValue("@numberofyears", houseloan.NumberOfYears);
                cmd.Parameters.AddWithValue("@monthlypayment", houseloan.MonthlyPayment);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    return View(houseloan);
                }
                finally
                {
                    conn.Close();
                }
                return View("LoanApplicationResult001", houseloan);
            }else
             return View();
        }


    }
}
