using AutoMapper;
using BusinessObject.Models;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using SneakerShoeStoreAPI.DTO;
using System.Security.Policy;
using System.Text.Json.Serialization;

namespace SneakerShoeStoreAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            static IEdmModel GetEdmModel()
            {
                ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
                builder.EntitySet<Product>("Products");
                builder.EntitySet<User>("Users");
                builder.EntitySet<Order>("Orders");
                builder.EntitySet<Brand>("Brands");
                builder.EntitySet<Cart>("Carts");
                builder.EntitySet<OrderDetail>("OrderDetails");
                return builder.GetEdmModel();
            }
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers().AddOData(option => {
                option
                .EnableQueryFeatures()
                .Select()
                .Filter()
                .Count()
                .OrderBy()
                .Expand()
                .SetMaxTop(100);
                option.AddRouteComponents("odata", GetEdmModel()).RouteOptions.EnableKeyInParenthesis = false;
            });
            // Add services to the container.
            builder.Services.AddDbContext<SneakerShoeStoreContext>();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddControllers().AddJsonOptions(x =>
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            builder.Services.AddSwaggerGen();
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}