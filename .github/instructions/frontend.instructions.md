# Instruções de frontend

Estas instruções aplicam-se a todo o trabalho de frontend do projeto.

O frontend DEVE ser implementado com:

- Blazor Web App.
- Razor Components.
- CSS próprio centralizado em `wwwroot/app.css`.
- Refit para comunicação com o backend.
- Sistema visual canônico definido em `wwwroot/app.css`.
- Skill visual personalizada para gerar e manter o design base.
- Lucide Icons como sistema principal de ícones.

É proibido usar frameworks CSS como Bootstrap CSS, Tailwind, Bulma ou similares.

É proibido usar Bootstrap Icons como fonte principal de ícones.

---

## Relação com as especificações

Toda tela, componente, fluxo, validação visual ou alteração de UI DEVE estar descrito na especificação vigente.

É proibido implementar UI não descrita em `spec.md`.

Antes de modificar o frontend:

1. Ler `spec.md`.
2. Ler `plan.md`.
3. Ler `tasks.md`.
4. Identificar a tarefa exata.
5. Implementar apenas o que foi solicitado.
6. Usar apenas estilos existentes ou tokens definidos em `wwwroot/app.css`.
7. Usar Lucide Icons quando a UI exigir ícones.
8. Marcar a tarefa como `[X]` apenas quando estiver concluída e verificada.

Se a alteração visual não tiver uma tarefa em `tasks.md`, a implementação DEVE ser interrompida.

## Sistema visual do frontend

O sistema visual base do frontend DEVE ser gerado e mantido seguindo a skill:

```text
.github/skills/blazor-app-css-design-system/SKILL.md
```

O arquivo canônico de estilos é:

```text
wwwroot/app.css
```

Se `wwwroot/app.css` ainda não existir, ele DEVE ser criado apenas quando uma especificação aprovada o solicitar e houver uma tarefa correspondente em `tasks.md`.

É proibido criar componentes visuais com estilos *inline* enquanto `wwwroot/app.css` não existir.

É proibido definir cores, espaçamentos, bordas, sombras ou padrões visuais diretamente dentro de componentes Razor.

---

## Sistema de ícones

O sistema principal de ícones do frontend DEVE ser o Lucide Icons.

O Lucide Icons DEVE ser usado para:

- navegação principal
- ações de botões
- estados visuais
- cards
- formulários
- mensagens informativas
- dashboards
- elementos de gerenciamento

Os ícones DEVEM ser renderizados como SVG ou componentes Razor.

Os ícones DEVEM herdar a cor através de `currentColor` sempre que possível.

Os ícones DEVEM usar classes CSS definidas em `wwwroot/app.css`.

É proibido definir cores, tamanhos ou estilos de ícones diretamente no código (*hardcoding*) dentro de componentes Razor.

É proibido misturar Lucide Icons, Bootstrap Icons, Fluent Icons, Font Awesome ou outras bibliotecas na mesma interface sem uma especificação aprovada.

---

O Bootstrap Icons só pode ser usado como exceção temporária se uma especificação o justificar explicitamente.

### Instalação recomendada

Quando a especificação exigir a habilitação de ícones no Blazor, recomenda-se usar uma integração compatível com o Lucide Icons para Blazor — por exemplo, um pacote de componentes Razor baseado no Lucide.

A instalação exata do pacote DEVE estar descrita em `plan.md` e mapeada em `tasks.md`.

Exemplo de tarefa esperada:

```md
- [ ] Instalar e registrar a biblioteca Lucide Icons para Blazor.
- [ ] Criar classes CSS reutilizáveis ​​para ícones em `wwwroot/app.css`.
- [ ] Substituir qualquer ícone temporário pelo Lucide Icons.
```

### Classes CSS esperadas para ícones

O arquivo `wwwroot/app.css` DEVE conter classes reutilizáveis ​​para ícones.

Exemplo:

```css
.icon {
  width: 1.25rem;
  height: 1.25rem;
  color: currentColor;
  flex-shrink: 0;
}

.icon-sm {
  width: 1rem;
  height: 1rem;
}

.icon-md {
  width: 1.25rem;
  height: 1.25rem;
}

.icon-lg {
  width: 1.5rem;
  height: 1.5rem;
}

.icon-muted {
  color: var(--color-text-muted);
}

.icon-primary {
  color: var(--color-primary);
}

.icon-success {
  color: var(--color-success);
}

.icon-warning {
  color: var(--color-warning);
}

.icon-danger {
  color: var(--color-danger);
}
```

### Uso esperado em componentes Razor

Exemplo conceitual correto:

```razor
<button class="btn btn-primary">
    <LucideIcon Name="Plus" class="icon icon-sm" />
    Nova propriedade
</button>
```

Exemplo proibido:

