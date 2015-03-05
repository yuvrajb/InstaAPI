﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaAPI.Entities
{
    [Serializable]
    public class VideosData
    {
        public LowResolutionVideo LowResolution;
        public StandardResolutionVideo StandardResolution;
    }
}