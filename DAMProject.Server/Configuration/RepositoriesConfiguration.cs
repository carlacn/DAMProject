using DAMProject.Server.Repositories;

namespace DAMProject.Server.Configuration
{
    public static class RepositoriesConfiguration
    {
        public static WebApplicationBuilder ConfigureRepositories(this WebApplicationBuilder builder)
        {
            builder.Services
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IAuthorRepository, AuthorRepository>()
                .AddScoped<IBookRepository, BookRepository>()
                .AddScoped<IGenreRepository, GenreRepository>()
                .AddScoped<IPublisherRepository, PublisherRepository>()
                .AddScoped<ISeriesRepository, SerieRepository>()
                .AddScoped<IFavoritesRepository, FavoritesRepository>()
                .AddScoped<IScoreRepository, ScoreRepository>();

            return builder;
        }
    }
}
