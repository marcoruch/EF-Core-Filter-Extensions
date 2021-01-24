# EF-Core-Filter-Extensions
Filters IQueryable&lt;T> by  DateTime, Number, String, Bool and Guid.



FilterExtensions.cs Calling Method:
```
            .ApplyFilters(Parameter)
```

FilterExtensions2. Calling Method:

```
if (Parameter.PriorityFilter != null)
{
    Expression<Func<WorkItemEntity, decimal>> expr = (WorkItemEntity w) => w.Priority;
    query = query.ApplyFilter(new KeyValuePair<Expression<Func<WorkItemEntity, decimal>>, FilterModel<NumberFilterType>>(expr, Parameter.PriorityFilter));
}
```
