# Cook50

Cook50 is an AI-powered recipe generator that helps you create delicious meals based on the ingredients you have on hand. By leveraging the Gemini AI API, this application suggests creative recipes tailored to your available ingredients, making meal planning easier and reducing food waste.

## Features

- **Ingredient-Based Recipe Generation**: Enter the ingredients you have, and get AI-generated recipe suggestions
- **Detailed Recipe Information**: Each recipe includes a comprehensive list of ingredients and step-by-step instructions
- **PDF Download**: Save your favorite recipes as PDF files for offline use
- **Responsive Design**: Works seamlessly across desktop and mobile devices

## Tech Stack

### Frontend (Next.js)

Cook50 uses Next.js 15, a React framework that provides several key benefits:

- **App Router**: The project leverages Next.js's App Router architecture for improved routing with layouts, loading states, and error boundaries
- **Server Components**: Utilizes React Server Components to reduce client-side JavaScript and improve performance
- **Client Components**: Interactive elements like the ingredient input form and recipe download functionality use Client Components
- **Static/Dynamic Rendering**: Pages are optimized for performance with appropriate rendering strategies

#### Key Frontend Components

- **Search Page**: Allows users to input ingredients with restrictions to key words so the user cannot undo the AI prompt instructions
- **Recipe Display**: Renders AI-generated recipes with dynamic content
- **PDF Generation**: Uses html2canvas and jsPDF libraries to convert recipe components to downloadable PDFs

#### Styling

- **Tailwind CSS**: Used for responsive design with custom theme configuration
- **Custom color scheme**: Implemented with light and dark green themes for a cooking/fresh food aesthetic

### Backend (.NET)

The backend is built with .NET 9, providing API services for the application:

- **ASP.NET Core Web API**: Powers the API endpoints

#### API Structure

- **Controllers**:

    - `RecipeController`: Handles recipe generation requests

- **Services**:
    - `GeminiService`: Communicates with the Gemini API
    - `ParseRecipes`: Processes and normalizes the generated recipe
    - `GetRecipesFromAI`: Getting the recipe from the Gemini API
    - `CreateErrorRecipe`: Incase of an error were to occure when fetching the data from the AI

#### API Integration

- **Gemini API Integration**: For generating recipes from the ingredients the user inputs

## Architecture

The project follows a client-server architecture with separation of:

1. **Frontend (Next.js)**:

    - Handles user interactions and display
    - Communicates with backend via API calls
    - Manages client-side state

2. **Backend (.NET)**:

    - Processes requests from the frontend
    - Communicates with the Gemini API
    - Formats and normalizes data
    - Handles business logic and data persistence
    - Sends the normalized data to the frontend

3. **External Services**:
    - Google's Gemini API for AI recipe generation

### Data Flow

1. User inputs ingredients in the Next.js frontend
2. Frontend sends ingredients to the .NET backend API
3. Backend formats data and queries the Gemini API
4. Gemini API returns recipe suggestions
5. Backend processes and standardizes the response
6. Frontend displays the recipes to the user
7. User can download recipes as PDFs directly from the browser

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
