using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splice.Lexer.Tokens
{
	class TokenIdentifier : Token
	{
		public readonly string Value;
		public TokenIdentifier(string name, int line)
			: base(line)
		{
			this.Value = name;
		}

		public override string ToString()
		{
			return this.Value;
		}
	}
}
