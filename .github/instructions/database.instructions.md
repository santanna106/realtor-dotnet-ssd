# Instruções de banco de dados e persistência

Estas instruções aplicam-se a qualquer alteração relacionada a:

- EF Core.
- Npgsql.
- PostgreSQL.
- entidades persistentes.
- `AppDbContext`.
- configurações `IEntityTypeConfiguration<T>`.
- migrações do EF Core.
- seeders.
- inicialização do banco de dados.

O projeto utiliza:

- EF Core.
- Npgsql.
- PostgreSQL.
- configurações separadas via `IEntityTypeConfiguration<T>`.
- migrações do EF Core versionadas.
- `UseSeeding()` e `UseAsyncSeeding()` para dados iniciais.
- seeds idempotentes.
- aplicação automática de migrações pendentes na inicialização da aplicação.

Estas instruções definem as regras obrigatórias de persistência.

Os procedimentos operacionais detalhados DEVEM seguir as diretrizes oficiais do projeto.

---

## Relação com Spec-Driven Development

Qualquer alteração de persistência DEVE originar-se de uma especificação aprovada.

É proibido:

- criar entidades persistentes não descritas em `spec.md`
- modificar o modelo de dados fora do escopo da especificação vigente
- gerar migrações sem a tarefa correspondente em `tasks.md`
- adicionar ou modificar *seeds* sem justificativa na especificação
- modificar manualmente o esquema do banco de dados para contornar o fluxo definido
- criar arquivos de migração durante a inicialização da aplicação

Antes de realizar qualquer alteração de persistência, o agente DEVE:

1. Ler `spec.md`.
2. Ler `plan.md`.
3. Ler `tasks.md`.
4. Identificar a tarefa específica relacionada à persistência.
5. Determinar qual *skill* deve ser aplicado.
6. Implementar apenas as alterações definidas pela especificação.
7. Marcar a tarefa como `[X]` somente quando ela tiver sido concluída e verificada.

---

## Skills obrigatórios

Todo trabalho de persistência DEVE seguir um ou mais dos seguintes *skills*, dependendo do tipo de alteração:

```text
.github/skills/ef-core-entity-configuration/SKILL.md
.github/skills/ef-core-migrations/SKILL.md
.github/skills/database-migration-and-seeding/SKILL.md
```

Cada *skill* tem uma responsabilidade específica.

### `ef-core-entity-configuration`

Usar quando uma especificação:

- adiciona uma entidade persistente
- modifica uma entidade persistente
- adiciona ou modifica propriedades persistentes
- cria ou modifica relacionamentos
- adiciona índices
- adiciona restrições
- adiciona conversões de tipos
- requer o registro de um `DbSet<T>`

### `ef-core-migrations`

Usar quando uma especificação modifica o modelo persistente e é necessário gerar uma migração versionada do EF Core.

Este *skill* é executado após a conclusão de:

- entidades
- propriedades persistentes
- relacionamentos
- índices
- restrições
- configurações `IEntityTypeConfiguration<T>`
- registros necessários no `AppDbContext`

### `database-migration-id-seeding`

Utilizar para implementar e manter o mecanismo centralizado que:

- aplica migrações pendentes ao iniciar a aplicação
- configura `UseSeeding()`
- configura `UseAsyncSeeding()`
- executa seeds idempotentes via EF Core
- usa `MigrateAsync()` para provedores relacionais
- usa `EnsureCreatedAsync()` apenas como fallback controlado para provedores não relacionais ou de testes

---

## Fluxo obrigatório de persistência

Quando uma especificação (spec) modifica o modelo persistente, o fluxo esperado é:

```text
spec.md
    ↓
plan.md
    ↓
tasks.md
    ↓
Entidades ou alterações de modelo
    ↓
DbSet<T> quando aplicável
    ↓
IEntityTypeConfiguration<T>
    ↓
Migração do EF Core versionada
    ↓
Inicialização da aplicação
    ↓
app.MigrateAsync()
    ↓
Database.MigrateAsync()
    ↓
EF Core avalia e aplica de 0 a N migrações pendentes
    ↓
UseAsyncSeeding()
    ↓
DatabaseSeeder.SeedAsync()
    ↓
Banco de dados atualizado
```

A ordem DEVE ser respeitada.

É proibido gerar uma migração antes de concluir e validar o modelo persistente correspondente.

---

## Configuração de entidades

