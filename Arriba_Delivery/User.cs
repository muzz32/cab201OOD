namespace Arriba_Delivery
{
    /// <summary>
    /// An abstract user class. Contains all universal user attributes such as name, age, mobile, email and
    /// password. 
    /// </summary>
    abstract class User
    {
        public string Name { get; } //The users name
        public int Age { get; } //The users age
        public string Mobile { get;  } //The users phone number
        public string Email { get;  } // The users email
        public string Password { get;  } //The users password
        
        /// <summary>
        /// A default constructor to be used
        /// by all child users
        /// </summary>
        protected User()
        {
            Name = "";
            Age = 0;
            Mobile = "";
            Email = "";
            Password = ""; 
        }

        /// <summary>
        /// A constructor to be inherited by all child users
        /// </summary>
        protected User(string name, int age, string mobile, string email, string password)
        {
            this.Name = name;
            this.Age = age;
            this.Mobile = mobile;
            this.Email = email;
            this.Password = password;
        }
        
        /// <summary>
        /// A welcome message for all users
        /// </summary>
        /// <returns>A message as a string</returns>
        public string WelcomeMsg()
        {
            return $"Welcome back, {Name}!";
        }
        
        /// <summary>
        /// Gets universal user info.
        /// </summary>
        /// <returns>A string for methods such as Display to handle</returns>
        public virtual string GetInfo() 
        {
            return "Your user details are as follows:" +
                "\nName: " + Name + "" +
                "\nAge: " + Age + "" +
                "\nEmail: " + Email + "" +
                "\nMobile: " + Mobile;
        }
    }
}
