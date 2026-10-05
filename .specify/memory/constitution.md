<!--
Relatório de impacto:
- Versão: modelo não ratificado -> 1.0.0 (constituição inicial).
- Princípios modificados: não se aplica; estabelecem-se os princípios iniciais.
- Seções adicionadas: Restrições técnicas; Fluxo de desenvolvimento e qualidade.
- Seções removidas: nenhuma.
- Modelos revisados: plan-template.md ✅ sem alterações necessárias; spec-template.md ✅ sem alterações necessárias; tasks-template.md ✅ sem alterações necessárias.
- Diretrizes revisadas: copilot-instructions.md e as instruções de backend, frontend e persistência ✅ coerentes; não requerem alterações.
- Comandos em .specify/templates/commands/: não há arquivos nesse caminho.
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


**Versão**: 1.0.0 | **Ratificada**: 2026-10-05 | **Última emenda**: 2026-10-05