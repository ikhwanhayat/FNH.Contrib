using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Query.Utils;
using NHibernate;
using NHibernate.Criterion;

namespace FluentNHibernate.Query
{
	public class NHibernateQueryConjunction<TRt, T>
	{
		private ICriteria Criteria { get; set; }

		internal NHibernateQueryConjunction(ICriteria criteria)
		{
			Criteria = criteria;
		}

		public TRt Execute()
		{
			TRt rt;

			if (typeof(TRt).Equals(typeof(T)))
			{
				IList<TRt> results = Criteria.SetMaxResults(1).List<TRt>();

				if (results.Count > 0)
					rt = results[0];
				else
					rt = default(TRt);
			}
			else if (typeof(TRt).Equals(typeof(int)))
			{
				rt = Criteria.SetProjection(Projections.RowCount()).SetMaxResults(1).List<TRt>()[0];
			}
			else rt = (TRt)Criteria.List<T>();

			return rt;
		}

		public NHibernateQueryWhere<TRt, T, TV> And<TV>(Expression<Func<T, TV>> expression)
		{
			PropertyInfo propertyInfo = ReflectionHelper.GetProperty(expression);

			return new NHibernateQueryWhere<TRt, T, TV>(Criteria, propertyInfo);
		}

		public NHibernateChildQuery<NHibernateQueryConjunction<TRt, T>, TV> AndHasChild<TV>(Expression<Func<T, TV>> expression)
		{
			PropertyInfo propertyInfo = ReflectionHelper.GetProperty(expression);

			return GetChildCriteria<TV>(propertyInfo);
		}

		public NHibernateChildQuery<NHibernateQueryConjunction<TRt, T>, TV> AndHasChildCollection<TV>(Expression<Func<T, IEnumerable<TV>>> expression)
		{
			PropertyInfo propertyInfo = ReflectionHelper.GetProperty(expression);

			return GetChildCriteria<TV>(propertyInfo);
		}

		private NHibernateChildQuery<NHibernateQueryConjunction<TRt, T>, TV> GetChildCriteria<TV>(PropertyInfo propertyInfo)
		{
			ICriteria childCriteria = Criteria.CreateCriteria(propertyInfo.Name);

			NHibernateQueryConjunction<TRt, T> conjunction = new NHibernateQueryConjunction<TRt, T>(Criteria);

			return new NHibernateChildQuery<NHibernateQueryConjunction<TRt, T>, TV>(conjunction, childCriteria);
		}
	}
}