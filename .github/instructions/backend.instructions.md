---
applyTo: "app/backend/**/*.cs"
---

# Instruções de backend

Estas instruções se aplicam a todo o trabalho de backend do projeto.

O backend DEVE ser implementado com:

- .NET 11.
- ASP.NET Core Minimal APIs.
- Vertical Slice Architecture.
- EF Core.
- Npgsql.
- PostgreSQL.
- FluentValidation.
- ProblemDetails.
- logging estruturado via `ILogger`.
- operações assíncronas para I/O.
- propagação de `CancellationToken`.

A versão exata do SDK DEVE estar fixada através do `global.json` na raiz do repositório.

Os procedimentos especializados DEVEM seguir as skills oficiais do projeto quando aplicável.

---

## Relação com Spec-Driven Development

Toda alteração de backend DEVE ter origem em uma spec aprovada.

Antes de implementar qualquer alteração de backend, o agente DEVE:

1. Ler `spec.md`.
2. Ler `plan.md`.
3. Ler `tasks.md`.
4. Identificar a tarefa específica.
5. Determinar quais instruções e skills correspondem.
6. Implementar unicamente o escopo definido.
7. Verificar a implementação.
8. Marcar a tarefa como `[X]` apenas após concluir e verificar a alteração.

Fluxo obrigatório:

```text
spec.md
    ↓
plan.md
    ↓
tasks.md
    ↓
implementação
    ↓
verificação
    ↓
[X]
```

É proibido:

- implementar funcionalidade não descrita em `spec.md`
- implementar tarefas inexistentes em `tasks.md`
- ampliar o escopo por iniciativa própria
- criar abstrações antecipadas não requeridas
- realizar refatorações grandes fora do escopo da spec
- marcar uma tarefa como `[X]` antes de verificá-la

---

## Skills obrigatórias

Antes de implementar uma tarefa de backend, o agente DEVE determinar qual skill corresponde.

Conforme o tipo de alteração, usar uma ou mais das seguintes:

```text
.github/skills/minimal-api-slices/SKILL.md
.github/skills/minimal-api-validation/SKILL.md
.github/skills/vertical-slice-handlers/SKILL.md
.github/skills/vertical-slice-mapping/SKILL.md
.github/skills/result-problem-details/SKILL.md
.github/skills/testing-minimal-apis/SKILL.md

.github/skills/domain-events/SKILL.md
.github/skills/module-public-api/SKILL.md
.github/skills/ef-core-entity-configuration/SKILL.md
.github/skills/ef-core-migrations/SKILL.md
.github/skills/database-migration-and-seeding/SKILL.md
```

`domain-events` e `module-public-api` são condicionais e só devem ser utilizadas quando a spec ou o plano justificarem sua necessidade.

As regras detalhadas de persistência DEVEM seguir também:

```text
.github/instructions/database.instructions.md
```

---

## Arquitetura obrigatória

O backend DEVE ser organizado através de Vertical Slice Architecture.

A unidade principal de organização é:

```text
feature
+
caso de uso
```

É proibido organizar casos de uso através de pastas técnicas globais como:

```text
Controllers/
Services/
Managers/
Repositories/
DTOs/
Validators/
Helpers/
Commands/
Queries/
```

Exemplo:

```text
Features/
  Properties/
    CreateProperty/
      CreateProperty.Endpoint.cs
      CreateProperty.Handler.cs
      CreateProperty.Mapping.cs
      CreateProperty.Validators.cs
```

Cada slice DEVE manter juntos seus elementos relacionados.

A infraestrutura transversal realmente compartilhada pode residir fora das features, por exemplo:

```text
Infrastructure/
  Endpoints/
  Validation/
  Handlers/
  Persistence/
```

A lógica compartilhada só DEVE ser extraída quando houver reutilização real e comprovável.

---

## Convenção de estrutura de Vertical Slices

Cada caso de uso DEVE residir em seu próprio diretório dentro da feature correspondente.

Preferir nomes como:

```text
CreateProperty
UpdateProperty
DeleteProperty
ChangePropertyStatus
GetPropertyById
ListProperties
```

Evitar nomes técnicos genéricos.