```razor
<i class="bi bi-plus" style="color: #2563eb; font-size: 20px;"></i>
```

---

## Estilo visual pretendido

A aplicação DEVE ter uma aparência moderna, limpa e profissional, inspirada visualmente em:

- Notion.
- Google Keep.
- Dashboards administrativos contemporâneos.
- Painéis modernos de gestão imobiliária.

A interface deve transmitir:

- clareza
- organização
- amplitude visual
- simplicidade
- foco em cartões de conteúdo
- navegação lateral clara

Não se deve criar uma interface carregada, escura, saturada ou com excesso de bordas.

---

## Layout principal

A aplicação DEVE usar uma composição *desktop-first*.

No desktop, a estrutura principal deve ser:

```text
barra lateral fixa à esquerda + conteúdo principal amplo à direita
```

A barra lateral DEVE permanecer fixa ou visualmente estável enquanto o usuário navega pela aplicação.

O conteúdo principal DEVE ter espaço horizontal suficiente para exibir grades, formulários, painéis de gestão e cartões.

Classes esperadas em `wwwroot/app.css`:

```css
.app-shell {}
.app-sidebar {}
.app-main {}
.app-content {}
.page-container {}
.page-header {}
.page-title {}
.page-subtitle {}
.page-actions {}
```

---

## Barra lateral

A barra lateral DEVE:

- estar localizada à esquerda no desktop
- ter largura fixa
- usar fundo claro ou com leve contraste
- exibir navegação vertical
- ter estados *hover* e *active*
- manter separação visual em relação ao conteúdo principal
- usar Lucide Icons para os itens de navegação quando ícones forem necessários

Classes esperadas:

```css
.app-sidebar {}
.sidebar-brand {}
.sidebar-nav {}
.sidebar-link {}
.sidebar-link-active {}
```
Em dispositivos móveis, a barra lateral pode se transformar em navegação superior, *drawer* ou bloco recolhível, desde que a especificação permita.

---

## Conteúdo principal

O conteúdo principal DEVE:

- ocupar o espaço restante disponível
- ter *padding* amplo
- usar um fundo geral suave
- permitir *layouts* de *dashboard* e grades (*grids*)
- evitar que os elementos fiquem colados às bordas

Classes esperadas:

```css
.app-main {}
.app-content {}
.page-container {}
.page-header {}
.page-actions {}
```

---

## Componentes Razor

Os componentes Razor devem ser pequenos, claros e orientados a uma responsabilidade única.

Evitar componentes grandes com lógica excessiva.

A lógica de negócios NÃO deve residir em componentes de interface (UI).

Os componentes podem conter:

- lógica de apresentação
- estado visual local
- chamadas para serviços de aplicação *frontend*
- validações visuais simples
- composição de componentes filhos

Não devem conter:

- regras de negócios complexas
- lógica de persistência
- acesso direto ao `HttpClient`
- estilos *inline*
- cores definidas diretamente no código (*hardcoded*)
- ícones com estilos definidos diretamente no código
- contratos duplicados manualmente sem especificação

Exemplo proibido:

```razor
<button style="background-color: #2563eb; padding: 12px;">
    Salvar
</button>
```

Exemplo correto:

```razor
<button class="btn btn-primary">
    Salvar
</button>
```

---

## Comunicação com o backend

O frontend DEVE consumir o backend utilizando o Refit.

Utilizar interfaces tipadas por módulo ou funcionalidade (*feature*).

Exemplo:

```csharp
public interface IPropertiesApi
{
    [Get("/api/properties")]
    Task<IReadOnlyList<PropertySummaryResponse>> GetPropertiesAsync();
}
```

É proibido utilizar `HttpClient` diretamente em componentes Razor.

É proibido utilizar `RestService.For<T>()` fora do registro com `IHttpClientFactory`.

As interfaces do Refit devem estar organizadas por módulo ou funcionalidade, e não em um único arquivo global enorme.

---

## CSS

Todo o CSS do projeto deve estar centralizado em:

```text
wwwroot/app.css
```

O arquivo `wwwroot/app.css` é a fonte única de verdade para:

- cores
- tipografias
- espaçamentos
- bordas
- sombras
- tamanhos
- layout base
- barra lateral (*sidebar*)
- botões
- formulários
- cards
- tabelas
- badges
- ícones
- estados visuais
- utilitários reutilizáveis
- grades responsivas

É proibido criar arquivos CSS adicionais por componente, exceto se uma especificação aprovada indicar isso explicitamente.

É proibido definir estilos *inline* dentro de componentes Razor.

É proibido utilizar cores fixas (*hardcoded*) em arquivos `.razor`, `.cs` ou novos arquivos CSS.

Exemplos proibidos:

