# GameVault

GameVault is a cross-platform game tracking application that allows users to manage their personal game libraries, track playtime, write reviews, and connect with other players. It was built as an academic project at AUE-FON University.

## Overview

GameVault consists of two applications that share a single SQL Server database:

- `GameVault.API` - a .NET 8 REST API that handles data access and business logic
- `GameVault.Mobile` - a React Native mobile application built with Expo for end users

A separate web portal for administrators and moderators is planned as a future deliverable.

## Repository Structure

```text
GameVault/
	GameVault.API/       .NET 8 Web API
	GameVault.Mobile/    React Native Expo app
	README.md
```

## Tech Stack

### API

- .NET 8
- Entity Framework Core 8
- SQL Server
- Swagger / OpenAPI

### Mobile

- React Native with Expo SDK 54
- React Navigation
- expo-image
- react-native-render-html
- dayjs
- NativeWind

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server, local or remote
- Node.js 18 or higher
- Expo CLI tools available through `npx expo`
- Expo Go on a mobile device if you want to test on-device

### Database Setup

1. Create a database named `GameVault` on your SQL Server instance.
2. Configure the API project to point at that database.
3. Seed the database with the data required by the API and mobile app.

### Running the API

1. Navigate to the API project.
2. Update the connection string in `GameVault.API/appsettings.json`.
3. Start the project with `dotnet run`.

The API exposes Swagger in development mode.

### Running the Mobile App

1. Navigate to the mobile project.
2. Install dependencies with `npm install`.
3. Update the API base URL in `GameVault.Mobile/src/config.js` to match your network environment.
4. Start the Expo development server with `npx expo start`.

If you are testing on a physical device, make sure the phone and development machine are on the same network.

## Features

### Guest Users

- Browse the game catalogue
- View game details, screenshots, developer info, and descriptions
- Read community reviews and ratings
- Search and filter games by title, category, and platform
- View news and static information pages

### Registered Users

- Manage a personal game library with custom statuses
- Log playtime hours per game
- Write and edit reviews
- Follow other users and view activity updates
- Submit new games for review
- Customize a public profile

### Moderators

- Review and approve submitted games
- Edit existing catalogue entries

### Administrators

- Manage users, catalogue data, categories, and platforms
- Publish news and static content
- Access audit and moderation workflows

## Rating System

GameVault uses an aggregate rating model. A game must have enough reviews before a numeric score is shown; otherwise it is displayed as unrated.

## Project Status

This is an academic prototype. The current implementation focuses on the guest-facing mobile experience and the read-only API endpoints that support it. Authentication, user library features, and the future admin web portal are planned for later phases.
