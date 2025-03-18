

# Notebook App
================

A desktop application for note-taking and organization.

## Overview
-----------

Notebook App is a Windows desktop application designed to help users create, organize, and manage their notes. The application allows users to create notebooks, add notes to those notebooks, and edit and save their notes.

## Features
------------

* Create and manage multiple notebooks
* Add, edit, and delete notes within notebooks
* Save notes to file locations
* Load and display saved notes
* Basic text formatting options (font family, font size)

## Technical Details
--------------------

* Built using C# and Windows Presentation Foundation (WPF)
* Utilizes SQLite for data storage
* Implements Model-View-ViewModel (MVVM) architecture

## Getting Started
-------------------

1. Clone the repository to your local machine.
2. Open the solution in Visual Studio.
3. Build and run the application.

## Project Structure
---------------------

* `Notebook_App`: The main application project.
* `Notebook_App.Model`: The data model project, containing classes for notebooks, notes, and users.
* `Notebook_App.ViewModel`: The view model project, containing classes for managing data and business logic.
* `Notebook_App.View`: The view project, containing XAML files for the user interface.
