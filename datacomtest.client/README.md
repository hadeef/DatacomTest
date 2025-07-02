# DatacomTest  

This is a full stack solution using .NET 8 for the backend and Angular for the frontend.  

## Projects  

- **DatacomTest.Server**: ASP.NET Core Web API (.NET 8)  
- **datacomtest.client**: Angular application  

## Getting Started  

### Backend  

1. Open `DatacomTest.sln` in Visual Studio.  
2. Set `DatacomTest.Server` as the startup project.  
3. Run the project (F5 or Ctrl+F5).  

### Frontend  

1. Open a terminal in `datacomtest.client`.  
2. Run `npm install`.  
3. Run `ng serve` or `npm start`.  

### API Proxy  

The Angular app uses a proxy (`src/proxy.conf.js`) to forward `/api` requests to the backend.  

### Setting Port Number  

To set a custom port number for the Angular application, update the `application.service.ts` file:  

1. Open `src/app/services/application.service.ts`.  
2. Locate the configuration object or method where the base URL is defined.  
3. Modify the port number in the URL to your desired value.  
4. Save the file and restart the Angular application using `ng serve` or `npm start`.  

## Running Tests  

- Backend: Use Visual Studio Test Explorer or `dotnet test`.  

## License  

This project is for demonstration and technical assessment purposes.
