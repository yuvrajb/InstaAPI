using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Entities
{
    [Serializable]
    public class RelationshipData
    {
        /// <summary>
        ///     <para>gets or sets the outgoing relationship status</para>
        /// </summary>
        public String OutgoingStatus = String.Empty;
        /// <summary>
        ///     <para>gets or sets the incoming relationship status</para>
        /// </summary>
        public String IncomingStatus = String.Empty;
        /// <summary>
        ///     <para>gets or sets the privacy of the target user</para>
        /// </summary>
        public Boolean TargetUserIsPrivate = false;
    }
}
