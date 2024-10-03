# Тестовое задание Game Quests
## Описание задачи
Необходимо разработать REST API для системы управления квестами в игре. Система должна позволять игрокам
* Просматривать доступные квесты
* Принимать квесты
* Обновлять прогресс выполнения квестов
* Завершать квесты и получать награду

## Описание разработанного решения
Архитектура приложения представляет собой DDD и разделена на четыре части:

* API
* Domain
* Domain test
* Infrastructure

В проекте используется паттер Options, для описания [ограничений и параметров для пользователя] (https://github.com/VictorKomshn/GameQuests/blob/main/GameQuests.Domain/AggregatesModel/PlayerAggregate/PlayerOptions.cs) (максимальное количество квестов), а также [ConnectionString для БД] (https://github.com/VictorKomshn/GameQuests/blob/main/GameQuests.Infrastructure/Options/PostgreOptions.cs)

### Domain
[GameQuest.Domain](https://github.com/VictorKomshn/GameQuests/tree/main/GameQuests.Domain) Представляет собой "сердце" проекта, содержит в себе всю логику, основанной на сервисах, а также POCO объектах.
Объекты в проекте разделены на две папки - [AggregatesModel](https://github.com/VictorKomshn/GameQuests/tree/main/GameQuests.Domain/AggregatesModel), содержащую классы работы с отдельными объектами и [SeedWork](https://github.com/VictorKomshn/GameQuests/tree/main/GameQuests.Domain/SeedWork), содержащую базовые классы.

### API
Разработанное приложение основано на REST подходе, имеет два контроллера: для работы c пользователем(игроком) и для работы с квестами.
Объекты, возвращаемые методами представляют собой DTO ViewModels. 

### Infrastructure
В данной части проекта реализованы методы взаимодействия приложения с внешними источниками данных, в данном случае БД. Мной, в данном случае, было реализовано подключение к Postgres.
Для работы с таблицами был реализован базовый класс [RepositoryBase](https://github.com/VictorKomshn/GameQuests/blob/main/GameQuests.Infrastructure/Data/Repositories/RepositoryBase.cs), содержащий наиболее часто используемые методы. Все последующие репозитории наследуются от него.


## Багодарю за внимание
Буду благодарен за Ваши комментарии и обратную связь :)