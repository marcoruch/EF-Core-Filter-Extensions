using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models.Filter
{
    public class FilterModel<TFilterType>
    {
        public TFilterType FilterType { get; set; }
        /// <summary>
        /// Model used to pass Values, when the Filter is not a "Between" Filter, the first Value will be used.
        /// </summary>
        public object[] Value { get; set; }
        public string Name { get; set; }

        public FilterModel(TFilterType filterType, ValueModel value)
        {
            FilterType = filterType;
            Value = value;
        }
    }
}
