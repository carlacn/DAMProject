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
                    comments, user_id AS UserId, image 
                    FROM books";
            return await _localDbConnection.QueryAsync<Book>(query);
        }

        public async Task<Book> GetBookById(int id)
        {
            var query = @"SELECT id, title, genre_id AS GenreId, publisher_id AS PublisherId, series_id AS SeriesId, 
                    comments, user_id AS UserId, image 
                    FROM books WHERE id = @Id";
            return await _localDbConnection.QuerySingleOrDefaultAsync<Book>(query, new { Id = id });
        }

		public async Task<int> CreateBook(Book book)
        {
            //TODO pasar a bbdd.
            if (book.SeriesId == 0)
            {
                book.SeriesId = 1;
            }
            var query = @"INSERT INTO books (title, genre_id, publisher_id, series_id, comments, user_id, image)
                  VALUES (@Title, @GenreId, @PublisherId, @SeriesId, @Comments, @UserId, @Image)";
            var result = await _localDbConnection.ExecuteAsync(query, book);

            return result > 0 ? result : throw new Exception("Error creating book.");
        }

        public async Task<int> UpdateBook(Book newBook)
        {
            var currentBook = await GetBookById(newBook.Id);

            if (currentBook == null)
            {
                return 0;
            }

            var updatedBook = MapBook(newBook, currentBook);

            var query = @"UPDATE books 
                  SET title = @Title, genre_id = @GenreId, publisher_id = @PublisherId, series_id = @SeriesId, 
                      comments = @Comments, user_id = @UserId, image = @Image
                  WHERE id = @Id";

            var result = await _localDbConnection.ExecuteAsync(query, new
            {
                updatedBook.Title,
                updatedBook.GenreId,
                updatedBook.PublisherId,
                updatedBook.SeriesId,
                updatedBook.Comments,
                updatedBook.UserId,
                updatedBook.Image,
                updatedBook.Id
            });

            return result > 0 ? result : throw new Exception("Error updating book.");
        }

        public async Task DeleteBook(int id)
        { 
            var deleteBookAuthorRelations = "DELETE FROM book_author WHERE book_id = @BookId";
            await _localDbConnection.ExecuteAsync(deleteBookAuthorRelations, new { BookId = id });

            var deleteRatingsRelations = "DELETE FROM score WHERE book_id = @BookId";
            await _localDbConnection.ExecuteAsync(deleteRatingsRelations, new { BookId = id });

            var deleteFavoritesRelations = "DELETE FROM favorites WHERE book_id = @BookId";
            await _localDbConnection.ExecuteAsync(deleteFavoritesRelations, new { BookId = id });

            var deleteBookQuery = "DELETE FROM books WHERE id = @Id";
            await _localDbConnection.ExecuteAsync(deleteBookQuery, new { Id = id });
            
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
                Comments = !string.IsNullOrEmpty(newBook.Comments) ? newBook.Comments : currentBook.Comments,
                UserId = newBook.UserId > 0 ? newBook.UserId : currentBook.UserId,
                Image = !string.IsNullOrEmpty(newBook.Image) ? newBook.Image : currentBook.Image
            };
        }

    }
}


