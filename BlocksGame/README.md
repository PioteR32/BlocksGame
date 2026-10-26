# BlocksGame (.NET MAUI)

A high-performance, grid-based logic game implemented with **.NET MAUI**. This project serves as a foundational exploration of cross-platform UI interactions, complex coordinate mapping, and automated unit testing in the .NET ecosystem.

## 🚀 Overview

BlocksGame is a puzzle game where players manipulate geometric shapes on a dynamic grid. Unlike simple CRUD applications, this project focuses on the intersection of **UI Gesture Recognition** and **Mathematical Grid Logic**, ensuring that visual transformations on the screen translate accurately to the underlying game state.

## 🛠 Tech Stack

*   **Framework:** .NET MAUI (.NET 8)
*   **Language:** C#
*   **UI Pattern:** MVVM (Model-View-ViewModel)
*   **Testing:** xUnit (Unit Testing)
*   **Platforms:** Android, iOS, Windows, MacCatalyst

## 🧠 Core Engineering Challenges

### 1. Coordinate Space Mapping
One of the primary challenges was implementing a seamless translation between **Screen-Space Coordinates** (where the user touches/drags) and **Grid-Space Coordinates** (the logical position of a block). I implemented a custom mapping algorithm to handle:
*   Real-time translation during `PanGestureRecognizer` updates.
*   Dynamic scaling of blocks to fit the target grid cell.
*   Collision detection between visual elements and logical grid cells.

### 2. Algorithmic Grid Manipulation
The game engine implements complex 2D array manipulations, including:
*   **Dynamic Shape Generation:** Algorithmic generation of unique block structures.
*   **Automated Row/Column Clearing:** Logic to detect and remove completed lines in a 2D array, including "cascading" effects.
*   **Geometric Trimming:** An optimized algorithm to "cut" empty space from block structures to ensure efficient memory and rendering.

### 3. Automated Testing (High-Signal Logic)
To ensure the mathematical integrity of the game engine, I implemented a comprehensive suite of unit tests. The tests cover:
*   **Boundary conditions** in the grid generation algorithms.
*   **Data integrity** during 2D-to-List-to-2D transformations.
*   **Game state validation** (e.g., determining if the board is clear or if a move is valid).

## 🏗 Architecture

The project follows the **MVVM (Model-View-ViewModel)** pattern to ensure a clean separation of concerns:
*   **Models:** Represent the pure state of the game (Blocks, Fields, Boards).
*   **ViewModels:** Manage the game lifecycle, score tracking, and command execution.
*   **Views:** Purely declarative XAML layers utilizing `BindableLayout` for high-performance rendering of dynamic UI elements.

## 🧪 Testing Suite

The `BlocksGameTests` project utilizes **xUnit** to validate the core engine. 
*   **Coverage:** High coverage for the `Block` and `BlocksBoard` logic.
*   **Reliability:** Ensures that complex geometric calculations remain consistent across updates.

## 🚀 Getting Started

### Prerequisites
*   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
*   Visual Studio 2022 (with .NET MAUI workload) or VS Code

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/BlocksGame.git
