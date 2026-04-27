# StoreMaster
Proyecto de tienda en linea con .NET
# StoreMaster
Sistema de gestión de tienda y almacén desarrollado en ASP.NET Core 10 MVC.

## Descripción
StoreMaster es una aplicación web completa para la gestión de una tienda o almacén. 
Incluye control de inventario, punto de venta, facturación electrónica (CFDI 4.0), 
gestión de clientes y reportes estadísticos.

## Tecnologías
- **Backend:** ASP.NET Core 10 MVC, C#
- **ORM:** Entity Framework Core 10 (Code-First)
- **Base de datos:** SQL Server
- **Autenticación:** ASP.NET Core Identity
- **Frontend:** Bootstrap 5, jQuery, DataTables, Chart.js
- **Otros:** AutoMapper, QuestPDF

## 🏗️ Arquitectura
El proyecto implementa una arquitectura en capas:

StoreMaster.Web            → Controladores, Vistas, ViewModels
StoreMaster.Core           → Entidades, Interfaces, Servicios
StoreMaster.Infrastructure → Repositorios, DbContext, Migraciones

## ✨ Funcionalidades
- ✅ Autenticación y roles (Admin, Vendedor)
- ✅ Gestión de productos con control de stock
- ✅ Categorías y proveedores
- ✅ Punto de venta con carrito interactivo (AJAX)
- ✅ Historial de ventas con cancelación
- ✅ Facturación electrónica estructura CFDI 4.0
- ✅ Gestión de clientes con datos fiscales SAT
- ✅ Dashboard con KPIs en tiempo real
- ✅ Reportes: ventas por período, inventario valorizado
- ✅ Búsqueda y paginación con DataTables
- ✅ Subida de imágenes de productos

## ⚙️ Configuración local

### Requisitos
- .NET 10 SDK
- SQL Server
- Visual Studio 2022

### Instalación
1. Clona el repositorio
```bash
   git clone https://github.com/FrankenMAG/StoreMaster.git
```

2. Copia `appsettings.Example.json` como `appsettings.json` y configura:
```json
   {
     "AdminPassword": "TU_PASSWORD",
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=StoreMasterDb;Trusted_Connection=True;TrustServerCertificate=True"
     }
   }
```

3. Aplica las migraciones
```bash
   dotnet ef database update --project StoreMaster.Infrastructure --startup-project StoreMaster.Web
```

4. Ejecuta el proyecto
```bash
   dotnet run --project StoreMaster.Web
```

5. Accede con las credenciales por defecto:
   - **Email:** admin@storemaster.com
   - **Password:** el que configuraste en AdminPassword

## 📐 Patrones y buenas prácticas
- Patrón Repositorio + Servicio
- Inyección de dependencias
- Soft Delete con filtros globales
- ViewModels separados de entidades
- Validación con Data Annotations
- Protección CSRF con AntiForgeryToken

## 👨‍💻 Autor
**Martin Acosta Galvez**  
[LinkedIn](https://www.linkedin.com/in/isc-martin-acosta-galvez/) · [GitHub](https://github.com/FrankenMAG)