using System.Diagnostics;
using System.IO;

namespace StringStreamSample
{
	public static class Program
	{
		private static void Main()
		{
			var text = "lorem ipsum dolor sit amet";

			using (var stream = new StringStream(text))
			using (var reader = new StreamReader(stream))
			{
				var read = reader.ReadToEnd();

				Debug.Assert(text.Equals(read));
			}
		}
	}
}