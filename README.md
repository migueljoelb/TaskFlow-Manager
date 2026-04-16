# TaskFlow Manager

TaskFlow Manager es una aplicación de gestión de tareas desarrollada en .NET con patrón MVC.  
Permite a los usuarios registrar cuentas, crear tareas, asignarlas y darles seguimiento con estados y fechas de actualización.

## 🚀 Características
- Registro y autenticación de usuarios.
- Creación, edición y eliminación de tareas.
- Relación 1:N entre usuarios y tareas.
- Control de estados de las tareas (pendiente, en proceso, completada).
- Persistencia de datos con Entity Framework Core.

## 🛠️ Tecnologías utilizadas
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- C#

## 📂 Estructura del proyecto
- **Models/** → Entidades `User` y `Task`.
- **Data/** → `AppDbContext` y migraciones.
- **Controllers/** → Lógica de negocio y endpoints.
- **Views/** → Interfaz de usuario en Razor Pages.

## ⚙️ Configuración
1. Clonar el repositorio:
   ```bash
   git clone https://github.com/tuusuario/TaskFlowManager.git
