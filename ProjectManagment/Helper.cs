using System.Reflection;

namespace ProjectManagment
{
    public abstract class Helper
    {
        public static void Copy<T>(T from, T to)
        {
            Type t = typeof(T);
            PropertyInfo[] props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in props)
            {
                if (!p.CanRead || !p.CanWrite) continue;

                object val = p.GetGetMethod().Invoke(from, null);
                object defaultVal = p.PropertyType.IsValueType ? Activator.CreateInstance(p.PropertyType) : null;
                if (null != defaultVal && !val.Equals(defaultVal))
                {
                    p.GetSetMethod().Invoke(to, new[] { val });
                }
            }
        }
    }
}
