using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Entities
{
    [Serializable]
    public class LocationInfoData
    {
        /// <summary>
        ///     <para>gets or sets the response meta data</para>
        /// </summary>
        public MetaData Meta;
        /// <summary>
        ///     <para>gets or sets the location information</para>
        /// </summary>
        public LocationData Data;
    }
}
