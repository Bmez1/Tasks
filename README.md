# Plantilla de Arquitectura Limpia - Gestión de Tareas y Usuarios

## Descripción del Proyecto

Esta API es una solución construida con los principios de la Arquitectura Limpia en .NET, diseñada para la gestión eficiente de tareas, categorías y usuarios. Proporciona una base para el desarrollo de aplicaciones empresariales, destacando por su modularidad, testabilidad y mantenibilidad.

### Características Principales:

*   **Gestión Completa de Tareas**: Permite crear, actualizar, eliminar, completar y organizar tareas de forma efectiva.
*   **Categorización Flexible**: Soporte para la creación y asignación de categorías a las tareas, facilitando su organización.
*   **Gestión Segura de Usuarios**: Funcionalidades para el registro y autenticación de usuarios.
*   **Autenticación JWT**: Implementación de autenticación basada en JSON Web Tokens para asegurar los endpoints de la API.
*   **Hashing de Contraseñas Robusto**: Las contraseñas de los usuarios se almacenan de forma segura utilizando un algoritmo de hashing fuerte (`Rfc2898DeriveBytes.Pbkdf2`).
*   **Manejo de Errores**: Todos los mensajes de error de la API han sido localizados al español para una mejor experiencia del usuario.
*   **Configuración CORS**: Políticas de Cross-Origin Resource Sharing configuradas para permitir la integración con aplicaciones cliente.

## ¿Qué incluye esta plantilla?

-   Proyecto SharedKernel con abstracciones comunes de Domain-Driven Design.
-   Capa de Dominio con entidades de ejemplo (Tareas, Categorías, Usuarios).
-   Capa de Aplicación con abstracciones para:
    -   CQRS (Command Query Responsibility Segregation)
    -   Casos de uso de ejemplo (Crear Tarea, Completar Tarea, Registrar Usuario, Login)
    -   Aspectos transversales (logging, validación)
-   Capa de Infraestructura con:
    -   Autenticación
    -   Autorización basada en permisos
    -   Entity Framework Core, PostgreSQL para la persistencia de datos.
    -   Serilog para el logging estructurado.
-   Seq para la búsqueda y análisis de logs estructurados
    -   Seq está disponible en `http://localhost:8081` por defecto.
-   HealthCheck para ver el estado de salud de la DB como servicio externo
    -   health está disponible en `http://localhost:5000/health` por defecto.
-   Proyectos de testing (pruebas unitarias y de arquitectura).

## Primeros Pasos

Para levantar el proyecto y sus dependencias (base de datos PostgreSQL y Seq), puedes usar Docker Compose. Asegúrate de tener Docker instalado y ejecutándose en tu sistema.

1.  **Clonar el repositorio:**
    ```bash
    git clone https://github.com/Bmez1/Tasks.git
    cd Tasks
    ```
2.  **Iniciar los servicios con Docker Compose:**
    ```bash
    docker-compose up -d
    ```
    Esto iniciará la base de datos PostgreSQL y el servidor Seq. La API se ejecutará en tu máquina local.
3.  **Ejecutar la aplicación .NET:**
    Desde la raíz del proyecto `Tasks`:
    ```bash
    dotnet run --project src/Web.Api/Web.Api.csproj
    ```
    La API estará disponible en `http://localhost:5000` (o el puerto configurado).

## Endpoints de la API (Ejemplos)

La API utiliza Minimal APIs y define sus endpoints de manera modular. Algunos ejemplos incluyen:

*   `POST /api/v1/users/register`: Para registrar un nuevo usuario.
*   `POST /api/v1/users/login`: Para autenticar un usuario y obtener un JWT.
*   `GET /api/v1/tasks`: Para obtener todas las tareas.
*   `POST /api/v1/tasks`: Para crear una nueva tarea.
*   `DELETE /api/v1/tasks`: Para eliminar una tarea.
*   `GET /api/v1/tasks/filtered`: Para obtener tareas filtradas.
*   `PUT /api/v1/tasks/{id}/complete`: Para marcar una tarea como completada.
*   `POST /api/v1/categories`: Para crear una nueva categoría.

---

### Daniel Enrique Barros Agamez