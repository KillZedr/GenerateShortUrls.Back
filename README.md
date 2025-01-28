# GenerateShortUrls
________________________________________
Документация по проекту: Генерация коротких ссылок
Описание проекта
Сервис для генерации и управления короткими ссылками, реализованный на ASP.NET Core. Проект использует PostgreSQL в качестве базы данных, JWT-аутентификацию для защиты API, а также поддерживает тестирование и контейнеризацию с помощью Docker.
________________________________________
Технологии
•	Backend: ASP.NET Core 8.0
•	Database: PostgreSQL
•	ORM: Entity Framework Core
•	JWT Authentication: Для защиты API
•	Docker: Для контейнеризации
•	pgWeb: Веб-интерфейс для работы с базой данных
•	Логирование: Serilog (включено в проект)
________________________________________
Зависимости проекта
Основные пакеты

    <PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
    <PackageReference Include="FluentValidation" Version="11.11.0" />
    <PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.12" />
    <PackageReference Include="Microsoft.AspNetCore.Identity" Version="2.3.0" />
    <PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="8.0.12" />
    <PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="7.0.14" />
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.1" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="9.0.1">
        <PrivateAssets>all</PrivateAssets>
        <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="Microsoft.VisualStudio.Azure.Containers.Tools.Targets" Version="1.19.5" />
    <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="9.0.3" />
    <PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />

Пакеты для тестирования

    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.6.0" />
    <PackageReference Include="MockQueryable.Moq" Version="7.0.3" />
    <PackageReference Include="Moq" Version="4.20.72" />
    <PackageReference Include="Moq.EntityFrameworkCore" Version="9.0.0.1" />
    <PackageReference Include="xunit" Version="2.4.2" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.4.5">
        <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
        <PrivateAssets>all</PrivateAssets>
    </PackageReference>
    <PackageReference Include="coverlet.collector" Version="6.0.0">
        <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
        <PrivateAssets>all</PrivateAssets>
    </PackageReference>

________________________________________
Инструкция по запуску проекта локально
1. Установите необходимые инструменты
Перед началом работы убедитесь, что на вашей машине установлены:
•	.NET SDK 8.0
•	Docker и Docker Compose
•	PostgreSQL (если не используете Docker для базы данных)
2. Настройка строки подключения
В файле appsettings.json проверьте корректность строки подключения к PostgreSQL:
"ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=UrlShortenerDb;Username=postgres;Password=KIDPay321;"
}
Если используется Docker, настройка базы данных уже включена в docker-compose.yml.
3. Запустите Docker-контейнеры
Выполните команду в корне проекта:
docker-compose up
Это запустит следующие сервисы:
•	PostgreSQL (доступен на localhost:5433)
•	pgWeb (доступен на http://localhost:9091)
4. Запуск проекта
1.	Откройте проект в IDE (например, Visual Studio или Rider).
2.	Убедитесь, что выбран режим запуска Development.
3.	Выполните команду для запуска приложения: 
4.	dotnet run
5.	Приложение будет доступно по адресу:
https://localhost:5001/swagger
5. Тестирование
Для запуска тестов выполните:
dotnet test
6. Работа с базой данных через pgWeb
1.	Перейдите на http://localhost:9091.
2.	Войдите, используя следующие данные: 
o	Host: db
o	Port: 5432
o	Username: postgres
o	Password: KIDPay321
o	Database: UrlShortenerDb
________________________________________

