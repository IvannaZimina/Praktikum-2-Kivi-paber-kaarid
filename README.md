# Kivi-Paber-Käärid Turniir (Rock-Paper-Scissors Tournament)

Desktop application built with **WPF (.NET)** implementing a modular Core + UI architecture as part of Laboratory Work 2.

---

## Solution Content

The solution (`RpsTournament`) consists of two main projects:
1. **`RpsTournament.Core`** (Class Library) — contains the core business logic of the game:
   - Enums: `Move` and `RoundResult`.
   - Struct: `GameRound` representing a single game round.
   - Static class `GameLogic` handling game rules and random computer move generation.
2. **`RpsTournament.WpfApp`** (WPF Application) — the user interface project referencing the `Core` library.

---

## User Guide (How to Play)

* **Enter Name**: Type your name into the "Mängija nimi" field (validated for 2 to 30 characters). If the name is missing or too short, an error message appears directly beneath the field.
* **Select Move**: Choose your move by clicking one of the intuitive buttons ("Kivi", "Paber", "Käärid"). Your selected move is clearly highlighted in gold.
* **Play Round**: Click the "Mängi voor" button to play. The computer makes a random choice, and the round result is instantly added to the history table.
* **New Tournament**: At any time during the game, you can click the "Uus turniir" button to manually reset the history, scores, and game state.

---

## Features & Game Rules

* **5-Round Tournament**: The game consists of exactly five rounds, after which the final score is calculated.
* **Input Validation**: 
  - Player name and move selection are strictly validated. 
  - Required fields are marked with an asterisk `*` (`* - kohustuslikud väljad`).
* **User Feedback & Errors**:
  - Error messages appear directly beneath the respective fields in Estonian.
* **History & Scoring**:
  - Each played round is added to a `DataGrid` displaying the round number, player move, computer move, and result (`Win`, `Loss`, `Draw`).
  - A summary panel tracks current statistics (`Võidud`, `Kaotused`, `Viigid`).
* **Tournament Completion & Reset**:
  - After the 5th round, a message box (`MessageBox`) announces the winner or reports a tie (*«Mäng lõppes viigiseisuga!»*).
  - Once the game finishes, the table and scores are automatically cleared for a fresh start.

---

## User Interface (UI) Design

* Clean, modern light-themed layout (`#F4F5F7`).
* Strict grid alignment ensuring input fields and control panels match in width.
* Interactive move buttons featuring a distinct gold highlight for the selected choice.
* Professionally styled result table with centered data and clean borders.

---

## Technologies & C# Features

* **Object-Oriented Programming & Data Structures**: Enums, structs, static classes.
* **Algorithms**: Random number generation (`Random.Shared`), pattern matching (`switch` expressions) for round outcome calculation.
* **WPF Elements**: `DataGrid`, `Grid`, `StackPanel`, custom button control templates, and localized resource integration (`Resources.resx`).