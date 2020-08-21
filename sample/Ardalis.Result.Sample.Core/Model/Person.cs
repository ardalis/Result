using System;
using System.Collections.Generic;

namespace Ardalis.Result.Sample.Core.Model
{
    public class Person
    {
		public int Id { get; set; }
		public string Surname { get; set; }
		public string Forename { get; set; }

		public List<Person> Children { get; set; }

		public DateTime DateOfBirth { get; set; }
	}
}
