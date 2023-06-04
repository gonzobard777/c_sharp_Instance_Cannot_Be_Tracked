using Microsoft.EntityFrameworkCore;
using Domain;

namespace Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Company> Companies { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<License> Licenses { get; set; }

    public AppDbContext()
    {
        // Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("host=127.0.0.1;port=5577;database=db;username=root;password=12345");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region User связи

        modelBuilder.Entity<User>()
            .HasOne(user => user.Role)
            .WithMany(role => role.Users)
            .HasForeignKey(user => user.RoleId)
            .HasConstraintName("FK_RoleId_one_Role_withMany_Users")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasOne(user => user.Company)
            .WithMany(company => company.Users)
            .HasForeignKey(user => user.CompanyId)
            .HasConstraintName("FK_CompanyId_one_Company_withMany_Users")
            .OnDelete(DeleteBehavior.Cascade);

        #endregion User связи

        # region License  связи

        modelBuilder.Entity<License>()
            .HasOne(license => license.Company)
            .WithMany(company => company.Licenses)
            .HasForeignKey(license => license.CompanyId)
            .HasConstraintName("FK_CompanyId_one_Company_withMany_Licenses")
            .OnDelete(DeleteBehavior.Cascade);

        # endregion License  связи

        #region Заполнение первоначальными данными

        // Структура Росгидромета - https://www.meteorf.gov.ru/about/structure/
        modelBuilder.Entity<Company>().HasData(
            new() { Id = 1, Name = "Гидрометцентр России", Contacts = "+7(495)627-44-44, +7(495)628-98-47", IsActive = true },
            new() { Id = 2, Name = "ФГБУ \"ГАМЦ Росгидромета\"", Contacts = "+7(800)200-09-00", IsActive = false },
            new() { Id = 3, Name = "ФГБУ \"Обь-Иртышское УГМС\"", Contacts = "+7(495)926-99-77", IsActive = true },
            new() { Id = 4, Name = "ФГБУ \"Камачатское УГМС\"", Contacts = "(4152) 29-83-95", IsActive = true }
        );

        modelBuilder.Entity<Role>().HasData(
            new() { Id = 1, Key = RoleKey.admin, Name = "Администратор" },
            new() { Id = 2, Key = RoleKey.companyAdmin, Name = "Администратор компании" },
            new() { Id = 3, Key = RoleKey.meteorologist, Name = "Метеоролог" },
            new() { Id = 4, Key = RoleKey.user, Name = "Пользователь", Description = "Обычный пользователь" }
            // new() { Id = , Key = RoleKey, Name = "", Description = "" }
        );

        modelBuilder.Entity<User>().HasData(
            new() { CompanyId = 1, RoleId = 1, Id = 1, Login = "sivanov", Name = "Сергей", Surname = "Иванов", Email = "ivanovSergei@mail.ru", EmailConfirmed = true, Phone = "+7(905)458-65-52", IsActive = true },
            new() { CompanyId = 1, RoleId = 2, Id = 2, Login = "yuli777", Name = "Юлия", Surname = "Смирнова", Email = "smirnova@mail.ru", EmailConfirmed = true, Phone = "+7(905)158-85-52", IsActive = true },
            new() { CompanyId = 2, RoleId = 4, Id = 3, Login = "mishagor", Name = "Михаил", Surname = "Горский", Email = "jojo1963@hotmail.com", EmailConfirmed = false, Phone = "+7(912)417-02-02", IsActive = false },
            new() { CompanyId = 3, RoleId = 3, Id = 4, Login = "user023", Name = "John", Surname = "Doe", Email = "john_doe@list.ru", EmailConfirmed = true, Phone = "+1(905)789-43-51", IsActive = true }
            // new() { CompanyId = , RoleId = , Id = , Login = "", Name = "", Surname = "", Email = "", EmailConfirmed = , Phone = "", IsActive =  },
        );

        modelBuilder.Entity<License>().HasData(
            new() { CompanyId = 1, Id = 1, Name = "Лицензия 9972" },
            new() { CompanyId = 1, Id = 2, Name = "Лицензия 2355" },
            new() { CompanyId = 1, Id = 3, Name = "Лицензия 2634" }
            // new() {CompanyId = , Id = , Name = }
        );

        #endregion Заполнение первоначальными данными
    }
}