using DAMProject.Server.Services;

namespace DAMProject.Server.Configuration
{
    public static class ServicesConfiguration
    {
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services
                .AddScoped<IUserService, UserService>()
                .AddScoped<IAuthorService, AuthorService>()
                .AddScoped<IBookService, BookService>()
                .AddScoped<IGenreService, GenreService>()
                .AddScoped<IPublisherService, PublisherService>()
                .AddScoped<ISeriesService, SeriesService>()
                .AddScoped<IFavoritesService, FavoritesService>()
                .AddScoped<IRatingService, RatingService>();
         

            return builder;
        }
    }
}
