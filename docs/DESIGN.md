# Design Document

## Table of Contents

1. [Overview](#1-overview)  
  1.1 [Purpose](#11-purpose)  
  1.2 [Scope](#12-scope)  

2. [System Architecture](#2-system-architecture)  
  2.1 [Tech Stack](#21-tech-stack)  

3. [System Components](#3-system-components)  
  3.1 [Reservation System](#31-reservation-system)  
    - [Features](#reservation-system-features)  
    - [Database Schema](#reservation-system-database-schema)  
  3.2 [Online Ordering System](#32-online-ordering-system)  
    - [Features](#ordering-system-features)  
    - [Database Schema](#ordering-system-database-schema)  
  3.3 [Online Menus](#33-online-menus)  
    - [Features](#online-menus-features)  
    - [Database Schema](#online-menus-database-schema)  
  3.4 [Loyalty Program & User Profiles](#34-loyalty-program--user-profiles)  
    - [Features](#loyalty-program-features)  
    - [Database Schema](#loyalty-program-database-schema)  
  3.5 [Kitchen Portal](#35-kitchen-portal)  
    - [Features](#kitchen-portal-features)  
    - [Database Schema](#kitchen-portal-database-schema)  
  3.6 [Checkout & Payments](#36-checkout--payments)  
    - [Features](#checkout--payments-features)  

4. [Frontend Design](#4-frontend-design)  
  4.1 [Pages/Views](#pagesviews-react)  

5. [Security](#5-security)  

6. [Data Flow Example](#6-data-flow-example)  
  - [Table Reservation Flow](#table-reservation-flow)  

7. [Testing Strategy](#7-testing-strategy)  
  - [Types & Tools](#types--tools)  

8. [Deployment](#8-deployment)  
---
## 1. Overview


### 1.1 Purpose
To provide a complete technical design for building a web application that modernizes a restaurant franchise’s reservation, ordering, and kitchen logistics through an interactive, responsive, and secure platform.


### 1.2 Scope
The platform enables:
- Table reservations with floorplan selection 
- Online food ordering for dine-in, pickup, or delivery 
- Account creation with loyalty tracking 
- Inventory-managed kitchen interface 
- Integrated payment and menu systems
- See SCOPE.md for full documentation


---


## 2. System Architecture


### 2.1 Tech Stack


| Layer | Technology |
|-------|------------|
| Frontend | Blazor cshtml |
| Backend | .NET Core MVC (C#) |
| Database | SQL Server |


---


## 3. System Components


### 3.1 Reservation System


#### Features:
- Interactive 50x50 grid floor plan UI
- Table layouts pulled from CSV files
- Each location has its own plan
- Real-time booking status


#### Database:
```sql
Table: Tables
- TableID (PK)
- LocationID (FK)
- PositionX
- PositionY
- SizeX
- SizeY
- Capacity


Table: Reservations
- ReservationID (PK)
- TableID (FK)
- UserID (FK)
- DateTime
- Status (Pending, Confirmed, Cancelled)
```


---


### 3.2 Online Ordering System


#### Features:
- Dine-in, Pickup, and Delivery options
- Delivery fee calculation using distance API (e.g., Google Maps Distance Matrix)
- Menu pulled dynamically from database
- Pre-ordering and scheduling


#### Database:
```sql
Table: Orders
- OrderID (PK)
- UserID (FK)
- OrderType (Enum)
- Status (Pending, Preparing, Completed, Cancelled)
- ScheduledTime
- TotalPrice
- DeliveryFee
- Address


Table: OrderItems
- OrderItemID (PK)
- OrderID (FK)
- MenuItemID (FK)
- Quantity
```

---


### 3.3 Online Menus


#### Features:
- Menu browsable without login
- Each item links to dietary info, availability, and pricing
- Updated in real-time based on inventory


#### Database:
```sql
Table: MenuItems
- MenuItemID (PK)
- Name
- Description
- Price
- Category
- IsAvailable (Bool)
- ImageURL
```


---


### 3.4 Loyalty Program & User Profiles


#### Features:
- Points system based on order history
- Preferences stored per user (fav dishes, payment info)
- Rewards and discount logic engine


#### Database:
```sql
Table: Users
- UserID (PK)
- Email
- PasswordHash
- Phone
- Address
- SavedPaymentInfo
- RewardPoints


Table: UserPreferences
- PreferenceID (PK)
- UserID (FK)
- FavoriteDish
- DietaryNotes
```


---


### 3.5 Kitchen Portal


#### Features:
- Order queue (filtered by order type)
- Itemized breakdown per order
- Inventory deductions as orders are made
- Alerts when inventory is low


#### Database:
```sql
Table: Ingredients
- IngredientID (PK)
- Name
- QuantityAvailable
- Unit


Table: MenuItemIngredients
- MenuItemID (FK)
- IngredientID (FK)
- QuantityNeeded
```


---


### 3.6 Checkout & Payments


#### Features:
- Simulated payment gateway integration
- Cart interface with order summary
- Multiple payment types (credit, debit, Apple/Google Pay)


---


## 4. Frontend Design


### 4.1 Pages/Views :
- HomePage: Overview and links
- ReservationPage: 50x50 grid w/ table picker
- MenuPage: Menu items + filters
- OrderPage: Cart + order type
- CheckoutPage: Payment & summary
- Login/RegisterPage: Auth flow
- Dashboard: Loyalty points, saved info
- AdminPanel: Floorplan upload, inventory status

---

## 5. Security

- Encrypted storage for saved payment info (simulation only)


---


## 6. Data Flow Example


### Table Reservation Flow:
```
User → Grid UI → SQL DB 
→ Reservation table updated → update to other users
```

---


## 7. Testing Strategy


| Test Type | Tools |
|-----------|-------|
| Unit Tests | xUnit (.NET) |
| UI Testing | Bunit, xUnit |
| Manual QA | Admin/Customer test flows |


---


## 8. Deployment

- Database seeding for test data


---