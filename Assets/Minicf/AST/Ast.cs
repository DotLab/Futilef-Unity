using System;

namespace Minicf.AST {


	public enum AddOp {
		Plus,
		Minus,
	}

	// <expression> ::= <term> [<addop> <term>]*
	public struct Expression {
		public int num;
	}

	public enum MulOp {
		Multiply,
		Divide,
	}

	// <term> ::= <factor>  [ <mulop> <factor> ]*

	// <factor> ::= <number> | (<expression>) | <variable>

}

