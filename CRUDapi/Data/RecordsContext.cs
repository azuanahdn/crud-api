using Microsoft.EntityFrameworkCore;
using CRUDapi.Models;
//using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace CRUDapi.Data
{
    public class RecordsContext : DbContext
    {
        public RecordsContext(DbContextOptions<RecordsContext> options) : base(options) { }
        public System.Data.Entity.DbSet<Record> Records { get; set; }
    }
}
