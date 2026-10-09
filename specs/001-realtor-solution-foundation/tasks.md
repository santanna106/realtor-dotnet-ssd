# Tasks: Base de la solución Realtor

**Input**: Documentos de diseño de `/specs/001-realtor-solution-foundation/`

**Prerequisitos**: `spec.md` y `plan.md` obligatorios; `research.md` y `quickstart.md` como validación adicional.

## Fase 1: Setup (Infraestructura compartida)

**Objetivo**: Crear la estructura base y los proyectos iniciales sin funcionalidad de negocio.

- [X] T001 Crear la estructura de carpetas en `app/`, `app/backend/`, `app/backend/src/`, `app/backend/tests/`, `app/frontend/`, `app/frontend/src/` y `app/frontend/test/`.
- [X] T002 [P] Crear el archivo `app/Realtor.sln` y registrar los proyectos backend y frontend.
- [X] T003 [P] Inicializar el proyecto `app/backend/src/RealtorApi` como ASP.NET Core con Minimal APIs.
- [X] T004 [P] Inicializar el proyecto de pruebas `app/backend/tests/RealtorApiTests` separado del código de producción.
- [X] T005 [P] Inicializar el proyecto `app/frontend/src/RealtorWeb` como Blazor Web App con Razor Components.
- [X] T006 [P] Inicializar el proyecto de pruebas `app/frontend/test/RealtorWeb`.
- [X] T007 [P] Validar que la versión del SDK usada coincida con la indicada por `global.json` y que el archivo no sea modificado.

**Punto de control**: La estructura de la solución queda lista para que el backend y el frontend se desarrollen sobre una base común y neutral.

---

## Fase 2: Foundational (Bloqueantes para todas las historias)

**Objetivo**: Dejar la infraestructura base validada antes de implementar cualquier historia de usuario funcional.

- [X] T008 Verificar que no haya lógica de dominio, controllers, entidades de negocio ni funcionalidades concretas en `app/backend/src/RealtorApi`.
- [X] T009 Verificar que no haya componentes funcionales ni lógica de negocio en `app/frontend/src/RealtorWeb`.
- [X] T010 Revisar que `Program.cs` de backend contenga solamente servicios básicos, middleware inicial y endpoints mínimos.
- [X] T011 Revisar que la solución principal en `app/Realtor.sln` incluya los cuatro proyectos esperados y mantenga rutas consistentes.
- [X] T012 Validar la compilación base de `app/Realtor.sln` para confirmar que la infraestructura inicial queda estable.

**Punto de control**: La base queda lista para que cada historia de usuario pueda validar su parte sin depender de lógica de negocio no autorizada.

---

## Fase 3: User Story 1 - Preparar la base compartida de la solución (Prioridad: P1)

**Objetivo**: Dejar la solución principal organizada y consistente para backend y frontend.

**Prueba independiente**: La estructura puede validarse abriendo la solución y confirmando que los proyectos están en las rutas esperadas.

### Implementación para User Story 1

- [X] T013 [US1] Crear la referencia de solución en `app/Realtor.sln` y mantener la estructura principal del repositorio coherente.
- [X] T014 [US1] Confirmar que `app/backend/` y `app/frontend/` queden separados y organizados por capas de proyecto.
- [X] T015 [US1] Validar que la base no incluya entidades de dominio ni casos de uso en `app/backend/src/RealtorApi` ni en `app/frontend/src/RealtorWeb`.
- [X] T016 [US1] Ejecutar la validación estructural de la solución para confirmar rutas, nombres y ubicaciones esperadas.

**Punto de control**: La solución base queda lista para trabajar en futuras features sin introducir dominio ni comportamiento funcional.

---

## Fase 4: User Story 2 - Definir el backend con APIs mínimas y estructura de pruebas (Prioridad: P1)

**Objetivo**: Preparar el backend mínimo conforme a ASP.NET Core Minimal APIs y mantener pruebas separadas del código de producción.

**Prueba independiente**: Puede validarse inspeccionando `app/backend/src/RealtorApi` y comprobando que `Program.cs` solo configura servicios, middleware y endpoints iniciales, sin controllers.

### Implementación para User Story 2

- [X] T017 [P] [US2] Crear `app/backend/src/RealtorApi/Program.cs` con configuración mínima de servicios, middleware inicial y mapeo de endpoints básicos.
- [X] T018 [US2] Confirmar que `app/backend/src/RealtorApi` no contiene `Controllers/` ni lógica de negocio funcional.
- [X] T019 [P] [US2] Crear una prueba de humo mínima en `app/backend/tests/RealtorApiTests` para comprobar que el proyecto compila y el backend queda inicializable.
- [X] T020 [US2] Ejecutar `dotnet test app/backend/tests/RealtorApiTests` para validar la infraestructura del backend.

