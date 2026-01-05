using System.Data;
using System.Reflection;

namespace PaymentTrackingSystem.Common.Helpers.Extensions
{
    public static class ConvertDataTableToGenericList
    {
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            //TODO add validation here to ensure all columns are mapped to property Error out if not

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    //in case you have a enum/GUID datatype in your model
                    //We will check field's dataType, and convert the value in it.
                    if (pro.Name.ToLower() == column.ColumnName.Replace("_", "").ToLower())
                    {
                        var convertedValue = GetValueByDataType(pro.PropertyType, dr[column.ColumnName]);
                        pro.SetValue(obj, convertedValue, null);
                    }
                }
            }
            return obj;
        }

        private static object GetValueByDataType(Type propertyType, object o)
        {
            if (o.ToString() == "null" || o.ToString() == "")
            {
                return null;
            }
            if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
            {
                return Guid.Parse(o.ToString());
            }
            else if (propertyType == typeof(int) || propertyType.IsEnum)
            {
                return Convert.ToInt32(o);
            }
            else if (propertyType == typeof(decimal))
            {
                return Convert.ToDecimal(o);
            }
            else if (propertyType == typeof(long))
            {
                return Convert.ToInt64(o);
            }
            else if (propertyType == typeof(bool) || propertyType == typeof(bool?))
            {
                return Convert.ToBoolean(o);
            }
            else if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
            {
                return Convert.ToDateTime(o);
            }
            else if (propertyType == typeof(float) || propertyType == typeof(float?))
            {
                return Convert.ToSingle(o);
            }
           
            return o.ToString();
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
        Func<TSource,
        TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
