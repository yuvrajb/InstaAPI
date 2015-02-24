using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Entities
{
    [Serializable]
    public class UserRelationship
    {
        /// <summary>
        ///     <para>gets or sets the response meta data</para>
        /// </summary>
        public MetaData Meta;
        /// <summary>
        ///     <para>gets or sets the relationship data</para>
        /// </summary>
        public RelationshipData Data;
    }
}
