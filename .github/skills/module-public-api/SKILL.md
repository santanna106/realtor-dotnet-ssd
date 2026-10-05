---
name: module-public-api
description: Define contratos públicos para comunicação entre módulos quando existem limites modulares reais.
---

# Skill: Module Public API

## Status

Skill condicional. Não usar em uma aplicação sem módulos funcionais explícitos.

## Objetivo

Evitar acesso direto a internals de outro módulo e manter contratos estáveis de comunicação.

## Quando usar

Apenas quando a spec ou o plano aprovarem limites modulares reais.

## Regras

Um módulo NÃO deve acessar diretamente:

- entidades internas de outro módulo
- seu DbContext interno
- handlers internos
- serviços internos
- infraestrutura interna

A comunicação deve utilizar:

```text
PublicApi
```

ou:

```text
Events
```

`PublicApi` pode expor:

- interfaces
- requests
- responses
- resultados públicos
- contratos necessários entre módulos

## Proibições

- criar projetos PublicApi de forma especulativa
- compartilhar entidades internas
- resolver o DbContext de outro módulo
- invocar handlers internos diretamente
- vazar infraestrutura interna para consumidores externos

## Checklist

- [ ] Existem limites modulares reais aprovados.
- [ ] O contrato público é mínimo.
- [ ] Não expõe entidades internas.
- [ ] Não expõe DbContext nem infraestrutura.
- [ ] Não foi criada infraestrutura especulativa.