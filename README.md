# Cook50

Cook50 is an AI-powered recipe generator that helps you create meals based on the ingredients you have on hand. By using the Gemini AI API, the application suggests recipes tailored to your available ingredients.

## Tech Stack

- **Frontend**: Next.js with JavaScript
- **Styling**: Tailwind CSS
- **Backend**: ASP.NET with C#
- **AI Technologies**:
  - Gemini API (Recipe Generator)

## Key Features

- **Ingredient Based Recipe Generation**: Generating Recipes based on the inputted ingredients
- **PDF Download**: Downloads recipes in a PDF formate

## How It Works

1. Enter the ingredients available
2. Save recipes as PDF files for offline use

## Getting Started

### Prerequisites

- Node.js
- .NET 9.0
- API key for Google's Gemini AI

### Installation

1. Install frontend dependencies

   ```bash
   cd cook50-frontend
   npm install
   ```

2. Install backend dependencies

   ```bash
   cd cook50-backend/cook50-backend
   dotnet restore
   ```
### Environment Setup

Create dotnet user secrets, in it paste
```
  {
      "Gemini": {
          "ApiKey": "api-key"
      }
  }

```

### Run the Application

1. Start the backend server

   ```bash
   cd cook50-backend
   dotnet run
   ```

2. Start the frontend development server

   ```bash
   cd cook50-frontend
   npm run dev
   ```

## Project Structure

```
cook50/
├── cook50-frontend/                 # Next.js frontend application
│   ├── src/                         # Next.js app directory
│   ├── components/                  # Reusable React components
│   └── public/                      # Static assets
├── cook50-backend/ cook50-backend   # .NET backend API
│   ├── Controllers/                 # API controllers
│   ├── Services/                    # Business logic
│   └── Program.cs                   # Entry point
└── README.md
```

## API Endpoints

- `POST /api/recipe/get-recipes` - Generate recipes based on provided ingredients
    - Request body: `{ "ingredients": ["tomato", "basil", "mozzarella"] }`
    - Response: `{ "recipes": [{ "title": "[Title of the recipe]", "ingredients": [...], "instructions": [...] }] }`

## Deployment

> [!IMPORTANT]
> When deploying the backend through Azure, due to global AI restrictions, I am unable to get the Gemini API working with a free tier plan.
> But the application should be working as intended in the local enviorement

### Frontend Deployment

The Next.js application has been deployed to Vercel with: `https://cook50-lac.vercel.app/`

### Backend Deployment

The .NET backend has been deployed to Azure with: `cook50-f8ffgqbgaadqcfc3.canadacentral-01.azurewebsites.net`

## Acknowledgments

- CS50
