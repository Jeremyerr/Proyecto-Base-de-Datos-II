# Sistema Integrado NoSQL & API - Inventario

Proyecto final de la materia **Bases de Datos II (No Relacionales)** de la **Universidad Evangélica de El Salvador**.

## Descripción

API REST para gestionar un inventario de productos, consultar su información y generar reportes. Está desarrollada con ASP.NET Core y utiliza MongoDB como base de datos NoSQL.

La base de datos `InventarioDB` utiliza tres colecciones:

- **productos:** nombre, descripción, precio, stock, categoría, etiquetas y proveedor embebido.
- **usuarios:** correo electrónico, contraseña y rol (`Creador` o `Visualizador`).
- **movimientos:** entradas y salidas de inventario asociadas a productos mediante `ProductoId`.

## Requisitos previos

- .NET SDK compatible con la versión indicada en el archivo `InventarioAPI.csproj`.
- MongoDB disponible localmente en el puerto `27017`.
- MongoDB Shell (`mongosh`) para ejecutar los scripts.
- Git para clonar el repositorio.
- Postman para probar los endpoints (opcional).

## Instalación y ejecución

1. Clonar el repositorio:

   `git clone URL_DEL_REPOSITORIO`

2. Abrir una terminal en la raíz del repositorio y comprobar que MongoDB esté ejecutándose en `mongodb://localhost:27017`.

3. Crear o actualizar las colecciones y sus reglas de validación `$jsonSchema`:

   `mongosh "mongodb://localhost:27017" --file MongoDB/validaciones.js`

4. Cargar los datos ficticios de prueba:

   `mongosh "mongodb://localhost:27017" --file MongoDB/seed.js`

   El script carga **60 usuarios de prueba, 60 productos y 80 movimientos** en una base de pruebas vacía.

   **Advertencia:** el script elimina todos los documentos existentes en `productos` y `movimientos`, y reemplaza los usuarios de prueba con correos `usuarioN@api.com`. No debe ejecutarse en una base con datos que se necesite conservar. Los usuarios ficticios del script tienen contraseñas de ejemplo en texto plano y no deben usarse en producción.

5. Revisar la configuración de conexión a MongoDB en `appsettings.json`. Para la instancia local, la dirección de MongoDB es `mongodb://localhost:27017` y la base de datos es `InventarioDB`. Mantener cualquier credencial real fuera del repositorio.

6. Abrir una terminal en la carpeta que contiene `InventarioAPI.csproj` y ejecutar:

   `dotnet restore`

   `dotnet run --launch-profile https`

   También se puede iniciar el proyecto desde Visual Studio utilizando el perfil `https`.

7. Consultar la dirección mostrada por la consola al arrancar. Si se utiliza la configuración original con el puerto `7059` y Swagger está habilitado, abrir:

   `https://localhost:7059/swagger`

## Pruebas de API

Importar en Postman el archivo `Postman/INVENTARIO_PRUEBAS_ACTUALIZADA.postman_collection.json` cuando esté agregado al repositorio.

Algunas rutas utilizadas en las pruebas son:

| Método | Ruta | Descripción |
| --- | --- | --- |
| POST | `/api/auth/login` | Iniciar sesión. |
| GET | `/api/productos/0/5` | Consultar productos paginados. |
| GET | `/api/productos/reporte-inversion` | Consultar el reporte de inversión. |

Ingresar las credenciales de prueba correspondientes y, cuando sea necesario, utilizar el token obtenido en el inicio de sesión. El puerto y las rutas definitivas deben coincidir con la configuración del proyecto.

## Archivos de base de datos

- `MongoDB/validaciones.js`: configura las reglas `$jsonSchema` de las tres colecciones con validación estricta y acción de error.
- `MongoDB/seed.js`: inserta los registros ficticios para pruebas.

## Entregables

- Código fuente del backend en GitHub.
- Scripts para inicializar MongoDB y cargar los datos de prueba.
- Archivo de colección de Postman.
- Documento PDF con la justificación del modelo NoSQL, las decisiones de diseño y las pruebas realizadas.

## Nota sobre Docker

La ejecución indicada en este README es **local**. Si se agrega un archivo `docker-compose.yml`, documentar también el comando `docker compose up -d` y los servicios definidos. No se requiere ejecutar Docker para seguir los pasos locales anteriores.