Cada entidade persistente DEVE ter sua própria classe de configuração do EF Core. Cada configuração DEVE implementar:

```csharp
IEntityTypeConfiguration<T>
```

As configurações DEVEM estar localizadas em:

```text
app/backend/src/RealtorApi/Infrastructure/Persistence/Configurations/
```

A convenção de nomenclatura obrigatória é:

```text
<NomeDaEntidade>Configuration.cs
```

O procedimento detalhado DEVE seguir:

```text
.github/skills/ef-core-entity-configuration/SKILL.md
```

---

## AppDbContext

O `AppDbContext.OnModelCreating` DEVE descobrir as configurações automaticamente:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.ApplyConfigurationsFromAssembly(
        typeof(AppDbContext).Assembly);
}
```

É proibido configurar entidades *inline* dentro do `OnModelCreating`.

---

## DbSet

Registre o `DbSet<T>` quando apropriado, de acordo com o modelo e os casos de uso.

Exemplo:

```csharp
public DbSet<Property> Properties => Set<Property>();
```

Não adicione `DbSet<T>` automaticamente para todas as entidades sem avaliar se é apropriado.

---

## Migrações do EF Core

Qualquer modificação no modelo de persistência que exija alteração de esquema DEVE gerar uma migração versionada do EF Core.

A geração DEVE seguir:

```text
.github/skills/ef-core-migrations/SKILL.md
```

A tarefa correspondente DEVE existir em `tasks.md`.

O projeto DEVE compilar antes de gerar a migração.

---

## Uma migração por alteração funcional coerente

É proibido criar automaticamente uma migração para cada entidade.

Exemplo incorreto:

```text
AddProperty
AddTenant
AddLease
```

Exemplo correto:

```text
AddPropertyManagementEntities
```

A migração representa uma alteração funcional coerente, não necessariamente uma única entidade.

---

## Validações antes de gerar uma migração

Antes de gerar qualquer migração, valide:

- [ ] Existe `spec.md`.
- [ ] Existe `plan.md`.
- [ ] Existe `tasks.md`.
- [ ] Existe uma tarefa explícita para a migração.
- [ ] Todas as entidades exigidas pela especificação foram criadas.
- [ ] Todas as alterações de persistência foram concluídas.
- [ ] Os `DbSet<T>` necessários foram registrados.
- [ ] As configurações `IEntityTypeConfiguration<T>` foram criadas.
- [ ] Os relacionamentos foram configurados.
- [ ] Os índices foram configurados.
- [ ] As restrições foram configuradas.
- [ ] O projeto compila.

Se alguma condição não for atendida, a geração da migração DEVE ser interrompida.

---

## Geração de migrações

Exemplo:

```powershell
dotnet ef migrations add AddPropertyManagementEntities `
  --project app/backend/src/RealtorApi `
  --startup-project app/backend/src/RealtorApi `
  --output-dir Infrastructure/Persistence/Migrations
```

O nome DEVE representar claramente a alteração funcional. ---

## Revisão obrigatória de migrações

Toda migração gerada DEVE ser revisada antes de marcar a tarefa como concluída.

Revise, no mínimo:

- `Up()`
- `Down()`
- snapshot do modelo
- tabelas
- colunas
- tipos de dados
- nulabilidade
- chaves primárias (primary keys)
- chaves estrangeiras (foreign keys)
- índices
- restrições
- renomeações
- operações destrutivas

Se houver risco de perda de dados, o agente DEVE parar e solicitar confirmação explícita.

---

## Separação entre geração e execução de migrações

### Geração

O skill:

```text
.github/skills/ef-core-migrations/SKILL.md
```

gera e revisa arquivos de migração versionados.

### Execução

O skill:

```text
.github/skills/database-migration-and-seeding/SKILL.md
```

define como a aplicação aplica migrações e executa o *seeding* nativo do EF Core durante a inicialização.

---

## Aplicação automática de migrações

A aplicação DEVE centralizar a execução utilizando:

```csharp
await app.MigrateAsync();
```

A implementação detalhada DEVE seguir:

```text
.github/skills/database-migration-and-seeding/SKILL.md
```

Exemplo:

```csharp
public static class MigrationExtensions
{
    public static async Task MigrateAsync(
        this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider
            .GetRequiredService<AppDbContext>();

        if (context.Database.IsRelational())
        {
            await context.Database.MigrateAsync();
        }
        else
        {
            await context.Database.EnsureCreatedAsync();
        }
    }
}
```

