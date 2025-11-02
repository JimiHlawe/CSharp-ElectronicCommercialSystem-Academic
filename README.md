# Electronic Commercial System

A comprehensive C# console-based e-commerce platform that simulates an online marketplace where buyers can purchase products from sellers, manage shopping carts, and track order history.

## 📋 Table of Contents

- [Overview](#-overview)
- [Features](#-features)
- [Architecture](#-architecture)
- [Class Structure](#-class-structure)
- [Installation](#-installation)
- [Usage](#-usage)
- [System Requirements](#-system-requirements)
- [Project Structure](#-project-structure)
- [Key Functionalities](#-key-functionalities)
- [Design Patterns](#-design-patterns)
- [Contributing](#-contributing)
- [Academic Project](#-academic-project)

## 🎯 Overview

The **Electronic Commercial System** is an academic project that implements a complete e-commerce solution using C# and Object-Oriented Programming (OOP) principles. The system enables users to register as buyers or sellers, manage products, handle shopping carts, complete orders, and maintain a comprehensive order history.

This application demonstrates key programming concepts including:
- Inheritance and polymorphism
- Interface implementation
- Operator overloading
- Exception handling
- Generic collections
- LINQ operations
- Event-driven programming

## ✨ Features

### User Management
- **Dual User Types**: Support for both Buyers and Sellers
- **User Registration**: Secure registration with username and password validation
- **Address Management**: Complete address details including country, city, street, and building number
- **User Validation**: 
  - Username must contain letters and be unique
  - Password must be at least 8 characters with letters and numbers

### Product Management
- **Product Categories**: Four distinct categories
  - Clothing
  - Electronics
  - Kids
  - Office
- **Special Packaging**: Products can have special packaging options with additional costs
- **Product Variants**: Regular and Special Packaged Products
- **Unique Product IDs**: Auto-generated IDs starting from 1000

### Shopping Features
- **Shopping Cart**: Add products to cart with flexible packaging options
- **Product Search**: Search by ID or name
- **Multi-Seller Support**: Same products can be sold by different sellers
- **Price Calculation**: Automatic total price calculation including packaging costs
- **Order Completion**: Complete orders with minimum 2 items requirement
- **Order History**: Track all previous orders with timestamps

### Advanced Features
- **Cart Recreation**: Create new carts from previous order history
- **Buyer Comparison**: Compare shopping cart totals between two buyers
- **Seller Ranking**: Sellers sorted by number of products and username
- **Clone Support**: Deep cloning for buyers, sellers, orders, and products

## 🏗️ Architecture

The system follows a layered architecture with clear separation of concerns:

```
┌─────────────────────────────────────┐
│      Presentation Layer             │
│      (Console Interface)            │
└─────────────────────────────────────┘
              ↓
┌─────────────────────────────────────┐
│      Business Logic Layer           │
│      (SystemManager)                │
└─────────────────────────────────────┘
              ↓
┌─────────────────────────────────────┐
│      Domain Model Layer             │
│  (User, Product, Order, Address)    │
└─────────────────────────────────────┘
```

## 📚 Class Structure

### Core Classes

#### 1. **User** (Base Class)
- Properties: UserID, UserName, Password, Address
- Provides base functionality for both Buyers and Sellers
- Implements validation for username and password
- Can be inherited by specialized user types

#### 2. **Buyer** (Inherits from User)
- Implements: `ICloneable`
- Properties: ShoppingCart, LastOrders
- Key Methods:
  - `AddProductToCart()`: Add products with packaging options
  - `GetShoppingCartTotalPrice()`: Calculate cart total
  - `AddShoppingCartToOrders()`: Convert cart to order
  - `CreateNewCartFromHistory()`: Recreate cart from previous order
- Operator Overloading: `>`, `<` for cart comparison

#### 3. **Seller** (Inherits from User)
- Implements: `IComparable<Seller>`, `ICloneable`
- Properties: ProductsForSale
- Key Methods:
  - `AddProduct()`: Add product to inventory
  - `FindProductByName()`: Search products
  - `CompareTo()`: Compare sellers by product count

#### 4. **Product**
- Properties: ID, Category, Name, Price, PackagingOptions, OriginalPrice
- Supports two packaging options: Regular and Special
- Implements `Clone()` for deep copying

#### 5. **SpecialPackagedProduct** (Inherits from Product)
- Additional Property: PackagingPrice
- Overrides Price to include packaging cost
- Formula: `Total Price = Product Price + Packaging Price`

#### 6. **Order**
- Implements: `ICloneable`
- Properties: OrderID, BuyerDetails, Products
- Auto-generated OrderID with timestamp format: `#### [TAB] dd/MM/yyyy,HH:mm`
- Maintains product list snapshot at time of order

#### 7. **Address**
- Implements: `ICloneable`
- Properties: Country, City, Street, BuildingNumber
- Validation: All text fields must contain only letters
- Building number must be positive

#### 8. **SystemManager**
- Central management class for the entire system
- Properties: SystemName, Buyers, Sellers
- Custom capacity management with doubling strategy
- Initial capacity: 2 users per type, doubles when full
- Operator Overloading: `+` for adding buyers/sellers to the system
- Key Methods:
  - User management (add, validate, find)
  - Product management (add, search)
  - Order processing
  - Input parsing and validation

### Enumerations

#### ProductCategory
```csharp
enum ProductCategory { Clothing, Electronics, Kids, Office, Undefined }
```

#### PackagingOptions
```csharp
enum PackagingOptions { Regular, Special }
```

## 💾 Installation

### Prerequisites
- .NET Framework 4.7.2 or higher
- Visual Studio 2017 or higher (or any C# compatible IDE)
- Windows Operating System

### Setup Steps

1. **Clone the Repository**
   ```bash
   git clone <repository-url>
   cd CSharp-ElectronicCommercialSystem-Academic
   ```

2. **Open the Solution**
   - Open `FinalProject.sln` in Visual Studio

3. **Build the Project**
   - Press `Ctrl+Shift+B` or go to Build → Build Solution
   - Ensure no compilation errors

4. **Run the Application**
   - Press `F5` or click Start
   - The console interface will launch

## 🚀 Usage

### Main Menu Options

When you run the application, you'll see the following menu:

```
********************
Welcome to the Electronic Commercial System!
1. Add Buyer
2. Add Seller
3. Add Product to Seller
4. Add Product to Buyer's Cart
5. Complete Order for Buyer
6. Display All Buyers
7. Display All Sellers
8. Create New Cart from Order History
9. Compare Two Buyers
10. Exit
```

### Step-by-Step Usage Guide

#### 1. Adding a Buyer
```
Choice: 1
Username: john_doe
Password: Password123
Country: USA
City: NewYork
Street: Broadway
Building Number: 100
```

#### 2. Adding a Seller
```
Choice: 2
Username: tech_store
Password: Secure456
Country: USA
City: Seattle
Street: Market
Building Number: 50
```

#### 3. Adding Products
```
Choice: 3
Seller's username: tech_store
Product name: Laptop
Category: 2 (Electronics)
Price: 999.99
Special packaging: 2 (Special)
Packaging price: 25.00
```

#### 4. Shopping
```
Choice: 4
Buyer's username: john_doe
[View available products]
Product ID or name: 1000
Special package: 1 (Regular)
```

#### 5. Completing an Order
```
Choice: 5
Buyer's username: john_doe
[Review cart]
Confirm: 1 (Yes)
```

### Example Workflow

```
1. Add Seller "TechStore"
2. Add Products:
   - Laptop (Electronics, $999.99)
   - Mouse (Office, $29.99)
   - Keyboard (Office, $79.99)
3. Add Buyer "Customer1"
4. Add Laptop and Mouse to cart
5. Complete order (Order #0001 created)
6. View order history
7. Create new cart from previous order
```

## 🖥️ System Requirements

- **Operating System**: Windows 7 or higher
- **Framework**: .NET Framework 4.7.2
- **RAM**: Minimum 512 MB
- **Storage**: 50 MB free space
- **Display**: Console window support

## 📁 Project Structure

```
CSharp-ElectronicCommercialSystem-Academic/
│
├── FinalProject.sln              # Visual Studio Solution file
├── FinalProject.csproj            # Project configuration
├── App.config                     # Application configuration
│
├── Main.cs                        # Entry point and UI logic
├── SystemManager.cs               # System management
│
├── User.cs                        # Base user class
├── Buyer.cs                       # Buyer implementation
├── Seller.cs                      # Seller implementation
│
├── Product.cs                     # Product class
├── SpecialPackagedProduct .cs    # Special packaging variant
│
├── Order.cs                       # Order management
├── Address.cs                     # Address entity
│
├── Properties/
│   └── AssemblyInfo.cs           # Assembly metadata
│
├── bin/                          # Compiled binaries
└── obj/                          # Object files
```

## 🔑 Key Functionalities

### Validation Features

1. **Username Validation**
   - Must contain at least one letter
   - Must be unique across all users
   - Cannot be empty

2. **Password Validation**
   - Minimum 8 characters
   - Must contain both letters and numbers
   - Cannot be empty

3. **Price Validation**
   - Must be non-negative
   - Supports decimal values

4. **Address Validation**
   - Country, City, Street: Only letters allowed
   - Building Number: Must be positive

### Business Rules

1. **Order Completion**
   - Minimum 2 products required in cart
   - Cart is cleared after successful order
   - Order is added to buyer's history

2. **Product Management**
   - Each product has a unique ID
   - Products can have different packaging options
   - Special packaging adds to total price

3. **Seller Sorting**
   - Primary: Number of products (descending)
   - Secondary: Username (alphabetical)

4. **Dynamic Capacity Management**
   - Initial capacity: 2 users of each type
   - Doubles when capacity is reached
   - Custom implementation with manual tracking

## 🎨 Design Patterns

### 1. **Inheritance**
- `User` → `Buyer`, `Seller`
- `Product` → `SpecialPackagedProduct`

### 2. **Polymorphism**
- Virtual/Override methods for Price calculation
- Interface implementations (ICloneable, IComparable)

### 3. **Encapsulation**
- Private fields with public properties
- Property setters with validation logic

### 4. **Factory Pattern**
- Dynamic product creation based on packaging type
- Centralized object creation in SystemManager

### 5. **Singleton-like Behavior**
- Static counters for IDs (User, Product, Order)
- Ensures unique identifiers across instances

### 6. **Cloneable Pattern**
- Deep copy support for all major entities
- Protects against reference issues in collections

## 🎓 Academic Project

This is an academic project designed to demonstrate:
- Object-Oriented Programming principles
- C# language features and best practices
- Console application development
- Data structure usage (Lists, Collections)
- Exception handling and input validation
- Business logic implementation
- Clean code and documentation

### Learning Objectives
- Understanding inheritance and polymorphism
- Implementing interfaces (ICloneable, IComparable, IComparer)
- Working with generic collections
- Operator overloading
- Property validation
- Exception handling
- Menu-driven console applications

## 🤝 Contributing

This is an academic project, but contributions for educational purposes are welcome:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This is an academic project created for educational purposes.

## 🙏 Acknowledgments

- Created as part of a C# programming course
- Demonstrates practical application of OOP concepts
- Suitable for learning and educational reference

---

**Note**: This is a console-based application designed for educational purposes. It demonstrates core programming concepts and is not intended for production use.
