---
name: domain-events
description: Implementa eventos e handlers desacoplados para side effects reais aprovados pela spec ou pelo plano.
---

# Skill: Domain Events

## Objetivo

Desacoplar efeitos colaterais reais do caso de uso principal sem introduzir eventos desnecessários.

## Quando usar

Usar apenas quando a spec ou o plano justificarem:

- um ou múltiplos side effects
- múltiplos consumidores potenciais
- desacoplamento entre a ação principal e as reações
- publicação posterior a uma operação principal bem-sucedida

## Fluxo

```text
Handler principal
    ↓
Persistência bem-sucedida
    ↓
Evento
    ↓
Event Handler(s)
```

## Regras

- o handler principal não deve conhecer todos os consumidores
- o evento deve representar um fato já ocorrido
- os handlers de evento devem ter uma responsabilidade clara
- propagar `CancellationToken`
- registrar logging estruturado quando cabível
- definir explicitamente a estratégia de confiabilidade se for necessária entrega garantida

## Não usar quando

- existir apenas uma chamada direta simples
- não houver necessidade de desacoplamento
- o objetivo for apenas antecipar um possível requisito futuro
- o comportamento puder ser resolvido com clareza dentro do caso de uso

## Confiabilidade

Se o evento precisar sobreviver a falhas de processo ou garantir entrega após o commit, a spec e o plano devem definir uma estratégia explícita — por exemplo, um mecanismo transacional/outbox aprovado.

Não assumir confiabilidade distribuída automaticamente.

## Checklist

- [ ] O evento está justificado pela spec ou plano.
- [ ] Representa um fato real ocorrido.
- [ ] Existe uma necessidade real de desacoplamento.
- [ ] O handler principal não conhece consumidores concretos.
- [ ] A ordem de execução em relação à persistência está definida.
- [ ] O nível de confiabilidade requerido está explicitamente definido.