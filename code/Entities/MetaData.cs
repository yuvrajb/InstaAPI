using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Entities
{
    [Serializable]
    public class MetaData
    {
        /// <summary>
        ///     <para>gets or sets the api response code</para>
        ///     <para>set to default value 10 if meta data not exclusively set</para>
        /// </summary>
        public int Code = 10;
        /// <summary>
        ///     <para>gets or sets the error type if web exception is thrown</para>
        /// </summary>
        public String ErrorType = "No Error";
        /// <summary>
        ///     <para>gets or set the error message if web exception is thrown</para>
        /// </summary>
        public String ErrorMessage = "No Error Message";
    }
}
