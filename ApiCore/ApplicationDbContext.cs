using ApiCore.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiCore
{
    public class ApplicationDbContext :DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

            
        }
        public DbSet<Employeee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          modelBuilder.Entity<Employeee>().HasData(
              new Employeee { Id=1,Name="Awais Ansari",Salary=24000,UAN="RAWKZY8446",CreatedDate= new DateTime(2020,08,01)},
              new Employeee { Id = 2, Name = "Sadain Ansari", Salary = 24000, UAN = "QWERTY9890",CreatedDate = new DateTime(2029, 04, 01) },
              new Employeee { Id = 3, Name = "Awais Ansari", Salary = 24000, UAN = "ZXCVBN0987", CreatedDate = new DateTime(2023,02,01) }

              );
        }
    }
}
