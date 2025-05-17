using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arriba_Delivery
{
    class Review
    {   
        public string reviewer {  get; private set; }
        public float rating {  get; private set; }
        public string starrating { get; private set; }
        public string comment { get; private set; }

        public Review(string reviewer, float rating, string comment)
        {
            this.reviewer = reviewer;
            this.rating = rating;
            this.comment = comment;
            string tempstar = "";
            for (int i = 0; i < rating; i++)
            {
                tempstar += "*";
            }
            starrating = tempstar;
        }

        public string GetInfo()
        {
            return "Reviewer: " + reviewer + "\nRating: " + starrating + "\nComment: " + comment;
        }
    }
}
