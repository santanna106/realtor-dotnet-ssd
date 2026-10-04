---
name: vertical-slice-handlers
description: Implementa handlers concretos por caso de uso, autorregistro por meio de IHandler e acesso direto ao AppDbContext quando apropriado.
---

# Skill: Vertical Slice Handlers

## Objetivo

Manter um handler concreto por caso de uso, sem MediatR obrigatório, sem interfaces individuais desnecessárias e sem repositories triviais.

## Quando usar

Usar quando uma tarefa:

- adiciona um caso de uso no backend
- cria ou modifica um handler
- modifica o marker `IHandler`
- modifica o registro automático de handlers

## Convenção

```csharp
public interface IHandler;
```

Exemplo:

```csharp
internal sealed class CreatePropertyHandler(
    AppDbContext context,
    ILogger<CreatePropertyHandler> logger)
    : IHandler
{
    public async Task<Result<CreatePropertyResponse>> HandleAsync(
        CreatePropertyRequest request,
        CancellationToken cancellationToken)
    {
        // caso de uso
    }
}
```

## Responsabilidades

Um handler deve:

- representar um único caso de uso
- coordenar domínio, persistência e dependências externas
- usar I/O assíncrono
- propagar `CancellationToken`
- retornar resultado explícito
- manter lógica específica do caso de uso

## Registro automático

A infraestrutura deve:

1. escanear o assembly aprovado
2. encontrar classes concretas que implementem `IHandler`
3. registrá-las com o lifetime aprovado
4. evitar duplicados

Adicionar um handler não deve exigir modificar o `Program.cs`.

## Persistência

É permitido usar `AppDbContext` diretamente a partir do handler quando o caso de uso justificar.

Não criar repositories para encapsular chamadas simples ao EF Core.

## MediatR

Não introduzir MediatR unicamente para conectar endpoint e handler.

Só deve ser incorporado se uma spec ou decisão arquitetural aprovada demonstrar necessidade concreta.

## Proibições

- criar `ICreatePropertyHandler` sem necessidade real de múltiplas implementações
- registrar handlers individualmente
- manter listas manuais de handlers
- criar scanners por feature
- criar generic repositories
- criar wrappers vazios de command/query
- bloquear I/O assíncrono

## Checklist

- [ ] O handler representa um caso de uso.
- [ ] Implementa `IHandler`.
- [ ] É registrado automaticamente.
- [ ] Propaga `CancellationToken`.
- [ ] Não há interface individual desnecessária.
- [ ] Não há repository trivial.
- [ ] O logging é estruturado quando aplicável.
- [ ] O projeto compila.