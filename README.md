# The Movement Group Storage

This repository provides the Movement Group home assignment solution.

---

## ✨ Features

* Clean Architecture using .NET 8
* Docker and Docker Compose support
* Design Patterns usage
* Application of SOLID design principles
* REST API

---

## 📁 Project Structure

```
.
│ Domain Layer/           //Defines Domain
│ ├─ Entities/
│ ├─ Repositories/        //Data manipulating services
│ Application Layer/      //Defines business logic
│ ├─ Models/
│ ├─ Services/
│ ├─ Managers/            //Business logic aggregators
│ Infrastructure Layer/   //Includes implementations
│ ├─ Services/
│ ├─ Managers/
│ ├─ Repositories/
```

---

## 🐳 Docker

The solution includes:

* **Production Docker setup** – optimized build
* **Development Docker setup** – debugging support

---

## 🚀 Getting Started

### Run the solution.

Just select Docker Compose within the Debug Target dropdown and run debugger (or without debugger if you want). That's it!\
Your default browser will open `swagger/index.html` page automatically.

### Browse the Mongo DB data.
If you want to see the data you've saved then after the application has started
1. Open http://localhost:5081/
2. Click `movement_group` within the `Databases` list to open the database.
3. Click `dump` collection to open the collection and see it's content.
It's called `dump` because contains unknown types items. Use strongly typed collections to separate data and simplify search.
4. Enjoy.

---

## 👤 Author

**Aaron Urkin**

---
