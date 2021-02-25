using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OODProject
{
    public class Games
    {
        //Properties
        public string Name { get; set; }
        public int YearOfRelease { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string AvgRating { get; set; }

        //Constructors
        public Games(string Name, int YearOfRelease, double Price, string Description, string AvgRating)
        {
            this.Name = Name;
            this.YearOfRelease = YearOfRelease;
            this.Price = Price;
            this.Description = Description;
            this.AvgRating = AvgRating;
        }

        public Games() : this("Unknown", 0, 0, "Unknown", "Unknown")
        {

        }

        //ToString
        public override string ToString()
        {
            return string.Format($"Game Title: {Name} \nYear of Release: {YearOfRelease} \nPrice: {Price:c2} \nDescription: {Description} \nAvg Rating: {AvgRating}");
        }
    }
}
