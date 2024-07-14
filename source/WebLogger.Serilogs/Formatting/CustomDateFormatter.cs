using System;
using System.Globalization;

namespace WebLogger
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.IFormatProvider" />
    public sealed class CustomDateFormatter : IFormatProvider
    {
        private readonly IFormatProvider _basedOn;
        private readonly string _shortDatePattern;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomDateFormatter"/> class.
        /// </summary>
        /// <param name="shortDatePattern">The short date pattern.</param>
        /// <param name="basedOn">The based on.</param>
        public CustomDateFormatter(string shortDatePattern, IFormatProvider basedOn)
        {
            _shortDatePattern = shortDatePattern;
            _basedOn = basedOn;
        }
        /// <summary>
        /// Returns an object that provides formatting services for the specified type.
        /// </summary>
        /// <param name="formatType">An object that specifies the type of format object to return.</param>
        /// <returns>
        /// An instance of the object specified by <paramref name="formatType" />, if the <see cref="T:System.IFormatProvider" />
        /// implementation can supply that type of object; otherwise, <see langword="null" />.
        /// </returns>
        public object GetFormat(Type formatType)
        {
            if (formatType != typeof(DateTimeFormatInfo)) 
                return this._basedOn.GetFormat(formatType);

            if (!(_basedOn.GetFormat(formatType) is DateTimeFormatInfo basedOnFormatInfo)) 
                return null;

            var dateFormatInfo = (DateTimeFormatInfo)basedOnFormatInfo.Clone();
            dateFormatInfo.ShortDatePattern = this._shortDatePattern;
            return dateFormatInfo;
        }
    }
}
