# Book Lending Management System

A desktop application developed in C# using WPF and MVVM architecture for managing a library's book inventory and lending process. This application supports core CRUD operations, loan management, and includes features such as undo, smart search, and data integrity constraints.

## Features

### Book Management
- **Add Book**: Create new books by specifying Title, Author, and Quantity.
- **Update Book**: Modify existing book details.
- **Delete Book**: Remove books from the system.
- **Search**: Filter books in real-time by title or author.
- **Constraints**:
  - Quantity cannot be reduced below the number of active loans.
  - Books with active loans cannot be deleted.

### Loan Management
- **Loan a Book**: Select a book to loan. Loaning is restricted when stock is depleted.
- **Return a Book**: Select and return an active loan. Returns are blocked if already returned.
- **View Active Loans**: Displays all current loans with loan and return dates.

### Undo Functionality
- Undo supported for:
  - Book creation
  - Book updates
  - Book deletions

### Navigation
- Pages: Dashboard, Book Management, Loan Management
- Each page includes back navigation support to return to the Dashboard.

## Getting Started

### Prerequisites
- Visual Studio 2022
- .NET 8

### Setup Instructions
1. Clone the repository:

```
git clone https://github.com/Mihai005/SiemensInternshipDotNet.git
```

2. Open the solution file (`.sln`) in Visual Studio.
3. Build the project to restore NuGet packages.
4. Run the application (Start without debugging or press `F5`).

> The database is powered by SQLite and is created automatically in your local environment. No additional setup is required. The local path is set inside Data/LibraryContext.cs and it should work on your system, but, if it doesn't, please adjust the path accordingly

## Usage

### Book Management
1. Navigate to the Book Management page.
2. Use the input fields to add a book.
3. Select any book in the list to update or delete.
4. Use the Undo button to revert the latest change.
5. Search using the provided search bar.

### Loan Management
1. Navigate to the Loan Management page.
2. Select a book to loan from the searchable list.
3. View and select active loans to return.

## Architecture

- **MVVM (Model-View-ViewModel)**: Promotes clean separation of concerns.
- **Entity Framework Core (EF Core)**: Used for data access and migrations.
- **SQLite**: Lightweight, file-based local database.

## Added Functionality

- As highlighted above, I implemented an Undo Action functionality for the CRUD.
