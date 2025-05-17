using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Arriba_Delivery
{
    /// <summary>
    /// An abstract user class. Contains all universal user attributes such as name, age, mobile, email and
    /// password. 
    /// </summary>
    abstract class User
    {
        public string name { get; protected set; }
        public int age { get; protected set; }
        public string mobile { get; protected set; }
        public string email { get; protected set; }
        public string password { get; protected set; }
        
        protected User()
        {
            name = "";
            age = 0;
            mobile = "";
            email = "";
            password = ""; 
        }

        protected User(string name, int age, string mobile, string email, string password)
        {
            this.name = name;
            this.age = age;
            this.mobile = mobile;
            this.email = email;
            this.password = password;
        }
        
        public void WelcomeMsg()
        {
            CMD.Display($"Welcome back, {name}!");
        }
        
        /// <summary>
        /// Gets universal user info.
        /// </summary>
        /// <returns>A string for methods such as Display to handle</returns>
        public virtual string Getinfo() 
        {
            return "Your user details are as follows:" +
                "\nName: " + name + "" +
                "\nAge: " + age + "" +
                "\nEmail: " + email + "" +
                "\nMobile: " + mobile;
        }
        
        /// <summary>
        /// A function the finds the taxicab distance between two user locations
        /// </summary>
        /// <param name="location1">The first users location</param>
        /// <param name="location2">The second users location</param>
        /// <returns>The taxi cab distance in float form</returns>
        public static float GetDistance(string location1, string location2)
        {
            int[] coords1 = Array.ConvertAll(location1.Split(","), int.Parse);
            int[] coords2 = Array.ConvertAll(location2.Split(","), int.Parse);
            float distance = Math.Abs(coords1[0] - coords2[0]) + Math.Abs(coords1[1] - coords2[1]);
            return distance;
        }
        
        /// <summary>
        /// Returns the distance to travel between three locations. Useful when finding
        /// how long a trip will take for a deliverer.
        /// </summary>
        /// <param name="location1">The first users location, often the deliverer</param>
        /// <param name="location2">The second users location, often the restaurant/client</param>
        /// <param name="location3">The third and final location, often the destination/customer</param>
        /// <returns>The taxi cab distance in float form</returns>
        public static float TotalDistance(string location1, string location2, string location3)
        {
            return GetDistance(location1, location2) + GetDistance(location2, location3);
        }
    }
}