```css
color: #2563eb;
background: blue;
border-radius: 10px;
margin: 17px;
```

Exemplo correto:

```css
color: var(--color-primary);
background: var(--color-surface);
border-radius: var(--radius-md);
margin: var(--space-4);
```

---

## Tokens visuais obrigatórios

O arquivo `wwwroot/app.css` DEVE conter tokens CSS dentro de `:root`.

Os componentes NÃO devem depender de valores fixos (hardcoded).

### Cores base

```css
:root {
  --color-bg: #f8fafc;
  --color-surface: #ffffff;
  --color-surface-muted: #f1f5f9;

  --color-text: #0f172a;
  --color-text-muted: #64748b;

  --color-primary: #2563eb;
  --color-primary-hover: #1d4ed8;

  --color-success: #16a34a;
  --color-warning: #d97706;
  --color-danger: #dc2626;

  --border-color: #e2e8f0;
  --color-focus: #93c5fd;

  --color-sidebar: #ffffff;
}
```

### Espaçamento

```css
:root {
  --space-1: 0.25rem;
  --space-2: 0.5rem;
  --space-3: 0.75rem;
  --space-4: 1rem;
  --space-6: 1.5rem;
  --space-8: 2rem;
  --space-12: 3rem;
}
```

### Bordas

```css
:root {
  --radius-sm: 0.375rem;
  --radius-md: 0.5rem;
  --radius-lg: 0.75rem;
  --radius-xl: 1rem;
}
```

### Sombras

```css
:root {
  --shadow-sm: 0 1px 2px rgb(15 23 42 / 0.08);
  --shadow-md: 0 8px 24px rgb(15 23 42 / 0.10);
}
```

### Layout

```css
:root {
  --sidebar-width: 260px;
  --content-max-width: 1440px;
}
```

---

## Classes reutilizáveis ​​obrigatórias

O desenvolvedor DEVE priorizar classes reutilizáveis ​​existentes antes de criar novas classes.

O arquivo `wwwroot/app.css` deve conter classes base para padrões comuns.

### Layout

```css
.app-shell {}
.app-sidebar {}
.app-main {}
.app-content {}
.page-container {}
.page-header {}
.page-title {}
.page-subtitle {}
.page-actions {}
```

### Botões

```css
.btn {}
.btn-primary {}
.btn-secondary {}
.btn-danger {}
.btn-ghost {}
```

Os botões devem utilizar tokens visuais, e não valores fixos (hardcoded).

### Cards

```css
.card {}
.card-header {}
.card-body {}
.card-footer {}
.property-card {}
```

Os cards DEVEM ter:

- fundo branco
- bordas arredondadas
- sombras suaves
- borda sutil
- espaçamento interno (padding) consistente
- separação visual clara entre os cards

Os cards NÃO devem utilizar sombras agressivas, cores saturadas ou bordas pesadas. ### Formulários

```css
.form {}
.form-group {}
.form-label {}
.form-control {}
.form-error {}
.form-actions {}
```

### Estados de tela

```css
.loading-state {}
.empty-state {}
.error-state {}
.success-state {}
```

---

## Grade de propriedades

Na página de propriedades, `properties.html` ou seu equivalente Razor, a grade DEVE exibir 3 cards por linha em desktop.

A classe principal recomendada é:

```css
.properties-grid {}
```

Regra obrigatória para desktop:

```css
.properties-grid {
  display: grid;
  grid-template-columns: repeat(3, minmax(0, 1fr));
  gap: var(--space-6);
}
```

Em telas médias, pode ser reduzido para 2 colunas.

Em telas de dispositivos móveis, deve ser reduzido para 1 coluna.

Exemplo esperado:

```css
.properties-grid {
  display: grid;
  grid-template-columns: repeat(3, minmax(0, 1fr));
  gap: var(--space-6);
}

@media (max-width: 1024px) {
  .properties-grid {
    grid-template-columns: repeat(2, minmax(0, 1fr));
  }
}

@media (max-width: 640px) {
  .properties-grid {
    grid-template-columns: 1fr;
  }
}
```

---

## Regras para novos componentes visuais

Quando uma especificação exigir um novo componente visual, o agente DEVE seguir esta ordem:

1. Verificar se já existe uma classe reutilizável em `wwwroot/app.css`.
2. Reutilizar a classe existente, se aplicável.
3. Se não existir, criar uma nova classe em `wwwroot/app.css`.
4. A nova classe DEVE usar tokens existentes.
5. O componente Razor DEVE consumir a classe, não definir estilos inline.
6. Se o componente exigir ícones, DEVE usar Lucide Icons.
7. A criação da classe DEVE estar registrada em `tasks.md`.

Exemplo correto:

```razor
<section class="property-card">
    <h3 class="property-card-title">@Property.Name</h3>
    <p class="property-card-meta">@Property.Address</p>
</section>
```

