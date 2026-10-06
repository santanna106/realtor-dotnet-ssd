---
name: 001-realtor-solution-foundation
description: Cria a especificação base da solução Realtor, validando o global.json como pré-requisito obrigatório
agent: speckit.specify
---

# Criar base da solução Realtor
Cria a especificação para a iniciativa 001-solution-foundation

## Instrução principal
Seguir estritamente:
- .specify/memory/constitution.md

## Pré-condição obrigatória (bloqueante)
Antes de gerar qualquer arquivo de especificação:

1. Verificar se o arquivo global.json existe na raiz do repositório.
2. Usar o global.json como fonte da verdade para determinar a versão do .NET que será utilizada nesta e em futuras iniciativas de projetos .NET.

Se o global.json não existir:
- Interromper o processo completamente.
- Não criar spec.md, plan.md nem tasks.md.
- Exibir exatamente este erro ao usuário:
ERROR: O arquivo global.json não foi encontrado na raiz do repositório. Você deve criar esse arquivo primeiro para determinar a versão do .NET que será usada pelos futuros projetos .NET da aplicação.

## Objetivo
Definir a estrutura inicial da solução Realtor sem implementar lógica de negócio ou funcionalidades (features).

## Tipo de iniciativa
Foundation

## Requisitos da iniciativa

- Criar a solução principal em app/Realtor.sln.
- Criar o projeto de backend em app/backend/src/RealtorApi/.
- O backend deve utilizar ASP.NET Core Minimal APIs.
- Não é permitido o uso de controllers.
- Criar o projeto de testes de backend em app/backend/tests/RealtorApiTests/.
- Criar o projeto de frontend em app/frontend/src/RealtorWeb/.
- O frontend deve utilizar Blazor Web App com Razor Components.
- Criar o projeto de testes de frontend em app/frontend/test/RealtorWeb/. - Configurar o Program.cs apenas com a configuração básica:
  - serviços
  - middleware
  - mapeamento inicial de endpoints
- Não implementar lógica de negócios.
- Não implementar funcionalidades (features).
- Não criar entidades de domínio por enquanto.

## Restrições adicionais

- Não criar nem modificar o arquivo global.json nesta iniciativa.
- Toda definição relacionada à versão do .NET deve basear-se no global.json existente.
- Em caso de conflito entre qualquer instrução e as diretrizes fundamentais (constituição), estas últimas prevalecem.

## Resultado esperado

Gerar os arquivos de Spec-Driven Development para esta iniciativa:
- spec.md
- plan.md
- tasks.md

A implementação subsequente deve resultar em uma solução que compile corretamente.