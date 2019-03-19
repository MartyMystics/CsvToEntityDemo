using System;

namespace CsvToEntityDemo.Exceptions
{
    public class FieldException : Exception
    {
        /// <summary>
        /// The name of the problematic field.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Constructor with error message.
        /// </summary>
        /// <param name="fieldName">The name of the problematic field.</param>
        /// <param name="errorMessage">The associated error message.</param>
        public FieldException(string fieldName, string errorMessage)
            : base(errorMessage)
        {
            FieldName = fieldName;
        }

        /// <summary>
        /// Constructor with formatted error message.
        /// </summary>
        /// <param name="fieldName">The name of the problematic field.</param>
        /// <param name="errorMessageFormat">The associated error message format.</param>
        /// <param name="args">The arguments to include in the error message format.</param>
        public FieldException(string fieldName, string errorMessageFormat, params object[] args)
            : base(string.Format(errorMessageFormat, args))
        {
            FieldName = fieldName;
        }

        /// <summary>
        /// Constructor with error message and inner exception.
        /// </summary>
        /// <param name="fieldName">The name of the problematic field.</param>
        /// <param name="errorMessage">The associated error message.</param>
        /// <param name="innerException">The associated inner exception.</param>
        public FieldException(string fieldName, string errorMessage, Exception innerException)
            : base(errorMessage, innerException)
        {
            FieldName = fieldName;
        }

        /// <summary>
        /// Constructor with formatted error message and inner exception.
        /// </summary>
        /// <param name="fieldName">The name of the problematic field.</param>
        /// <param name="errorMessageFormat">The associated error message format.</param>
        /// <param name="innerException">The associated inner exception.</param>
        /// <param name="args">The arguments to include in the error message format.</param>
        public FieldException(
            string fieldName, 
            string errorMessageFormat, 
            Exception innerException, 
            params object[] args)
            : base(string.Format(errorMessageFormat, args), innerException)
        {
            FieldName = fieldName;
        }
    }
}
