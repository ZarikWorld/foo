using libJxIssues.Models;
using Microsoft.EntityFrameworkCore;

namespace libJxIssues
{
    public class efHelper
    {
        public static jxIssuesContext getConnection()
        {
            var options = new DbContextOptionsBuilder<jxIssuesContext>()
                         .UseSqlServer(@"Server=PITWPROG19\SQLEXPRESS; Database=_jxIssues; User Id=sysdba; Password=masterkey; Trusted_Connection=false; MultipleActiveResultSets=true; Encrypt=False;")
                 .Options;

            return  new jxIssuesContext(options);
        } 
    }
}
