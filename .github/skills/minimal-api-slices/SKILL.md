---
name: minimal-api-slices
description: Implementa e mantém endpoints Minimal API autodescobriveis por meio de ISlice, reflection e mapeamento centralizado.
---

# Skill: Minimal API Slices

## Objetivo

Permitir que adicionar um endpoint não exija modificar `Program.cs`, registries manuais nem listas centralizadas.

Arquitetura:

```text
Minimal API + ISlice + Reflection + DI + MapSliceEndpoints
```

## Quando usar

Usar quando uma tarefa:

- adiciona ou modifica um endpoint
- cria ou modifica `ISlice`
- modifica `RegisterSlices`
- modifica `MapSliceEndpoints`
- migra endpoints manuais para o padrão ISlice

## Pré-condições

1. Ler `spec.md`, `plan.md` e `tasks.md`.
2. Ler `backend.instructions.md`.
3. Verificar se a infraestrutura já existe.
4. Não recriar infraestrutura existente.

## Contrato

```csharp
public interface ISlice
{
    void AddEndpoint(IEndpointRouteBuilder app);
}
```

Cada endpoint deve ser público, concreto, não abstrato e stateless.

## Registro

`RegisterSlices(IServiceCollection, Assembly)` deve:

1. inspecionar o assembly aprovado
2. encontrar classes públicas, concretas e não abstratas que implementem `ISlice`
3. registrá-las sob o contrato `ISlice`
4. evitar duplicados

Não descobrir por nome de classe.

## Mapeamento

`MapSliceEndpoints(IEndpointRouteBuilder)` deve:

1. criar o `RouteGroupBuilder`
2. aplicar `ValidationFilterFactory.Create`
3. resolver `IEnumerable<ISlice>`
4. chamar `AddEndpoint(group)` para cada slice

## Adicionar um endpoint

1. Confirmar rota, verbo, request, response e status codes na spec.
2. Criar o diretório do caso de uso.
3. Criar a classe endpoint e implementar `ISlice`.
4. Delegar para o handler do caso de uso.
5. Não modificar `Program.cs`.
6. Não criar registry manual.
7. Executar build e testes autorizados.

## Restrições de lifetime

Se `ISlice` for registrado como singleton:

- deve ser stateless
- não deve receber dependências scoped no construtor
- as dependências da request devem ser resolvidas no handler de rota ou delegadas para o handler do caso de uso

## Proibições

- registrar endpoints individuais em `Program.cs`
- criar `PropertiesEndpoints.cs` ou outros registries manuais
- duplicar scanners de reflection
- manter listas manuais de tipos
- descobrir endpoints por convenção de nome
- guardar estado mutável da request em um slice

## Checklist

- [ ] A tarefa existe em `tasks.md`.
- [ ] O endpoint está descrito em `spec.md`.
- [ ] Implementa `ISlice`.
- [ ] A classe é pública, concreta e stateless.
- [ ] `AddEndpoint` contém o mapeamento de rota.
- [ ] `Program.cs` não foi modificado para registrar o endpoint.
- [ ] Nenhum registry manual foi criado.
- [ ] A validação global continua aplicada.
- [ ] O projeto compila.
- [ ] Os testes autorizados passam.