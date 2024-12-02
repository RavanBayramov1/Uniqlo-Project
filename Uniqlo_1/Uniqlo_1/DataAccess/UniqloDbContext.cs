using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Uniqlo_1.Models;

namespace Uniqlo_1.DataAccess;

public class UniqloDbContext : IdentityDbContext
{
    public DbSet<User> Users { get; set ; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Slider> Sliders {  get; set; }
    public UniqloDbContext(DbContextOptions opt) : base(opt){}
}
