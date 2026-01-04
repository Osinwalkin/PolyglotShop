# PolyglotShop - Polyglot Persistence med .NET 8

Dette projekt er en "Proof of Concept", der demonstrerer en Polyglot Persistence arkitektur. Projektet er udviklet som en del af kurset Databases for Developers for at vise, hvordan man kan kombinere MS SQL Server (til transaktionel dataintegritet) og MongoDB (til fleksibel datastruktur) i én samlet mikroservice-inspireret løsning.

Løsningen er bygget på Clean Architecture principper.

## Teknologistack

*   **.NET 8** (Web API)
*   **Docker og Docker Compose** (Infrastruktur)
*   **Entity Framework Core** (SQL ORM)
*   **MongoDB .NET Driver** (NoSQL adgang)
*   **Swagger/OpenAPI** (Dokumentation)

## Forudsætninger

For at køre projektet skal du have følgende installeret:

1.  **Docker Desktop** (skal køre)
2.  **.NET 8 SDK**
3.  **Git**

## Installation og Opsætning

### 1. Klon projektet
```
git clone https://github.com/Osinwalkin/PolyglotShop.git
cd PolyglotShop
```

### 2. Start Infrastrukturen (Databaser)
Projektet bruger Docker til at køre SQL Server og MongoDB. Start containerne fra root af mappen:

```
docker-compose up -d
```
> **Bemærk:** Vent ca. 15-20 sekunder efter kommandoen er kørt, så SQL Serveren kan nå at starte helt op, før du kører applikationen.

### 3. Opret SQL Databasen (Migration)
Applikationen bruger Entity Framework Core til at styre SQL-skemaet. Kør følgende kommandoer for at gendanne værktøjer og oprette databasen:

```
# Gendan lokale tools (dotnet-ef)
dotnet tool restore

# Opret databasen og tabellerne
dotnet ef database update -p src/PolyglotShop.Infrastructure -s src/PolyglotShop.Api
```

### 4. Kør API'en
Start applikationen:

```
dotnet run --project src/PolyglotShop.Api
```

Når applikationen kører, vil den være tilgængelig på:
*   **Swagger UI:** http://localhost:5236/swagger

---

## Sådan tester du systemet

Du kan teste systemet direkte gennem Swagger UI.

### Test 1: MongoDB (Fleksibelt Skema)
Opret et produkt med dynamiske attributter (f.eks. "ram", "gpu", "waterproof"), som **ikke** er defineret i C#-koden. Dette beviser NoSQL-fleksibiliteten.

**POST** `/api/Products`
```
{
  "name": "Super Gaming Laptop",
  "price": 2500,
  "details": {
    "brand": "Alienware",
    "cpu": "Intel i9",
    "ram": "64GB",
    "waterproof": false,
    "warrantyYears": 2
  }
}
```
Forventet response: 201

### Test 2: SQL Server (ACID Transaktion)
Opret en ordre. Dette gemmer data i SQL Serveren under en transaktion. Hvis noget fejler undervejs, rulles hele handlingen tilbage.

**POST** `/api/Orders`
```
{
  "orderDate": "2026-01-04T12:00:00Z",
  "totalAmount": 2500,
  "userId": 0,
  "user": {
    "name": "Censor Test",
    "email": "testmail@easv.dk"
  }
}
```
Forventet response: 200 OK

---

## Projektstruktur (Clean Architecture)

*   **src/PolyglotShop.Core**: Domænelag (Entities, Interfaces). Ingen afhængigheder.
*   **src/PolyglotShop.Infrastructure**: Datalag (Repositories, DB Contexts, EF Core, Mongo Driver).
*   **src/PolyglotShop.Api**: Præsentationslag (Controllers, DI Setup).

---

## Fejlfinding

*   **Connection refused (MongoDB):**
    *   Tjek at Docker kører (`docker ps`) og at port 27017 ikke er i brug af en anden service.