Como convenção padrão, cada slice pode separar responsabilidades em:

```text
<Slice>.Endpoint.cs
<Slice>.Handler.cs
<Slice>.Mapping.cs
<Slice>.Validators.cs
```

Não é obrigatório criar arquivos vazios. Crie apenas os arquivos de que o caso de uso necessitar.

---

## Minimal APIs e ISlice

Todos os endpoints DEVEM ser implementados por meio de ASP.NET Core Minimal APIs.

É proibido usar controllers.

Todos os endpoints DEVEM seguir o padrão `ISlice`.

Contrato:

```csharp
public interface ISlice
{
    void AddEndpoint(IEndpointRouteBuilder app);
}
```

Exemplo:

```csharp
public sealed class CreatePropertyEndpoint : ISlice
{
    public void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/properties", HandleAsync);
    }
}
```

Os endpoints DEVEM ser autodescobertos e mapeados através da infraestrutura centralizada.

É proibido:

- registrar endpoints individuais no `Program.cs`
- criar registros manuais por feature
- manter listas manuais de endpoints
- duplicar scanners de assemblies
- descobrir endpoints por nome de classe

Adicionar um novo endpoint NÃO DEVE exigir modificação no `Program.cs`.

As implementações de `ISlice` DEVEM ser stateless.

As dependências de escopo de uma requisição (scoped) NÃO DEVEM ser injetadas no construtor de um `ISlice` registrado como singleton. Elas devem ser resolvidas no handler de rota ou delegadas ao handler do caso de uso.

O procedimento detalhado DEVE seguir:

```text
.github/skills/minimal-api-slices/SKILL.md
```

---

## Program.cs

O `Program.cs` DEVE se limitar a:

- configurar serviços
- configurar middleware
- registrar infraestrutura
- registrar módulos
- mapear endpoints através do mecanismo centralizado
- iniciar o fluxo centralizado de migração
- iniciar a aplicação

Exemplo conceitual:

```csharp
builder.Services.AddValidatorsFromAssembly(
    typeof(Program).Assembly,
    ServiceLifetime.Scoped);

builder.Services.RegisterSlices(typeof(Program).Assembly);
builder.Services.RegisterHandlers(typeof(Program).Assembly);

var app = builder.Build();

app.MapSliceEndpoints();

await app.MigrateAsync();

app.Run();
```

O `Program.cs` NÃO DEVE conter:

- lógica de negócio
- lógica de validação
- consultas de negócio
- lógica de migrações
- lógica de seed
- inserção manual de dados iniciais
- registros individuais de endpoints, validators ou handlers

---

## Endpoints enxutos (delgados)

Os endpoints DEVEM ser enxutos.

Responsabilidade principal:

```text
HTTP Request
    ↓
Binding
    ↓
Validação automática
    ↓
Handler
    ↓
Result
    ↓
HTTP Response
```

Cada endpoint DEVE:

- ter request explícito quando receber entrada
- ter response explícito quando retornar dados
- delegar a execução ao handler correspondente
- retornar códigos HTTP corretos
- retornar `ProblemDetails` em erros esperados
- registrar logs estruturados quando cabível
- estar rastreado a uma tarefa em `tasks.md`
- respeitar o contrato definido pela spec

Os endpoints NÃO DEVEM conter:

- regras de negócio
- lógica complexa
- consultas complexas ao banco de dados
- decisões de domínio
- lógica reutilizável
- lógica de migração ou seed
- validação manual quando existir validação automática aprovada

---

## Validação automática de Minimal APIs

Toda entrada que possua um `IValidator<T>` registrado DEVE ser validada automaticamente antes de executar o handler.

O backend utiliza:

```text
FluentValidation
+
Assembly scanning
+
ValidationFilterFactory
+
IServiceProviderIsService
+
Endpoint Filters
```

A validação é baseada em convenções (convention-based).

Um endpoint com um request validável NÃO DEVE:

- invocar manualmente o validator
- injetar `IValidator<T>` apenas para validar a entrada
- registrar manualmente `ValidationFilter<T>`
- adicionar manualmente `AddEndpointFilter<T>()`

