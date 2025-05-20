namespace Arriba_Delivery
{
    class Review
    {   
        public string Reviewer {  get;  } //The name of the reviewer (Customer)
        public float Rating {  get; private set; } //The numerical rating from 1 to 5
        public string Starrating { get; } //The star rating from * to *****
        public string Comment { get;  } //The comment left on the review. Can be empty.

        /// <summary>
        /// A constructor for the review
        /// </summary>
        public Review(string reviewer, float rating, string comment)
        {
            Reviewer = reviewer;
            Rating = rating;
            Comment = comment;
            string tempstar = "";
            for (int i = 0; i < rating; i++)
            {
                tempstar += "*"; //Sets the star rating depending on the numerical rating
            }
            Starrating = tempstar;
        }

        /// <summary>
        /// Gets info about the review
        /// </summary>
        /// <returns>A string with information</returns>
        public string GetInfo()
        {
            return "Reviewer: " + Reviewer + "\nRating: " + Starrating + "\nComment: " + Comment;
        }
    }
}
