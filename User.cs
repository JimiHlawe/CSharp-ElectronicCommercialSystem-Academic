    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

namespace FinalProject
{
    internal class User
    {
        private static int userCounter = 1;
        public int UserID { get; private set; }
        protected string username;
        protected string password;
        protected Address address;

        public User(string username, string password, Address address)
        {
            UserID = userCounter++;
            UserName = username;
            Password = password;
            Address = address;
        }

        public string UserName
        {
            get { return username; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Name cannot be empty.");
                }
                username = value;
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Password cannot be empty.");
                }
                password = value;
            }
        }

        public Address Address
        {
            get { return address; }
            set { address = value; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            User other = (User)obj;
            return UserID == other.UserID && username == other.username && password == other.password && address == other.address;
        }

        public override string ToString()
        {
            return "ID: " + UserID + ", Username: " + username + ", Password: " + password + ", Address: " + address;
        }
    }
}
