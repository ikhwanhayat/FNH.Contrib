using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentNHibernate.Query.Utils
{
	public interface IAccessor
	{
		string FieldName { get; }

		Type PropertyType { get; }
		PropertyInfo InnerProperty { get; }
		void SetValue(object target, object propertyValue);
		object GetValue(object target);

		IAccessor GetChildAccessor<T>(Expression<Func<T, object>> expression);

		string Name { get; }
	}
}