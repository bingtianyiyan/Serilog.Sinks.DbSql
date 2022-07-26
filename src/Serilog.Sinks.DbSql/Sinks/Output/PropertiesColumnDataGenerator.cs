using Serilog.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Serilog.Sinks.DbSql.Output
{
    internal class PropertiesColumnDataGenerator : IPropertiesColumnDataGenerator
    {
        private readonly ColumnOptions _columnOptions;
        private readonly ISet<string> _standardColumnNames;

        public PropertiesColumnDataGenerator(ColumnOptions columnOptions)
        {
            _columnOptions = columnOptions ?? throw new ArgumentNullException(nameof(columnOptions));

            _standardColumnNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var stdCol in _columnOptions.Store)
            {
                var col = _columnOptions.GetStandardColumnOptions(stdCol);
                _standardColumnNames.Add(col.ColumnName);
            }
        }

        //changes
        public IEnumerable<KeyValuePair<string, object>> ConvertPropertiesToColumn(IReadOnlyDictionary<string, LogEventPropertyValue> properties)
        {
            foreach (var additionalColumn in _columnOptions.AdditionalColumns)
            {
                var property = properties.Where(x => x.Key == additionalColumn.PropertyName).FirstOrDefault();

                var columnName = additionalColumn.ColumnName;
                var columnType = additionalColumn.AsDataColumn().DataType;
                //only for
                if (property.Key == null && additionalColumn.AllowNull)
                {
                    yield return new KeyValuePair<string, object>(columnName, DBNull.Value);
                    continue;
                }

                if (!(property.Value is ScalarValue scalarValue))
                {
                    yield return new KeyValuePair<string, object>(columnName, property.Value.ToString());
                    continue;
                }

                if (scalarValue.Value == null && additionalColumn.AllowNull)
                {
                    yield return new KeyValuePair<string, object>(columnName, DBNull.Value);
                    continue;
                }

                if (columnType.IsAssignableFrom(scalarValue.Value.GetType()))
                {
                    yield return new KeyValuePair<string, object>(columnName, scalarValue.Value);
                    continue;
                }

                if (TryChangeType(scalarValue.Value, columnType, out var conversion))
                {
                    yield return new KeyValuePair<string, object>(columnName, conversion);
                }
                else
                {
                    yield return new KeyValuePair<string, object>(columnName, property.Value.ToString());
                }
            }

            //foreach (var property in properties)
            //{
            //    var additionalColumn = _columnOptions
            //        .AdditionalColumns
            //        .FirstOrDefault(ac => ac.PropertyName == property.Key);

            //    if (additionalColumn == null || _standardColumnNames.Contains(property.Key))
            //        continue;

            //    var columnName = additionalColumn.ColumnName;
            //    var columnType = additionalColumn.AsDataColumn().DataType;

            //    if (!(property.Value is ScalarValue scalarValue))
            //    {
            //        yield return new KeyValuePair<string, object>(columnName, property.Value.ToString());
            //        continue;
            //    }

            //    if (scalarValue.Value == null && additionalColumn.AllowNull)
            //    {
            //        yield return new KeyValuePair<string, object>(columnName, DBNull.Value);
            //        continue;
            //    }

            //    if (columnType.IsAssignableFrom(scalarValue.Value.GetType()))
            //    {
            //        yield return new KeyValuePair<string, object>(columnName, scalarValue.Value);
            //        continue;
            //    }

            //    if (TryChangeType(scalarValue.Value, columnType, out var conversion))
            //    {
            //        yield return new KeyValuePair<string, object>(columnName, conversion);
            //    }
            //    else
            //    {
            //        yield return new KeyValuePair<string, object>(columnName, property.Value.ToString());
            //    }
            //}
        }

        private static bool TryChangeType(object obj, Type type, out object conversion)
        {
            conversion = null;
            try
            {
                conversion = TypeDescriptor.GetConverter(type).ConvertFrom(obj);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}