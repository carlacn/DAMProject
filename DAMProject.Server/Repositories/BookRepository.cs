using DAMProject.Shared.Models;
using Dapper;
using System.Data;

namespace DAMProject.Server.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetBooks();
        Task<Book> GetBookById(int id);
        Task<int> CreateBook(Book book);
        Task<int> UpdateBook(Book book);
        Task DeleteBook(int id);
    }

    public class BookRepository(IDbConnection localDbConnection) : IBookRepository
    {
        private readonly IDbConnection _localDbConnection = localDbConnection;

        public async Task<IEnumerable<Book>> GetBooks()
        {
            var query = @"SELECT id, title, genre_id AS GenreId, publisher_id AS PublisherId, series_id AS SeriesId, 
                        read_date AS ReadDate, status, average_rating AS AverageRating, comments, user_id AS UserId, format 
                        FROM books";
            return await _localDbConnection.QueryAsync<Book>(query);
        }

        public async Task<Book> GetBookById(int id)
        {
            var query = @"SELECT id, title, genre_id, publisher_id, series_id, read_date, status, average_rating, comments, user_id, format FROM books WHERE id = @Id";
            return await _localDbConnection.QuerySingleOrDefaultAsync<Book>(query, new { Id = id });
        }

        public async Task<int> CreateBook(Book book)
        {
            var query = @"INSERT INTO books (title, genre_id, publisher_id, series_id, read_date, status, average_rating, comments, user_id, format)
                          VALUES (@Title, @GenreId, @PublisherId, @SeriesId, @ReadDate, @Status, @AverageRating, @Comments, @UserId, @Format)";
            var result = await _localDbConnection.ExecuteAsync(query, book);

            return result > 0 ? result : throw new Exception("Error creating book.");
        }

        public async Task<int> UpdateBook(Book newBook)
        {
            var currentBook = await _localDbConnection.QuerySingleOrDefaultAsync<Book>(
                    "SELECT * FROM books WHERE id = @Id", new { newBook.Id });

            if (currentBook == null)
            {
                return 0;
            }

            var updatedBook = MapBook(newBook, currentBook);

            var query = @"UPDATE books 
                  SET title = @Title, genre_id = @GenreId, publisher_id = @PublisherId, series_id = @SeriesId, read_date = @ReadDate, 
                      status = @Status, average_rating = @AverageRating, comments = @Comments, user_id = @UserId, format = @Format
                  WHERE id = @Id";

            var result = await _localDbConnection.ExecuteAsync(query, new
            {
                updatedBook.Title,
                updatedBook.GenreId,
                updatedBook.PublisherId,
                updatedBook.SeriesId,
                updatedBook.ReadDate,
                updatedBook.Status,
                updatedBook.AverageRating,
                updatedBook.Comments,
                updatedBook.UserId,
                updatedBook.Format,
                updatedBook.Id
            });

            return result > 0 ? result : throw new Exception("Error updating book.");
        }

        public async Task DeleteBook(int id)
        {
            var query = @"DELETE FROM books WHERE id = @Id";
            await _localDbConnection.ExecuteAsync(query, new { Id = id });
        }
        private Book MapBook(Book newBook, Book currentBook)
        {
            return new Book
            {
                Id = currentBook.Id,
                Title = !string.IsNullOrEmpty(newBook.Title) ? newBook.Title : currentBook.Title,
                GenreId = newBook.GenreId > 0 ? newBook.GenreId : currentBook.GenreId,
                PublisherId = newBook.PublisherId > 0 ? newBook.PublisherId : currentBook.PublisherId,
                SeriesId = newBook.SeriesId > 0 ? newBook.SeriesId : currentBook.SeriesId,
                ReadDate = newBook.ReadDate != default ? newBook.ReadDate : currentBook.ReadDate,
                Status = !string.IsNullOrEmpty(newBook.Status) ? newBook.Status : currentBook.Status,
                AverageRating = newBook.AverageRating > 0 ? newBook.AverageRating : currentBook.AverageRating,
                Comments = !string.IsNullOrEmpty(newBook.Comments) ? newBook.Comments : currentBook.Comments,
                UserId = newBook.UserId > 0 ? newBook.UserId : currentBook.UserId,
                Format = !string.IsNullOrEmpty(newBook.Format) ? newBook.Format : currentBook.Format
            };
        }
    }
}


