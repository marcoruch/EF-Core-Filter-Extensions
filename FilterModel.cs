using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models.Filter
{
    public class FilterModel<TFilterType>
    {
        public TFilterType FilterType { get; set; }
        /// <summary>
        /// Array used to pass Values, when the Filter is not a "Between" Filter, the first Value will be used.
        /// When the Filter is a Between Value [0] and [1] will be used
        /// When the Filter is a Number-Contains or a Guid-Contains [0]...[n] will be used
        /// </summary>
        public object[] Values { get; set; }
        public string Name { get; set; }

        public FilterModel(TFilterType filterType, ValueModel value)
        {
            FilterType = filterType;
            Value = value;
        }
    }
}
