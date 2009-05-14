using System.Collections.Generic;

namespace FluentNHibernate.Query.UnitTests.Model
{
	public class B
	{
		public C C { get; set; }
		public IList<C> CList { get; set; }
		public bool? GHI { get; set; }
		public char JKL { get; set; }
	}
}