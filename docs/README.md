# EXSM 3943 - C# Project | Restaurant Manager

A full-stack web application for the **Harvest & Hearth** restaurant, allowing customers to create an account to order food for dine-in, pickup, or delivery. The website also includes a web portal for the kitchen staff to manage incoming customer orders.

Developed as part of the **EXSM 3943** C# project at the University of Alberta.

---

## Features

### For Customers:
- Browse menu items with categories and descriptions
- Create an account and log in to order or make a reservation
- Add items to a shopping cart
- Choose between dine-in, pickup, or delivery
- Submit and pay for orders

### For Kitchen Staff:
- Access kitchen portal to view order queue
- See order details, status, and timestamps
- Update order progress (e.g., "In Progress", "Ready", "Delivered")

---

## Tech Stack

- **ASP.NET Core MVC** (.NET 8)
- **C#** backend logic
- **Entity Framework Core** for data access
- **SQLite Server** (for database development)
- **Razor Pages** for UI rendering
- **Bootstrap/CSS** (additional styling)

---

## Project Setup

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- Entity Framework CLI  
  Install with:  
  ```bash
  dotnet tool install --global dotnet-ef

- Clone repository:

```bash
git clone git@github.com:Web-Development-UAlberta/exsm-3951-exsm-3943-spring-2025-python-c-project-team-restaurant-c.git
```

- Initialize database:
Apply the latest Entity Framework Core migrations:
```bash
dotnet ef database update --project src/RestaurantManager/RestaurantManager.csproj
```

- Run application:

```bash
dotnet run --project src/RestaurantManager/RestaurantManager.csproj
```

## Contributors:

- [Arlyssa](https://github.com/Arlyssa)
- [Alex](https://github.com/Pewpy)
- [jmantei](https://github.com/jmantei)
