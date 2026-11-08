# ASP.NET Core Tasks
---
## 🗺️ Зміст (головний проєкт - AspNetAssignments)
* [📌 Структура проєкту](#-структура-проєкту)
* [🗂️ Простори імен та робота із контролерами](#-простори-імен-та-робота-із-контролерами)
* [🌿 Робота з Git та гілками](#-робота-з-git-та-гілками-як-виконується-кожна-робота)
* [🚀 Остання влита робота](#-остання-влита-робота)

---

Репозиторій містить практичні, класні та домашні завдання з курсу ***ASP.NET Core (.NET 10, C#, Visual Studio 2026, Entity Framework Core)***.

Усі початкові завдання збережено в гілці та далі в папці **[`Archive`](https://github.com/NightFuryL/ASP.NET_Tasks/tree/archive-branch)** (гілка не злита), після чого весь ключовий функціонал було централізовано та розбито на одне структуроване рішення.

---

## 📌 Структура проєкту
### Сюди винесено весь функціонал сутностей, з якими велася систематична робота протягом курсу:

* **Product (Товар) & FluentValidation + Global Validation Filter**:
* **User (Користувач / Клієнт)**:
* **Order & OrderItem (Замовлення та позиції)**:
* **Book (Книги)**
* **Invoice (Рахунки/Інвойси)**
* **Project & TaskItem (Проєкти та задачі)**

---
## 🗂 Простори імен та робота із контролерами
### Замість `<сутність>` требя замінити, на ту сутність, з якою треба працювати, але загальна назва, наприклад: `Product`:
***Наразі існують такі сутності в цьому проєкті:***
* **Product -> `Product`**
* **Order & OrderTime -> `Order`**
* **User -> `User`**
* **Book -> `Book`**
* **Invoice -> `Invoice`**
* **Project & TaskItem -> `Project`**

<b>Налаштування дозволених контролерів у Program.cs</b>

<br>

Ви можете обрати, з якими контролерами працювати наразі. Для цього просто дoдайте в метод параметр `"<назва>"` (замість `<назва>` вставте назву сутності, з якою хочете працювати, наприклад: `"Product"`):

```csharp
// Реєстрація контролерів із глобальним фільтром валідації
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilterAttribute>();

    // Рядок нижче потрібен для обмеження доступу до контролерів лише з дозволеного списку
    // Якщо список дозволених контролерів порожній, то доступ до всіх контролерів буде дозволений
    options.Conventions.Add(new OnlyAllowedControllersConvention("Product", "User"));
});
```

> Повний код конфігурації дивіться у файлі: [`Program.cs`](./AspNetAssignments/Program.cs)

### Самі простори імен:

* **Data** — `using AspNetAssignments.Data;`
* **Models** — `using AspNetAssignments.Models.<сутність>;`
* **DTOs** — `using AspNetAssignments.DTOs.<сутність>;`
* **DTOs (Pagination)** — `using AspNetAssignments.DTOs.Pagination;`
* **DTOs (Common)** — `using AspNetAssignments.DTOs.Common;`
* **Validators** — `using AspNetAssignments.Validators.<сутність>;`
* **Services (Abstract)** — `using AspNetAssignments.Services.Abstract;`
* **Services** — `using AspNetAssignments.Services;`
* **Filters** — `using AspNetAssignments.Filters;`
* **Middleware** — `using AspNetAssignments.Middleware;` 
* **Controllers** — `using AspNetAssignments.Controllers;`
* **Mapping** — `using AspNetAssignments.Mapping;` 
* **Exceptions** — `using AspNetAssignments.Exceptions;` 
* **Root / Program** — `using AspNetAssignments;` 

---

## 🌿 Робота з Git та гілками, як виконуєтсья кожна робота:

* Для кожного нового завдання (класної, практичної чи домашньої роботи) створюється окрема тематична гілка 
  * `classwork-dd-mm-yyyy` 
  * `hw-dd-mm-yyyy`
  * `practice-dd-mm-yyyy` 
  
(Де, dd, mm та yyy - день, місяць та рік(повна дата), коли робили або дали завдання):  
* Усі логічно завершені етапи фіксуються атомарними коммітами за конвенцією **Conventional Commits**:  
  `feat(<гілка_роботи>): implement ValidationFilterAttribute for automated FluentValidation`, `feat(<гілка_роботи>): add RequestLoggingMiddleware with Stopwatch timing`, `feat: register global action filter in Program.cs` тощо.
* Замість `<гілка_роботи>` вставляється назва гілки, наприклад: `classwork-31-10-2026`, `hw-15-11-2026` тощо.
* Після перевірки та тестування гілка зливається в основну гілку `main`.
 

---

## 🚀 Остання влита робота:
### Класна робота 31.10.2026
Остання актуальна робота, що була перевірена та влита в `main` (або не влита, та знаходиться в гілці classwork-31-10-2026):
### Підтвердження виконання (Скріншоти та результати тестування) для практичної або домашньої роботи:

![Немає](Images/0.png)
![Немає](Images/0.png)
![Немає](Images/0.png)
