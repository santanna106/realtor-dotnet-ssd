<!--
Relatório de impacto:
- Versão: 1.0.0 -> 1.1.0
- Princípios modificados: II. Spec-Driven Development (expansão para ciclo de estados de specs).
- Seções adicionadas: Ciclo de estado de specs; regras de transição e bloqueios operacionais.
- Seções removidas: nenhuma.
- Modelos revisados: spec-template.md ✅ atualizado para status inicial em português; plan-template.md ✅ sem alterações necessárias; tasks-template.md ✅ sem alterações necessárias.
- Comandos e hooks: .specify/extensions.yml ✅ já registra before_implement e after_implement; mantidos conforme a governança.
- Pendências: nenhuma.
-->

# Constituição do Projeto Realtor

## Princípios Fundamentais

### I. Solução Única e Compartilhada

Esta solução é una e não pode ser dividida. Frontend, backend, domínio e persistência evoluem dentro do mesmo sistema.

Toda iniciativa em `specs` DEVE contribuir para esta solução compartilhada, independentemente de pertencer à camada de frontend ou de backend.

Não são permitidas soluções paralelas, bifurcações de arquitetura nem estruturas separadas por tipo de camada.

### II. Spec-Driven Development (Não Negociável)

As specs aprovadas em `specs/`, pasta localizada na raiz do repositório, são a única fonte da verdade do projeto.

NÃO SE DEVE implementar nenhuma funcionalidade que não esteja descrita na spec vigente.

O fluxo obrigatório mínimo é:
`speckit.specify` -> `speckit.plan` -> `speckit.tasks` -> `speckit.implement`

Para specs fundamentais ou de alto impacto, recomenda-se:
`speckit.specify` -> `speckit.clarify` -> `speckit.plan` -> `speckit.analyze` -> `speckit.tasks` -> `speckit.implement`

Nenhuma fase obrigatória pode ser ignorada.

### III. Arquitetura Canônica de Backend e Frontend

O backend DEVE ser organizado por features e casos de uso com Vertical Slice Architecture.

São proibidos controllers e pastas técnicas globais genéricas para orquestrar o domínio.

O `Program.cs` apenas configura serviços, middleware, registro de infraestrutura e mapeamento de endpoints.

O frontend DEVE ser implementado com Blazor Web App e Razor Components.

### IV. Stack Tecnológico Obrigatório

A stack não é negociável e DEVE ser aplicada sem exceção:

* SDK: .NET 11.

* Frontend: Blazor Web App com Razor Components.

* Backend: ASP.NET Core Minimal APIs.

* Persistência: EF Core + Npgsql + PostgreSQL.

* Comunicação: Refit com `IHttpClientFactory`.

* Validação e erros: FluentValidation + ProblemDetails.

* Estilos: CSS próprio centralizado em `wwwroot/app.css`, sem frameworks CSS.

* Ícones: Lucide Icons como sistema principal.

### V. Qualidade de Domínio e Contratos

A lógica de negócio NÃO DEVE residir em componentes de UI, endpoints nem `DbContext`.

As entidades de domínio NÃO DEVEM ser misturadas com requests ou responses.

Todo endpoint DEVE ter validação consistente e retornar `ProblemDetails` com status codes HTTP corretos.

DEVE existir logging estruturado.

Apenas testes unitários são permitidos, salvo quando uma spec aprovada justificar explicitamente outro tipo de teste no plano e nas tarefas.

### VI. Idioma e Documentação

Todo conteúdo markdown próprio do repositório DEVE estar escrito em português.

São permitidos termos técnicos, APIs, comandos, namespaces e pacotes em inglês quando apropriado.

É proibido misturar idiomas dentro do mesmo documento.

### VII. Governança e Divergências

Esta constituição prevalece sobre qualquer preferência pessoal ou convenção não normativa.

Caso exista divergência entre convenções locais e o comportamento operacional upstream do spec-kit, o agente DEVE parar, reportar o conflito e aguardar instrução humana explícita.

Toda divergência é resolvida por emenda explícita desta constituição, e não por edição silenciosa de scripts ou realocação unilateral de arquivos.

## Estrutura do Repositório

As specs residem na raiz do repositório:

* `specs/NNN-nome/spec.md`

* `specs/NNN-nome/plan.md`

* `specs/NNN-nome/tasks.md`

Não é permitido armazenar specs funcionais dentro de `.specify`.

A pasta `.specify` é reservada para a infraestrutura operacional do spec-kit.

## Método de Trabalho

Cada iniciativa DEVE conter exatamente três arquivos:

* `spec.md`

* `plan.md`

* `tasks.md`

Antes de implementar:

1. Ler `spec.md`.

2. Ler `plan.md`.

3. Ler `tasks.md`.

4. Identificar a tarefa específica.

5. Implementar unicamente o escopo aprovado.

6. Verificar o resultado.

7. Marcar a tarefa como `[X]` apenas após verificação.

Regras de sequência obrigatórias:

1. `speckit.clarify` exige `spec.md`.

2. `speckit.plan` exige `spec.md`.

3. `speckit.analyze` exige `spec.md` e `plan.md`.

4. `speckit.tasks` exige `plan.md`.

5. `speckit.implement` exige `tasks.md`.

## Modo Interativo de Perguntas

Os comandos `speckit.specify` e `speckit.clarify` operam em modo interativo obrigatório: apresentam suas perguntas uma a uma e aguardam resposta antes de continuar.

O comando `speckit.plan` opera em modo interativo condicional: lança perguntas apenas se existirem decisões estruturais que afetem todas as specs futuras e que não estejam resolvidas nesta constituição nem na spec vigente.

Quando um comando opera em modo interativo, DEVE seguir este protocolo sem exceção.