A extensão NÃO DEVE invocar manualmente o seeder.

---

## Execução de seed sem migrações pendentes

A aplicação DEVE sempre invocar:

```csharp
await app.MigrateAsync();
```

durante a inicialização, mesmo quando se estime que não existem migrações pendentes.

A ausência de migrações pendentes NÃO implica que o fluxo de inicialização deva ser omitido.

Para provedores relacionais, a extensão executa:

```csharp
await context.Database.MigrateAsync();
```

O EF Core avalia as migrações pendentes e, ao finalizar a operação, executa o delegado configurado via `UseAsyncSeeding()`, mesmo que nenhuma nova migração tenha sido aplicada.

O fluxo válido é:

```text
Aplicação inicia
    ↓
app.MigrateAsync()
    ↓
Database.MigrateAsync()
    ↓
EF Core avalia 0..N migrações pendentes
    ↓
UseAsyncSeeding()
    ↓
DatabaseSeeder.SeedAsync()
    ↓
Banco de dados pronto
```

O seeder DEVE ser idempotente, pois será avaliado a cada execução do fluxo de inicialização.

É proibido condicionar a chamada a `MigrateAsync()` apenas à existência de migrações pendentes.

Exemplo proibido:

```csharp
var pendingMigrations =
    await context.Database.GetPendingMigrationsAsync();

if (pendingMigrations.Any())
{
    await context.Database.MigrateAsync();
}
```

Esse padrão pode impedir que `UseAsyncSeeding()` seja executado quando não existem migrações pendentes.

O padrão correto é:

```csharp
await context.Database.MigrateAsync();
```

É proibido invocar manualmente:

```csharp
await DatabaseSeeder.SeedAsync(context);
```

como fallback quando não existirem migrações pendentes.

O seed DEVE ser executado exclusivamente por meio de:

```text
UseSeeding()
UseAsyncSeeding()
```

---

## Registro obrigatório do seeding

O `DbContext` DEVE configurar ambos os mecanismos:

```csharp
UseSeeding()
```

e:

```csharp
UseAsyncSeeding()
```

Exemplo:

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options
        .UseNpgsql(
            builder.Configuration.GetConnectionString("Default"))
        .UseSeeding((context, _) =>
        {
            DatabaseSeeder.Seed((AppDbContext)context);
        })
        .UseAsyncSeeding(
            async (context, _, cancellationToken) =>
            {
                await DatabaseSeeder.SeedAsync(
                    (AppDbContext)context,
                    cancellationToken);
            }));
```

Ambos os delegados DEVEM delegar a execução para o `DatabaseSeeder`.

É proibido duplicar a lógica de seed dentro dos delegados.

---

## DatabaseSeeder

O seeder DEVE estar localizado em:

```text
app/backend/src/RealtorApi/Infrastructure/Persistence/DatabaseSeeder.cs
```

Deve expor:

```csharp
Seed(AppDbContext context)
```

e:

```csharp
SeedAsync(
    AppDbContext context,
    CancellationToken cancellationToken = default)
```

As versões síncrona e assíncrona DEVEM:

- implementar a mesma lógica
- manter-se sincronizadas
- ser idempotentes
- evitar a duplicação de dados

A versão assíncrona DEVE propagar o `CancellationToken`.

---

## Registro no Program.cs

A aplicação DEVE chamar:

```csharp
await app.MigrateAsync();