Os validators DEVEM ser registrados automaticamente via assembly scanning.

Exemplo:

```csharp
builder.Services.AddValidatorsFromAssembly(
    typeof(Program).Assembly,
    ServiceLifetime.Scoped);
```

É proibido registrar validators individualmente no `Program.cs`.

O `ValidationFilterFactory` DEVE detectar se existe `IValidator<T>` para um parâmetro do handler.

Fluxo:

```text
Endpoint
    ↓
ValidationFilterFactory
    ↓
Existe IValidator<T>?
   │
   ├── Sim → ValidateAsync
   │          │
   │          ├── válido → Handler
   │          └── inválido → HTTP 400 + ValidationProblemDetails
   │
   └── Não → Pass-through → Handler
```

A ausência de validator NÃO DEVE gerar erro.

A validação assíncrona DEVE propagar `CancellationToken`.

O procedimento detalhado DEVE seguir:

```text
.github/skills/minimal-api-validation/SKILL.md
```

---

## Handlers de casos de uso

Cada handler DEVE representar um único caso de uso.

Os handlers DEVEM:

- coordenar o caso de uso
- aplicar regras de aplicação
- delegar invariantes ao domínio quando cabível
- usar operações assíncronas para I/O
- aceitar e propagar `CancellationToken`
- retornar resultados explícitos
- utilizar `AppDbContext` diretamente quando cabível
- manter a lógica específica dentro do slice

Os handlers NÃO DEVEM exigir uma interface individual para cada implementação.

Evitar:

```text
ICreatePropertyHandler
CreatePropertyHandler
```

Preferir uma marker interface comum:

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
}
```

Os handlers DEVEM ser registrados automaticamente via assembly scanning.

É proibido:

- registrar handlers individualmente no `Program.cs`
- manter listas manuais de handlers
- criar scanners por feature
- introduzir MediatR unicamente para conectar endpoint e handler
- criar wrappers vazios de command/query sem valor real

O procedimento detalhado DEBE seguir:

```text
.github/skills/vertical-slice-handlers/SKILL.md
```

---

## Lógica de negócio

A lógica de negócio NÃO deve residir em:

- componentes de UI
- endpoints
- `Program.cs`
- `DbContext`
- configurações do EF Core
- classes de infraestrutura
- `MigrationExtensions`
- `DatabaseSeeder`
- arquivos de mapping

A lógica deve residir no:

```text
Domínio
```

ou no:

```text
Handler do caso de uso
```

conforme o caso.

Como regra geral:

- invariantes e regras próprias do domínio pertencem ao domínio
- coordenação do caso de uso pertence ao handler
- transporte HTTP pertence ao endpoint
- persistência pertence ao EF Core e à infraestrutura correspondente

---

## Contratos

As entidades de domínio ou de persistência NÃO devem ser misturadas com requests ou responses.

É proibido retornar entidades do EF Core diretamente de endpoints.

Utilizar contratos explícitos por caso de uso.

Exemplos:

```text
CreatePropertyRequest
CreatePropertyResponse
UpdatePropertyRequest
PropertySummaryResponse
PropertyDetailsResponse
```

Os contratos DEVEM expor apenas as informações necessárias.

Quando um request ou response pertencer exclusivamente a um slice, DEVE permanecer dentro do slice.

Apenas mover contratos para `Shared/` quando houver reutilização real entre múltiplos slices do mesmo módulo.

É proibido criar pastas globais de DTOs, requests ou responses para contratos específicos de casos de uso.

---

## Estados do domínio

Os estados do domínio DEVEM ser modelados através de tipos explícitos.

É proibido utilizar strings mágicas para estados conhecidos.

Para propriedades, usar um tipo explícito equivalente a:

```csharp
public enum PropertyStatus
{
    Available,
    Rented,
    Maintenance,
    Inactive
}
```

As regras e valores definitivos sempre devem respeitar a spec vigente.

---

## Mapping explícito

Os mappings DEVEM ser explícitos.

Não introduzir bibliotecas de mapeamento automático apenas para evitar transformações simples.

É proibido introduzir:

```text
AutoMapper
Mapster
Mapperly
```

a menos que a spec ou o `plan.md` justifiquem explicitamente a necessidade.

### Objeto individual

Para mappings individuais específicos de um slice, preferir C# Extension Blocks quando melhorarem a clareza e o agrupamento.

Exemplo:

```csharp
internal static class CreatePropertyMappings
{
    extension(CreatePropertyRequest request)
    {
        internal Property ToEntity()
        {
            return new Property(
                request.Name,
                request.Address);
        }
    }

