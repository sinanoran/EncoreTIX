
 # EncoreTIX

EncoreTIX is an ASP.NET Core MVC web application that integrates with the Ticketmaster Discovery API to allow users to search for attractions (e.g., bands, artists, sports teams), view details, and explore related events.

---

## 🛠 Tech Stack
- ASP.NET Core 9.0 MVC
- Razor Views
- Ticketmaster Discovery API v2
- JSON deserialization with `System.Text.Json`
- Dependency Injection & SOLID principles
- xUnit & Moq for unit testing

---

## 🔧 Setup Instructions

### 1. Prerequisites
- [.NET SDK 9.0](https://dotnet.microsoft.com)
- Visual Studio 2022 or newer

### 2. Clone the Repository
```
git clone https://github.com/sinanoran/EncoreTIX.git
cd EncoreTIX
```

### 3. Configure Ticketmaster API
Update `appsettings.json` or use secrets to store your API key:

```json
{
  "TicketMaster": {
    "Key": "YOUR_TICKETMASTER_API_KEY"
  }
}
```

Or via Secret Manager:

```bash
dotnet user-secrets set "TicketMaster:Key" "YOUR_API_KEY"
```

### 4. Run the App
```
dotnet run
```
Open [https://localhost:5001](https://localhost:5001)

---

## ✅ Features
- 🔍 Search for Attractions (default = "Phish")
- 🎭 View detailed Attraction info with external links
- 🎟 List of upcoming events with date, venue, and image
- 🎬 Splash screen with redirect
- 🔄 Responsive layout with styled components
- 🧪 Unit tested controller and service logic

---

## 🧪 Running Tests
```
dotnet test
```
Tests are located in `EncoreTIX.Tests/` and cover:
- Controller responses
- JSON deserialization
- HttpClient mocking

---

## 📦 Project Structure
```
EncoreTIX/
├── Controllers/
├── Models/
├── Services/
├── Views/
│   ├── Home/
│   └── Shared/
├── wwwroot/css/site.css
├── Program.cs
EncoreTIX.Tests/
```

---
