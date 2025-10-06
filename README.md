#PRUEBATECNICA

-PRUEBATEC es una aplicación web para gestionar un almacén de productos.
-Permite realizar operaciones CRUD (Crear, Leer, Editar, Eliminar) sobre productos, con validaciones, paginación y seguridad básica.

#Tecnologías utilizadas#

-Backend: ASP.NET Core 8 MVC / Razor Pages
-ORM: Entity Framework Core
-Base de datos: SQL Server
-Frontend: Razor Views + HTML5 + CSS
-Documentación de API: Swagger
-Sweet Alert
-JS

#Estructura del proyecto
PRUEBATEC/
│
├─ Controllers/
│   ├─ ProductosController.cs          
│   └─ ProductosApiController.cs       
│
├─ Models/
│   └─ ProductoModel.cs               
│
├─ Data/
│   └─ AppDbContext.cs                 
│
├─ Repositories/
│   ├─ IProductoRepository.cs          
│   └─ ProductoRepository.cs           

├─ Views/
│   └─ Productos/
│       ├─ Index.cshtml
│       ├─ Create.cshtml
│       ├─ Edit.cshtml
│       └─ Delete.cshtml
│
└─ Program.cs   

Requisitos

-.NET 8 SDK
-SQL Server
-Visual Studio 2022

#Configuración

1. Clonar el repositorio:

git clone <URL-del-proyecto>
cd PRUEBATEC

2. Configurar la cadena de conexión en appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=TU_SERVIDOR;Database=PRUEBATEC;Trusted_Connection=True;TrustServerCertificate=True;"
}

3. Crear la base de datos y aplicar migraciones:

Update-Database


4. Ejecutar la aplicación desde Visual Studio (F5 o Ctrl+F5).

| Operación         | URL / Método                                            | Descripción                                                    |
| ----------------- | ------------------------------------------------------- | -------------------------------------------------------------- |
| Listar productos  | `/Productos` (MVC) o `/api/productos` (API)             | Muestra todos los productos con paginación y búsqueda opcional |
| Crear producto    | `/Productos/Create` o `POST /api/productos`             | Permite agregar un nuevo producto con validaciones             |
| Editar producto   | `/Productos/Edit/{id}` o `PUT /api/productos/{id}`      | Edita los datos de un producto existente                       |
| Eliminar producto | `/Productos/Delete/{id}` o `DELETE /api/productos/{id}` | Elimina un producto                                            |


| Campo             | Tipo     | Validaciones                 |
| ----------------- | -------- | ---------------------------- |
| IdProduct         | int      | Primary Key                  |
| NombreP           | string   | Required, Max 100 caracteres |
| SKU               | string   | Required, Regex válido       |
| Cantidad          | int      | Required, ≥ 0                |
| PrecioV           | decimal  | Required, ≥ 0                |
| PrecioC           | decimal  | Opcional, ≥ 0                |
| FechaCreacion     | DateTime | Set autom. al crear          |
| FechaModificacion | DateTime | Set autom. al editar         |

#Seguridad

-Uso de Entity Framework Core → evita SQL Injection
-Validaciones del lado servidor y cliente ([Required], [StringLength], [Range])
-Protegido contra CSRF con [ValidateAntiForgeryToken]
-Prevención básica de XSS mediante Razor 
-Manejo de errores 

#API Documentacion Swagger

Ruta: /swagger

Permite probar todos los endpoints de la API ProductosApiController

Endpoints disponibles:

GET    /api/productos
GET    /api/productos/{id}
POST   /api/productos
PUT    /api/productos/{id}
DELETE /api/productos/{id}


GET /api/productos soporta parámetros de query:
| Parámetro | Tipo   | Descripción                                  |
| --------- | ------ | -------------------------------------------- |
| busqueda  | string | Filtra por Nombre o SKU                      |
| pagina    | int    | Número de página (default 1)                 |
| registros | int    | Cantidad de registros por página (default 5) |

-------------------------------------------------------------------------------------------

                           C R E D E N C I A L E S

------------------------------------------------------------------------------------------

Usuario: admin
Pass: 1234
