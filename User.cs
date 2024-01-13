using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace NEA_Document
{
    
    /*
     * A User of this application.
     */
    public class User
    {
        public User(int id, string username, string password, int skillRating) { 
            Id = id;
            Username = username;
            Password = password;
            SkillRating = skillRating;
        }
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int SkillRating { get; set; }
 
        public override string ToString() { return "id " + Id + ", username " +Username + ", skillRating " +SkillRating; }
 
    }
}
