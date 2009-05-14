using System.Collections.Generic;
using NHibernate;

namespace FluentNHibernate.Query
{
	public static class NHibernateExtensions
	{
		public static NHibernateQuery<T, T> GetOne<T>(this ISession session)
		{
			return new NHibernateQuery<T, T>(session.CreateCriteria(typeof(T)));
		}

		public static NHibernateQuery<IList<T>, T> GetAll<T>(this ISession session)
		{
			return new NHibernateQuery<IList<T>, T>(session.CreateCriteria(typeof(T)));
		}

		public static NHibernateQuery<int, T> GetCount<T>(this ISession session)
		{
			return new NHibernateQuery<int, T>(session.CreateCriteria(typeof(T)));
		}
	}
}