using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace OODProject
{
    public class Company : IComparable
    {
        //Properties
        public string CompanyName { get; set; }
        public int YearFormed { get; set; }
        public string Founders { get; set; }
        public List<Games> GamesList { get; set; }

        //Constructors
        public Company(string CompanyName, int YearFormed, string Founders)
        {
            this.CompanyName = CompanyName;
            this.YearFormed = YearFormed;
            this.Founders = Founders;

            GamesList = new List<Games>();
        }

        public Company() : this("Unknown", 0, "Unknown")
        {

        }

        //ToString
        public override string ToString()
        {
            return CompanyName + " - Founded " + YearFormed;
        }

        public int CompareTo(object obj)
        {
            Company otherCompany = obj as Company;
            return this.CompanyName.CompareTo(otherCompany.CompanyName);
        }
    }
}
