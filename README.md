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

* **Об'єднання проєктів UserManager та Shop (Єдина архітектура та спільна БД)**:
  * Поєднано модуль користувачів (`UserManager`) та модуль магазину/замовлень (`Shop`) в єдину багатошарову структуру із централізованим Dependency Injection (DI) та спільним контекстом `AppDbContext`.
  * Реалізовано реляційні зв'язки між сутностями:
    * **User (1) ↔ (N) Order**: користувач має список замовлень `Orders`, у замовленні наявне зовнішнє посилання `UserId` та навігаційна властивість `User`.
    * **Order (1) ↔ (N) OrderItem**: кожне замовлення містить перелік позицій `Items`, позиція зберігає `OrderId` та посилання на `Order`.
    * **OrderItem (N) ↔ (1) Product**: позиція замовлення посилається на конкретний товар через `ProductId` (`Guid?`) та властивість `Product`.
* **Product (Товар) & FluentValidation + Global Validation Filter**
* **User (Користувач / Клієнт)**
* **Order & OrderItem (Замовлення та товарні позиції)**
* **Book (Книги)**
* **Invoice (Рахунки/Інвойси)**
* **Project & TaskItem (Проєкти та задачі)**

---
## 🗂 Простори імен та робота із контролерами
### Замість `<сутність>` треба замінити на ту сутність, з якою треба працювати, наприклад: `Product`:
***Наразі існують такі сутності в цьому проєкті:***
* **Product -> `Product`**
* **ProductEntity -> `ProductRepo`** (для роботи з патерном Repository)
* **Order & OrderItem -> `Order`, `OrderItem`**
* **User -> `User`**
* **Book -> `Book`**
* **Invoice -> `Invoice`**
* **Project & TaskItem -> `Project`**

<b>Налаштування дозволених контролерів у Program.cs</b>

<br>

Ви можете обрати, з якими контролерами працювати наразі. Для цього просто додайте в метод параметр `"<назва>"` (замість `<назва>` вставте назву сутності, з якою хочете працювати):

```csharp
// Реєстрація контролерів із глобальним фільтром валідації
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilterAttribute>();

    // Дозволено контролери модулів UserManager та Shop
    options.Conventions.Add(new OnlyAllowedControllersConvention("User", "Product", "Order", "OrderItem"));
});
```

> Повний код конфігурації дивіться у файлі: [`Program.cs`](./AspNetAssignments/Program.cs)

### Самі простори імен:

* **Data** — `using AspNetAssignments.Data;`
* **Models** — `using AspNetAssignments.Models.<сутність>;`
* **Entity** — `using AspNetAssignments.Entity.<сутність>;`
* **DTOs** — `using AspNetAssignments.DTOs.<сутність>;`
* **DTOs (Pagination)** — `using AspNetAssignments.DTOs.Pagination;`
* **DTOs (Common)** — `using AspNetAssignments.DTOs.Common;`
* **Validators** — `using AspNetAssignments.Validators.<сутність>;`
* **Repository** — `using AspNetAssignments.Repository.<сутність>;`
* **Services (Abstract)** — `using AspNetAssignments.Services.Abstract;`
* **Services** — `using AspNetAssignments.Services;`
* **Filters** — `using AspNetAssignments.Filters;`
* **Middleware** — `using AspNetAssignments.Middleware;` 
* **Controllers** — `using AspNetAssignments.Controllers;`
* **Mapping** — `using AspNetAssignments.Mapping;` 
* **Exceptions** — `using AspNetAssignments.Exceptions;` 
* **Root / Program** — `using AspNetAssignments;` 

---

## 🌿 Робота з Git та гілками, як виконується кожна робота:

* Для кожного нового завдання (класної, практичної чи домашньої роботи) створюється окрема тематична гілка:
  * `classwork-dd-mm-yyyy` 
  * `hw-dd-mm-yyyy`
  * `practice-dd-mm-yyyy` 
  
(Де dd, mm та yyyy — день, місяць та рік, коли робили або видали завдання):  
* Усі логічно завершені етапи фіксуються атомарними коммітами за конвенцією **Conventional Commits**:  
  `feat(<гілка_роботи>): implement ValidationFilterAttribute for automated FluentValidation`, `feat(<гілка_роботи>): add RequestLoggingMiddleware with Stopwatch timing`, `feat: register global action filter in Program.cs` тощо.
* Замість `<гілка_роботи>` вставляється назва гілки, наприклад: `classwork-31-10-2026`, `hw-01-11-2026` тощо.
* Після перевірки та тестування гілка зливається в основну гілку `main`.
 

---

## 🚀 Остання влита робота:
### Класна робота за 07.11.2026 (classwork-07-11-2026):
* **Було реалізовано:**
  * Виправлено створення `UserController` у модульному тесті `UserControllerTests`.
  * Додано mock-об’єкт для залежності `IOrderService`.
  * Передано необхідні залежності `IUserService` та `IOrderService` у конструктор контролера.
  * Налаштовано тест отримання користувача за ідентифікатором.
  * Перевірено, що метод `GetById` повертає успішний HTTP-відповідь `200 OK`, якщо користувач існує.
* **Чого навчилися:**
  * Створювати mock-об’єкти за допомогою бібліотеки `Moq`.
  * Передавати всі необхідні залежності під час тестування класів із використанням dependency injection.
  * Налаштовувати поведінку mock-сервісів через `Setup` і `ReturnsAsync`.
  * Перевіряти тип і статус результату дії контролера за допомогою `xUnit`.
  * Розуміти, що для створення контролера в тесті потрібно передати всі обов’язкові параметри його конструктора, навіть якщо конкретний тест не використовує кожну залежність.

### Підтвердження виконання (Скріншоти та результати тестування) для практичної або домашньої роботи:
### Класна робота за 07.11.2026 (classwork-07-11-2026):
* **Було реалізовано:**
### Підтвердження виконання (Скріншоти та результати тестування) для практичної або домашньої роботи:
![немає](images/0.jpg)
![немає](images/0.jpg)
![немає](images/0.jpg)
![немає](images/0.jpg)