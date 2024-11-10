using DAMProject.Server.Queries;

namespace DAMProject.Server.Configuration
{
    public static class QueriesConfiguration
    {
        public static WebApplicationBuilder ConfigureQueries(this WebApplicationBuilder builder)
        {
            builder.Services
                .AddScoped<IBookDetailsQueries, BookDetailsQueries>();
            return builder;
        }
    }
}