Exemplo de CSS correto:

```css
.property-card {
  background: var(--color-surface);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-lg);
  box-shadow: var(--shadow-sm);
  padding: var(--space-4);
``` }

.property-card-title {
  color: var(--color-text);
  margin: 0 0 var(--space-2);
}

.property-card-meta {
  color: var(--color-text-muted);
  margin: 0;
}
```

---

## Design responsivo

A composição segue a abordagem *desktop-first*.

No desktop:

- barra lateral fixa à esquerda
- conteúdo principal amplo
- grade de propriedades com 3 colunas

No tablet:

- a barra lateral pode permanecer compacta
- as grades podem passar para 2 colunas

No mobile:

- a barra lateral pode se transformar em navegação superior, *drawer* ou bloco recolhido
- o conteúdo principal deve usar uma única coluna
- os cards devem ocupar toda a largura disponível

---

## Acessibilidade

Toda a interface (UI) deve considerar:

- contraste suficiente
- rótulos (*labels*) visíveis ou acessíveis
- navegação por teclado
- estados de foco (*focus*)
- mensagens de erro claras
- botões com texto compreensível
- estrutura semântica HTML correta
- ícones decorativos marcados como não relevantes para leitores de tela, quando aplicável
- ícones informativos acompanhados de texto visível ou texto acessível

Não utilize a cor como único indicador de estado.

Exemplo incorreto:

```razor
<span class="status-dot status-danger"></span>
```

Exemplo correto:

```razor
<span class="badge badge-danger">
    Erro de validação
</span>
```

---

## Formulários

Os formulários devem:

- exibir erros próximos ao campo correspondente
- evitar validações conflitantes com o backend
- utilizar mensagens claras
- desabilitar ações durante o envio, quando aplicável
- exibir feedback de sucesso ou erro
- utilizar classes reutilizáveis ​​definidas em `wwwroot/app.css`

Os formulários devem priorizar classes como:

```text
.form
.form-group
.form-label
.form-control
.form-error
.form-actions
```

---

## Estados de tela

Toda tela que consuma dados deve considerar:

- carregamento (loading)
- estado vazio (empty state)
- estado de erro
- estado de sucesso
- estado com dados

Não crie telas que funcionem apenas no cenário ideal (*happy path*).

Exemplo de estados esperados:

```razor
@if (isLoading)
{
    <div class="loading-state">Carregando propriedades...</div>
}
else if (hasError)
{
    <div class="error-state">Não foi possível carregar as propriedades.</div>
}
else if (!properties.Any())
{
    <div class="empty-state">Não há propriedades cadastradas.</div>
}
else
{
    <PropertyList Items="properties" />
}
```

---

## Restrições visuais

É proibido:

- Usar Bootstrap CSS.
- Usar Bootstrap Icons como sistema principal de ícones.
- Usar Tailwind.
- Usar estilos inline.
- Usar cores hardcoded (fixas no código).
- Criar CSS não mapeado em `tasks.md`.
- Alterar tokens visuais sem especificação aprovada.
- Criar variantes visuais por preferência estética.
- Duplicar estilos existentes com novos nomes.
- Introduzir bibliotecas de UI sem aprovação na especificação.
- Misturar múltiplas bibliotecas de ícones sem especificação aprovada.
- Colocar lógica de negócio em componentes de UI.
- Criar uma grade de propriedades que não respeite o layout de 3 cards por linha em desktop.

---

## Checklist antes de finalizar uma tarefa de frontend

Validar:

- A UI está descrita em `spec.md`.
- A tarefa existe em `tasks.md`.
- Nenhum framework CSS foi adicionado.
- O `HttpClient` não foi usado diretamente.
- O Refit foi usado para consumir o backend.
- Lucide Icons foram usados ​​para ícones, quando aplicável.
- Bootstrap Icons não foram usados ​​como sistema principal.
- Não há estilos inline.
- Não há cores hardcoded.
- Os ícones usam classes CSS e `currentColor`, quando aplicável.
- Os tokens visuais de `wwwroot/app.css` foram respeitados.
- Classes existentes foram reutilizadas sempre que possível.
- Toda classe nova foi adicionada a `wwwroot/app.css`.
- A aplicação possui barra lateral esquerda em desktop, quando aplicável.
- O conteúdo principal é amplo e legível.
- Os cards são brancos, arredondados e possuem sombra suave.
- A grade de propriedades usa 3 cards por linha em desktop.
- A grade reduz para 2 colunas em tablets.
- A grade reduz para 1 coluna em dispositivos móveis.
- A tela contempla estados de carregamento, vazio e erro, quando aplicável.
- Não há lógica de negócio dentro do componente.
- O projeto compila.