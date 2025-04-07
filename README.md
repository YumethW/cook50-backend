# Cook50

Cook50 is an AI-powered recipe generator that helps you create delicious meals based on the ingredients you have on hand. By leveraging the Gemini AI API, this application suggests creative recipes tailored to your available ingredients, making meal planning easier and reducing food waste.

## Tech Stack

### Frontend (Next.js)

- **Frontend**: Next.js with TypeScript
- **Styling**: Tailwind CSS
- **Backend**: ASP.NET
- **AI Technologies**:
  - Gemini API (Recipe Generator)

## Features

- **Ingredient-Based Recipe Generation**: Enter the ingredients you have, and get AI-generated recipe suggestions
- **Detailed Recipe Information**: Each recipe includes a comprehensive list of ingredients and step-by-step instructions
- **PDF Download**: Save your favorite recipes as PDF files for offline use
- **Responsive Design**: Works seamlessly across desktop and mobile devices

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

3. Configure environment variables

- Create dotnet user secrets, in it paste
  `{
      "Gemini": {
          "ApiKey": "api-key"
      }
  }

### Running the Application

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

3. Open your browser and navigate to `http://localhost:3000`

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

The .NET backend has been deployed to Azure with: `https://cook50-f8ffgqbgaadqcfc3.eastasia-01.azurewebsites.net/`

## Acknowledgments

- CS50
- Google Gemini API for powering the recipe generation
- Next.js and .NET communities for excellent documentation and resources
