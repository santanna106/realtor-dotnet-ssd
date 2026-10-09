# Guía rápida de la base solution Realtor

## Objetivo

Validar la estructura inicial de la solución sin introducir lógica de negocio ni casos de uso funcionales.

## Requisitos

- SDK .NET según la versión indicada por `global.json`.
- Visual Studio 2022 o Visual Studio Code con C# Dev Kit.
- Acceso a la terminal del repositorio.

## Verificaciones esperadas

1. Abrir la solución en `app/Realtor.sln`.
2. Confirmar que existen los proyectos de backend y frontend en las rutas previstas.
3. Ejecutar compilación general:

```bash
dotnet build app/Realtor.sln
```

4. Ejecutar pruebas del backend:

```bash
dotnet test app/backend/tests/RealtorApiTests
```

5. Ejecutar pruebas del frontend:

```bash
dotnet test app/frontend/test/RealtorWeb
```

6. Validar que no se hayan incorporado entidades de dominio, casos de uso ni lógica de negocio en esta etapa.

## Criterios de salida

- La solución compila y se abre correctamente.
- La estructura de carpetas coincide con la especificación.
- Se respetan los principios de arquitectura mínima y de base compartida.
- No se introduce funcionalidad que no pertenezca a la fundación del proyecto.
