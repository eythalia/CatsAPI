# 🎉 Building RESTful APIs with ASP.NET Core 8 🎉
A simple API with three endpoints.A POST that receives a collection of cats from an external [API](https://thecatapi.com/) and returns those cats that were added on the database and two GET calls. One is to GET just one cat by its id and the other one is to retrieve many cats with paging support allowing also the users to add extra tag if they like.
---

## 📌 Features 
✅ **Clean Architecture** 
✅ **Utilizes the Repository pattern for data access and management.** 
✅ **Entity Framework Core (EF Core)**: Efficiently managing database operations with EF Core.  
✅ **Result pattern for error-handling**: Implementing robust error management and logging strategies to ensure smooth operations and traceability.  
✅ **Docker Compose file included in order to run the API**: Mastering user authentication and authorization for secure access control.  
✅ **Unit Tests**: Writing and executing unit tests with mocking techniques to ensure code correctness and reliability.  
✅ **Supports Swagger for API documentation**: Using a Layered Architecture approach for better separation of concerns and maintainable code.  
 

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

 🚀
# CatsAPI