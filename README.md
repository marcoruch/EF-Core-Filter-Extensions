# EF-Core-Filter-Extensions
Filters IQueryable&lt;T> by  DateTime, Number, String, Bool and Guid.

Allows to filter accross multiple Entities Parameters instead of Selecting and then Applying the filters


FilterExtensions.cs Calling Method:
```
.ApplyFilters(Parameter)
```

Example of adding a Filter:

```
if (Parameter.PriorityFilter != null)
{
    Expression<Func<WorkItemEntity, decimal>> expr = (WorkItemEntity w) => w.Priority;
    query = query.ApplyFilter(new KeyValuePair<Expression<Func<WorkItemEntity, decimal>>, FilterModel<NumberFilterType>>(expr, Parameter.PriorityFilter));
}
```
