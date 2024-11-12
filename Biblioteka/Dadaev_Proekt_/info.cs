using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dadaev_Proekt_
{
    internal class info
    {
        
        public static string connectionString = "data source=stud-mssql.sttec.yar.ru,38325;initial catalog=user97_db;user id=user97_db;password=user97;MultipleActiveResultSets=True;App=EntityFramework";
        public static int ID;
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoleName { get; set; }
    }

}
