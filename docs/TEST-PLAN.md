# Test Plan Document

## Overview

This document outlines the test plan for the restaurant web platform project. The plan includes backend and frontend testing, broken down by system component, with success conditions and expected error responses.

---

## 1. Back End Database Testing
We're testing whether the database correctly enforces business rules. It should block invalid data, such as duplicate usernames or unsupported characters and provide appropriate feedback when something goes wrong.

| Component | Success Condition | Error Conditions | Edge Cases |
|----------|--------------------|------------------|------------|
Reservation | Prevents max-capacity via location, date, and time. | Exceeds max capacity for location. | Bookings at exactly time of closing.
Order | Links valid customer_id, order_id, orderstatus, orderitems. | Missing payment method, delivery fee invalid. | Orders with 0 items or max items
MenuItem | Properly links to category, isAvailable, and price. | Missing category, no price, or invalid availability. | Category names with special characters ( e.g emojis) or empty/max length strings
Customer | Enforces unique email and username. | Duplicate emails or usernames. | Same user names with different cases (upper vs lower)
PaymentMethod | Includes valid card, type, and security code. | Expired or insecure data (test mode). | Card expired on the same day or unsupported cards

---

## 2. Back End Function Testing
This section tests the backend logic that powers key features like registering users or placing orders. We want to ensure that no matter what kind of data is entered (correct or incorrect), the system responds appropriately.

| Function | Success Condition | Error Conditions | Edge Cases |
|---------|-------------------|------------------|-------------|
register_customer | Creates a new Customer with secure password hash/salt. | Missing fields, duplicate email or username. | Passwords at min/max length, whitespace in email.
login_customer | Verifies credentials, returns session info. | Invalid password or email. | Absurd amount of logins, spaces in email, caps lock on.
create_order | Saves Order, links OrderMenuItem. | Invalid menuitem_id. | Order with large quantity.
create_reservation | Adds new Reservation if table_id, date, time are free. | Double booking, invalid time range or table. | Booking overlapses, booking on open/close.
load_menu_items | Returns available MenuItem records where isAvailable = true. | None available, DB failure. | Menu items with same deatails.
apply_rewards | Deducts rewardpoints, adjusts order total. | Not enough points, inactive rewards. | Reward points cover entire bill, rewards used multiple times, inactive rewards used.
save_address | Adds or updates UserAddress record tied to a customer. | Invalid city/state/postcode entry. | Email address text limit, unicode in address field
add_payment_method | Saves PaymentMethod for checkout (simulation mode). | Missing or invalid card details. | Alphabetical characters entered, white space entered, unicode, etc.

---


## 3. Frontend Page Tests
This is where we put ourselves in the shoes of a real user. We’ll interact with every page to see what works, what breaks and how well the site handles unexpected or invalid input.

| Page | Success Condition | Error Conditions | Edge Cases |
|------|-------------------|------------------|------------|
/register | Displays Customer registration form, stores hashed password, redirects to login | Validation errors, duplicate email or username | Whitespace, unicode, leading and trailing space.
/login | Authenticates Customer, sets session or cookie, redirects to dashboard | Incorrect credentials, empty fields, backend error | Absurd amount of logins, long login sessions.
/menu | Dynamically renders available MenuItem records with description, price | No items returned, unavailable menu state shows: "Menu unavailable" | Limited items available, displaying special characters
/order | Cart displays selected MenuItems, quantities tracked, total calculated | Missing items, invalid quantities, or form not submitted properly | Maximum/minimum quantity
/reservation | Allows Reservation input | Overlapping bookings, invalid date/time, form errors | Bookings overlapping by seconds, reserve date maximum
/checkout | Shows order summary, uses saved UserAddress and PaymentMethod if available | Missing fields, invalid card info, address not linked to customer | Checkout with empty cart, switching credit cards, mismatched expiry/CVV
/dashboard | Loads Customer profile, rewardpoints, saved UserAddress and preferences | Expired session: redirect to /login, data loading errors | Missing rewards, corrupted preferences

---

## 4. Manual QA Flows
This is a hands-on walkthrough of full user workflows to catch anything our automated tests might miss. It also ensures the site looks and works consistently across browsers and devices.

- Register and login users
- Place valid/invalid orders
- Make and cancel reservations
- Test user/admin permission boundaries
- Cross-browser UI testing
- Cross device testing (PC, tablet, phone)
- Accessibility compliance testing (form labels, colour contrast)
- Form resubmission on double-clicks
- Session timeout limits

---

## 5. Tools & Frameworks

- Unit Testing: xUnit (.NET)
- UI Testing: xUnit
- Manual QA: Browser-based test cases
- DB Validation: SQL data seeding

---

## 6. Test Data Seeding
We’ll pre-load the system with sample data so we can test features quickly without entering everything from scratch. This includes example users, menu items, and sample orders or reservations. 

- Sample users: admin, customer
- Sample menu items: 20+ entries
- Sample reservations and orders

---

