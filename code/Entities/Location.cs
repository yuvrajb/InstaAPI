using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Entities
{
    [Serializable]
    public class LocationData
    {
        /// <summary>
        ///     <para>gets or sets the id of the location</para>
        /// </summary>
        public String Id;
        /// <summary>
        ///     <para>gets or sets the latitude of the location</para>
        /// </summary>
        public String Latitude;
        /// <summary>
        ///     <para>gets or sets the longitude of the location</para>
        /// </summary>
        public String Longitude;
        /// <summary>
        ///     <para>gets or sets the name of the location</para>
        /// </summary>
        public String Name;
    }
}