**Punto de control**: El backend queda preparado como base estándar del repositorio, sin funcionalidad de dominio ni controllers.

---

## Fase 5: User Story 3 - Definir el frontend con Blazor y estructura de pruebas (Prioridad: P1)

**Objetivo**: Preparar el frontend mínimo con Blazor Web App y Razor Components, sin flujo funcional aún definido.

**Prueba independiente**: Puede validarse revisando que `app/frontend/src/RealtorWeb` quede configurado como Blazor Web App y que exista el proyecto de pruebas en `app/frontend/test/RealtorWeb`.

### Implementación para User Story 3

- [X] T021 [P] [US3] Crear la aplicación base de frontend en `app/frontend/src/RealtorWeb` con Blazor Web App y Razor Components.
- [X] T022 [US3] Confirmar que `app/frontend/src/RealtorWeb` no incluye lógica de negocio ni componentes funcionales aún no especificados.
- [X] T023 [P] [US3] Crear una prueba de humo mínima en `app/frontend/test/RealtorWeb` para verificar que la app de frontend compila y arranca en su estructura base.
- [X] T024 [US3] Ejecutar `dotnet test app/frontend/test/RealtorWeb` para validar la infraestructura del frontend.

**Punto de control**: El frontend queda listo para futuras interfaces sin introducir casos de uso ni lógica de negocio prematura.

---

## Fase 6: Polish & Cross-Cutting Concerns

**Objetivo**: Revisar la base del proyecto y cerrar la validación final antes de iniciar cualquier feature funcional.

- [X] T025 [P] Revisar `specs/001-realtor-solution-foundation/quickstart.md` para asegurar que los pasos de validación reflejan la estructura base correcta.
- [X] T026 [P] Confirmar que la solución completa compila sin introducir lógica de dominio ni funcionalidad no autorizada.
- [X] T027 Ejecutar `dotnet build app/Realtor.sln` como validación final de la base compartida.
- [X] T028 Revisar la solución para asegurar que no haya entidades de dominio, controllers, modelos de negocio ni casos de uso operativos en esta etapa.

**Punto de control**: La base de la solución queda validada y lista como punto de partida seguro para futuras iniciativas.

---

## Dependencias y orden de ejecución

### Dependencias de fase

- **Fase 1**: No depende de ninguna otra fase; puede iniciarse de inmediato.
- **Fase 2**: Depende de la finalización del setup; bloquea la implementación de historias de usuario.
- **Fase 3, 4 y 5**: Pueden ejecutarse en paralelo una vez completada la Fase 2, siempre que se mantenga el alcance de infraestructura base.
- **Fase 6**: Depende de la culminación de todas las validaciones de infraestructura.

### Dependencias por historia

- **User Story 1**: Requiere que la estructura base y la solución principal estén creadas.
- **User Story 2**: Depende de la base compartida y no puede introducir controllers ni dominio.
- **User Story 3**: Depende de la base compartida y no puede introducir lógica funcional prematura.

### Paralelización recomendada

- Los proyectos de backend y frontend se pueden inicializar en paralelo en la Fase 1.
- Las pruebas de humo del backend y del frontend se pueden preparar en paralelo en las Fases 4 y 5.
- La revisión final de quickstart y la validación compilatoria pueden ejecutarse en paralelo con la revisión estructural final.

---

## Ejemplo de ejecución paralela

```bash
# Inicialización en paralelo de backend y frontend
Task: "Inicializar app/backend/src/RealtorApi como ASP.NET Core Minimal APIs"
Task: "Inicializar app/frontend/src/RealtorWeb como Blazor Web App"

# Validación paralela de pruebas de humo
Task: "Crear prueba de humo para app/backend/tests/RealtorApiTests"
Task: "Crear prueba de humo para app/frontend/test/RealtorWeb"
```

---

## Estrategia de implementación

### MVP primero

1. Completar la Fase 1: Setup.
2. Completar la Fase 2: Foundational.
3. Completar la Fase 3: User Story 1.
4. Completar la Fase 4: User Story 2.
5. Completar la Fase 5: User Story 3.
6. Ejecutar la Fase 6 de validación final.

### Criterio de salida

La base de la solución queda validada en `app/Realtor.sln`, con backend y frontend separados, pruebas aisladas y sin lógica de negocio ni dominio introducidos en esta iniciativa.