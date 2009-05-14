using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Query.Utils;
using NHibernate;
using NHibernate.Criterion;

namespace FluentNHibernate.Query
{
	public class NHibernateQueryOrderBy<TRt, T, TV>
	{
		private NHibernateQuery<TRt, T> Query { get; set; }
		private ICriteria Criteria { get; set; }
		private PropertyInfo PropertyInfo { get; set; }

		public NHibernateQueryOrderBy(NHibernateQuery<TRt, T> query, ICriteria criteria, Expression<Func<T, TV>> expression)
		{
			Query = query;
			Criteria = criteria;
			PropertyInfo = ReflectionHelper.GetProperty(expression);
		}

		public NHibernateQuery<TRt, T> Ascending()
		{
			Criteria.AddOrder(Order.Asc(PropertyInfo.Name));
			return Query;
		}

		public NHibernateQuery<TRt, T> Descending()
		{
			Criteria.AddOrder(Order.Asc(PropertyInfo.Name));
			return Query;
		}

	}
}