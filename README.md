# RechargeFunctions

Sistema backend desarrollado con ASP.NET Core y Entity Framework Core para la gestión de recargas eléctricas desde una plataforma móvil.  
El proyecto permite administrar clientes, tarjetas y recargas mediante una API RESTful conectada a una aplicación .NET MAUI.

---

## Tecnologías utilizadas

- ASP.NET Core
- Entity Framework Core
- SQL Server
- .NET MAUI
- Swagger
- REST API
- Clean Architecture
- Repository Pattern
- Azure SQL

---

## Características

- Gestión de clientes
- Gestión de tarjetas
- Registro de recargas eléctricas
- Marcado de recargas como pagadas
- Soft Delete
- Validaciones
- API RESTful
- Integración mobile con .NET MAUI
- Persistencia con SQL Server
- Documentación Swagger

---

## Arquitectura del proyecto

El sistema fue desarrollado utilizando una arquitectura por capas para mantener separación de responsabilidades y escalabilidad.

```txt
RechargeFunctions.Domain
RechargeFunctions.Application
RechargeFunctions.Persistence
RechargeFunctions.Api
RechargeFunctions.Mobile
```

## Responsabilidades
- Domain → Entidades y reglas de negocio
- Application → Servicios y lógica de aplicación
- Persistence → Entity Framework y acceso a datos
- Api → Endpoints RESTful
- Mobile → Cliente .NET MAUI

## Funcionalidades principales
### Clientes
- Crear clientes
- Editar clientes
- Buscar clientes
- Soft Delete

### Tarjetas
- Registrar tarjetas
- Activar/desactivar tarjetas
- Relacionar tarjetas con recargas

### Recargas
- Registrar recargas
- Marcar recargas como pagadas
- Historial de recargas
- Relación entre clientes y tarjetas

### Aplicación Mobile
- Consumo de API REST
- Gestión de recargas desde dispositivo móvil
- Interfaz desarrollada con .NET MAUI

## Imágenes del proyecto

## Mobile App
![MobileApp](images/mobileApp.jpg)

## Endpoints funcionando
![Swagger](images/swagger.png)

## Integración Mobile

El sistema cuenta con una aplicación .NET MAUI que consume la API REST para gestionar las recargas eléctricas desde un dispositivo móvile.

## Autor

Desarrollado por Stwart Amarante.