using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CsvToEntityDemo.Exceptions;
using CsvToEntityDemo.Interfaces;

namespace CsvToEntityDemo
{
    public class CsvLoader : ICsvLoader
    {
        public List<string> GetColumnNamesFromHeader(string headerLine)
        {
            List<string> columnNames = GetItemsFromCsvLine(headerLine);
            return columnNames.ToList();
        }

        public T GetEntityFromRow<T>(string rowLine, List<string> fieldNames) 
            where T : class, new() 
        {
            if (fieldNames == null || !fieldNames.Any())
            {
                return null;
            }

            T entity = new T();

            List<string> fieldValues = GetItemsFromCsvLine(rowLine);
            PropertyInfo[] properties = typeof(T).GetProperties();

            for (int i = 0; i < fieldNames.Count; i++)
            {
                string fieldName = fieldNames[i];
                string fieldValue = fieldValues[i];

                PropertyInfo property = properties.SingleOrDefault(p => p.Name == fieldName);
                if (property == null)
                {
                    throw new FieldException(fieldName, $"Could not create entity: {typeof(T).Name} - Unknown field");
                }

                if (!property.CanWrite)
                {
                    throw new FieldException(fieldName, $"Could not create entity: {typeof(T).Name} - Field is readonly");
                }

                try
                {
                    property.SetValue(entity, Convert.ChangeType(fieldValue, property.PropertyType));
                }
                catch (Exception ex)
                {
                    throw new FieldException(
                        fieldName, 
                        $"Could not create entity: {typeof(T).Name} - Incompatible value: {fieldValue}", 
                        ex);
                }
            }

            return entity;
        }

        public Type GetEntityTypeFromFilename(string filename)
        {
            string baseName = filename.Replace(".csv", "");
            string[] parts = baseName.Split('/');
            string typeName = $"CsvToEntityDemo.Models.{parts[parts.Length - 1]}";
            try
            {
                Type entityType = Type.GetType(typeName, true);
                return entityType;
            }
            catch (Exception ex)
            {
                throw new TypeException(typeName, "Unknown type", ex);
            }
        }

        private List<string> GetItemsFromCsvLine(string line)
        {
            if (string.IsNullOrEmpty(line))
            {
                return null;
            }

            string[] items = line.Split(',');
            return items
                .Select(c => c.Trim())
                .ToList();
        }
    }
}
