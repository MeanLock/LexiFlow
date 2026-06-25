CREATE DATABASE LexiFlow;
GO

USE LexiFlow;
GO

CREATE TABLE Users
(
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),

    FullName NVARCHAR(200) NOT NULL,

    Dob DATE NOT NULL,

    Email NVARCHAR(255) NOT NULL,

    Password NVARCHAR(500) NULL,

    GoogleId NVARCHAR(200) NULL,

    Role INT NOT NULL DEFAULT 1, -- 1 Learner, 2 Admin

    ContributeScore INT NOT NULL DEFAULT 0,

    IsVerified BIT NOT NULL DEFAULT 0,
    IsDeleted BIT NOT NULL DEFAULT 0,

    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NULL
);

CREATE UNIQUE INDEX IX_Users_Email
ON Users(Email);

CREATE TABLE EmailVerifications
(
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),

    Email NVARCHAR(255) NOT NULL,

    Otp VARCHAR(128) NOT NULL,---- hash

    ExpiredAt DATETIME2 NOT NULL,

    VerifiedAt DATETIME2 NULL,

    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

CREATE TABLE Decks
(
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),

    OwnerId UNIQUEIDENTIFIER NOT NULL,

    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(1000) NULL,

    IsDeleted BIT NOT NULL DEFAULT 0,

    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NULL,

    CONSTRAINT FK_Decks_Users
        FOREIGN KEY (OwnerId)
        REFERENCES Users(Id)
);

CREATE TABLE Cards
(
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),

    DeckId UNIQUEIDENTIFIER NOT NULL,

    CardType INT NOT NULL,

    Term NVARCHAR(300) NOT NULL,

    NormalizedTerm NVARCHAR(300) NOT NULL,

    PrimaryMeaning NVARCHAR(1000) NULL,

    IsPublic BIT NOT NULL DEFAULT 0,

    SourceType INT NOT NULL,

    SourceCardId UNIQUEIDENTIFIER NULL,

    IsDeleted BIT NOT NULL DEFAULT 0,

    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NULL,

    CONSTRAINT FK_Cards_Decks
        FOREIGN KEY (DeckId)
        REFERENCES Decks(Id),

    CONSTRAINT FK_Cards_SourceCard
        FOREIGN KEY (SourceCardId)
        REFERENCES Cards(Id)
);

CREATE TABLE VocabularyCardDetails
(
    CardId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,

    Phonetic NVARCHAR(200) NULL,

    AudioUrl NVARCHAR(1000) NULL,

    CefrLevel NVARCHAR(20) NULL,

    CONSTRAINT FK_VocabularyCardDetails_Cards
        FOREIGN KEY (CardId)
        REFERENCES Cards(Id)
);

CREATE TABLE VocabularyMeanings
(
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),

    CardId UNIQUEIDENTIFIER NOT NULL,

    PartOfSpeech NVARCHAR(50) NOT NULL,

    Definition NVARCHAR(MAX) NOT NULL,

    Example NVARCHAR(MAX) NULL,

    ExampleMeaning NVARCHAR(MAX) NULL,

    Synonyms NVARCHAR(MAX) NULL,
    Antonyms NVARCHAR(MAX) NULL,

    CONSTRAINT FK_VocabularyMeanings_Cards
        FOREIGN KEY (CardId)
        REFERENCES Cards(Id)
);

CREATE TABLE CollocationCardDetails
(
    CardId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,

    Pattern NVARCHAR(500) NOT NULL,

    Explanation NVARCHAR(MAX) NULL,

    Example NVARCHAR(MAX) NULL,

    ExampleMeaning NVARCHAR(MAX) NULL,

    CONSTRAINT FK_CollocationCardDetails_Cards
        FOREIGN KEY (CardId)
        REFERENCES Cards(Id)
);

CREATE TABLE IdiomCardDetails
(
    CardId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,

    LiteralMeaning NVARCHAR(MAX) NULL,

    FigurativeMeaning NVARCHAR(MAX) NULL,

    UsageNote NVARCHAR(MAX) NULL,

    Example NVARCHAR(MAX) NULL,

    ExampleMeaning NVARCHAR(MAX) NULL,

    CONSTRAINT FK_IdiomCardDetails_Cards
        FOREIGN KEY (CardId)
        REFERENCES Cards(Id)
);

CREATE TABLE PhraseCardDetails
(
    CardId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,

    PhraseType NVARCHAR(100) NULL,

    UsageNote NVARCHAR(MAX) NULL,

    Example NVARCHAR(MAX) NULL,

    ExampleMeaning NVARCHAR(MAX) NULL,

    CONSTRAINT FK_PhraseCardDetails_Cards
        FOREIGN KEY (CardId)
        REFERENCES Cards(Id)
);

CREATE TABLE ReviewStates
(
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),

    UserId UNIQUEIDENTIFIER NOT NULL,

    CardId UNIQUEIDENTIFIER NOT NULL,

    DueDate DATE NOT NULL,

    EaseFactor DECIMAL(5,2) NOT NULL,

    IntervalDays INT NOT NULL,

    Repetition INT NOT NULL,

    Lapses INT NOT NULL,

    LastReviewedAt DATETIME2 NULL,

    SuspendedAt DATETIME2 NULL,

    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NULL,

    CONSTRAINT FK_ReviewStates_Users
        FOREIGN KEY (UserId)
        REFERENCES Users(Id),

    CONSTRAINT FK_ReviewStates_Cards
        FOREIGN KEY (CardId)
        REFERENCES Cards(Id)
);

CREATE TABLE ReviewSessions
(
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),

    UserId UNIQUEIDENTIFIER NOT NULL,

    DeckId UNIQUEIDENTIFIER NOT NULL,

    StartedAt DATETIME2 NOT NULL,

    CompletedAt DATETIME2 NULL,

    TotalCards INT NOT NULL DEFAULT 0,

    CorrectCount INT NOT NULL DEFAULT 0,

    CONSTRAINT FK_ReviewSessions_Users
        FOREIGN KEY (UserId)
        REFERENCES Users(Id),

    CONSTRAINT FK_ReviewSessions_Decks
        FOREIGN KEY (DeckId)
        REFERENCES Decks(Id)
);

CREATE TABLE ReviewRecords
(
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),

    ReviewSessionId UNIQUEIDENTIFIER NOT NULL,

    UserId UNIQUEIDENTIFIER NOT NULL,

    CardId UNIQUEIDENTIFIER NOT NULL,

    ReviewMode INT NOT NULL,

    Rating INT NOT NULL,

    AnswerText NVARCHAR(MAX) NULL,

    IsCorrect BIT NOT NULL,

    ResponseTimeMs INT NULL,

    PreviousEaseFactor DECIMAL(5,2) NOT NULL,
    NewEaseFactor DECIMAL(5,2) NOT NULL,

    PreviousIntervalDays INT NOT NULL,
    NewIntervalDays INT NOT NULL,

    ReviewedAt DATETIME2 NOT NULL,

    CONSTRAINT FK_ReviewRecords_Session
        FOREIGN KEY (ReviewSessionId)
        REFERENCES ReviewSessions(Id),

    CONSTRAINT FK_ReviewRecords_Users
        FOREIGN KEY (UserId)
        REFERENCES Users(Id),

    CONSTRAINT FK_ReviewRecords_Cards
        FOREIGN KEY (CardId)
        REFERENCES Cards(Id)
);

CREATE UNIQUE INDEX IX_ReviewStates_UserId_CardId
ON ReviewStates(UserId, CardId);