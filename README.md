# 🤖 AzharGPT - AI Chatbot (.NET + React)

AzharGPT is a full-stack AI chatbot built with a .NET 9 Web API backend and a React + Vite frontend. It integrates Google Gemini to provide conversational AI responses in a modern chat UI.

## Live Demo
- Frontend (Chat UI): https://azhar-gpt-dot-net.vercel.app
- Backend (API): https://azhargptdotnet.onrender.com/api/chat

## Tech Stack
- Backend: .NET 9 Web API (C#)
- AI: Google Gemini
- Frontend: React (Vite)
- Deployment: Render (backend), Vercel (frontend)
- Docs: Swagger / OpenAPI

## Key Features
- Real-time AI chat using Google Gemini
- Clean, responsive React UI
- Docker-ready .NET backend
- CORS configured for frontend-backend communication

## Prerequisites
- .NET 9 SDK: https://dotnet.microsoft.com/
- Node.js (v18+): https://nodejs.org/
- Google Gemini API Key

## Local setup

1. Clone the repo
```bash
git clone https://github.com/mdazharulislamnk/AzharGPTDotNet.git
cd AzharGPTDotNet
```

2. Backend (GeminiChat.API)
```bash
cd GeminiChat.API
```
Set your Gemini API key (choose one):

- Option A — User Secrets (recommended for local dev)
```bash
dotnet user-secrets init
dotnet user-secrets set "Gemini:ApiKey" "YOUR_REAL_API_KEY_HERE"
```

- Option B — appsettings.json (not recommended for secrets)
Edit `appsettings.json` and set:
```json
"Gemini": {
  "ApiKey": "YOUR_REAL_API_KEY_HERE"
}
```

Run the backend:
```bash
dotnet run
```
Default URL: http://localhost:5015 (or as shown in the console). Swagger UI is available when running.

3. Frontend (geminichat-ui)
```bash
cd ../geminichat-ui
npm install
```
For local testing, ensure the frontend calls your local backend. Open `src/App.jsx` and update the fetch URL if needed:
```js
// Example (local)
const response = await fetch("http://localhost:5015/api/chat", { ... })
```
Run the frontend:
```bash
npm run dev
```
Default URL: http://localhost:5173

## Environment / Deployment Notes

- Render (Backend)
  - Use Docker or standard Render service.
  - Set environment variable key for Gemini API:
    - Key: `Gemini__ApiKey` (double underscore -> nested config)
    - Value: your Google Gemini API key

- Vercel (Frontend)
  - Point the frontend API calls to your live backend URL (e.g., the Render service URL).
  - Set any required environment variables in Vercel if you use them client-side (avoid exposing secret keys in the frontend).

## Project Structure
AzharGPTDotNet/
- GeminiChat.API/        # .NET Web API
  - Controllers/
  - Services/
  - Program.cs
  - Dockerfile
- geminichat-ui/         # React + Vite frontend
  - src/
    - App.jsx
    - App.css
  - package.json
- README.md

## Tips
- Keep your Gemini API key secret. Do not commit it to the repository.
- Use user-secrets for local dev and environment variables for deployed environments.
- Check Swagger at the backend URL for API docs and testing.

