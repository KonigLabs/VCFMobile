﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Mobile.Model
{
    public class Filter
    {
        public string Header { set; get; }
        public ICollection<FilterItem> Items { set; get; }
    }
}
