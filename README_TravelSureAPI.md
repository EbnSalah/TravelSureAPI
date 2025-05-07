# 📘 TravelSureAPI – User Authentication API

## 🧾 Overview
This is a simple ASP.NET Core Web API for user registration, login, and membership upgrade.  
Data is stored **in-memory** for testing purposes only (no external DB).

---

## 📌 Features
- ✅ **User Registration** (with unique email and username check)
- ✅ **Password Hashing** using SHA256
- ✅ **User Login** with hashed credentials check
- ✅ **Membership Upgrade** endpoint
- ✅ **Validation errors** return meaningful JSON messages
- ✅ **Unit Testing** using xUnit
- ✅ **Swagger UI** for testing endpoints

---

## 🚀 Endpoints

### 🔹 Register a User
`POST /api/users/register`  
**Body** (JSON):
```json
{
  "userName": "Ahmed",
  "email": "ahmed@example.com",
  "passwordHash": "A123A",
  "membershipTier": "Basic"
}
```

**Success Response (200 OK):**
```json
{ "message": "Registeration successful" }
```

**Failure (400 BadRequest):**
- Username already exists
- Email already registered
- Invalid membership tier

---

### 🔹 Login
`POST /api/users/login`  
**Body**:
```json
{
  "email": "ahmed@example.com",
  "password": "A123A"
}
```

**Success:**
```json
{
  "message": "Login Successful",
  "username": "Ahmed",
  "memberShip": "Basic"
}
```

**Failure:**
```json
{ "message": "Invalid email or password" }
```

---

### 🔹 Upgrade Membership
`PUT /api/users/upgrade/{email}`  
Upgrades user membership from Basic to Premium.  

**Success:**
```json
{ "message": "Membership upgraded to Premium" }
```

**Failure:**
```json
{ "message": "User not found or already Premium" }
```

---

## 🧪 Unit Tests
Tested using `xUnit`:
- Register with existing email → returns BadRequest
- Register with existing username → returns BadRequest
- Register new valid user → succeeds
- Login With correct credentials → succeeds
- Login With wrong password -> returns unauthorized

---

## 🛠️ Tools Used
- ASP.NET Core Web API
- xUnit
- Swagger (OpenAPI)

---

