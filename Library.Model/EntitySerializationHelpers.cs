using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Library.Model;
namespace Library.Model.Helpers
	{
	public static class EntitySerializationHelpers
		{

		public static IDbSet<Book> DeserializeBooks(this LibraryEntities context, Stream xstream)
			{
			XDocument xdoc = XDocument.Load(xstream);
			// Just write a custom deserializer for now...
			foreach (XElement xelem in xdoc.Descendants("Book"))
				{
				Book entity = new Book()
				{
					ISBN = xelem.Element("ISBN").Value,
					Title = xelem.Element("Title").Value,
					Publisher = xelem.Element("Publisher").Value,
					Author = xelem.Element("Author").Value,
					Synopsis = xelem.Element("Synopsis").Value
				};
				var result = context.Books
										.Where(b => (b.ISBN == entity.ISBN))
										.FirstOrDefault();
				if (result == null)
					context.Books.Add(entity);
				else
					result = entity;
				}

			return context.Books;
			}


		public static IDbSet<Book> DeserializeBookCheckouts(this LibraryEntities context, Stream xstream)
			{
			XDocument xdoc = XDocument.Load(xstream);
			// Just write a custom deserializer for now...
			foreach (XElement xelem in xdoc.Descendants("BookCheckout"))
				{
				BookCheckout entity = new BookCheckout()
				{
					ISBN = xelem.Element("ISBN").Value,
					Name = xelem.Element("Name").Value,
					Address = xelem.Element("Address").Value,
					City = xelem.Element("City").Value,
					State = xelem.Element("State").Value,
					ZIP = xelem.Element("ZIP").Value,
					Email = xelem.Element("Email").Value
				};
				var result = context.BookCheckouts
										.Where(bc => (bc.ISBN == entity.ISBN && bc.Email == entity.Email))
										.FirstOrDefault();
				if (result == null)
					context.BookCheckouts.Add(entity);
				else
					result = entity;
				}

			return context.Books;
			}

		public static Stream SerializeBooks(IEnumerable<Book> books, Stream xstream)
			{
			XDocument xdoc = new XDocument();
			XElement root = new XElement("Books");
			xdoc.Add(root);
			foreach (var book in books)
				{
				XElement xbook = new XElement("Book");
				xbook.Add(new XElement("ISBN", book.ISBN.ToString()));
				xbook.Add(new XElement("Title", book.Title.ToString()));
				xbook.Add(new XElement("Publisher", book.Publisher.ToString()));
				xbook.Add(new XElement("Author", book.Author.ToString()));
				xbook.Add(new XElement("Synopsis", book.Synopsis.ToString()));

				root.Add(xbook);
				}
			xdoc.Save(xstream);

			return xstream;
			}

		public static Stream SerializeBook(Book book, Stream xstream)
			{
			XElement xbook = new XElement("Book");
			xbook.Add(new XElement("ISBN", book.ISBN.ToString()));
			xbook.Add(new XElement("Title", book.Title.ToString()));
			xbook.Add(new XElement("Publisher", book.Publisher.ToString()));
			xbook.Add(new XElement("Author", book.Author.ToString()));
			xbook.Add(new XElement("Synopsis", book.Synopsis.ToString()));
			xbook.Save(xstream);

			return xstream;
			}


		}
	}