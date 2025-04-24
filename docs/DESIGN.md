# Design Document

## Table of Contents
1. [Overview](#1-overview)  
   1.1 [Purpose](#11-purpose)  
   1.2 [Scope](#12-scope)  

2. [System Architecture](#2-system-architecture)  
   2.1 [Tech Stack](#21-tech-stack)  
   2.2 [Entity-Relationship Diagram](#22-entity-relationship-diagram)  

3. [System Components](#3-system-components)  
   3.1 [Reservation System](#31-reservation-system)  
       - [Features](#reservation-system-features)  
   3.2 [Online Ordering System](#32-online-ordering-system)  
       - [Features](#ordering-system-features)  
   3.3 [Online Menus](#33-online-menus)  
       - [Features](#online-menus-features)  
   3.4 [Loyalty Program & User Profiles](#34-loyalty-program--user-profiles)  
       - [Features](#loyalty-program-features)  
   3.5 [Kitchen Portal](#35-kitchen-portal)  
       - [Features](#kitchen-portal-features)  
   3.6 [Checkout & Payments](#36-checkout--payments)  
       - [Features](#checkout--payments-features)  

4. [Frontend Design](#4-frontend-design)  
   4.1 [Pages/Views](#41-pagesviews)  

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
[View Project Scope Document](./SCOPE.md)

---


## 2. System Architecture


### 2.1 Tech Stack


| Layer | Technology |
|-------|------------|
| Frontend | Razor cshtml |
| Backend | .NET Core MVC (C#) |
| Database | SQL Server |

### 2.2 Entity-Relationship Diagram  
The following diagram outlines the database structure for the entire application:

[View ERD Diagram](./ERD.drawio)  
> *Note: The ERD includes schemas for Reservations, Orders, Users, Payments, Inventory, and more. Refer to this diagram when reviewing each system component.*

---


## 3. System Components


### 3.1 Reservation System


#### Features:
- Basic reservation handling supported (date/time, table ID, customer info), but no real-time booking conflict detection is included in this phase.
- (Out of Scope) GUI reservation interface (e.g., 50x50 grid floor plan) has been deferred for future development.
- (Out of Scope) Table layouts pulled from CSV files

---


### 3.2 Online Ordering System


#### Features:
- User login to view 
- Dine-in, Pickup, and Delivery options
- Flat delivery fee applied (no proximity-based calculations in this phase)
- Menu pulled from database
- Pre-ordering and scheduling
> *Note: Distance-based delivery fees are excluded per project scope and may be implemented in a future release.*
---


### 3.3 Online Menus


#### Features:
- Menu browsable without login
- Each item links to dietary info, availability, and pricing
- Menu updates are performed manually by the admin; real-time availability updates are not supported in this phase.
> *Note: Dynamic updates tied to inventory levels are outside the current scope.*
---


### 3.4 Loyalty Program & User Profiles


#### Features:
- Points system based on order history
- Preferences stored per user (fav dishes, payment info)
- (Out of Scope) Discount or redemption engine
> *Note: Users accumulate points, but rewards or automatic discounts are excluded from initial release.*
---


### 3.5 Kitchen Portal


#### Features:
- Admin can login to kitchen portal
- Order queue (filtered by order type)
- Itemized breakdown per order
- (Out of Scope) Ingredient inventory tracking integration with order processing
- (Out of Scope) Admin can edit menu
-  

---


### 3.6 Checkout & Payments


#### Features:
- Simulated payment gateway integration
- Cart interface with order summary
- Card payment types (credit, debit)
> *Note: All payment processing is simulated in this phase.*
---


## 4. Frontend Design


### 4.1 Pages/Views :
- HomePage: Overview and links
- ReservationPage: Reservation Form
- MenuPage: Menu items + filters
- OrderPage: Cart + order type
- CheckoutPage: Payment & summary
- Login/RegisterPage: Auth flow
- Dashboard: Loyalty points, saved user info
- AdminPanel: Kitchen Portal, Order Queue table

---

## 5. Security

- Encrypted storage for saved payment info (simulation only)


---


## 6. Data Flow Example


### Table Reservation Flow:
```
User → Reservation Form → SQL DB 
→ Reservation entry created (manual validation only)
```
> *Note: No real-time updates for preventing double bookings; handled via manual processes or future phase logic.*
---


## 7. Testing Strategy


| Test Type | Tools |
|-----------|-------|
| Unit Tests | xUnit (.NET) |
| UI Testing | xUnit |
| Manual QA | Admin/Customer test flows |


---


## 8. Deployment

- Database seeding for test data:
  - 5 sample users (admin, customer)
  - 20+ sample menu items across 3 categories
  - Sample reservation and order entries



---

