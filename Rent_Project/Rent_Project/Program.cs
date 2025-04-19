
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Rent_Project.Model;


namespace Rent_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<RentAppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            

    //            var post = new Post
    //            {
    //                Description = "Test",
    //                Number_of_viewers =10,
    //            rental_status =0,
    //            Title  ="ehg",
   
    //      Accsepted_Status =1,
    //     images  ="shgf",
    //      location ="hfd",
    //      Price =800,
    //      User_id =1,
    //     Landlord_name =1
    //};

                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
                var app = builder.Build();


                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseAuthorization();


                app.MapControllers();

                app.Run();



            }
        
    }
}