# ByteBrigade Pharmacy

ByteBrigade Pharmacy is a full-stack online pharmacy project with an ASP.NET Core backend, SQL Server persistence, and an Angular standalone-component frontend. It supports customer registration/login, medicine browsing, cart management, checkout, prescription upload, promotions, order history, loyalty points, and admin-only medicine/order operations.

For endpoint-level details, see [API_DOCUMENTATION.md](API_DOCUMENTATION.md).

## Tech Stack

Backend:
- ASP.NET Core on .NET 10
- ASP.NET Core Controllers
- EF Core
- SQL Server
- JWT bearer authentication
- Native .NET OpenAPI plus Scalar API reference UI

Frontend:
- Angular standalone components
- Angular Router
- Angular HTTP interceptors
- Bootstrap 5
- SSR/prerender-capable Angular build setup

## Project Structure

```text
D:\ByteBrigade
├── API_DOCUMENTATION.md
├── README.md
├── backend
│   └── HackathonBackend
│       ├── Controllers
│       ├── Data
│       ├── Dtos
│       ├── Migrations
│       ├── Models
│       ├── Services
│       ├── Program.cs
│       ├── appsettings.json
│       └── HackathonBackend.csproj
└── frontend
    └── hackathon-frontend
        ├── src
        │   └── app
        │       ├── components
        │       ├── guards
        │       ├── interceptors
        │       ├── models
        │       ├── pages
        │       └── services
        ├── angular.json
        └── package.json
```

## How To Run

### Backend

From:

```powershell
cd D:\ByteBrigade\backend\HackathonBackend
```

Restore/build:

```powershell
dotnet restore
dotnet build
```

Apply migrations:

```powershell
dotnet ef database update
```

Run:

```powershell
dotnet run
```

Default backend URL from launch settings:

```text
http://localhost:5019
```

OpenAPI:

```text
http://localhost:5019/openapi/v1.json
```

Scalar API UI:

```text
http://localhost:5019/swagger
```

### Frontend

From:

```powershell
cd D:\ByteBrigade\frontend\hackathon-frontend
```

Install dependencies:

```powershell
npm install
```

Run:

```powershell
npm start
```

Default frontend URL:

```text
http://localhost:4200
```

Build:

```powershell
npm run build
```

## Database

The backend uses SQL Server through EF Core. The connection string is stored in:

```text
backend/HackathonBackend/appsettings.json
```

Current connection string:

```json
"DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=HackathonDB;Trusted_Connection=True;TrustServerCertificate=True;"
```

Main database tables are configured in `AppDbContext`:

- `Users`
- `Medicines`
- `CartItems`
- `Orders`
- `OrderItems`
- `Promotions`
- `LoyaltyTransactions`
- `Entities` for the original scaffold entity

`AppDbContext.OnModelCreating()` configures decimal precision, unique indexes, seed data, default promotions, and a seeded medicine catalog.

## Backend Implementation

### Program.cs

`Program.cs` wires the backend application:

- Adds controllers with `builder.Services.AddControllers()`.
- Configures EF Core SQL Server with `UseSqlServer(...)`.
- Registers `IEmailService` as `EmailService`.
- Configures JWT bearer authentication.
- Enables role-based authorization.
- Enables CORS for the Angular app.
- Adds .NET native OpenAPI with `AddOpenApi()`.
- Maps OpenAPI in development with `MapOpenApi()`.
- Maps Scalar API reference at `/swagger`.
- Ensures upload/output folders exist:
  - `wwwroot/prescriptions`
  - `wwwroot/emails`
- Enables static file serving.
- Maps controller routes.

### Authentication

Authentication lives in `AuthController`.

Implemented endpoints:

- `POST /api/Auth/register`
- `POST /api/Auth/login`

Register creates a user row in SQL Server. Login validates username/password and returns a JWT with:

- user id
- username
- role

The frontend stores the token and role in `localStorage`. The Angular HTTP interceptor attaches the JWT as:

```text
Authorization: Bearer <token>
```

Important note: the current project is hackathon/demo oriented and stores passwords in plain text. For production, replace this with password hashing and move the JWT secret to configuration or user secrets.

### Authorization

Protected backend controllers/actions use `[Authorize]`.

Admin-only actions use:

```csharp
[Authorize(Roles = "Admin")]
```

Examples:

- Add/update/delete medicine
- View all orders
- Update order status
- Create promotions

### Medicine Flow

Medicine data is stored in the `Medicines` table.

Main model:

```text
Models/Medicine.cs
```

Controller:

```text
Controllers/MedicineController.cs
```

The public can list and view medicines. Admin users can add, update, and delete medicines. Stock is stored directly on `Medicine.StockQuantity`.

The frontend medicine page:

```text
frontend/hackathon-frontend/src/app/pages/medicine-list
```

It loads medicines through `MedicineService`, supports searching by name/category, shows stock state, and lets customers add items to cart.

### Cart Flow

Cart data is persisted in SQL Server through the `CartItems` table.

Backend files:

- `Models/CartItem.cs`
- `Controllers/CartController.cs`

Frontend files:

- `models/cart-item.model.ts`
- `services/cart.service.ts`
- `pages/cart/cart.ts`
- `pages/cart/cart.html`

Implemented behavior:

- Add medicine to cart.
- If the same medicine is added again, quantity is increased.
- Update quantity with `PUT /api/Cart/{id}`.
- Remove cart item with `DELETE /api/Cart/{id}`.
- Calculate line total using medicine price and quantity.
- Calculate cart total on both backend and frontend.
- Validate quantity against available stock.

Cart items are user-specific. The backend reads the current user id from the JWT claim and only returns/modifies cart rows for that user.

### Checkout And Orders

Orders are created from the current user's cart.

Backend files:

- `Models/Order.cs`
- `Models/OrderItem.cs`
- `Models/PlaceOrderRequest.cs`
- `Controllers/OrderController.cs`

Frontend files:

- `pages/checkout`
- `pages/orders`
- `services/order.service.ts`
- `models/order.model.ts`

When placing an order:

1. The backend loads the current user's cart.
2. It checks that every cart medicine exists.
3. It validates stock.
4. It checks whether any medicine requires a prescription.
5. If required, a prescription file must be uploaded.
6. It calculates subtotal.
7. It applies a promotion if a valid promo code is supplied.
8. It calculates final amount.
9. It creates an `Order` and related `OrderItem` rows.
10. It reduces medicine stock.
11. It clears the cart.
12. It awards loyalty points.
13. It writes email/demo notification output through `EmailService`.

Prescription files are saved under:

```text
backend/HackathonBackend/wwwroot/prescriptions
```

They are served through ASP.NET Core static files.

### Promotions

Promotion data is stored in the `Promotions` table.

Backend files:

- `Models/Promotion.cs`
- `Controllers/PromotionController.cs`
- `Dtos/PromotionDtos.cs`

Implemented behavior:

- List active promotions.
- Validate promotion by code and order amount.
- Create promotion as admin.

Seeded promo examples are configured in `AppDbContext`.

### Loyalty

Loyalty data is stored in:

- `Users.LoyaltyPoints`
- `LoyaltyTransactions`

Backend files:

- `Models/LoyaltyTransaction.cs`
- `Controllers/LoyaltyController.cs`

Frontend files:

- `pages/loyalty`
- `services/loyalty.service.ts`
- `models/loyalty.model.ts`

Current rule:

```text
1 loyalty point for every Rs. 10 of final order amount
```

When an order is placed, points are added to the user and a transaction row is recorded.

### Email Service

The email implementation is demo/file based.

Files:

- `Services/IEmailService.cs`
- `Services/EmailService.cs`

Instead of sending real SMTP email, the service writes notification output to:

```text
backend/HackathonBackend/wwwroot/emails
```

This keeps the project simple and demo friendly.

### DTO Folder

The `Dtos` folder documents request and response shapes for the API:

- `AuthDtos.cs`
- `MedicineDtos.cs`
- `CartDtos.cs`
- `OrderDtos.cs`
- `PromotionDtos.cs`
- `LoyaltyDtos.cs`
- `UserDtos.cs`
- `AdminDtos.cs`

Some controllers currently use inline request classes or models directly. The DTO files are still useful as API contract documentation and can be used later to separate database models from HTTP request/response contracts more strictly.

## Frontend Implementation

### Angular App Configuration

Main app setup:

```text
frontend/hackathon-frontend/src/app/app.config.ts
```

It provides:

- Angular Router
- Angular HTTP client
- Fetch-backed HTTP
- Auth interceptor

Routes:

```text
frontend/hackathon-frontend/src/app/app.routes.ts
```

### Routing

Public routes:

- `/medicines`
- `/login`
- `/register`

Authenticated routes:

- `/dashboard`
- `/cart`
- `/checkout`
- `/orders`
- `/loyalty`

Admin routes:

- `/add-medicine`
- `/admin-orders`

Route protection:

- `auth-guard.ts` checks if a token exists.
- `admin-guard.ts` checks token and `role === "Admin"`.

### Auth Interceptor

File:

```text
src/app/interceptors/auth.interceptor.ts
```

This interceptor reads the JWT token from browser `localStorage` and adds the `Authorization` header to API requests.

It is SSR-safe: during server-side rendering/prerendering it skips `localStorage`, because `localStorage` only exists in the browser.

### Services

Angular services wrap backend API calls:

- `auth.service.ts`
- `medicine.service.ts`
- `cart.service.ts`
- `order.service.ts`
- `promotion.service.ts`
- `loyalty.service.ts`
- `entity.service.ts`

These services keep HTTP logic out of components.

### Components And Pages

Reusable component:

- `components/navbar`

Main pages:

- `pages/login`
- `pages/register`
- `pages/dashboard`
- `pages/medicine-list`
- `pages/add-medicine`
- `pages/cart`
- `pages/checkout`
- `pages/orders`
- `pages/loyalty`

Bootstrap is included globally in `angular.json`, so templates use Bootstrap classes for responsive layout, cards, tables, alerts, badges, and buttons.

## Main User Flows

### Customer Flow

1. Open `/medicines`.
2. Browse/search medicines.
3. Register or login.
4. Add medicines to cart.
5. Open `/cart`.
6. Update quantity or remove items.
7. Proceed to checkout.
8. Upload prescription if needed.
9. Apply promo code if available.
10. Place order.
11. View order history and loyalty points.

### Admin Flow

1. Login with an admin account.
2. Open dashboard.
3. Add or manage medicines from `/add-medicine`.
4. View all orders from `/admin-orders`.
5. Update order statuses.
6. Review prescription files linked to orders.

## Important Commands

Backend build:

```powershell
cd D:\ByteBrigade\backend\HackathonBackend
dotnet build
```

Backend run:

```powershell
dotnet run
```

Frontend build:

```powershell
cd D:\ByteBrigade\frontend\hackathon-frontend
npm run build
```

Frontend run:

```powershell
npm start
```

Apply migrations:

```powershell
cd D:\ByteBrigade\backend\HackathonBackend
dotnet ef database update
```

Add a migration:

```powershell
dotnet ef migrations add MigrationName
```

## Troubleshooting

### Build fails because HackathonBackend.exe is in use

This means the backend is already running and Windows locked the build output.

Find backend processes:

```powershell
Get-CimInstance Win32_Process |
  Where-Object { $_.Name -eq 'HackathonBackend.exe' -or $_.CommandLine -like '*HackathonBackend*' } |
  Select-Object ProcessId, Name, CommandLine
```

Stop the process:

```powershell
Stop-Process -Id <process-id> -Force
```

Then build again:

```powershell
dotnet build
```

### Frontend cannot call backend

Check that:

- Backend is running on `http://localhost:5019`.
- Frontend services point to `http://localhost:5019/api/...`.
- CORS policy `AllowAngular` is enabled in `Program.cs`.
- Browser has a valid token after login for protected endpoints.

### Protected endpoints return 401

Login again and confirm localStorage has:

- `token`
- `username`
- `role`
- `userId`

The Angular interceptor sends the token automatically.

### Admin page redirects

Admin-only routes require:

```text
role = Admin
```

The role is stored after login and checked by `admin-guard.ts`.

## Current Notes

- The project is hackathon/demo oriented.
- Passwords are plain text in the current backend and should be hashed before production use.
- JWT secret is hardcoded and should be moved to configuration/user secrets before production use.
- Email output is file-based, not SMTP-based.
- `API_DOCUMENTATION.md` contains the API endpoint reference.