    extension(Property property)
    {
        internal CreatePropertyResponse ToResponse()
        {
            return new CreatePropertyResponse(
                property.Id,
                property.Name,
                property.Status);
        }
    }
}
```

### Coleção já materializada

Quando a coleção já estiver em memória e o resultado completo precisar ser materializado, preferir:

```csharp
var response = properties
    .Select(x => x.ToSummaryResponse())
    .ToList();
```

### Consulta EF Core de leitura

Preferir projeção direta a partir do EF Core quando não for necessário carregar a entidade completa.

```csharp
var response = await context.Properties
    .AsNoTracking()
    .Select(property => new PropertySummaryResponse(
        property.Id,
        property.Name,
        property.Status))
    .ToListAsync(cancellationToken);
```

### Streaming real

Usar:

```text
IEnumerable<T>
yield return
IAsyncEnumerable<T>
```

apenas quando o caso de uso exigir avaliação diferida (deferred execution), streaming ou processamento incremental.

### Regra de seleção

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

Os mappings NÃO DEVEM conter lógica de negócio, consultas ao banco de dados, validações, side effects ou persistência.

O procedimento detalhado DEVE seguir:

```text
.github/skills/vertical-slice-mapping/SKILL.md
```

---

## Shared dentro de uma feature ou módulo

Cada feature ou módulo pode ter um diretório `Shared/` unicamente para elementos reutilizados de fato por múltiplos slices do mesmo contexto.

Exemplo:

```text
Features/
  Properties/
    CreateProperty/
    UpdateProperty/
    DeleteProperty/
    Shared/
      Errors/
      Requests/
      Responses/
      Routes/
      Mapping/
```

O `Shared/` NÃO DEVE se transformar em um diretório genérico para código cujo local adequado seja incerto.

Preferir duplicação pequena e localizada a abstrações prematuras.

---

## Acesso à persistência a partir dos handlers

Os handlers podem utilizar o `AppDbContext` diretamente.

Não criar repositories para encapsular operações simples do EF Core.

Exemplo desnecessário:

```text
PropertyRepository.GetByIdAsync()
PropertyRepository.AddAsync()
PropertyRepository.SaveAsync()
```

quando apenas delegam para `DbSet<T>`, LINQ e `SaveChangesAsync()`.

Preferir:

```text
Handler
    ↓
AppDbContext
    ↓
EF Core
```

Criar uma abstração de persistência unicamente quando houver uma necessidade real e aprovada.

É proibido criar repositórios genéricos.

---

## Result Pattern e ProblemDetails

Erros de negócio esperados NÃO DEVEM utilizar exceções como controle de fluxo normal.

Exemplos:

```text
NotFound
Conflict
Validation
BusinessRule
Forbidden
```

Os handlers DEVEM retornar um resultado explícito equivalente a `Result<T>` quando um caso de uso puder terminar com um erro esperado.

Fluxo:

```text
Handler
    ↓
Result<T>
    │
    ├── Success → Response HTTP
    │
    └── Error → Mapeamento centralizado → ProblemDetails
```

As exceções devem ser reservadas para condições inesperadas ou falhas técnicas excepcionais.

A conversão de erros para HTTP DEVE ser centralizada e consistente.

O procedimento detalhado DEVE seguir:

```text
.github/skills/result-problem-details/SKILL.md
```

---

## Logging estruturado

O backend DEVE utilizar `ILogger<T>`.

Preferir:

```csharp
logger.LogInformation(
    "Property {PropertyId} was created",
    property.Id);
