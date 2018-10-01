using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Customers.Models
{
	public static class DtoParamsToList
	{
		public const char Delimiter = ',';

		public static List<string> StringsAsList(this string csv)
		{
			return string.IsNullOrWhiteSpace(csv)
				? new List<string>()
				: csv
					.Split(Delimiter, StringSplitOptions.RemoveEmptyEntries)
					.Select(p => p.Trim())
					.ToList();
		}

		public static List<char> CharsAsList(this string csv)
		{
			return string.IsNullOrWhiteSpace(csv)
				? new List<char>()
				: csv
					.Split(Delimiter, StringSplitOptions.RemoveEmptyEntries)
					.Select(x =>
					{
						char.TryParse(x, out var character);
						return character;
					})
					.ToList();
		}

		public static List<T> AsList<T>(this string csv) where T : struct, IConvertible
		{
			return string.IsNullOrWhiteSpace(csv)
				? new List<T>()
				: csv
					.Split(Delimiter, StringSplitOptions.RemoveEmptyEntries)
					.Select(x =>
					{
						Enum.TryParse<T>(x, out var num);
						return num;
					})
					.ToList();
		}

		public static List<int> IntsAsList(this string csv)
		{
			return string.IsNullOrWhiteSpace(csv)
				? new List<int>()
				: csv
					.Split(Delimiter, StringSplitOptions.RemoveEmptyEntries)
					.Select(x =>
					{
						int.TryParse(x, out var num);
						return num;
					})
					.Where(x => x > 0)
					.ToList();
		}
	}
}