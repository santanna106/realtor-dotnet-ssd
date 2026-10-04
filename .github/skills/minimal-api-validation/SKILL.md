---
name: minimal-api-validation
description: Implementa validação automática de Minimal APIs por meio de FluentValidation, assembly scanning e ValidationFilterFactory.
---

# Skill: Minimal API Validation

## Objetivo

Adicionar validadores sem modificar `Program.cs`, sem executar validação manual e sem adicionar filtros individuais por endpoint.

Arquitetura:

```text
FluentValidation + Assembly Scanning + ValidationFilterFactory + IServiceProviderIsService
```

## Quando usar

Usar quando uma tarefa:

- cria ou modifica um validador
- modifica o registro de validadores
- modifica `ValidationFilterFactory`
- altera o contrato de `ValidationProblemDetails`
- modifica a integração com `MapSliceEndpoints`

## Registro automático

Preferir o mecanismo oficial do FluentValidation:

```csharp
builder.Services.AddValidatorsFromAssembly(
    typeof(Program).Assembly,
    ServiceLifetime.Scoped);
```

Não criar um scanner próprio, salvo justificativa arquitetural aprovada.

## Localização

Os validadores específicos devem ficar próximos ao slice:

```text
Features/
  Properties/
    CreateProperty/
      CreateProperty.Validators.cs
```

## Factory

`ValidationFilterFactory.Create` deve:

1. inspecionar a assinatura do handler
2. identificar parâmetros candidatos
3. construir `IValidator<T>`
4. consultar a disponibilidade com `IServiceProviderIsService`
5. adicionar validação quando existir validador
6. retornar pass-through quando não existir

## Fluxo

```text
Request
    ↓
Factory
    ↓
Existe IValidator<T>?
   │
   ├── Sim → ValidateAsync
   │           ├── válido → Handler
   │           └── inválido → 400 + ValidationProblemDetails
   │
   └── Não → Handler
```

## Regras

- usar `ValidateAsync`
- propagar `CancellationToken`
- manter formato consistente de `ValidationProblemDetails`
- agrupar erros por propriedade quando apropriado
- manter regras de negócio fora dos validadores

## Proibições

- registrar validadores individualmente
- chamar `Validate` ou `ValidateAsync` a partir dos endpoints
- adicionar filtros individuais aos endpoints
- criar validadores vazios para forçar convenções
- retornar strings ou exceções como erro de validação
- duplicar `ValidationFilterFactory`

## Checklist

- [ ] A validação está descrita na spec.
- [ ] O validador reside no slice correspondente.
- [ ] Nenhum registro manual foi adicionado.
- [ ] O validador é descoberto automaticamente.
- [ ] O endpoint não valida manualmente.
- [ ] Sem validador, ocorre pass-through.
- [ ] O erro retorna HTTP 400.
- [ ] Utiliza-se `ValidationProblemDetails`.
- [ ] `CancellationToken` é propagado.
- [ ] O projeto compila.