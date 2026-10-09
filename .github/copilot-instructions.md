<!-- SPECKIT START -->
Para más contexto sobre tecnologías, estructura del proyecto, comandos y otra información relevante, lee el plan actual en `specs/001-realtor-solution-foundation/plan.md`.
<!-- SPECKIT END -->

# Instruções globais do projeto

Este repositório utiliza *Spec-Driven Development* como fluxo de trabalho obrigatório.

Toda implementação DEVE respeitar a constituição do projeto localizada em:

`.specify/memory/constitution.md`

A constituição tem precedência sobre qualquer preferência pessoal, convenção automática ou sugestão do agente.

## Idioma obrigatório

Todo arquivo `.md` do repositório DEVE estar escrito em espanhol.

Isso inclui:

- `.github/`
- `.specify/`
- `specs/`
- `README.md`
- documentação técnica
- instruções
- prompts
- skills próprias do projeto

É proibido misturar idiomas dentro de um mesmo documento `.md`.

Nomes técnicos, APIs, classes, comandos, namespaces, pacotes e nomes próprios do ecossistema .NET podem ser mantidos em inglês quando apropriado.

### Exceção: skills importadas de terceiros

Skills importadas de terceiros ou do ecossistema (por exemplo, aquelas provenientes de pacotes externos, do Blazor SDK ou de fornecedores) PODEM ser mantidas em inglês e NÃO precisam ser traduzidas.

Consideram-se skills próprias do projeto aquelas criadas e mantidas dentro deste repositório para sua governança (por exemplo, `spec-driven-workflow`, `minimal-api-slices`, `vertical-slice-handlers`, `blazor-app-css-design-system` e outras skills escritas originalmente em espanhol). Estas DEVEM permanecer em espanhol.

Uma skill importada em inglês NÃO é considerada uma mistura proibida de idiomas, desde que cada documento `.md` mantenha um único idioma internamente.

## Spec-Driven Development

As especificações (*specs*) aprovadas, localizadas em `specs/`, são a única fonte de verdade do projeto.

É proibido implementar funcionalidades que não estejam descritas no `spec.md` vigente.

Cada iniciativa DEVE conter exatamente três arquivos:

- `spec.md`
- `plan.md`
- `tasks.md`

O fluxo obrigatório mínimo é:

```text
speckit.specify → speckit.plan → speckit.tasks → speckit.implement
```

Para especificações fundamentais ou de alto impacto, recomenda-se:

```text
speckit.specify → speckit.clarify → speckit.plan → speckit.analyze → speckit.tasks → speckit.implement
```

Nenhuma fase obrigatória pode ser ignorada.

## Validação de sequência

Antes de executar qualquer fase, valide se os artefatos anteriores existem.

Se houver tentativa de executar `speckit.plan` sem que `spec.md` exista, interrompa a execução e exiba:

```text
Execução interrompida: antes de executar speckit.plan, você deve criar o arquivo spec.md usando speckit.specify.
```

Se houver tentativa de executar `speckit.tasks` sem que `plan.md` exista, interrompa a execução e exiba:

```text
Execução interrompida: antes de executar speckit.tasks, você deve criar o arquivo plan.md usando speckit.plan.
```

Se houver tentativa de executar `speckit.implement` sem que `tasks.md` exista, interrompa a execução e exiba:

```text
Execução interrompida: antes de executar speckit.implement, você deve criar o arquivo tasks.md usando speckit.tasks.
```

É proibido criar arquivos vazios ou simulados para pular fases.

## Rastreabilidade obrigatória

Todo código, arquivo, template, migração, seeder, componente, endpoint ou alteração de configuração DEVE estar vinculado a uma tarefa específica em `tasks.md`.

Se uma alteração não tiver uma tarefa associada em `tasks.md`, ela deve ser rejeitada.

Cada tarefa concluída DEVE ser marcada como `[X]`.

## Ordem de implementação

A ordem de implementação é definida pelo prefixo numérico de cada especificação.

A especificação com número menor é SEMPRE implementada antes de uma especificação com número maior. Exemplo:

```text
specs/001-foundation/
specs/002-backend-contracts/
specs/003-frontend-ui/
```

A spec `001` deve ser implementada antes da `002`, e a `002` antes da `003`.

## Estrutura das specs

As specs ficam na raiz do repositório:

```text
specs/
  001-nome-da-spec/
    spec.md
    plan.md
    tasks.md
```

É proibido criar specs dentro de `.specify/specs/`.

A pasta `.specify/` é reservada exclusivamente para a infraestrutura do spec-kit.

## Estrutura de `.specify/`

A pasta `.specify/` só pode conter:

```text
.specify/
  memory/
  scripts/
  templates/
  extensions.yml
  feature.json
```

É proibido criar:

```text
.specify/specs/
```

Se essa pasta existir, será considerada uma falha crítica de governança.

## Stack obrigatório

O projeto utiliza:

- .NET 11
- ASP.NET Core Minimal APIs.
- Blazor Web App com Razor Components.
- EF Core.
- PostgreSQL com Npgsql.
- Refit para comunicação tipada entre frontend e backend.
- CSS próprio em `wwwroot/app.css`.

É proibido introduzir frameworks, bibliotecas ou padrões que contradigam a constituição.

## Regras de implementação

Antes de escrever código:

1. Ler a spec vigente.
2. Ler `plan.md`.
3. Ler `tasks.md`.
4. Identificar a tarefa exata a ser implementada.
5. Confirmar que a tarefa não esteja marcada como `[X]`.
6. Implementar apenas o escopo descrito.
7. Marcar a tarefa como `[X]` apenas quando estiver concluída e verificada.

## Proibições gerais

É proibido:

- Implementar funcionalidades não descritas em `spec.md`.
- Pular fases do fluxo spec-driven.
- Criar arquivos não rastreados em `tasks.md`.
- Alterar a arquitetura sem atualizar `plan.md`.
- Modificar scripts upstream do spec-kit sem uma decisão humana explícita.
- Mover specs para caminhos não canônicos.
- Criar soluções paralelas.
- Adicionar frameworks CSS.
- Usar controllers no backend.
- Configurar entidades do EF Core inline em `OnModelCreating`.
- Criar migrações sem uma tarefa correspondente em `tasks.md`. - Criar uma migração para cada entidade quando elas pertencem à mesma especificação.