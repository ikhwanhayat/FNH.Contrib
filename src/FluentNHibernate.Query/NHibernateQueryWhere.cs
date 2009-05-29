using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NHibernate;
using NHibernate.Criterion;

namespace FluentNHibernate.Query
{
	public class NHibernateQueryWhere<TRt, T, TV> : NHibernateQueryWhere<NHibernateQueryConjunction<TRt, T>, TV>
	{
		internal NHibernateQueryWhere(ICriteria criteria, PropertyInfo propertyInfo) : base(criteria, propertyInfo) { }

		protected override NHibernateQueryConjunction<TRt, T> GetConjunction()
		{
			return new NHibernateQueryConjunction<TRt, T>(Criteria);
		}
	}

	public abstract class NHibernateQueryWhere<T, TV>
	{
		protected ICriteria Criteria { get; private set; }
		private PropertyInfo PropertyInfo { get; set; }

		internal protected NHibernateQueryWhere(ICriteria criteria, PropertyInfo propertyInfo)
		{
			Criteria = criteria;
			PropertyInfo = propertyInfo;
		}

		protected abstract T GetConjunction();

		public T IsEqualTo(TV value)
		{
			Criteria.Add(Restrictions.Eq(PropertyInfo.Name, value));

			return GetConjunction();
		}

		public T IsNotEqualTo(TV value)
		{
			Criteria.Add(Restrictions.Not(Restrictions.Eq(PropertyInfo.Name, value)));

			return GetConjunction();
		}

		public T IsLessThan(TV value)
		{
			Criteria.Add(Restrictions.Lt(PropertyInfo.Name, value));

			return GetConjunction();
		}

		public T IsLessThanOrEqualTo(TV value)
		{
			Criteria.Add(Restrictions.Le(PropertyInfo.Name, value));

			return GetConjunction();
		}

		public T IsGreaterThan(TV value)
		{
			Criteria.Add(Restrictions.Gt(PropertyInfo.Name, value));

			return GetConjunction();
		}

		public T IsGreaterThanOrEqualTo(TV value)
		{
			Criteria.Add(Restrictions.Ge(PropertyInfo.Name, value));

			return GetConjunction();
		}

		public T IsIn(IEnumerable<TV> list)
		{
			ICollection collection;

			if (list is ICollection)
				collection = (ICollection)list;
			else
				collection = new List<TV>(list);

			Criteria.Add(Restrictions.In(PropertyInfo.Name, collection));

			return GetConjunction();
		}

		public T IsNotNull()
		{
			Criteria.Add(Restrictions.IsNotNull(PropertyInfo.Name));

			return GetConjunction();
		}

		public T IsNull()
		{
			Criteria.Add(Restrictions.IsNull(PropertyInfo.Name));

			return GetConjunction();
		}

		public T IsLike(TV value)
		{

			Criteria.Add(Restrictions.Like(PropertyInfo.Name, value));

			return GetConjunction();

		}

		public T IsLikeInsensitive(TV value)
		{

			Criteria.Add(Restrictions.InsensitiveLike(PropertyInfo.Name, value));

			return GetConjunction();

		}

	}

}