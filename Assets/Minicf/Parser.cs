using System;
using System.IO;

using Minicf.AST;

namespace Minicf {
	public class Parser {
		readonly TextReader _reader;

		public Parser(string str) {
			_reader = new StringReader(str);
		}

		public char Look() {
			return (char)_reader.Peek();
		}

		public char GetChar() {
			return (char)_reader.Read();
		}

		public void Error(string str) {
			throw new System.Exception("Error: " + str);
		}

		public void Expected(char c) {
			Error(string.Format("'{0} expected.'", c));
		}

		public void Expected(string str) {
			Error(string.Format("'{0} expected.'", str));
		}

		public void Match(char c) {
			if (Look() == c) GetChar();
			else Expected(c);
		}

		public bool IsAlpha(char c) {
			return char.IsLetter(c);
		}

		public bool IsDigit(char c) {
			return char.IsDigit(c);
		}

		public char GetName() {
			if (!IsAlpha(Look())) Expected("Name");
			return GetChar();
		}

		public char GetNum() {
			if (!IsDigit(Look())) Expected("Num");
			return GetChar();
		}

		public Expression Expression() {
			return new Expression();
		}
	}
}

