# ByteBrigade Pharmacy — Complete API Reference

Base URL: `https://localhost:5001` (or HTTP fallback `http://localhost:5000`)
OpenAPI document: `GET /openapi/v1.json` (via `app.MapOpenApi()`)

All authenticated endpoints expect a JWT bearer token in the `Authorization` header:

```
Authorization: Bearer <token-from-login>
```

---

## 1. Auth APIs — `api/Auth`

| Method | Route                | Auth   | Description                                                    |
|--------|----------------------|--------|----------------------------------------------------------------|
| POST   | `/api/Auth/register` | Public | Register a new customer (or admin if explicitly passed).       |
| POST   | `/api/Auth/login`    | Public | Authenticate and return a JWT token, username, role, userId.   |

### Register body
```json
{
  "username": "john",
  "password": "secret",
  "role": "Customer",
  "email": "john@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "phoneNumber": "9876543210"
}
```

### Login body
```json
{ "username": "john", "password": "secret" }
```

---

## 2. Medicine APIs — `api/Medicine`

| Method | Route                  | Auth         | Description                          |
|--------|------------------------|--------------|--------------------------------------|
| GET    | `/api/Medicine`        | Public       | List all medicines (browsing).       |
| GET    | `/api/Medicine/{id}`   | Public       | Get a single medicine by id.         |
| POST   | `/api/Medicine`        | Admin        | Add a new medicine.                  |
| PUT    | `/api/Medicine/{id}`   | Admin        | Update an existing medicine.         |
| DELETE | `/api/Medicine/{id}`   | Admin        | Delete a medicine.                   |

### Medicine object
```json
{
  "id": 1,
  "name": "Dolo 650",
  "category": "Pain Relief",
  "manufacturer": "Micro Labs",
  "composition": "Paracetamol 650mg",
  "dosageForm": "Tablet",
  "strength": "650mg",
  "packagingType": "Strip of 15",
  "price": 35,
  "stockQuantity": 250,
  "description": "Effective relief from fever and mild pain.",
  "requiresPrescription": false,
  "imageUrl": "..."
}
```

---

## 3. Cart APIs — `api/Cart` (all require auth)

| Method | Route                | Auth         | Description                                            |
|--------|----------------------|--------------|--------------------------------------------------------|
| GET    | `/api/Cart`          | Authenticated| Get current user's cart with medicine details.         |
| GET    | `/api/Cart/total`    | Authenticated| Get cart total quantity and total price.               |
| POST   | `/api/Cart`          | Authenticated| Add a medicine to cart (or bump quantity if existing). |
| PUT    | `/api/Cart/{id}`     | Authenticated| Update cart item quantity.                             |
| DELETE | `/api/Cart/{id}`     | Authenticated| Remove an item from cart.                              |

### Add to cart body
```json
{ "medicineId": 1, "quantity": 2 }
```

---

## 4. Order APIs — `api/Order` (all require auth)

| Method | Route                       | Auth         | Description                                                                 |
|--------|-----------------------------|--------------|-----------------------------------------------------------------------------|
| POST   | `/api/Order/place`          | Authenticated| Place an order. multipart/form-data — accepts optional `Prescription` file. |
| GET    | `/api/Order/my`             | Authenticated| Get the current user's order history.                                       |
| GET    | `/api/Order/all`            | Admin        | Get all orders across all customers.                                        |
| PUT    | `/api/Order/{id}/status`    | Admin        | Update an order's status (`Approved`, `Rejected`, `Delivered`, etc.).       |

### Place order — form-data fields
| Field         | Type      | Notes                                                       |
|---------------|-----------|-------------------------------------------------------------|
| Prescription  | File      | JPG / PNG / PDF — required if any cart item needs Rx.       |
| PromoCode     | string    | Optional promotion code.                                    |
| Address       | string    | Delivery address.                                           |
| Phone         | string    | Delivery phone number.                                      |
| Notes         | string    | Delivery notes.                                             |

On success the response includes `orderNumber`, `finalAmount`, `estimatedDeliveryDate`, and `pointsEarned` (loyalty).

---

## 5. Prescription Handling

Prescription upload is handled within `POST /api/Order/place` (multipart form-data) under the `Prescription` field. Uploaded files are persisted to `wwwroot/prescriptions/{guid}_{originalName}` and exposed via static files. The filename is stored in `Order.PrescriptionFile` so admins can review it in the admin orders view.

Allowed types (validated client-side): `.jpg`, `.jpeg`, `.png`, `.pdf`.

---

## 6. Promotion APIs — `api/Promotion`

| Method | Route                       | Auth          | Description                                |
|--------|-----------------------------|---------------|--------------------------------------------|
| GET    | `/api/Promotion`            | Public        | List all currently active promotions.      |
| POST   | `/api/Promotion/validate`   | Authenticated | Validate a promo code for an order amount. |
| POST   | `/api/Promotion`            | Admin         | Create a new promotion.                    |

### Validate body
```json
{ "code": "WELCOME10", "orderAmount": 1200 }
```

---

## 7. Loyalty APIs — `api/Loyalty` (all require auth)

| Method | Route                          | Auth          | Description                                  |
|--------|--------------------------------|---------------|----------------------------------------------|
| GET    | `/api/Loyalty/points`          | Authenticated | Get current user's total loyalty points.     |
| GET    | `/api/Loyalty/transactions`    | Authenticated | List the user's loyalty transaction history. |

Loyalty rule: customers earn `1 point for every ₹10` of final order amount on each placed order. Points are appended to `User.LoyaltyPoints` and a `LoyaltyTransaction` row is recorded.

---

## 8. Admin / User Mapping

There is no separate `AdminController` per hackathon rules; admin functionality is exposed through `[Authorize(Roles = "Admin")]` on existing endpoints:

| Capability               | Endpoint                                                       |
|--------------------------|----------------------------------------------------------------|
| Add medicine             | `POST   /api/Medicine`                                         |
| Edit medicine            | `PUT    /api/Medicine/{id}`                                    |
| Delete medicine          | `DELETE /api/Medicine/{id}`                                    |
| Update inventory (stock) | `PUT    /api/Medicine/{id}` (via `stockQuantity` field)        |
| View all customer orders | `GET    /api/Order/all`                                        |
| Approve / Reject order   | `PUT    /api/Order/{id}/status`                                |
| View uploaded prescriptions | Static URL: `/prescriptions/{filename}` (from `Order.PrescriptionFile`) |
| Create promotion         | `POST   /api/Promotion`                                        |

User-facing endpoints (Profile / History / Points) are served via `api/Loyalty` and `api/Order/my`.

---

## 9. OpenAPI / Swagger

The backend uses .NET 10 native OpenAPI:

```csharp
builder.Services.AddOpenApi();
// ...
app.MapOpenApi();
```

- The OpenAPI JSON document is served at `GET /openapi/v1.json` in Development.
- All endpoints decorated with `[Http*]` attributes are visible.
- JWT authorization is achieved by passing `Authorization: Bearer <token>` in the request header when calling the endpoint from any OpenAPI consumer (Scalar UI, Swagger UI, Bruno, or curl).
