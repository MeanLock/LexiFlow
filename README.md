# LexiFlow

An intelligent vocabulary learning platform inspired by Anki that helps language learners build and retain vocabulary more efficiently through spaced repetition, automated dictionary integration, and interactive review activities.

---

## Features

### Authentication

* User registration
* JWT authentication
* OAuth2 with Google
* User profile management

### Vocabulary Management

* Create, edit, and delete vocabulary cards
* Organize cards into decks
* Search vocabulary
* Public and private decks (planned)

Each card may contain:

* Word
* IPA pronunciation
* Definitions
* Examples
* Synonyms
* Antonyms
* CEFR level
* Audio pronunciation

### Dictionary Integration

When a user creates a new vocabulary card, LexiFlow allows users to retrieve information from external dictionary APIs, including:

* Definitions
* IPA pronunciation
* Examples
* Audio pronunciation

Current integration:

* WordsAPI

Future integrations:

* Free Dictionary API
* Oxford Dictionary API
* Cambridge Dictionary API
* Shared dictionary definitions across users

### Study Modes

* Flashcards
* Multiple Choice (planned)
* Fill in the Blank (planned)

### Spaced Repetition

SM-2 inspired algorithm.

Each review updates:

* Review interval
* Ease factor
* Next review date

Review options:

* Again
* Hard
* Good
* Easy

### Dashboard

* Daily review progress
* Learning streak
* Review statistics

---

# Technology Stack

## Backend

* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* JWT Authentication

## Frontend

* React
* TypeScript
* Redux Toolkit
* React Query
* Tailwind CSS
* shadcn/ui

## Infrastructure

* Azure (planned)
* Docker (planned)
* Cloudflare R2 (optional)

---

# Project Architecture

The backend follows a traditional **three-layer architecture**.

```text
                React Client
                     │
                     ▼
             LexiFlow.API
      (Controllers, Authentication,
      Middleware, Swagger)
                     │
                     ▼
             LexiFlow.BLL
      (Business Logic, Services,
      Models, Validation)
                     │
                     ▼
             LexiFlow.DAL
      (EF Core, DbContext,
      Entities, Migrations)
                     │
                     ▼
                SQL Server
```

## Layer Responsibilities

### LexiFlow.API

* Exposes RESTful APIs
* Handles authentication and authorization
* Configures middleware and dependency injection
* Receives requests and returns responses

### LexiFlow.BLL

* Contains business logic
* Implements application services
* Performs validation
* Maps DTOs and entities
* Coordinates application workflows

### LexiFlow.DAL

* Handles data persistence
* Defines EF Core entities
* Contains DbContext and configurations
* Executes database operations

---

# Database Modules

## Identity

* Users
* Roles

## Learning

* Decks
* Cards
* Review Sessions
* Review Records

---

# Roadmap

## MVP

* Authentication
* User management
* Deck CRUD
* Cards CRUD
* WordsAPI integration
* Flashcard review
* Basic spaced repetition

## Future

* Multiple Choice review
* Fill in the Blank review
* Daily goals
* Statistics dashboard
* AI-generated examples
* AI-generated quizzes
* Adaptive learning algorithm
* Mobile application
* Browser extension

---

# Getting Started

## Backend

```bash
git clone https://github.com/MeanLock/LexiFlow.git

cd LexiFlow

dotnet restore

dotnet ef database update

dotnet run --project LexiFlow.API
```

## Frontend

```bash
cd frontend

npm install

npm run dev
```

---

# Configuration

Sensitive information such as:

* API Keys
* SMTP Passwords
* Connection Strings
* WordsAPI Api Key

should be stored using:

* User Secrets (Development)
* Environment Variables (Production)

---

# Author

**Hoàng Minh Lộc**

Final Year Software Engineering Student

**ASP.NET Core • React • SQL Server**
