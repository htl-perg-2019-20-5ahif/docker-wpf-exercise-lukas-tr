using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Linq;
using System.Threading;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ECarsContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]);
            });
            services.AddControllers();
        }

        public static bool CanConnect(ECarsContext ctx)
        {
            try
            {
                return ctx.Database.CanConnect();
            }
            catch (SqlException)
            {
                return false;
            }
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // runtime migrations
            using (IServiceScope scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                ECarsContext context = scope.ServiceProvider.GetService<ECarsContext>();
                // mssql takes too long to start
                while (!CanConnect(context))
                {
                    System.Console.WriteLine("Can't connect to database, retrying");
                    Thread.Sleep(1000);
                }
                context.Database.Migrate();
                if (context.Cars.Count() == 0)
                {
                    context.Database.ExecuteSqlRaw(File.ReadAllText("./cars.sql"));
                }
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