```

Evitar interpolação de strings quando couber logging estruturado.

É proibido registrar em log:

- senhas
- tokens
- segredos
- connection strings
- credenciais

Não adicionar logs sem valor operacional.

---

## Operações assíncronas

As operações de I/O DEVEM utilizar APIs assíncronas sempre que disponíveis.

Exemplos:

```csharp
await context.SaveChangesAsync(cancellationToken);

await context.Properties
    .FirstOrDefaultAsync(
        x => x.Id == id,
        cancellationToken);
```

Handlers e validators assíncronos DEVEM aceitar e propagar `CancellationToken`.

Evitar:

```text
.Result
.Wait()
.GetAwaiter().GetResult()
```

salvo necessidade excepcional justificada.

---

## Side effects por meio de eventos

Utilizar eventos apenas quando um caso de uso produzir efeitos colaterais que exijam desacoplamento real.

Exemplo:

```text
CreatePropertyHandler
    ↓
Salvar propriedade
    ↓
Persistência bem-sucedida
    ↓
PropertyCreatedEvent
    ↓
Event Handler(s)
```

Não criar eventos por antecipação nem para substituir chamadas diretas simples.

Usar eventos apenas quando:

- existir um efeito colateral real
- existir mais de um consumidor em potencial ou desacoplamento justificado
- a spec ou o `plan.md` exigirem

Se for necessária entrega confiável após uma transação, a estratégia deve ser definida explicitamente na spec e no plano.

O procedimento detalhado DEVE seguir:

```text
.github/skills/domain-events/SKILL.md
```

---

## Comunicação entre módulos

Esta seção se aplica apenas quando a aplicação possuir módulos funcionais explícitos.

Um módulo NÃO DEVE acessar diretamente:

- entidades internas de outro módulo
- `DbContext` de outro módulo
- handlers internos de outro módulo
- infraestrutura interna de outro módulo

A comunicação entre módulos DEVE ser realizada via `PublicApi` ou eventos.

Não criar projetos `PublicApi` de maneira especulativa.

O procedimento detalhado DEVE seguir:

```text
.github/skills/module-public-api/SKILL.md
```

---

## Comunicação frontend-backend

A comunicação do frontend para o backend DEVE usar Refit com interfaces tipadas.

As interfaces DEVEM:

- ser tipadas
- ser organizadas por módulo ou capacidade
- ser registradas via `IHttpClientFactory`
- ser resolvidas via injeção de dependência

A configuração HTTP DEVE centralizar:

- base address
- timeouts
- handlers
- autenticação quando cabível
- logging HTTP
- resiliência

A resiliência HTTP DEVE utilizar a configuração padrão do projeto via `Microsoft.Extensions.Http.Resilience`.

É proibido usar `HttpClient` diretamente para chamadas de API a partir de componentes ou serviços de UI.

É proibido usar `RestService.For<T>()` fora do registro aprovado com `IHttpClientFactory`.

---

## Persistência

Toda alteração relacionada a:

- EF Core
- Npgsql
- PostgreSQL
- entidades persistentes
- `AppDbContext`
- `IEntityTypeConfiguration<T>`
- migrações
- seeders
- inicialização de banco de dados

DEVE seguir:

```text
.github/instructions/database.instructions.md
```

Quando uma tarefa de backend modificar persistência, ambas as instruções DEVEM ser cumpridas.

A aplicação DEVE executar durante a inicialização:

```csharp
await app.MigrateAsync();

app.Run();
```

A chamada a `MigrateAsync()` NÃO DEVE ser condicionada a uma verificação prévia de migrações pendentes.

O seeder NÃO DEVE ser invocado manualmente a partir do `Program.cs` nem de `MigrationExtensions`.

---

## Testes

Os testes DEVEM seguir a constituição vigente e a spec ativa.

Por padrão, são permitidos apenas testes unitários.

Os testes unitários devem focar em:

- lógica de domínio
- handlers
- validators
- regras de negócio
- comportamento isolado do caso de uso

É proibido criar testes não solicitados pela spec vigente.

Qualquer tipo de teste diferente de testes unitários exige justificativa explícita na spec, no plano e nas tarefas, além de permissão expressa na constituição.

---

## Fluxo completo de um Vertical Slice

```text
Cliente
    ↓
Refit
    ↓
Minimal API
    ↓
