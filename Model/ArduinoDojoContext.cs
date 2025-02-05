using Microsoft.EntityFrameworkCore;

namespace ArduinoDOJO.Model
{
    public class ArduinoDojoContext : DbContext
    {
        public DbSet<DataModel> DataModels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=ArduinoDOJO;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }
    }
}
