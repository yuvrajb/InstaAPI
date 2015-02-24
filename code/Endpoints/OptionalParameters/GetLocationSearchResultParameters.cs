using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Endpoints
{
    public class GetLocationSearchResultParameters
    {
        /// <summary>
        ///     <para>gets or sets the distance</para>
        /// </summary>
        public String Distance = "1km";
        /// <summary>
        ///     <para>gets or sets the facbook places id</para>
        /// </summary>
        public String FacebookPlacesId = String.Empty;
        /// <summary>
        ///     <para>gets or sets the foursquare v2 id</para>
        /// </summary>
        public String FoursquareV2Id = String.Empty;
    }
}
