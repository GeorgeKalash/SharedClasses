using System.Reflection;
using System;
using System.Linq;

namespace SharedClasses
{
	public class ArgusMapper
	{
		public static T CloneProperties<T, S>(S source) 
			where T : new() 
			where S : new()
		{
			if (source == null) return default;

			T target = new T();
			PropertyInfo[] sourceProperties = typeof(S).GetProperties();
			PropertyInfo[] targetProperties = typeof(T).GetProperties();

			foreach (PropertyInfo sourceProp in sourceProperties)
			{
				PropertyInfo targetProp = targetProperties.FirstOrDefault(x => x.Name == sourceProp.Name & x.CanWrite);
				targetProp?.SetValue(target, sourceProp.GetValue(source));
			}

			return target;
		}
	}
}
