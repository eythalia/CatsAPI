# 🎉 Cat API 🎉
---
## 📌 Description
### This API offers three endpoints. The first is a **POST** endpoint (`/cats`) that receives a collection of cats from an external API ([The Cat API](https://thecatapi.com/)), adds them to the database, and returns the cats that were successfully added. The second is a **GET** endpoint (`/cats/{id}`) that retrieves a single cat by its ID. The third is another **GET** endpoint (`/cats`) that retrieves a paginated list of cats from the database, allowing users to include an additional custom tag for each cat if desired.
---
---
## 📌 Features 
✅ **Clean Architecture**<br /> 
✅ **Utilizes the Repository pattern for data access and management.**<br />
✅ **Entity Framework Core (EF Core)**<br />
✅ **Result pattern for error-handling**<br />
✅ **Docker Compose file included in order to run the API**<br />
✅ **Unit Tests**<br />
✅ **Supports Swagger for API documentation**<br />
 

---

## 💻 Technologies Used  
- **.NET 8**  
- **Entity Framework Core**  
- **Docker**  
- **XUnit Testing Frameworks**  

---

## 📂 Clone and Run
1. Clone the repository to your local machine:

    ```
    git clone https://github.com/eythalia/CatsAPI.git
    ```

2. In root folder run:

    ```
    docker-compose build
    ```


1. Then run:

    ```
    docker-compose up
    ```

3. Run App through any browser 
    ```
    http://localhost:32770/swagger/index.html
    ```

4. Send requests by using Swagger

---

## 🌟 API Endpoints!  
|Verb| URL|
|---|---|
|POST |/api/cats/fetch|
|GET |/api/cats/{id}|
|GET |/api/cats|
