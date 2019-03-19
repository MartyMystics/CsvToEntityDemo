using System;
using System.Collections.Generic;

namespace CsvToEntityDemo.Interfaces
{
    public interface ICsvLoader
    {
        List<string> GetColumnNamesFromHeader(string headerLine);

        T GetEntityFromRow<T>(string rowLine, List<string> fieldNames)
            where T : class, new();

        Type GetEntityTypeFromFilename(string filename);
    }
}
