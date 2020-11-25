using System.Diagnostics.CodeAnalysis;
using System.Net.Mail;

namespace ChildcareWorldwide.Integration.Subscriber.Helpers
{
	internal static class StringExtensions
	{
		public static bool IsNullOrEmpty([NotNullWhen(false)] this string? str) => string.IsNullOrEmpty(str);

		public static string? NullIfEmpty(this string str) => string.IsNullOrEmpty(str) ? null : str;

		public static bool IsValidEmailAddress([NotNullWhen(true)] this string? email)
		{
			if (email.IsNullOrEmpty())
				return false;

			try
			{
				var addr = new MailAddress(email);
				return addr.Address == email;
			}
			catch
			{
				return false;
			}
		}
	}
}
