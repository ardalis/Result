using System;
using System.Collections.Generic;

namespace Ardalis.Result.Sample.Core.Model
{
    public class Person
    {
		public int Id { get; set; }
		public string Surname { get; set; } = String.Empty;
		public string Forename { get; set; } = String.Empty;

		public List<Person> Children { get; set; } = new();

		public DateTime DateOfBirth { get; set; }
	}
}
