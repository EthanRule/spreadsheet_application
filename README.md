# Spreadsheet Application

A comprehensive spreadsheet application built in C# using Windows Forms, featuring advanced expression evaluation, undo/redo functionality, and XML-based file persistence.

## Overview

This spreadsheet application provides a Microsoft Excel-like interface with a powerful calculation engine that supports mathematical expressions, cell references, and dependency tracking. The application is built using a clean architecture pattern with separate layers for the UI, business logic, and data persistence.

## Features

### Core Functionality

- **50x26 Grid**: Standard spreadsheet layout with columns A-Z and rows 1-50
- **Cell Editing**: Direct text input with real-time value calculation
- **Mathematical Expressions**: Support for complex formulas using cell references (e.g., `=A1+B2*C3`)
- **Cell References**: Dynamic linking between cells with automatic dependency tracking
- **Background Color Customization**: Visual cell formatting with color picker
- **Undo/Redo System**: Complete command pattern implementation for all operations

### Expression Engine

- **Four Basic Operations**: Addition (+), Subtraction (-), Multiplication (\*), Division (/)
- **Order of Operations**: Proper precedence handling with parentheses support
- **Variable Support**: Cell references as variables in expressions
- **Error Handling**: Comprehensive error detection and user-friendly error messages
- **Circular Reference Detection**: Prevents infinite loops with advanced DFS algorithm
- **Self-Reference Detection**: Identifies and prevents cells referencing themselves

### File Operations

- **Save/Load**: XML-based persistence for spreadsheet data
- **Format Preservation**: Maintains cell text, values, and background colors
- **Cross-Session**: Complete state restoration between application sessions

### Error Handling

- **Bad References**: Invalid cell references display `!(bad reference)`
- **Circular References**: Circular dependencies show `!(circular reference)`
- **Self References**: Self-referencing cells display `!(self reference)`
- **Division by Zero**: Proper exception handling for mathematical errors
- **Invalid Operators**: Graceful handling of unsupported operations

## Architecture

### Project Structure

```
Solution/
├── SpreadsheetEngine/          # Core business logic
│   ├── Cell.cs                # Abstract cell implementation
│   ├── Spreadsheet.cs          # Main spreadsheet controller
│   ├── ExpressionTree/         # Mathematical expression engine
│   ├── CellTextCommand.cs      # Text change commands
│   ├── CellBackgroundCommand.cs # Background color commands
│   └── ICommand.cs            # Command pattern interface
├── Spreadsheet_Ethan_Rule/     # Windows Forms UI
│   ├── Form1.cs               # Main application form
│   └── Form1.Designer.cs      # UI layout definitions
├── TestProject1/               # Comprehensive unit tests
│   ├── SpreadSheetTests.cs    # Spreadsheet functionality tests
│   └── ExpressionTreeTests.cs  # Expression engine tests
└── ConsoleApp1/                # Console testing application
```

### Key Components

#### Spreadsheet Engine

- **Cell Class**: Abstract base class implementing `INotifyPropertyChanged`
- **Spreadsheet Class**: Main controller managing cell matrix and operations
- **Expression Tree**: Dijkstra's Shunting-yard algorithm for expression parsing
- **Command Pattern**: Undo/redo functionality for all operations

#### Expression Tree System

- **Node Hierarchy**: Abstract base with concrete implementations
- **Binary Operators**: Factory pattern for operator node creation
- **Variable Management**: Dynamic variable binding and evaluation
- **Postfix Conversion**: Efficient expression evaluation

#### UI Layer

- **DataGridView**: Grid-based cell display and editing
- **Event Handling**: Property change notifications and user interactions
- **Menu System**: File operations and formatting controls
- **Color Dialog**: Background color selection interface

## Technical Details

### Dependencies

- **.NET 8.0**: Modern C# framework with nullable reference types
- **Windows Forms**: Cross-platform UI framework
- **NUnit**: Comprehensive unit testing framework
- **StyleCop**: Code style and quality enforcement

### Design Patterns

- **Command Pattern**: Undo/redo functionality
- **Observer Pattern**: Property change notifications
- **Factory Pattern**: Operator node creation
- **Strategy Pattern**: Expression evaluation strategies

### Algorithms

- **Shunting-yard Algorithm**: Expression parsing and postfix conversion
- **Depth-First Search**: Circular reference detection
- **Dependency Tracking**: Cell reference management

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- Windows operating system (for Windows Forms)
- Visual Studio 2022 or compatible IDE

### Building the Application

1. Clone the repository
2. Open `Solution/Solution.sln` in Visual Studio
3. Build the solution (Ctrl+Shift+B)
4. Run the `Spreadsheet_Ethan_Rule` project

### Running Tests

```bash
dotnet test Solution/TestProject1/TestProject1.csproj
```

## Usage Examples

### Basic Cell Operations

- Enter numbers directly: `123`
- Use expressions: `=A1+B2`
- Reference cells: `=C3*2`

### Complex Expressions

- Order of operations: `=A1+B2*C3`
- Parentheses: `=(A1+B2)*C3`
- Multiple operations: `=A1+B2-C3/D4`

### File Operations

- Save: File → Save (Ctrl+S)
- Load: File → Load (Ctrl+O)
- Format: Right-click → Change Background Color

## Testing

The project includes comprehensive unit tests covering:

- Expression tree evaluation
- Cell reference handling
- Circular reference detection
- Command pattern operations
- File save/load functionality
- Error handling scenarios

Run tests to verify functionality:

```bash
dotnet test --verbosity normal
```

## Error Handling

The application provides robust error handling for:

- **Mathematical Errors**: Division by zero, overflow
- **Reference Errors**: Invalid cell references, out-of-bounds access
- **Expression Errors**: Malformed expressions, unsupported operators
- **File Errors**: Corrupted files, permission issues

## Performance Considerations

- **Efficient Evaluation**: Postfix notation for fast expression evaluation
- **Dependency Tracking**: Minimal recalculation on cell changes
- **Memory Management**: Proper disposal of resources and event handlers
- **Lazy Loading**: Cells created on-demand for optimal memory usage

## Future Enhancements

Potential improvements for future versions:

- Additional mathematical functions (sin, cos, sqrt, etc.)
- Chart and graph generation
- Multi-sheet support
- Formula bar interface
- Copy/paste operations
- Find and replace functionality

## License

This project is developed as part of academic coursework. Please refer to the copyright notices in individual source files for specific licensing information.

## Author

**Ethan Rule**  
WSU ID: 11714155

---

_This spreadsheet application demonstrates advanced C# programming concepts including design patterns, event handling, file I/O, and comprehensive testing methodologies._
