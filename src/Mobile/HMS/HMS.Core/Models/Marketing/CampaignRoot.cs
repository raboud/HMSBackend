﻿namespace HMS.Core.Models.Marketing
{
    using System.Collections.Generic;

    public class CampaignRoot
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public List<CampaignItem> Data { get; set; }
    }
}