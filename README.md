```markdown
# Minimal API with PostgreSQL

Минимальное API для работы с пользователями и их блогами в PostgreSQL

## 📋 Требования

- .NET 7+
- PostgreSQL 12+
- PgAdmin (или другой клиент для работы с БД)

## 🛠 Настройка базы данных

1. Создайте базу данных `TestDb` в PgAdmin
2. Выполните SQL-скрипты для создания таблиц:

```sql
-- Таблица пользователей
CREATE TABLE IF NOT EXISTS public.users
(
    id integer NOT NULL DEFAULT nextval('users_id_seq'::regclass),
    "firstName" character varying(30) COLLATE pg_catalog."default" NOT NULL,
    "lastName" character varying(50) COLLATE pg_catalog."default" NOT NULL,
    "dateOfBirth" date NOT NULL,
    CONSTRAINT users_pkey PRIMARY KEY (id)
)
TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.users OWNER to postgres;

-- Таблица блогов
CREATE TABLE IF NOT EXISTS public.blogs
(
    id serial NOT NULL,
    title character varying(30) NOT NULL,
    created date NOT NULL,
    context text NOT NULL,
    user_id integer NOT NULL,
    PRIMARY KEY (id),
    CONSTRAINT fk_user FOREIGN KEY (user_id)
        REFERENCES public.users (id) MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

ALTER TABLE IF EXISTS public.blogs OWNER to postgres;
```

## 🔧 Настройка проекта

1. Измените строку подключения в `Program.cs`:
```csharp
builder.Services.AddDbContext<MyContext>(o => 
    o.UseNpgsql("Host=localhost;Port=5432;Database=TestDb;Username=postgres;Password=251187"));
```

2. Установите зависимости:
```bash
dotnet restore
```

## 🚀 Запуск проекта

```bash
dotnet run
```
## 📚 API Endpoints

### Пользователи

| Метод | Путь | Описание |
|-------|------|----------|
| GET | `/users` | Получить всех пользователей |
| GET | `/users/{id}` | Получить пользователя по ID с его блогами |
| POST | `/users` | Создать нового пользователя |
| PUT | `/users/{id}` | Обновить данные пользователя |
| DELETE | `/users/{id}` | Удалить пользователя |

### Блоги

| Метод | Путь | Описание |
|-------|------|----------|
| GET | `/blogs/{id}` | Получить блог по ID |
| POST | `/blogs/{userId}` | Создать новый блог для пользователя |
| DELETE | `/blogs/{id}` | Удалить блог |

## 📊 Модели данных

### Пользователь (User)
```json
{
  "id": 1,
  "firstName": "Иван",
  "lastName": "Петров",
  "dateOfBirth": "1990-01-01"
}
```

### Блог (Blog)
```json
{
  "id": 1,
  "title": "Мой первый блог",
  "createdDate": "2023-10-01T00:00:00",
  "context": "Содержимое блога...",
  "userId": 1
}
```

## 🔒 Валидация

- Все ID проверяются на положительное значение
- Длина имени пользователя: 1-30 символов
- Длина фамилии пользователя: 1-50 символов
- Дата рождения не может быть в будущем

## 🛠 Технологии

- ASP.NET Core Minimal API
- Entity Framework Core 7
- PostgreSQL
- Swagger для документации API

## 📝 Примечания

Для доступа к Swagger UI перейдите по `/swagger` при запуске в Development-режиме

```