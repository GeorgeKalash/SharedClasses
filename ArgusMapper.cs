using System.Reflection;
using System;
using System.Linq;

namespace SharedClasses
{
	public class ArgusMapper
	{
		public static T CloneProperties<T, S>(S _source) 
			where T : new() 
			where S : new()
		{
			if (_source == null) return default;

			T target = new T();
			PropertyInfo[] sourceProperties = typeof(S).GetProperties();
			PropertyInfo[] targetProperties = typeof(T).GetProperties();

			foreach (PropertyInfo sourceProp in sourceProperties)
			{
				PropertyInfo targetProp = targetProperties.FirstOrDefault(x => x.Name == sourceProp.Name & x.CanWrite);
				targetProp?.SetValue(target, sourceProp.GetValue(_source));
			}

			return target;
		}
	}
}
