using System.IO;
using System.Text;

namespace Minicf {
	public static class Minic {
		sealed class Helper {
			readonly Encoding _encoding;
			readonly TextReader _reader;

			public Helper(TextReader reader) {
				_encoding = Encoding.UTF8;
				_reader = reader;
			}

			public static char Look() {
				return (char)_reader.Peek();
			}

			public static char GetChar() {
				return (char)_reader.Read();
			}

			public static void Error(string str) {
				throw new System.Exception("Error: " + str);
			}

			public static void Expected(char c) {
				Error(string.Format("'{0} expected.'", c));
			}

			public static void Expected(string str) {
				Error(string.Format("'{0} expected.'", str));
			}

			public static void Match(char c) {
				if (Look() == c) GetChar();
				else Expected(c);
			}

			public static bool IsAlpha(char c) {
				return char.IsLetter(c);
			}

			public static bool IsDigit(char c) {
				return char.IsDigit(c);
			}

			public static char GetName() {
				if (!IsAlpha(Look())) Expected("Name");
				return GetChar();
			}

			public static char GetNum() {
				if (!IsDigit(Look())) Expected("Num");
				return GetChar();
			}
		}
	}
}