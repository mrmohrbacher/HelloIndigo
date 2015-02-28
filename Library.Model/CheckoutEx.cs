using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Model
	{
	[MetadataTypeAttribute(typeof(CheckoutValidationModel))]
	public partial class Checkout
		{
		}

	public class CheckoutValidationModel
		{
		[Required]
		public string ISBN { get; set; }

		[Required]
		public string Email { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public string Address { get; set; }
		
		[Required]
		public string City { get; set; }

		[Required]
		public string State { get; set; }

		[Required]
		public string PostalCode { get; set; }
		}
	}
