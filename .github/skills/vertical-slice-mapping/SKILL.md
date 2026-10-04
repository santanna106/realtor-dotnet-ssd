---
name: vertical-slice-mapping
description: Define mapeamentos explícitos por slice e seleciona a estratégia correta para objetos, coleções, consultas EF Core e streaming.
---

# Skill: Vertical Slice Mapping

## Objetivo

Manter mapeamentos explícitos, simples e localizados, selecionando a estratégia de acordo com o comportamento real do caso de uso.

## Quando usar

Usar quando uma tarefa precisa transformar:

- request em domínio ou entidade
- entidade em response
- entidade em evento
- coleções em memória
- consultas EF Core em DTOs
- fluxos diferidos ou streaming

## Decisão de estratégia

```text
Objeto individual
    ↓
Extension Block

Coleção já materializada
    ↓
Select + ToList

Consulta EF Core de leitura
    ↓
Select projection + ToListAsync

Streaming real
    ↓
IAsyncEnumerable<T> ou yield return
```

## Mapeamento individual

Preferir C# Extension Blocks quando melhorarem a clareza.

```csharp
internal static class CreatePropertyMappings
{
    extension(CreatePropertyRequest request)
    {
        internal Property ToEntity()
            => new(request.Name, request.Address);
    }
}
```

## Coleções materializadas

```csharp
var response = properties
    .Select(x => x.ToSummaryResponse())
    .ToList();
```

Usar quando a coleção já estiver em memória e o resultado completo for necessário imediatamente.

## Projeção EF Core

Preferir projeção direta para consultas de leitura:

```csharp
var response = await context.Properties
    .AsNoTracking()
    .Select(x => new PropertySummaryResponse(
        x.Id,
        x.Name,
        x.Status))
    .ToListAsync(cancellationToken);
```

Não carregar entidades completas se apenas uma projeção for necessária.

## Streaming

Usar `IAsyncEnumerable<T>` ou `yield return` apenas quando houver necessidade real de streaming, processamento incremental ou avaliação diferida.

## Localização

Mapeamento específico:

```text
<Slice>.Mapping.cs
```

Mapeamento genuinamente compartilhado:

```text
Shared/Mapping/
```

Não mover para Shared antecipadamente.

## Proibições

- lógica de negócio em mapeamento
- consultas ao banco de dados dentro de mapeamento
- validação dentro de mapeamento
- efeitos colaterais (side effects) dentro de mapeamento
- logging operacional dentro de mapeamento
- introduzir AutoMapper, Mapster ou Mapperly sem justificativa aprovada

## Checklist

- [ ] O mapeamento é explícito.
- [ ] A estratégia escolhida corresponde ao cenário.
- [ ] Não contém lógica de negócio.
- [ ] Está localizado no slice quando for específico.
- [ ] Está em Shared apenas se houver reutilização real.
- [ ] As consultas de leitura projetam diretamente quando apropriado.