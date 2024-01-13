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
    public class DatabaseUtils
    {
        /* The database connection string - added here to allow it to be easily changed */
        public const String CONNECTION_STRING = @"Provider = Microsoft.ACE.OLEDB.12.0;" + @"Data Source =C:\Users\ethan_grb0ji1\OneDrive\Desktop\School\NEA\NEA Database 2002.mdb; Persist Security Info = True";
    }
}
