# Modelo de datos de la base de la solución

## Estado del diseño

El alcance de esta iniciativa es de infraestructura y no incluye dominio ni persistencia. Por ello, no se definen entidades de negocio, modelos de dominio ni estructuras persistentes en esta etapa.

## Decisiones aplicables

- No se crearán entidades de negocio ni modelos de dominio.
- No se habilitará EF Core ni PostgreSQL en esta base.
- No se definirán contratos de API específicos, porque la primera fase solo prepara la solución.

## Estructura esperada para fases futuras

En próximas especificaciones, se introducirá el modelo de datos cuando exista un requisito funcional concreto. Para esta fase, la prioridad es mantener la solución neutra, compilable y preparada para evolución.
