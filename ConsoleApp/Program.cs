using Microsoft.EntityFrameworkCore;
using Persistence;
using Domain;

namespace ConsoleApp;

class Program
{
    public static void Main(string[] args)
    {
        using (var dbContext = new AppDbContext())
        {
            // update user with Id = 4:
            //   new() { Id = 4, Login = "user023", Name = "John", Surname = "Doe", Email = "john_doe@list.ru", EmailConfirmed = true, Phone = "+1(905)789-43-51", IsActive = true, CompanyId = 3, RoleId = 3 }
            var model = new User
            {
                Id = 4,
                Login = "agent007",
                Name = "Agent",
                Surname = "007",
                Email = "",
                EmailConfirmed = true,
                Phone = "",
                IsActive = false,
                RoleId = 3,
                Role = null,
                CompanyId = 3,
                Company = null
            };


            Update_Error1(dbContext, model);

            // Update_NoError_Detach(dbContext, model);
            // Update_NoError_AsNoTracking(dbContext, model);
            // Update_NoError_ModelMapping(dbContext, model);
        }

        Console.WriteLine("done");
    }

    public static async Task Update_Error1(AppDbContext dbContext, User model)
    {
        try
        {
            var fromDb = dbContext.Users.FirstOrDefault(user => user.Id == model.Id);
            if (fromDb != null)
            {
                dbContext.Users.Update(model);
                dbContext.SaveChanges();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /*
     * Вообще, просто задетачить недостаточно, если у загруженной сущности
     * есть связи с другими таблицами, которые тоже загружены.
     * В таком случае надо проверить весь граф объектов на наличие отслеживаемых объектов.
     * https://stackoverflow.com/questions/70095949/the-instance-of-entity-type-cannot-be-tracked-because-another-instance-with-the#70103298
     */
    public static async Task Update_NoError_Detach(AppDbContext dbContext, User model)
    {
        try
        {
            var fromDb = dbContext.Users.FirstOrDefault(user => user.Id == model.Id);
            if (fromDb != null)
            {
                dbContext.Entry(fromDb).State = EntityState.Detached;

                dbContext.Users.Update(model);
                dbContext.SaveChanges();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static async Task Update_NoError_ModelMapping(AppDbContext dbContext, User model)
    {
        var existedEntity = dbContext.Users
            .FirstOrDefault(user => user.Id == model.Id);

        if (existedEntity != null)
        {
            existedEntity.Login = model.Login;
            existedEntity.Name = model.Name;
            existedEntity.Surname = model.Surname;
            existedEntity.Email = model.Email;
            existedEntity.EmailConfirmed = model.EmailConfirmed;
            existedEntity.Phone = model.Phone;
            existedEntity.IsActive = model.IsActive;
            existedEntity.RoleId = model.RoleId;
            existedEntity.CompanyId = model.CompanyId;

            dbContext.Users.Update(existedEntity);
            dbContext.SaveChanges();
        }
    }

    public static async Task Update_NoError_AsNoTracking(AppDbContext dbContext, User model)
    {
        var fromDb = dbContext.Users
            .AsNoTracking()
            .FirstOrDefault(user => user.Id == model.Id);

        if (fromDb != null)
        {
            dbContext.Users.Update(model);
            dbContext.SaveChanges();
        }
    }
}