---
name: result-problem-details
description: Modela erros esperados por meio de Result<T> e os transforma de forma consistente em ProblemDetails.
---

# Skill: Result + ProblemDetails

## Objetivo

Evitar exceções como controle de fluxo para erros esperados e manter uma tradução HTTP consistente.

## Quando usar

Usar quando um caso de uso puder terminar com erros esperados como:

- NotFound
- Conflict
- Validation
- BusinessRule
- Forbidden

## Modelo conceitual

```text
Handler
    ↓
Result<T>
    │
    ├── Success → HTTP Response
    │
    └── Error → Error Mapping → ProblemDetails
```

## Regras

- os handlers retornam `Result<T>` quando apropriado
- os erros possuem código estável, tipo e mensagem clara
- a conversão HTTP é centralizada
- `ValidationProblemDetails` é usado para validação de entrada
- `ProblemDetails` é usado para outros erros HTTP esperados
- exceções são reservadas para falhas inesperadas

## Mapeamento recomendado

Definir uma estratégia central equivalente a:

```text
NotFound      → 404
Conflict      → 409
Validation    → 400
Forbidden     → 403
BusinessRule  → código definido pela arquitetura
```

O status exato para regras de negócio deve seguir a convenção aprovada do projeto.

## Localização

Erros específicos do slice permanecem no slice.

Erros genuinamente compartilhados do módulo podem ficar em:

```text
Shared/Errors/
```

## Proibições

- lançar exceção para `NotFound` esperado
- retornar strings arbitrárias como erro
- retornar objetos anônimos inconsistentes
- expor `Exception.Message` ou stack trace
- mapear o mesmo tipo de erro para status codes diferentes sem justificativa

## Checklist

- [ ] O erro é classificado corretamente como esperado ou inesperado.
- [ ] Usa-se `Result<T>` quando apropriado.
- [ ] O código de erro é estável.
- [ ] A tradução HTTP é centralizada.
- [ ] Utiliza-se `ProblemDetails`.
- [ ] Detalhes internos não são expostos.