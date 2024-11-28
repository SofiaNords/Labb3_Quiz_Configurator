# Labb3_Quiz_Configurator

This application is a WPF-based project designed to configure quiz questions and run quiz rounds. It follows the Model-View-ViewModel (MVVM) architectural pattern, which promotes separation of concerns by decoupling the UI (View) from the underlying logic (Model and ViewModel).

## Project Overview
The app consists of two main parts:

1. Configurator: A section for creating and managing question packs, allowing users to add, edit, or delete questions, set difficulty levels and time limits.
2. Player: A section for running quiz rounds where users answer questions from an active question pack, with results displayed at the end of the round.

## Key Features

1. Configuration Mode:

Users can create, edit, and delete question packs.
Each question has four options, one of which is correct.
Pack options allow customization of the quiz's name, difficulty, and time limits.
The ability to add/remove questions through menus, keyboard shortcuts, and on-screen buttons.

2. Play Mode:

Displays the number of questions in the active pack and the user's progress.
Questions and answer options are displayed in random order each time.
A countdown timer is available for each question, with feedback provided after each answer (correct or incorrect).

3. Menu:

A menu with icons that can be accessed via both mouse and keyboard shortcuts.

4. JSON Support:

Question packs and questions are saved and loaded from JSON files.
Supports asynchronous file operations for smooth performance.

5. Full Screen:

An option to run the application in full-screen mode.

## File Storage
Automatic JSON File Storage: The application saves question packs in a JSON file located in the user’s AppData\Local directory. This file is loaded automatically when the app starts.

## MVVM Architecture
The project follows the MVVM pattern, where:

- Model represents the data and business logic (question packs, questions).
- ViewModel connects the Model with the View, managing the application’s state.
- View handles the user interface, displaying the data and sending user commands.

## Development Setup
The project is built using WPF and XAML for the UI, with C# for the logic. The structure is designed to be scalable and maintainable, allowing for easy modification and extension. The components are structured as follows:

- Dialogs: PackOptionsDialog.xaml and CreateNewPackDialog.xaml for managing quiz packs.
- Views: MenuView.xaml, ConfigurationView.xaml, and PlayerView.xaml for displaying the application’s different modes.

## Conclusion
This app is designed to help users create and play quiz rounds with flexibility, supporting features like randomization, time limits, and JSON-based data storage. The MVVM pattern ensures a clean separation of concerns, promoting maintainability and testability.

