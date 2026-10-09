---
name: constitution-update-spec-status
description: Implementa uma política obrigatória de ciclo de estados para specs e garante que a mudança de estado ocorra automaticamente no fluxo Speckit.
agent: speckit.constitution
---

/speckit.constitution

### Objetivo
Implementa uma política obrigatória de ciclo de estados para specs e garante que a mudança de estado ocorra automaticamente no fluxo Speckit.

### Escopo
1. Atualiza a constituição em constitution.md para adicionar uma seção chamada Ciclo de estado de specs.
2. Ajusta o fluxo de implementação para que o estado da spec mude automaticamente durante a execução.
3. Mantém todo o conteúdo markdown em português.

### Estados Canônicos
- Rascunho: ao criar a spec.
- Aprovada: ao aprovar a revisão da spec.
- Em implementação: ao iniciar a implementação.
- Implementada: ao finalizar a implementação com validações concluídas.

### Regras Obrigatórias
1. Apenas estas transições são permitidas:
- Rascunho - Aprovada
- Aprovada - Em implementação
- Em implementação - Implementada
2. É proibido marcar uma spec como Implementada se:
- Existir ao menos uma tarefa não concluída em tasks.md
- Não existir evidência de validação em quickstart.md
3. Se alguma condição falhar, manter o estado Em implementação e relatar o bloqueio com causa explícita.

### Alterações Técnicas Necessárias
1. Manter o estado inicial no modelo de spec em spec-template.md
2. Estender a lógica de implementação em speckit.implement.agent.md para:
- Mudar para Em implementação no início
- Mudar para Implementada ao final apenas se as condições forem atendidas
3. Se o projeto utilizar hooks em extensões, adicionar ou ajustar os hooks before_implement e after_implement para garantir que a transição seja automática.

### Governança E Versionamento
- Classificar esta alteração constitucional como MINOR por adicionar uma nova regra de governança operacional.
- Atualizar a versão e a data da emenda na constituição.

### Critérios De Aceitação
1. O estado muda automaticamente sem edição manual.
2. Não é possível concluir a implementação com estado inválido.
3. O fluxo mantém rastreabilidade clara sobre o motivo da alteração (ou não) do estado.