ISlice
    ↓
Validação automática
    ↓
Handler
    ↓
Domínio
    ↓
AppDbContext
    ↓
EF Core
    ↓
PostgreSQL
    ↓
Result<T>
    ↓
ProblemDetails ou Response
```

Quando houver side effects aprovados:

```text
Handler
    ↓
Persistência bem-sucedida
    ↓
Evento
    ↓
Event Handler(s)
```

---

## Proibições gerais

É proibido:

- usar controllers
- implementar funcionalidade fora da spec
- implementar tarefas inexistentes
- organizar casos de uso por camadas técnicas globais
- colocar lógica de negócio em endpoints
- colocar lógica de negócio no `Program.cs`
- colocar lógica de negócio no `DbContext`
- retornar entidades do EF Core a partir de endpoints
- usar strings mágicas para estados do domínio
- criar repositórios genéricos
- criar serviços genéricos desnecessários
- introduzir MediatR sem necessidade aprovada
- criar interfaces individuais de handlers sem necessidade real
- registrar endpoints, validators ou handlers manualmente
- usar mapeamento automático sem justificativa
- retornar erros inconsistentes em vez de `ProblemDetails`
- usar exceções para erros esperados do negócio
- expor exceções internas ao cliente
- ignorar `CancellationToken` sem justificativa
- bloquear operações assíncronas desnecessariamente
- executar manualmente o seeder
- condicionar `MigrateAsync()` a migrações pendentes
- criar testes fora do escopo autorizado
- realizar refatorações grandes fora da tarefa vigente
- marcar uma tarefa como `[X]` sem verificá-la

---

## Checklist antes de finalizar uma tarefa de backend

### Rastreabilidade

- [ ] A spec ativa existe.
- [ ] `spec.md` foi lido.
- [ ] `plan.md` foi lido.
- [ ] `tasks.md` foi lido.
- [ ] A tarefa existe em `tasks.md`.
- [ ] A implementação corresponde ao escopo aprovado.
- [ ] Não foi adicionada funcionalidade fora da spec.

### Arquitetura

- [ ] O código respeita Vertical Slice Architecture.
- [ ] O caso de uso está organizado dentro de sua feature.
- [ ] Não foram criados controllers.
- [ ] Não foram criados services ou repositories genéricos.
- [ ] Não foram introduzidas abstrações especulativas.
- [ ] A lógica de negócio está no domínio ou no handler.

### Endpoint e validação

- [ ] O endpoint implementa `ISlice`.
- [ ] O endpoint permanece enxuto.
- [ ] Existe request explícito quando aplicável.
- [ ] Existe response explícito quando aplicável.
- [ ] Não são retornadas entidades do EF Core.
- [ ] A entrada é validada automaticamente quando existe validator.
- [ ] Os status codes HTTP estão corretos.
- [ ] Os erros usam `ProblemDetails` ou `ValidationProblemDetails`.

### Handler

- [ ] O handler representa um único caso de uso.
- [ ] Implementa `IHandler`.
- [ ] É registrado automaticamente.
- [ ] Propaga `CancellationToken`.
- [ ] Não existe uma interface individual desnecessária.
- [ ] Usa `AppDbContext` diretamente quando aplicável.

### Mapping

- [ ] O mapping é explícito.
- [ ] A estratégia corresponde ao cenário real.
- [ ] Não contém lógica de negócio.
- [ ] Permanece no slice quando for específico.
- [ ] Foi movido para `Shared` apenas quando há reutilização real.

### Qualidade

- [ ] O logging é estruturado.
- [ ] As operações de I/O são assíncronas.
- [ ] Dados sensíveis não são expostos.
- [ ] Os testes autorizados passam.
- [ ] O projeto compila sem erros.

### Persistência, quando aplicável

- [ ] O arquivo `database.instructions.md` foi seguido.
- [ ] As skills de persistência correspondentes foram aplicadas.
- [ ] A migração foi revisada.
- [ ] `Program.cs` chama `await app.MigrateAsync();`.
- [ ] O seeder não é invocado manualmente.

Apenas após concluir as verificações correspondentes a tarefa pode ser marcada como `[X]`.