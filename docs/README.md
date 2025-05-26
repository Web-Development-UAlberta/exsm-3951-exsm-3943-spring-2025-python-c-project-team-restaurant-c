# EXSM 3943 - C# Project | Restaurant Manager

A full-stack web application for the **Harvest & Hearth** restaurant, allowing customers to create an account to order food for dine-in, pickup, or delivery. The website also includes a web portal for the kitchen staff to manage incoming customer orders.

Developed as part of the **EXSM 3943** C# project at the University of Alberta.

Live Site: [https://harvestandhearth.webthesite.com/](https://harvestandhearth.webthesite.com/)

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

## Usage - Customers

- Users can select the "Menu" Navigation Tab to browse the menu. Menu items can be viewed by category and filtered by dietary preference.
- Selecting the "Order" or "Book Table" Navigation Tabs before logging in will redirect the user to the login page. Here they can either log in to an existing account or create a new account using an email and a phone number.
- Once logged in, the user will have access to the account page where the they can view and edit their profile.
- Users can select the "Order" Navigation Tab to add items to their cart and place an order. 
- Users can select from "Take out" or "Delivery". The user will need to add an address in their "Account" page before they checkout.
- Users can select the "Checkout" option once they have completed their order. This will take them to the Checkout page where they can confirm their order, add their payment through stripe and finally view their receipt.
- Users can select the "Book Table" Navigation Tab to enter the information required to reserve a table. Selecting the "Book Now" button will redirect the user to the Reservation confirmation page. If the user wishes, they can pre order their menu by clicking the "Pre-Order Menu" button. 

## Usage - Staff 
- Staff can login to th Kitchen portal in the footer. 
- Staff can view the upcoming reservations, active orders and available menu itmes. 
- Users are able to view all menu items, edit the menu items, add to the menu or delete a menu item. 
- Users can track current reservaions and manage the booking. They can also see all past reservations. 
- Orders can be viewed under the "Orders" tab in the navigation. Here staff can view the incoming orders, the type of order (Take out, dine in or delivery), set the status of the order and review additional info such as the order date, the scheduled time and additional infromation about the delivery or reservation. All statuses and order types are colour coded to help staff manage orders. 

## Tech Stack

- **ASP.NET Core MVC** (.NET 8)
- **C#** backend logic
- **Entity Framework Core** for data access
- **SQLite Server** (for database development)
- **Razor Pages** for UI rendering
- **Bootstrap/CSS** (additional styling)
- **Nginx web server** (site deployment)

## Project Setup

### Prerequisites

- Install the .NET 8 SDK at [https://dotnet.microsoft.com/en-us/download](https://dotnet.microsoft.com/en-us/download)
- Install the Entity Framework CLI:

```bash
dotnet tool install --global dotnet-ef
```

### Steps

- Clone repository:

```bash
git clone https://github.com/Web-Development-UAlberta/exsm-3951-exsm-3943-spring-2025-python-c-project-team-restaurant-c.git
```

- Navigate to repository:

```bash
cd exsm-3951-exsm-3943-spring-2025-python-c-project-team-restaurant-c
```

- Apply the latest Entity Framework Core migrations:

```bash
dotnet ef database update --project src/RestaurantManager/RestaurantManager.csproj
```

- Run application:

```bash
dotnet run --project src/RestaurantManager/RestaurantManager.csproj
```

- The default browser should open the project at https://localhost:xxxx or click the link in the terminal

---

## Contributors:

- [Arlyssa](https://github.com/Arlyssa)
- [Alex](https://github.com/Pewpy)
- [Josh](https://github.com/jmantei)
