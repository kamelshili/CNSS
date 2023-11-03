using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CNSSInv.Models
{
    public class UserLogin
    {
        [PrimaryKey]
        public string Name_User { get; set; }
        public string Password { get; set; }
        public UserLogin()
        {

        }

        public UserLogin(string userId, string password)
        {
            this.Name_User = userId;
            this.Password = password;
        }

        public UserLogin(UserLogin value)
        {
            this.Name_User = value.Name_User;
            this.Password = value.Password;
        }

        public bool checkInformation()
        {
            if(this.Name_User == null || this.Password == null )
            {
                return false;
            }
            if (this.Name_User.CompareTo("") == 0 || this.Password.CompareTo("") == 0)
                return false;
            return true;
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
