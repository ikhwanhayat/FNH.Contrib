using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Query.Utils;
using NHibernate;

namespace FluentNHibernate.Query
{
	public class NHibernateChildQueryConjunction<TConj, T>
	{
		private TConj Conjunction { get; set; }
		private ICriteria Criteria { get; set; }

		internal NHibernateChildQueryConjunction(TConj conjunction, ICriteria criteria)
		{
			Conjunction = conjunction;
			Criteria = criteria;
		}

		public NHibernateChildQueryWhere<TConj, T, TV> And<TV>(Expression<Func<T, TV>> expression)
		{
			PropertyInfo propertyInfo = ReflectionHelper.GetProperty(expression);

			return new NHibernateChildQueryWhere<TConj, T, TV>(Conjunction, Criteria, propertyInfo);
		}

		public TConj EndChild()
		{
			return Conjunction;
		}

		public NHibernateChildQuery<NHibernateChildQueryConjunction<TConj, T>, TV> AndHasChild<TV>(Expression<Func<T, TV>> expression)
		{
			PropertyInfo propertyInfo = ReflectionHelper.GetProperty(expression);

			return GetChildCriteria<TV>(propertyInfo);
		}

		public NHibernateChildQuery<NHibernateChildQueryConjunction<TConj, T>, TV> AndHasChildCollection<TV>(Expression<Func<T, IEnumerable<TV>>> expression)
		{
			PropertyInfo propertyInfo = ReflectionHelper.GetProperty(expression);

			return GetChildCriteria<TV>(propertyInfo);
		}

		private NHibernateChildQuery<NHibernateChildQueryConjunction<TConj, T>, TV> GetChildCriteria<TV>(PropertyInfo propertyInfo)
		{
			ICriteria childCriteria = Criteria.CreateCriteria(propertyInfo.Name);

			NHibernateChildQueryConjunction<TConj, T> conjunction = new NHibernateChildQueryConjunction<TConj, T>(Conjunction, Criteria);

			return new NHibernateChildQuery<NHibernateChildQueryConjunction<TConj, T>, TV>(conjunction, childCriteria);
		}
	}
}