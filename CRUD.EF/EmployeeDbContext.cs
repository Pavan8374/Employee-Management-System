using CRUD.Domain.Employee;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.EF
{
    public class EmployeeDbContext:DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
        {

        }
        public DbSet<Emp> EmployeesTable { get; set; }
        public DbSet<SkillsList> Skills { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<SkillsList>()
                .HasOne(p => p.EmployeesTable)
                .WithMany(p => p.Skills)
                .HasForeignKey(s => s.EmployeeId);
        }
    }
}
