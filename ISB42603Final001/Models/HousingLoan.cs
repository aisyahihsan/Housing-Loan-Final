using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ISB42603Final001.Models
{
    public class HousingLoan
    {
        //Generate id
        [Required]
       [Display(Name = "Customer id")]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Gender (M/F)")]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Credit status (A/B/C)")]
        public char CreditRisk { get; set; }

        [DisplayFormat(DataFormatString = "{0,000:n2}")]
        [Required]
        [Display(Name = "Principal (RM)")]
        public double Principal { get; set; }

        [Required]
        [Display(Name = "Number of years")]
        public int NumberOfYears { get; set; }

        [DisplayFormat(DataFormatString = "{0:n2}")]
        public double InterestRate
        {
            get
            {
                if (CreditRisk == 'A')
                {
                    return 3.00;
                }

                else
                {
                    return 3.10;
                }
            }
            set { }
        }

        [DisplayFormat(DataFormatString = "{0,000:n2}")]
        public double MonthlyPayment
        {
            get 
            {
                double monthlyrate = (InterestRate /100)/ 12;
                double totalmonth = NumberOfYears * 12;
                double amount1 = monthlyrate * Math.Pow(1 + monthlyrate, totalmonth);
                double amount2 = Math.Pow(1 + monthlyrate, totalmonth) - 1;
                double monthlypayment = Principal * (amount1 / amount2);

                return monthlypayment;
            }
            set { }
        }

    }
}
