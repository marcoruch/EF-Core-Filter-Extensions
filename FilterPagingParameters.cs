namespace Shared.Models
{
    public class FilterPagingParameters
    {
        public FilterModel<NumberFilterType>[] NumberFilters { get; set; }
        public FilterModel<StringFilterType>[] StringFilters { get; set; }
        public FilterModel<DateTimeFilterType>[] DateTimeFilters { get; set; }
        public FilterModel<BoolFilterType>[] BoolFilters { get; set; }
        public FilterModel<GuidFilterType>[] GuidFilters { get; set; }
    }
}
