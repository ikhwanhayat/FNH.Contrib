using System.Collections.Generic;

namespace FluentNHibernate.Query.UnitTests.Model
{
	public class A
	{
		public B B { get; set; }
		public IList<B> BList { get; set; }
		public string ABC { get; set; }
		public int? DEF { get; set; }
	}
}