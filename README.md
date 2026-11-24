# StudentApp (5-апта: ASP.NET Core Identity)

## Қолданылған нұсқалар
- .NET 10 SDK (ASP.NET Core MVC)
- SQLite (EF Core)

## Іске қосу
1. `dotnet restore`
2. Admin деректерін user-secrets-ке жазу:
   `dotnet user-secrets set "Admin:Email" "admin@studentapp.local"`
   `dotnet user-secrets set "Admin:Password" "Admin12345"`
3. `dotnet ef database update`
4. `dotnet run`

## Рөлдер
- Student: тіркелген кезде автоматты түрде беріледі, тізімді көреді.
- Admin: бастапқы толтыруда құрылады, студентті жоя алады.

## Қауіпсіздік
Құпиясөздер мен admin деректері репозиторийде жоқ (user-secrets). Logout және Delete тек POST + AntiForgeryToken арқылы жұмыс істейді.

## Қысқаша қорытынды
Мәселе: Admin құпиясөзін кодқа жазбай, қосымша бірінші іске қосылғанда admin құру. Шешім: user-secrets және SeedData.InitializeAsync (RoleManager + UserManager) қолдандым.
