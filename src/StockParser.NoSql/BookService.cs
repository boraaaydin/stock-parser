//using MongoDB.Driver;
//using StockParser.Domain;
//using StockParser.NoSql.Models;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace StockParser.NoSql
//{
//    public class BookService
//    {
//        private readonly IMongoCollection<Book> _books;

//        public BookService(MongoConnectionSettings settings)
//        {
//            var client = new MongoClient(settings.ConnectionString);
//            var database = client.GetDatabase(settings.DatabaseName);

//            _books = database.GetCollection<Book>(settings.CollectionName);
//        }

//        public List<Book> Get() =>
//            _books.Find(book => true).ToList();

//        public Book Get(string id) =>
//            _books.Find<Book>(book => book.Id == id).FirstOrDefault();

//        public Book Create(Book book)
//        {
//            _books.InsertOne(book);
//            return book;
//        }

//        public void Update(string id, Book bookIn) =>
//            _books.ReplaceOne(book => book.Id == id, bookIn);

//        public void Remove(Book bookIn) =>
//            _books.DeleteOne(book => book.Id == bookIn.Id);

//        public void Remove(string id) =>
//            _books.DeleteOne(book => book.Id == id);
//    }
//}