Toda pergunta DEVE ser redigida de forma clara, direta e compreensível para humanos, evitando ambiguidades ou jargões desnecessários, permitindo que a opção correta seja escolhida com segurança.

### Formato de pergunta com opções

Pergunta \[N de TOTAL\] - \[tema curto\]

\[Enunciado claro, concreto e compreensível da pergunta\]

Por que importa: \[1 linha sobre o impacto de decidir incorretamente\]

A) \[opção concreta com valor específico\]

B) \[opção concreta com valor específico\]  Recomendado

C) \[opção concreta com valor específico\]

D) Outro - escreva sua resposta

Responda com a letra (A, B, C ou D) ou escreva sua resposta livre.

### Formato de pergunta Sim/Não

Pergunta \[N de TOTAL\] - \[tema curto\]

\[Enunciado claro e compreensível da pergunta\]

Por que importa: \[1 linha\]

S) Sim  Recomendado

N) Não

Responda S ou N.

### Regras das opções

Cada opção A, B ou C DEVE ser concreta e executável, nunca genérica.

Exemplo correto: 1024px (tablet landscape).

Exemplo proibido: Um breakpoint padrão.

As opções DEVEM ser mutuamente exclusivas: cada uma conduz a um resultado de código diferente.

A opção marcada como Recomendado DEBE ser a mais adotada por equipes que utilizam esta stack ou a que melhor respeita os princípios desta constituição.

A opção D) Outro SEMPRE deve estar presente como alternativa de escape (*escape hatch*) para resposta personalizada.

### Regras de resposta

Se o usuário responder com uma letra (A, B, C, S ou N), o agente DEVE confirmar a escolha em uma única linha com o valor concreto selecionado e passar imediatamente para a pergunta seguinte.

Se o usuário responder com texto livre ou escolher D, o agente DEVE aceitar a resposta, confirmá-la em uma linha e passar imediatamente para a próxima pergunta.

Ao concluir todas as perguntas, o agente DEVE exibir um resumo das decisões tomadas e gerar o artefato correspondente: `spec.md`, seção de esclarecimentos em `spec.md`, ou `plan.md`.

## Ciclo de estado de specs

Toda spec em `specs/` DEVE manter um estado canônico e rastreável durante o ciclo de vida da iniciativa.

Os estados válidos são:

* `Rascunho`: estado inicial ao criar a spec.
* `Aprovada`: a spec foi revisada e aceita para execução.
* `Em implementação`: a implementação foi iniciada e a spec está em execução.
* `Implementada`: a implementação foi concluída e validada.

A transição entre estados DEVE ocorrer automaticamente no fluxo do Spec Kit e NÃO por edição manual ad hoc do documento de spec, salvo quando o próprio comando de criação ou execução do fluxo exigir a atualização do campo de status.

As transições permitidas são exclusivamente:

1. `Rascunho` -> `Aprovada`
2. `Aprovada` -> `Em implementação`
3. `Em implementação` -> `Implementada`

Qualquer outra transição é proibida. Se uma ação tentar avançar para um estado inválido, o agente DEVE manter a spec em `Em implementação` e reportar o bloqueio com a causa explícita.

A spec SOMENTE pode ser marcada como `Implementada` quando todas as condições abaixo forem atendidas:

* existe ao menos um arquivo `tasks.md` para a feature;
* todas as tarefas listadas em `tasks.md` estão marcadas como concluídas;
* existe evidência de validação em `quickstart.md` ou em documentação equivalente de execução e verificação;
* a execução foi validada conforme os critérios do plano e da spec.

Se alguma dessas condições falhar, a spec DEVE permanecer em `Em implementação` e o agente DEVE relatar claramente o motivo do bloqueio antes de concluir a execução.

A mudança automática de estado DEVE valer para os comandos de implementação e para a execução das fases do workflow, de modo que o estado reflita o progresso real e não um estado manual e inconsistentes.

## Governança

Esta constituição é o documento orientador do projeto e tem precedência sobre qualquer outra prática, convenção ou preferência individual.

As emendas DEVEM ser documentadas com versionamento semântico (SemVer):

* MAJOR: remoção ou redefinição incompatível de um princípio ou da stack obrigatória.

* MINOR: novo princípio ou seção adicionada; expansão material de um princípio existente.

* PATCH: esclarecimentos, correções de redação ou ajustes sem alteração semântica.

Todo PR ou code review DEVE verificar a conformidade com esta constituição antes da aprovação.

Qualquer violação DEVE ser justificada explicitamente no plano da respectiva iniciativa. Na ausência de justificativa válida, o PR DEVE ser rejeitado.

## Precedência diante de Conflitos com o Upstream Spec-Kit

Quando um script, hook, template ou estado ativo de feature (*active state*) fornecido pelo upstream do spec-kit entrar em conflito com esta constituição em relação a caminhos físicos de artefatos, localização de specs, nomes de pastas ou estrutura operacional, o agente DEVE parar imediatamente e encaminhar a decisão para um humano.

É proibido harmonizar o conflito movendo artefatos para caminhos não canônicos ou aplicando patches em scripts upstream para atender a convenções locais sem aprovação explícita.

## Protocolo do Agente diante de Divergências

Se um agente detectar divergência entre um script do spec-kit e esta constituição, DEVE:

1. Parar imediatamente e não executar o comando que provocaria a divergência.

2. Reportar ao humano a natureza exata do conflito.

3. Aguardar instrução explícita antes de continuar.

É proibido ao agente mover arquivos por iniciativa própria, renomear pastas ou editar scripts e templates upstream para contornar o conflito.

A única resolução legítima é a decisão humana documentada via emenda constitucional ou ajuste explícito de convenção local.


**Versão**: 1.1.0 | **Ratificada**: 2026-10-05 | **Última emenda**: 2026-10-09