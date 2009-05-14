using System.Reflection;
using NHibernate;

namespace FluentNHibernate.Query
{
	public class NHibernateChildQueryWhere<TConj, T, TV> : NHibernateQueryWhere<NHibernateChildQueryConjunction<TConj, T>, TV>
	{
		private TConj Conjunction { get; set; }

		internal NHibernateChildQueryWhere(TConj conjunction, ICriteria criteria, PropertyInfo propertyInfo) : base(criteria, propertyInfo)
		{
			Conjunction = conjunction;
		}

		protected override NHibernateChildQueryConjunction<TConj, T> GetConjunction()
		{
			return new NHibernateChildQueryConjunction<TConj, T>(Conjunction, Criteria);
		}
	}
}