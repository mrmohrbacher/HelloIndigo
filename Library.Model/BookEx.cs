using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Library.Model
	{
	[MetadataTypeAttribute(typeof(BookValidationModel))]
	public partial class Book
		{
		public DateTime? CheckedOut { get; set; }
		}

	public class BookValidationModel
		{
      [Required]
      public string ISBN;
		[Required]
		public string Title;
		[Required]
		public string Publisher { get; set; }
		[Required]
		public string Author { get; set; }
		}

	}
