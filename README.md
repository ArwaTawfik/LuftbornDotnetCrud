# Job Application Tracker

A Kanban board for tracking job applications. Built as a code test with an **ASP.NET Core** API (C#),
an **Angular** client (TypeScript) and hand-written **SCSS**.

---

## Quick start

**You need:** [.NET 10 SDK](https://dotnet.microsoft.com/download) and [Node.js](https://nodejs.org) (developed on Node 24, npm 11).
No database server and no global Angular CLI required.

Open two terminals in the repository root.

**1. Start the API**

```bash
dotnet run --project src/Api --launch-profile http
```

The API listens on `http://localhost:5247`. On startup it applies the EF Core migration, creates a SQLite
file (`src/Api/jobapplications.db`) and seeds five sample applications if the table is empty.
Interactive API docs: <http://localhost:5247/swagger>

**2. Start the client**

```bash
cd client
npm ci
npm start
```

Open <http://localhost:4200>.

> Keep the API on port **5247** and the client on **4200**: the client calls that API URL and the API's CORS
> policy allows that client origin.

**Run the tests**

```bash
dotnet test
```

**Reset the sample data:** stop the API and delete `src/Api/jobapplications.db*`, then start it again.

---

## API

Base URL `http://localhost:5247/api/jobapplications`

| Method | Path | Success | Errors |
|---|---|---|---|
| GET | `/` | `200` list | |
| GET | `/{id}` | `200` | `404` |
| POST | `/` | `201` + created item | `400` validation |
| PUT | `/{id}` | `204` | `400` validation, `404` |
| DELETE | `/{id}` | `204` | `404` |

Statuses are sent as strings: `Applied`, `Interviewing`, `Offer`, `Rejected`, `Withdrawn`.
Validation failures return a standard `400` problem response with an `errors` object keyed by property name.

---

## Design

### Backend (`src/`)

Three projects, dependencies point inward: the core knows nothing about HTTP or EF Core.

```
   Api  ─────────────▶  Application  ◀─────────────  DataAccess
 controllers, DTOs,     entities, service,           EF Core DbContext, migrations,
 mapper, error          validator, repository        SQLite, repository
 handling, DI wiring    interface

 Api also references DataAccess, but only in Program.cs to register services.
```

| Project | Responsibility |
|---|---|
| `Application` | Domain entity, service, `JobApplicationValidator` (FluentValidation), `IJobApplicationRepository` (interface only) |
| `DataAccess` | `AppDbContext`, migrations, repository implementation, seeding |
| `Api` | Controller, request/response DTOs, mapper, global exception handler, Swagger, CORS |

### Frontend (`client/`)

Standalone components, signals and Reactive Forms. No UI framework: hand-written SCSS, and Angular's own CDK for drag-and-drop.

```
client/src/app/
├── components/
│   ├── job-board/   smart component: loads data, groups it into columns (computed), drag & drop
│   ├── job-card/    presentational: input() the application, output() delete / statusChange
│   └── job-form/    create AND edit page (one form, two routes)
├── models/          TypeScript interfaces mirroring the API contract
├── services/        JobApplicationService (the only place that talks to HttpClient)
├── validators/      group-level date-range validator
└── app.routes.ts    /  ·  /applications/new  ·  /applications/:id/edit
client/src/styles/_statuses.scss   the status colour map, shared by the board and the cards
```

Moving a card is **optimistic**: the UI updates first and rolls back if the API call fails.

---

## Requirements checklist

| Requirement | Status |
|---|---|
| Empty ASP.NET Core application | Done (started from `dotnet new web`) |
| Basic CRUD | Done, with validation |
| Code in C#, TypeScript and SCSS | Done |
| Backend .NET 3.1+ | .NET 10 |
| Database | SQL (SQLite) |
| Frontend | Angular |
| Scalable, readable, single-purpose classes | Layered projects, small focused classes |
| Automated tests | Backend: 15 xUnit tests (validator, service, error handler). Frontend: not yet |
| SSO (optional) | **Not implemented.** Planned approach below |

---

## Where it goes next

**Make it multi-user.** Sign in with Microsoft Entra ID (the cloud successor of ADFS): JWT auth on the API,
an OIDC login and token interceptor in Angular, and an owner on every application so each user sees only their own board.

**Make it production-ready.** A server database, migrations as a deployment step, Docker, CI (build, test, `ng build`),
integration tests, and frontend component tests.

**Make it smarter.** Interviews, contacts and notes per application, follow-up reminders, a stats view
(applications per week, conversion per stage), and live updates across tabs with SignalR.
