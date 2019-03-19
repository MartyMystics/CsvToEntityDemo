using System;

namespace CsvToEntityDemo.Exceptions
{
    public class TypeException : Exception
    {
        /// <summary>
        /// The name of the problematic type name.
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Constructor with error message.
        /// </summary>
        /// <param name="typeName">The name of the problematic type.</param>
        /// <param name="errorMessage">The associated error message.</param>
        public TypeException(string typeName, string errorMessage)
            : base(errorMessage)
        {
            TypeName = typeName;
        }

        /// <summary>
        /// Constructor with formatted error message.
        /// </summary>
        /// <param name="typeName">The name of the problematic type.</param>
        /// <param name="errorMessageFormat">The associated error message format.</param>
        /// <param name="args">The arguments to include in the error message format.</param>
        public TypeException(string typeName, string errorMessageFormat, params object[] args)
            : base(string.Format(errorMessageFormat, args))
        {
            TypeName = typeName;
        }

        /// <summary>
        /// Constructor with error message and inner exception.
        /// </summary>
        /// <param name="typeName">The name of the problematic type.</param>
        /// <param name="errorMessage">The associated error message.</param>
        /// <param name="innerException">The associated inner exception.</param>
        public TypeException(string typeName, string errorMessage, Exception innerException)
            : base(errorMessage, innerException)
        {
            TypeName = typeName;
        }

        /// <summary>
        /// Constructor with formatted error message and inner exception.
        /// </summary>
        /// <param name="typeName">The name of the problematic type.</param>
        /// <param name="errorMessageFormat">The associated error message format.</param>
        /// <param name="innerException">The associated inner exception.</param>
        /// <param name="args">The arguments to include in the error message format.</param>
        public TypeException(
            string typeName,
            string errorMessageFormat,
            Exception innerException,
            params object[] args)
            : base(string.Format(errorMessageFormat, args), innerException)
        {
            TypeName = typeName;
        }
    }
}
