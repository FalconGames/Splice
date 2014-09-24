using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splice.Lexer
{
	public abstract class Token
	{
		public readonly int Line;

		public Token(int line)
		{
			this.Line = line;
		}
	}
}
