Проблема при Update:
```
The instance of entity type 'User' cannot be tracked because another instance with the same key value for {'Id'} is already being tracked.
```

Решения:
1. AsNoTracking
2. Использование маппинга или автомапперов
3. Detach, но [надо обходить весь возможный граф объектов](https://stackoverflow.com/questions/70095949/the-instance-of-entity-type-cannot-be-tracked-because-another-instance-with-the#70103298)