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
        public int id { get; set; }

        //Constructors
        public Company(string CompanyName, int YearFormed, string Founders, int id)
        {
            this.CompanyName = CompanyName;
            this.YearFormed = YearFormed;
            this.Founders = Founders;

            var rand = new Random();
            this.id = rand.Next(10001);

            GamesList = new List<Games>();
        }

        public Company() : this("Unknown", 0, "Unknown", 0)
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