app.Run();
```

O `Program.cs` NÃO DEVE conter lógica de migração nem lógica de seed.

---

## Provedores relacionais

Para PostgreSQL e qualquer provedor relacional que utilize migrações:

```csharp
await context.Database.MigrateAsync();
```

É proibido usar `EnsureCreatedAsync()` para um banco de dados relacional que utiliza migrações.

---

## Provedores não relacionais ou testes

É permitido usar:

```csharp
await context.Database.EnsureCreatedAsync();
```

apenas quando:

- `context.Database.IsRelational()` for `false`
- o provedor não oferecer suporte a migrações
- a especificação ou o arquivo `plan.md` justificarem o provedor utilizado

---

## Regra sobre `dotnet ef database update`

É proibido usar:

```text
dotnet ef database update
```

como etapa obrigatória de conclusão de cada especificação.

A aplicação aplica normalmente as migrações pendentes na inicialização por meio de:

```csharp
await app.MigrateAsync();
```

---

## Regras específicas para o EF Core 11

### Conflitos no snapshot

Se duas ramificações (branches) gerarem árvores de migração divergentes e ocorrer um conflito no snapshot, é proibido editá-lo manualmente para ocultar o conflito.

O conflito DEVE ser resolvido unificando corretamente as migrações.

### `dotnet ef database update --add`

É proibido usar:

```text
dotnet ef database update --add
```

no fluxo do projeto.

As migrações DEVEM ser geradas explicitamente, armazenadas, revisadas e aplicadas por meio do fluxo aprovado.

### `OldMigrationVersionWarning`

Se o aviso `OldMigrationVersionWarning` aparecer, o agente DEVE investigar a versão do snapshot e regenerá-lo corretamente.

É proibido criar uma migração redundante apenas para ocultar o aviso.

---

## HasData

É proibido usar `HasData()` para dados que exijam:

- lógica condicional
- valores dinâmicos
- `DateTime.UtcNow`
- consultas prévias
- decisões baseadas em existência
- integração com outros dados

Para esses casos, utilize:

```text
DatabaseSeeder
+
UseSeeding()
+
UseAsyncSeeding()
```

---

## Responsabilidades de MigrationExtensions

`MigrationExtensions` DEVE:

- criar o escopo de serviços
- resolver o `AppDbContext`
- detectar se o provedor é relacional
- executar `MigrateAsync()` para provedores relacionais
- executar `EnsureCreatedAsync()` apenas como fallback controlado

`MigrationExtensions` NÃO DEVE:

- invocar o `DatabaseSeeder` manualmente
- verificar migrações pendentes para decidir se chama ou não o `MigrateAsync()`
- criar migrações
- gerar arquivos de código
- criar tabelas manualmente
- modificar o modelo do EF Core
- conter dados de seed
- conter lógica de negócio

---

## Responsabilidades do DatabaseSeeder

O `DatabaseSeeder` DEVE:

- conter a lógica de *seed*
- oferecer uma versão síncrona
- oferecer uma versão assíncrona
- manter ambas as versões funcionalmente equivalentes
- executar *seeds* idempotentes
- respeitar as dependências entre os dados iniciais
- evitar a duplicação de dados
- propagar o `CancellationToken` em operações assíncronas

O `DatabaseSeeder` NÃO DEVE:

- executar migrações
- modificar o esquema
- criar arquivos de migração
- implementar lógica de negócios
- ser invocado manualmente a partir de `MigrationExtensions`

---

## SQL manual

É proibido criar tabelas manualmente no PostgreSQL para evitar migrações do EF Core.

É proibido executar scripts SQL que não estejam vinculados a uma tarefa no arquivo `tasks.md`.

---

## Operações destrutivas

Diante de qualquer alteração destrutiva, o agente DEVE parar.

Exemplos:

- exclusão de tabelas
- exclusão de colunas
- alterações de tipo com risco de perda de dados
- redução do tamanho de colunas
- alterações de nulabilidade incompatíveis com dados existentes
- exclusão de relacionamentos
- exclusão de índices críticos

O agente DEVE solicitar confirmação explícita antes de prosseguir.

---

## Relação com o tasks.md

Quando uma especificação exigir alterações persistentes, o `tasks.md` deve incluir tarefas equivalentes a:

```md
- [ ] Criar as entidades persistentes exigidas pela especificação.
- [ ] Criar as configurações do EF Core correspondentes.
- [ ] Registrar os DbSet<T> necessários.
- [ ] Gerar uma migração única do EF Core para a alteração funcional da especificação.
- [ ] Revisar o conteúdo da migração gerada.
- [ ] Atualizar DatabaseSeeder.Seed e DatabaseSeeder.SeedAsync se a especificação exigir dados iniciais.
- [ ] Verificar se UseSeeding e UseAsyncSeeding estão configurados.
- [ ] Verificar se app.MigrateAsync() é executado sempre ao iniciar a aplicação.
- [ ] Verificar se o EF Core avalia de 0 a N migrações pendentes.
- [ ] Verificar se o EF Core executa automaticamente o seed, mesmo que não existam migrações pendentes.
```

Não adicionar como tarefa obrigatória:

```text
Executar dotnet ef database update
```

---

## Proibições

É proibido:

- Criar tabelas manualmente no PostgreSQL.
- Criar entidades persistentes fora de uma especificação aprovada.
- Modificar o esquema sem migração quando aplicável.
- Usar scripts SQL não rastreados.
- Configurar entidades *inline* no `OnModelCreating`.
- Criar migrações sem uma tarefa correspondente em `tasks.md`.
- Gerar migrações antes de concluir o modelo persistente.
- Criar automaticamente uma migração para cada entidade de uma mesma especificação.
- Executar `dotnet ef database update` como etapa obrigatória de conclusão de uma especificação.
- Usar `dotnet ef database update --add`.
- Gerar arquivos de migração durante a inicialização.
- Usar `EnsureCreatedAsync()` em bancos de dados relacionais com migrações.
- Criar *seeds* não idempotentes.
- Inserir dados duplicados durante a inicialização.
- Invocar o *seeder* manualmente após `MigrateAsync()`.
- Invocar o *seeder* manualmente a partir de `MigrationExtensions`.
- Condicionar `MigrateAsync()` à existência de migrações pendentes.
- Usar `GetPendingMigrationsAsync()` para pular o `MigrateAsync()`.
- Configurar apenas `UseSeeding()` ou apenas `UseAsyncSeeding()`.
- Duplicar a lógica de *seed* dentro dos delegados.
- Usar `HasData()` para dados dinâmicos ou condicionais.
- Resolver conflitos de *snapshot* editando-o manualmente para ocultar o conflito.
- Marcar uma tarefa como `[X]` sem revisar a migração gerada.

---

## Checklist antes de finalizar uma tarefa de persistência

### Entidades e configurações

- [ ] A alteração está descrita em `spec.md`.
- [ ] Existe uma tarefa correspondente em `tasks.md`.
- [ ] A entidade está localizada corretamente.
- [ ] `DbSet<T>` foi registrado, se aplicável.
- [ ] Existe `<Entidade>Configuration.cs`. - [ ] A configuração implementa `IEntityTypeConfiguration<T>`.
- [ ] `OnModelCreating` utiliza `ApplyConfigurationsFromAssembly`.
- [ ] Não há configuração *inline*.

### Migrações

- [ ] A migração possui um nome descritivo.
- [ ] A migração representa uma alteração funcional coerente.
- [ ] Não existem migrações desnecessárias por entidade.
- [ ] O método `Up()` foi revisado.
- [ ] O método `Down()` foi revisado.
- [ ] O *snapshot* foi revisado.
- [ ] As tabelas e colunas foram verificadas.
- [ ] Os relacionamentos foram verificados.
- [ ] Os índices foram verificados.
- [ ] Não existem operações destrutivas sem aprovação.

### Registro de seeding

- [ ] O `DbContext` configura `UseSeeding()`.
- [ ] O `DbContext` configura `UseAsyncSeeding()`.
- [ ] Ambos delegados delegam para o `DatabaseSeeder`.
- [ ] Não há lógica duplicada dentro dos delegados.

### DatabaseSeeder

- [ ] Existe `DatabaseSeeder.Seed`.
- [ ] Existe `DatabaseSeeder.SeedAsync`.
- [ ] Ambas as versões implementam a mesma lógica.
- [ ] Os seeds são idempotentes.
- [ ] O `CancellationToken` é propagado corretamente.
- [ ] Não ocorrem inserções duplicadas após múltiplas execuções.

### Execução automática

- [ ] Existe a extensão `MigrateAsync`.
- [ ] `MigrationExtensions` aplica apenas migrações ou executa o fallback autorizado.
- [ ] `MigrationExtensions` não invoca manualmente o seeder.
- [ ] `Program.cs` chama `await app.MigrateAsync();` antes de `app.Run()`.
- [ ] A chamada para `MigrateAsync()` não é condicionada por `GetPendingMigrationsAsync()`.
- [ ] O PostgreSQL utiliza `MigrateAsync()`.
- [ ] `EnsureCreatedAsync()` é utilizado apenas como fallback não relacional ou para testes.
- [ ] A aplicação inicia corretamente.
- [ ] O EF Core avalia e aplica de 0 a N migrações pendentes.
- [ ] O EF Core executa automaticamente `UseAsyncSeeding()` mesmo quando não há migrações pendentes.
- [ ] O banco de dados é inicializado corretamente a cada inicialização.

A tarefa só pode ser marcada como `[X]` após a conclusão das validações correspondentes.