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
- Interactive 50x50 grid floor plan UI
- Table layouts pulled from CSV files
- Each location has its own plan
- Real-time booking status

---


### 3.2 Online Ordering System


#### Features:
- Dine-in, Pickup, and Delivery options
- Delivery fee calculation using distance API (e.g., Google Maps Distance Matrix)
- Menu pulled dynamically from database
- Pre-ordering and scheduling

---


### 3.3 Online Menus


#### Features:
- Menu browsable without login
- Each item links to dietary info, availability, and pricing
- Updated in real-time based on inventory

---


### 3.4 Loyalty Program & User Profiles


#### Features:
- Points system based on order history
- Preferences stored per user (fav dishes, payment info)
- Rewards and discount logic engine

---


### 3.5 Kitchen Portal


#### Features:
- Order queue (filtered by order type)
- Itemized breakdown per order
- Inventory deductions as orders are made
- Alerts when inventory is low

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

- Database seeding for test data:
  - 5 sample users (admin, customer, kitchen staff)
  - 20+ sample menu items across 3 categories
  - 3 floorplan CSVs
  - Sample reservation and order entries



---

