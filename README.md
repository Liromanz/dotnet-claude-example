# MetricBoard

Демонстрационное приложение для сбора метрик: два DDD/CQRS-микросервиса на .NET 10
(`Dictionaries` и `Analytics`), общающихся через имитацию единого Kafka-топика, и
React 18+/Vite/TanStack Router фронтенд в стиле редизайна 10 прототипа "Data Tracker".

## Структура

```
backend/
  src/Common/*            — общие Domain/Application/Infrastructure/Api библиотеки
  src/Broker/Broker.Api    — имитация единого Kafka-топика (HTTP)
  src/Services/Dictionaries/* — владеет справочником "Темы"
  src/Services/Analytics/*    — владеет "Статьями" и счётчиком просмотров (метрика)
  db/dictionaries, db/analytics — Liquibase changelog'и и SQLite-файлы (создаются при update)
frontend/
  src/pages/*       — атомарные страницы (View/Store/Hooks/Styles)
  src/components/*  — переиспользуемые UI-компоненты
  src/api/*         — типизированные клиенты к Dictionaries.Api и Analytics.Api
  src/directories.ts — реестр страниц/навигации для TanStack Router
```

## Запуск (без Docker)

Требуется: .NET 10 SDK, Node.js 20+, Liquibase CLI.

**1. Накатить схему БД** (один раз, или при сбросе данных):

```bash
cd backend/db/dictionaries && liquibase update
cd ../analytics && liquibase update
```

**2. Поднять три бэкенд-процесса** (разные терминалы, из `backend/`):

```bash
dotnet run --project src/Broker/Broker.Api --urls http://localhost:5080
dotnet run --project src/Services/Dictionaries/Dictionaries.Api --urls http://localhost:5081
dotnet run --project src/Services/Analytics/Analytics.Api --urls http://localhost:5082
```

**3. Поднять фронтенд**:

```bash
cd frontend
npm install
npm run dev
```

Открыть http://localhost:5173 — Vite проксирует `/api/dictionaries/*` → 5081 и
`/api/analytics/*` → 5082.

## Тесты

```bash
cd backend
dotnet test
```

## Сквозная проверка вручную

1. `/dictionaries/themes` → «Добавить» → создать тему → «Утвердить».
2. Тема должна почти сразу попасть в выпадающий список тем на `/analytics/articles`
   (через Broker.Api — подтверждает работу шины).
3. «Новая статья» → выбрать эту тему → сохранить.
4. Открыть статью («Подробнее») — счётчик просмотров должен увеличиться на 1.
