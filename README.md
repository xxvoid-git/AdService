# AdService - Рекламные площадки

Мини-сервис для подбора рекламных площадок по локациям.
Данные хранятся только в памяти. Быстрый поиск реализован через префиксное дерево (Trie).

## Инструкция по запуску веб-сервиса

### Требования
    - Установленный [.NET SDK 9]
    - Windows + Git Bash / PowerShell / VS Code (любая среда)

### Шаги запуска
1. После клонирования репозитория, нужно перейти в папку с сервисом:
    cd src/AdService

2. Запустить сервис:
    dotnet run

3. После запуска сервис будет доступен:
    Swagger UI: http://localhost:5000/swagger
    REST API: http://localhost:5000/api/platforms

4. Загрузка рекламных площадок из файла:
    POST http://localhost:5000/api/platforms/upload
    Тело запроса: multipart/form-data с ключом file и файлом platforms.txt

    Пример файла platforms.txt:
        Яндекс.Директ:/ru
        Ревдинский рабочий:/ru/svrd/revda,/ru/svrd/pervik
        Газета уральских москвичей:/ru/msk,/ru/permobl,/ru/chelobl
        Крутая реклама:/ru/svrd

    Ответ при успешной загрузке:
        Platforms loaded

    Данный файл располагается по пути src/AdService/sample-data/platforms.txt

5. Поиск рекламных площадок:
    GET http://localhost:5000/api/platforms/search?location=/ru/svrd/revda

    Пример ответа:
        ["Яндекс.Директ","Ревдинский рабочий","Крутая реклама"]

### Юнит-тесты
Чтобы запустить тесты:
    cd tests/AdService.Tests
    dotnet test
