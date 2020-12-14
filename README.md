Лабораторная работа 4 Протасов Стаселько

Проект DataManager содержит классы DataManager, DatabaseManager, XMLGenerator, а также интерфейсы для устранения сильной связности;
DataManager - основной класс проекта, работающий со всеми подсистемами;
TransferProtocol и DataManager работают независимо друг от друга в разных потоках. DataManager добавляет файлы в папку, которая находится под наблюдением TransferProtocol, никак с ним не взаимодействуя; благодаря потоком они работают параллельно.
DatabaseManager предоставляет API для работы с базой через ADO.NET;
XMLGenerator занимается созданием строк файла xml;
Для конфигурации используется IDatabaseConfig, его реализует обычный Config решения.
Проект LogMangager содержит интерфейс ILogger и его конкретную реализацию LogManager. Он выводит информацию о работе в консоль и в базу данных, информацию от других модулей получает через событийную модель.