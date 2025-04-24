# Test Plan Document

## Overview

This document outlines the test plan for the restaurant web platform project. The plan includes backend and frontend testing, broken down by system component, with success conditions and expected error responses.

---

## 1. Back End Database Testing

| Component | Success Condition | Error Conditions |
|----------|--------------------|------------------|
Reservation | Prevents max-capacity via location, date, and time. | Exceeds max capacity for location.
Order | Links valid customer_id, order_id, orderstatus, orderitems. | Missing payment method, delivery fee invalid.
MenuItem | Properly links to category, isAvailable, and price. | Missing category, no price, or invalid availability.
Customer | Enforces unique email and username. | Duplicate emails or usernames.
UserAddress | Complete address info tied to a valid customer_id. | Null fields or broken foreign key link.
PaymentMethod | Includes valid card, type, and security code. | Expired or insecure data (test mode).

---

## 2. Back End Function Testing

| Function | Success Condition | Error Conditions |
|---------|-------------------|------------------|
register_customer | Creates a new Customer with secure password hash/salt. | Missing fields, duplicate email or username.
login_customer | Verifies credentials, returns session info. | Invalid password or email.
create_order | Saves Order, links OrderMenuItem. | Invalid menuitem_id.
create_reservation | Adds new Reservation if table_id, date, time are free. | Double booking, invalid time range or table.
load_menu_items | Returns available MenuItem records where isAvailable = true. | None available, DB failure.
apply_rewards | Deducts rewardpoints, adjusts order total. | Not enough points, inactive rewards.
save_address | Adds or updates UserAddress record tied to a customer. | Invalid city/state/postcode entry.
add_payment_method | Saves PaymentMethod for checkout (simulation mode). | Missing or invalid card details.

---


## 3. Frontend Page Tests

| Page | Success Condition | Error Conditions |
|---------|-------------------|------------------|
/register | Displays Customer registration form, stores hashed password, redirects to login | Validation errors, duplicate email or username 
/login | Authenticates Customer, sets session or cookie, redirects to dashboard | Incorrect credentials, empty fields, backend error
/menu | Dynamically renders available MenuItem records with description, price | No items returned, unavailable menu state shows: "Menu unavailable"
/order | Cart displays selected MenuItems, quantities tracked, total calculated | Missing items, invalid quantities, or form not submitted properly
/reservation | Allows Reservation input | Overlapping bookings, invalid date/time, form errors
/checkout | Shows order summary, uses saved UserAddress and PaymentMethod if available | Missing fields, invalid card info, address not linked to customer
/dashboard | Loads Customer profile, rewardpoints, saved UserAddress and preferences | Expired session: redirect to /login, data loading errors
/kitchen | Displays current Order queue, sorted by ordertype | Empty queue, backend sync delay

---

## 4. Manual QA Flows

- Register and login users
- Place valid/invalid orders
- Make and cancel reservations
- Test user/admin permission boundaries
- Cross-browser UI testing
- Accessibility compliance testing (form labels, color contrast)

---

## 5. Tools & Frameworks

- Unit Testing: xUnit (.NET)
- UI Testing: xUnit
- Manual QA: Browser-based test cases
- DB Validation: SQL data seeding

---

## 6. Test Data Seeding

- Sample users: admin, customer
- Sample menu items: 20+ entries
- Sample reservations and orders


---

