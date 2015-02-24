using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Entities
{
    [Serializable]
    public class AttributionData
    {
        /// <summary>
        ///     <para>gets or sets the website of the third party app</para>
        /// </summary>
        public string Website;
        /// <summary>
        ///     <para>gets or sets the itunes url of the third party app</para>
        /// </summary>
        public string ItunesUrl;
        /// <summary>
        ///     <para>gets or sets the name of the third party app</para>
        /// </summary>
        public string Name;
    }
}
