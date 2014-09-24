using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Splice.Lexer
{
	class Lexer
	{
		private int lineNumber;
		private string source;
		private int position;
		private List<Token> tokens;

		public IList<Token> Tokens
		{
			get
			{
				return this.tokens;
			}
		}

		public Lexer(string source)
		{
			this.source = source;
			this.position = 0;
			this.lineNumber = 0;
			this.tokens = new List<Token>();
		}

		public void Scan()
		{
			while (position < source.Length)
			{

			}
		}

		private int peekChar(int offset = 0)
		{
			if (offset + position < source.Length)
			{
				return (int)source[position + offset];
			}
			else
			{
				return -1;
			}
		}

		private int readChar()
		{
			if (position < source.Length)
			{
				return (int)source[position++];
			}
			else
			{
				return -1;
			}
		}
	}
}
