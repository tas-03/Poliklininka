# Poliklininka
# Poliklininka — Система управления поликлиникой

Учебный проект на C# — десктопное приложение для управления поликлиникой.

## Технологии

- **WPF** — пользовательский интерфейс
- **MVVM** — паттерн архитектуры
- **Entity Framework Core** — работа с базой данных
- **PostgreSQL** — база данных
- **.NET DI Container** — внедрение зависимостей

---

## Структура проекта

```
Poliklininka/
├── Assets/                        # Изображения и ресурсы
├── Core/
│   ├── BaseViewModel.cs           # Базовый класс для всех ViewModel
│   └── RelayCommand.cs            # Реализация ICommand
├── Entities/                      # Сущности базы данных
│   ├── User.cs
│   ├── Patient.cs
│   ├── Doctor.cs
│   ├── Appointment.cs
│   ├── MedCard.cs
│   ├── VisitHistory.cs
│   ├── BloodGroup.cs
│   ├── MedService.cs
│   └── Schedule.cs
├── Helpers/
│   └── PhotoConverter.cs          # Конвертация фото byte[] ↔ ImageSource
├── Infrastructure/
│   ├── ADO/                       # ADO.NET подключение
│   └── EF/
│       ├── ApplicationDbContext.cs
│       ├── ApplicationDbContextFactory.cs
│       └── Migrations/
├── Services/
│   ├── IAuthService.cs            # Интерфейс авторизации
│   ├── EFAuthService.cs           # Реализация авторизации через EF Core
│   ├── IPatientService.cs         # Интерфейс сервиса пациента
│   └── EFPatientService.cs        # Реализация сервиса пациента через EF Core
├── ViewModels/
│   ├── Auth_Model/
│   │   └── AuthModel.cs           # ViewModel авторизации
│   ├── Patient_Model/
│   │   ├── PatientViewModel.cs    # ViewModel главного окна пациента
│   │   └── Anketa_RedViewModel.cs # ViewModel редактирования профиля
│   ├── Doctor_Model/              # (в разработке)
│   └── Admin_Model/               # (в разработке)
├── Views/
│   ├── Auth_View/
│   │   └── Login_Window.xaml      # Окно авторизации
│   ├── Patient_View/
│   │   ├── PatientWindow.xaml     # Главное окно пациента
│   │   ├── Controls/              # Переиспользуемые контролы
│   │   └── Dialogs/
│   │       └── Anketa_Red.xaml    # Диалог редактирования профиля
│   ├── Doctor_View/               # (в разработке)
│   └── Admin_View/                # (в разработке)
└── App.xaml                       # Точка входа, настройка DI контейнера
```

---

## Архитектура

Проект построен по паттерну **MVVM** с сервисным слоем:

```
View (XAML)
    ↕ Binding
ViewModel
    ↕ Interface
Service (EF Core)
    ↕
PostgreSQL
```

### Внедрение зависимостей

Все зависимости регистрируются в `App.xaml.cs` через `Microsoft.Extensions.DependencyInjection`:

```csharp
services.AddDbContext<ApplicationDbContext>();
services.AddScoped<IAuthService, EFAuthService>();
services.AddScoped<IPatientService, EFPatientService>();
```

---

## Роли пользователей

| Роль | Статус |
|---|---|
| Пациент | ✅ В разработке |
| Доктор | 🔲 Планируется |
| Администратор | 🔲 Планируется |

---

## Функциональность пациента

- [x] Авторизация
- [x] Просмотр профиля (ФИО, телефон, адрес, фото, полис)
- [x] Редактирование профиля
- [ ] Просмотр медкарты
- [ ] Запись на приём
- [ ] История визитов

---

## Запуск проекта

1. Клонировать репозиторий
2. Настроить строку подключения в `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Database=poliklininka;Username=postgres;Password=ваш_пароль"
  }
}
```
3. Применить миграции:
```bash
dotnet ef database update
```
4. Запустить проект через Visual Studio
