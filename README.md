# LexiFlow - Vocabulary Learning Platform

An intelligent vocabulary learning platform inspired by Anki, designed to help language learners acquire and retain vocabulary more effectively through spaced repetition, automated dictionary integration, and interactive learning activities.

---

## Overview

Traditional flashcard applications such as Anki are powerful but often difficult for new users due to manual setup requirements, limited guidance, and a steep learning curve.

This project aims to provide a more user-friendly learning experience by automating vocabulary collection, generating learning materials from trusted dictionary sources, and supporting multiple learning modes beyond simple flashcards.

---

## Objectives

### Learning Objectives

- Practice ASP.NET Core backend development
    
- Apply Clean Architecture principles
    
- Design scalable database structures
    
- Implement authentication and authorization
    
- Integrate third-party APIs
    
- Build a modern React frontend
    

### Product Objectives

- Reduce time spent creating flashcards
    
- Automate vocabulary enrichment
    
- Improve vocabulary retention through spaced repetition
    
- Support multiple review methods
    
- Track daily learning progress
    

---

## Core Features

### Authentication

- User registration
    
- Login with JWT authentication
    
- Refresh token support
    
- User profile management
    

---

### Vocabulary Management

Users can:

- Create vocabulary entries
    
- Edit vocabulary entries
    
- Delete vocabulary entries
    
- Organize words into decks
    
- Search vocabulary
    

Each vocabulary item may contain:

- Word
    
- IPA pronunciation
    
- Definitions
    
- Examples
    
- Synonyms
    
- Antonyms
    
- CEFR level
    
- Audio pronunciation
    

---

### Dictionary Integration

When a user adds a new word:

1. System searches dictionary APIs
    
2. Retrieves:
    
    - Definitions
        
    - Examples
        
    - IPA
        
    - Audio pronunciation
        
3. Automatically generates learning content
    

Potential integrations:

- Free Dictionary API
    
- Merriam-Webster API
    
- Oxford API
    
- Cambridge API (if available)
    

---

### Study System

Supported review modes:

#### Flashcard

Front side:

- Word
    

Back side:

- Meaning
    
- Example
    
- Pronunciation
    

#### Multiple Choice

Example:

What does "meticulous" mean?

A. Careless  
B. Precise ✅  
C. Lazy  
D. Reckless

#### Fill In The Blank

Example:

He is a very ______ engineer.

Answer:

meticulous

---

### Spaced Repetition

Initial implementation:

SM-2 inspired algorithm

Review buttons:

- Again
    
- Hard
    
- Good
    
- Easy
    

System calculates:

- Next review date
    
- Ease factor
    
- Review interval
    

---

### Daily Goals

Users can configure:

- New words per day
    
- Reviews per day
    
- Study reminders
    

Dashboard shows:

- Daily progress
    
- Streak count
    
- Review completion
    

---

### Audio Support

Priority order:

1. Dictionary audio
    
2. Generated TTS audio
    

Potential providers:

- Azure Speech Service
    
- Google Text-to-Speech
    

---

## Technical Architecture

### Backend

- ASP.NET Core Web API
    
- Entity Framework Core
    
- SQL Server
    
- JWT Authentication
    
- FluentValidation
    
- AutoMapper
    

### Frontend

- React
    
- TypeScript
    
- Redux Toolkit
    
- React Query
    
- TailwindCSS
    
- ShadCN UI
    

### Infrastructure

- Azure
    
- Cloudflare R2 (optional)
    
- Docker (future)
    

---

## Database Modules

### Identity

- Users
    
- Roles
    
- Refresh Tokens
    

### Learning

- Decks
    
- Vocabulary
    
- Review Sessions
    
- Review Records
    

### Dictionary

- Definitions
    
- Examples
    
- Pronunciations
    
- Audio Assets
    

---

## Project Status

### Phase 1 (MVP)

-  Authentication
    
-  User Management
    
-  Deck Management
    
-  Vocabulary CRUD
    
-  Dictionary API Integration
    
-  Review Session
    
-  Flashcard Mode
    
-  Basic Spaced Repetition
    

### Phase 2

-  Multiple Choice Review
    
-  Fill In Blank Review
    
-  Daily Goals
    
-  Statistics Dashboard
    
-  Audio Generation
    

### Phase 3

-  AI Example Generation
    
-  Adaptive Learning Algorithm
    
-  Mobile Application
    
-  Browser Extension
    

---

## Future Improvements

- AI-generated examples
    
- AI-generated quizzes
    
- Personalized review intervals
    
- IELTS vocabulary packs
    
- TOEIC vocabulary packs
    
- Community shared decks
    
- Mobile application
    

---

## Author

Hoàng Minh Lộc

Final Year Software Engineering Student

ASP.NET Core | React | SQL Server
