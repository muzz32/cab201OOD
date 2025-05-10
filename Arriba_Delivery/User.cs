using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Arriba_Delivery
{
    abstract class User
    {

        public string name { get; protected set; }
        public int age { get; protected set; }
        public string mobile { get; protected set; }
        public string email { get; protected set; }
        public string password { get; protected set; }

        protected User(List<User> users)
        {
            bool valid = true;
            string name;
            int age;
            string email;
            string mobile;
            string password;

            do
            {
                CMD.Display("Please enter your name:");
                name = CMD.StrIn("Invalid name.", "Please enter your name:");
                if (!Regex.IsMatch(name, @"^(?=.*[a-zA-Z])[a-zA-Z\s'-]+$"))
                {
                    CMD.Display("Invalid name.");
                    valid = false;
                }
                else
                {
                    valid = true;
                }
            } while (!valid);

            do
            {
                CMD.Display("Please enter your age (18-100):");
                age = CMD.IntIn("Invalid age.", "Please enter your age (18-100):");
                if (age < 18 || age > 100)
                {
                    CMD.Display("Invalid age.");
                    valid = false;
                }
                else
                {
                    valid = true;
                }
            } while (!valid);

            do
            {
                CMD.Display("Please enter your email address:");
                email = CMD.StrIn("Invalid email address.", "Please enter your email address:");
                if (!Regex.IsMatch(email, @"^.+@.+$"))
                {
                    CMD.Display("Invalid email address.");
                    valid = false;
                }
                else if (users.Any(i => i.email == email))
                {
                    CMD.Display("This email address is already in use.");
                    valid = false;
                }
                else
                {
                    valid = true;
                }
            } while (!valid);

            do
            {
                CMD.Display("Please enter your mobile phone number:");
                mobile = CMD.StrIn("Invalid phone number.", "Please enter your mobile phone number:");
                if (mobile.Length != 10 || mobile[0] != '0' || !Regex.IsMatch(mobile, @"^[0-9]+$"))
                {
                    CMD.Display("Invalid phone number.");
                    valid = false;
                }
                else
                {
                    valid = true;
                }
            } while (!valid);

            do
            {
                CMD.Display(Consts.passwdprompt);
                password = CMD.StrIn("Invalid password.", Consts.passwdprompt);
                if (!Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])[a-zA-Z0-9]+$") || password.Length < 8)
                {
                    CMD.Display("Invalid password.");
                    valid = false;
                }
                else
                {
                    CMD.Display("Please confirm your password:");
                    if (password != CMD.StrIn("Invalid password", "Please confirm your password"))
                    {
                        CMD.Display("Passwords do not match.");
                        valid = false;
                    }
                    else
                    {
                        valid= true;
                    }
                }
            } while (!valid);

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
        public virtual void Getinfo() 
        {
            CMD.Display("Your user details are as follows:" +
                "\nName: " + name + "" +
                "\nAge: " + age + "" +
                "\nEmail: " + email + "" +
                "\nMobile: " + mobile);
        }

        public float GetDistance(string location1, string location2)
        {
            int[] coords1 = Array.ConvertAll(location1.Split(","), int.Parse);
            int[] coords2 = Array.ConvertAll(location2.Split(","), int.Parse);
            float distance = Math.Abs(coords1[0] - coords2[0]) + Math.Abs(coords1[1] - coords2[1]);
            return distance;
        }

        public float TotalDistance(string location1, string location2, string location3)
        {
            return GetDistance(location1, location2) + GetDistance(location2, location3);
        }
    }
}
