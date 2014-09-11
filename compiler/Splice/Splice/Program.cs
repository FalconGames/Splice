using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splice
{
	class Program
	{
		static void Main(string[] args)
        {
			string code =
@"void main()
{
	print(""Hello"");
}";
			Console.WriteLine(code);
			Console.ReadKey();
		}
	}
}
