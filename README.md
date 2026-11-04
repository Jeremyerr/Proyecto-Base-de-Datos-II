# Sistema Integrado NoSQL & API - Inventario

Proyecto Final para la materia Bases de Datos II (No Relacionales) - Universidad Evangélica de El Salvador.

## Requisitos Previos
* .NET SDK
* Instancia de MongoDB corriendo localmente (puerto 27017).

## Instrucciones para levantar el entorno
1. Clonar este repositorio.
2. La API está configurada para conectarse a una instancia local de MongoDB. Si es necesario, modificar la cadena de conexión en el archivo `appsettings.json`.
3. Ejecutar el script de base de datos proporcionado en Mongo Shell o MongoDB Compass para crear las colecciones con sus reglas `$jsonSchema` e insertar los datos de prueba.
4. Abrir la terminal en la raíz del proyecto y compilar/ejecutar la API con el comando: `dotnet run` (o iniciar desde Visual Studio).
