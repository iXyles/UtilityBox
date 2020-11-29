# utilitybox-app
Open source project to ease the process of windows modifications

## Project setup
```
1) Install Node & .NET 5 SDK
2) npm install (in root folder)
```

### Developing
```
npm run serve - open localhost:8080 instead of 5000
```

### Build single exe
```
dotnet publish UtilityBox.App.csproj -r win7-x64 -c Release --self-contained false -o ./Publish
```
