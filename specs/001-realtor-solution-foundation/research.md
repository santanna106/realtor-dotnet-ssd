# Investigación de la base de la solución Realtor

## Decisión

Se define una base de solución mínima y alineada con la constitución del proyecto: solución principal en `app/Realtor.sln`, backend en `app/backend/src/RealtorApi` con ASP.NET Core Minimal APIs, frontend en `app/frontend/src/RealtorWeb` con Blazor Web App y Razor Components, y proyectos de prueba separados para backend y frontend.

## Racional

La especificación de la iniciativa exige preparar la infraestructura compartida sin introducir lógica de negocio ni entidades de dominio. Para cumplir con la arquitectura canónica, la base debe dejar la solución lista para futuras features, manteniendo la estructura mínima y coherente. La versión del SDK debe respetar el `global.json` existente y no alterarlo.

## Alternativas consideradas

1. Crear una solución con un único proyecto grande
   - Rechazada porque la constitución exige separar backend y frontend y mantener una estructura clara para futuras features.

2. Incluir lógica de negocio o entidades en esta etapa
   - Rechazada porque la iniciativa es de fundación y el alcance explícito prohíbe modelos de dominio, casos de uso y funcionalidades concretas.

3. Usar controllers en backend o MVC en frontend
   - Rechazada porque la constitución exige ASP.NET Core Minimal APIs y Blazor Web App con Razor Components.

4. Configurar una versión de .NET distinta a la recomendada por `global.json`
   - Rechazada porque la especificación exige respetar exactamente la versión ya definida en el repositorio.

## Requisitos confirmados

- La solución debe existir en `app/Realtor.sln`.
- El backend debe permanecer en `app/backend/src/RealtorApi`.
- El frontend debe estar en `app/frontend/src/RealtorWeb`.
- Los proyectos de pruebas deben vivir separados del código de producción.
- El `Program.cs` debe limitarse a servicios básicos, middleware inicial y endpoints mínimos.
- Ninguna entidad de dominio ni modelo de negocio debe crearse.
- La iniciativa solo prepara la base compartida, sin funcionalidad específica.
