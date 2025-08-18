# GameVault - Digital Game Marketplace

A comprehensive web-based digital game marketplace built with ASP.NET Core MVC, providing Steam-like functionality for game distribution and management.

## Table of Contents
- [Project Overview](#project-overview)
- [Architecture & Technologies](#architecture--technologies)
- [Key Features](#key-features)
  - [User Authentication & Authorization](#user-authentication--authorization)
  - [Localization & Globalization](#localization--globalization)
  - [Real-time Features](#real-time-features)
  - [AJAX Integration](#ajax-integration)
  - [Game Management](#game-management)
  - [Review & Rating System](#review--rating-system)
  - [Purchase System](#purchase-system)
- [User Experience](#user-experience)
- [Publisher Experience](#publisher-experience)
- [Security Features](#security-features)
- [Performance Optimizations](#performance-optimizations)
- [Development Best Practices](#development-best-practices)
- [Team Members](#team-members)

## Project Overview

GameVault is a full-featured digital game marketplace that replicates the functionality of major gaming platforms like Steam or Epic Games Store. The platform enables users to discover, purchase, and manage their digital game library while providing publishers with tools to distribute and monitor their games.

[![Login Page](https://i.postimg.cc/SQGqYCjB/Game-Vault-login.jpg)](https://postimg.cc/z3349LC7)
*GameVault Login Page with Social Authentication Options*

The platform features a modern, responsive design with support for multiple authentication methods including Google and Facebook OAuth integration alongside traditional email-based authentication.

## Architecture & Technologies

### Layered Architecture
The application follows a well-structured layered architecture ensuring clean separation of concerns:

- **Presentation Layer (ASP.NET Core MVC)**: Handles user interface, views, and controllers
- **Business Logic Layer (BLL)**: Contains core logic for game management, purchasing, reviews, and categories  
- **Data Access Layer (DAL)**: Manages database entities and persistence using Entity Framework Core

### Core Technologies Stack
- **Framework**: ASP.NET Core MVC
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: ASP.NET Core Identity with external OAuth providers
- **Real-time Communication**: SignalR for live updates
- **Background Processing**: Hosted Services for automated tasks
- **Client-side Technologies**: Bootstrap, jQuery, AJAX
- **Internationalization**: ASP.NET Core Localization services

![Home Page - Light Theme](https://i.postimg.cc/L5xrCk1h/Game-Vault-home2.jpg)
*GameVault Home Page - Light Theme with Featured Games*

## Key Features

### User Authentication & Authorization

**Multi-Method Authentication System**:
- Email confirmation with secure token validation
- External OAuth integration (Google & Facebook)
- Role-based authorization with custom policies
- Password complexity requirements and security measures

**Authorization Levels**:
- **Admin**: Full system management capabilities, game publishing and management rights
- **User**: Game browsing, purchasing, and reviewing permissions

### Localization & Globalization

**Multi-Language Support**:
- Complete internationalization infrastructure
- Resource-based localization system
- Seamless language switching without losing session state

### Real-time Features

**SignalR Integration**:
- Live sales notifications and updates
- Real-time game availability changes
- Background job processing with real-time feedback

### AJAX Integration

**Dynamic User Experience**:
- Asynchronous game search with instant results
- Add-to-cart functionality without page refreshes
- Dynamic content loading and pagination
- Smooth user interactions with loading indicators

### Game Management

**Comprehensive Game Catalog**:
- Advanced categorization system
- Rich media support (images, videos, screenshots)
- Detailed game descriptions and specifications
- Release date tracking and upcoming games
- Featured games and promotional content

### Review & Rating System

**Community-Driven Feedback**:
- Verified purchase requirement for reviews
- Star-based rating system (1-5 stars)
- Detailed written reviews with moderation
- Average rating calculations and display

### Purchase System

**Secure Transaction Processing**:
- Secure payment gateway integration
- Purchase history and receipt generation
- Automatic library updates after purchase

![Home Page - Dark Theme](https://i.postimg.cc/vBsFcZy8/Game-Vault-home.jpg)
*GameVault Home Page - Dark Theme showcasing popular games *
## User Experience

**For Gamers**:
- **Browse & Discover**: Explore extensive game catalog with advanced filtering
- **Secure Purchasing**: Safe and secure game acquisition process
- **Personal Library**: Organized collection of owned games with download management
- **Community Reviews**: Read and write detailed game reviews and ratings
- **Wishlist Management**: Save games for future purchase


## Security Features

**Comprehensive Security Measures**:
- **Input Validation**: Server-side and client-side validation
- **CSRF Protection**: Anti-forgery token implementation
- **Data Encryption**: Sensitive data encryption at rest and in transit
- **Secure Authentication**: Multi-factor authentication options

## Development Best Practices

**Code Quality & Maintenance**:
- **SOLID Principles**: Clean architecture implementation
- **Dependency Injection**: Loosely coupled component design
- **Unit Testing**: Comprehensive test coverage with xUnit
- **Integration Testing**: End-to-end testing scenarios
- **Code Documentation**: Detailed inline documentation
- **Version Control**: Git workflow with feature branches

**Error Handling & Logging**:
- Comprehensive exception handling
- Structured logging with different log levels
- User-friendly error pages

## Team Members

This project was collaboratively developed by :

- **[Ibrahim Abohola](https://github.com/Ibrahim-Abohola)** 
- **[Sherif](https://github.com/shereiff1)**  
- **[Bassam](https://github.com/bassam348)** 

---
