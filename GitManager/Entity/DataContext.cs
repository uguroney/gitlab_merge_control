using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.SQLite;
using System.Data.SQLite.EF6;

namespace GitManager.Entity
{
    public class DataContext : DbContext
    {
        public DataContext() : base(new SQLiteConnection
        {
            ConnectionString = new SQLiteConnectionStringBuilder
            {
                DataSource = "MergeRequestDataSoruce.db"
            }.ConnectionString
        }, true)
        {
            Database.SetInitializer<DataContext>(null);
        }

        public virtual DbSet<MergeRequestEntity> MergeRequests { get; set; }
    }
}