using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.Customers.Models
{
	public class Customer : BaseModel
	{
		[JsonIgnore] public string RowId => CustomerId;

		public string CustomerId { get; set; }
		public CustomerStatus Status { get; set; }
		public DateTimeOffset CreationDateTime { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Mobile { get; set; }
		public string Email { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (false)
				yield return new ValidationResult(
					"For testing purposes",
					new[] {"CustomerId"});
		}
	}
}