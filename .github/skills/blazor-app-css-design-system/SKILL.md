# Skill: Blazor App CSS Design System

## When to use this skill

Use this skill when a specification requires creating or modifying the Blazor frontend's visual system, specifically the following file:

```text
wwwroot/app.css
```

This skill applies to:

- the main application layout
- the fixed sidebar
- main content
- cards
- grids
- forms
- buttons
- visual states
- responsive adaptation
- pages such as `properties.html` or equivalent Razor components

## Main rule

The `wwwroot/app.css` file is the technical source of truth for the frontend's visual system.

Creating inline styles within Razor components is prohibited.

Using CSS frameworks such as Bootstrap CSS, Tailwind, Bulma, or similar is prohibited.

The primary icon system MUST be Lucide Icons. Using Bootstrap Icons, Font Awesome, Fluent Icons, or other icon libraries as the primary source is prohibited. Icons MUST be rendered as SVGs or Razor components, inherit color via `currentColor`, and use icon CSS classes defined in `wwwroot/app.css`.

Every new class MUST be traced to a task in `tasks.md`.

## Target visual style

The application MUST have a modern, clean, and professional appearance, visually inspired by:

- Notion
- Google Keep
- contemporary administrative dashboards
- modern real estate management panels

The interface must convey:

- clarity
- order
- visual spaciousness
- simplicity
- a focus on content cards
- clear side navigation

Avoid creating an interface that is cluttered, dark, saturated, or overloaded with borders.

## Main Layout

The application MUST use a desktop-first layout approach.

On desktop, the main structure must be:

```text
fixed sidebar on the left + spacious main content area on the right
```

The sidebar MUST remain fixed or visually stable while the user navigates the application.

The main content area MUST have sufficient horizontal space to display grids, forms, and management panels.

Expected classes:

```css
.app-shell
.app-sidebar
.app-main
.app-content
.page-container
.page-header
.page-title
.page-subtitle
```

## Sidebar

The sidebar MUST:

- be positioned on the left on desktop
- have a fixed width
- use a light or slightly contrasting background
- display vertical navigation
- include hover and active states
- maintain visual separation from the main content

Expected classes:

```css
.app-sidebar
.sidebar-brand
.sidebar-nav
.sidebar-link
.sidebar-link-active
```

## Main Content

The main content MUST:

- occupy the remaining available space
- have ample padding
- use a soft, neutral background
- accommodate dashboard and grid layouts
- prevent elements from touching the edges

Expected classes:

```css
.app-main
.app-content
.page-container
.page-header
.page-actions
```

## Cards

Cards MUST have:

- white background
- rounded corners
- soft shadows
- subtle border
- consistent padding
- clear visual separation between cards

Expected classes:

```css
.card
.card-header
.card-body
.card-footer
.property-card
```

Cards must NOT use harsh shadows, saturated colors, or heavy borders.

## Properties grid

On the properties page (`properties.html` or its Razor equivalent), the grid MUST display 3 cards per row on desktop.

The recommended main class is:

```css
.properties-grid
```

Mandatory rule for desktop:

```css
.properties-grid {
  display: grid;
  grid-template-columns: repeat(3, minmax(0, 1fr));
}
```

On medium screens, it may be reduced to 2 columns.

On mobile screens, it must be reduced to 1 column.

Expected example:

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

## Responsive design

The layout follows a desktop-first approach.

On desktop:

- fixed sidebar on the left
- spacious main content area
- 3-column property grid

On tablet:

- sidebar can remain compact
- grids can switch to 2 columns

On mobile:

- sidebar can become a top navigation bar, a drawer, or a collapsed block
- main content must use a single column
- cards must span the full available width

## Required tokens

The `wwwroot/app.css` file MUST define tokens within `:root` for:

- colors
- text
- backgrounds
- borders
- shadows
- spacing
- border radii
- transitions
- sidebar width
- maximum content width

Base example:

```css
:root {
  --color-bg: #f8fafc;
  --color-surface: #ffffff;
  --color-surface-muted: #f1f5f9;

  --color-text: #0f172a;
  --color-text-muted: #64748b;

  --color-primary: #2563eb;
  --color-primary-hover: #1d4ed8;

  --color-border: #e2e8f0;
  --color-sidebar: #ffffff;

  --space-2: 0.5rem;
  --space-3: 0.75rem;
  --space-4: 1rem;
  --space-6: 1.5rem;
  --space-8: 2rem;

  --radius-md: 0.5rem;
  --radius-lg: 0.75rem;
  --radius-xl: 1rem;

  --shadow-sm: 0 1px 2px rgb(15 23 42 / 0.08);
  --shadow-md: 0 8px 24px rgb(15 23 42 / 0.10);

  --sidebar-width: 260px;
  --content-max-width: 1440px;
}
```

## Processo obrigatório

Quando uma especificação exigir a criação do sistema visual base:

1. Confirmar se existe uma tarefa em `tasks.md`.
2. Criar ou atualizar `wwwroot/app.css`.
3. Definir tokens visuais em `:root`.
4. Criar o layout principal com barra lateral e conteúdo.
5. Criar classes base para cards.
6. Criar classes base para botões.
7. Criar classes base para formulários.
8. Criar classes base para estados de tela.
9. Criar a grade `.properties-grid` com 3 colunas em desktop.
10. Validar a adaptação para tablet e dispositivos móveis.
11. Confirmar se os componentes Razor utilizam classes CSS e não estilos inline.
12. Marcar a tarefa como `[X]` apenas quando o resultado estiver verificado.

## Proibições

É proibido:

- Usar estilos inline.
- Usar cores definidas diretamente no código (hardcoded) dentro de componentes Razor.
- Criar CSS fora de `wwwroot/app.css` sem uma especificação aprovada.
- Alterar tokens com base em critérios estéticos pessoais.
- Usar Bootstrap CSS.
- Usar Tailwind.
- Usar Bootstrap Icons, Font Awesome, Fluent Icons ou qualquer outra biblioteca de ícones que não seja Lucide Icons como fonte principal.
- Definir diretamente no código (hardcode) cores, tamanhos ou estilos de ícones dentro de componentes Razor.
- Criar uma grade de propriedades que não respeite o padrão de 3 cards por linha em desktop.
- Criar componentes visuais sem uma tarefa correspondente em `tasks.md`.
- Implementar alterações visuais não descritas em `spec.md`.

## Checklist de revisão

Antes de finalizar uma tarefa visual, validar:

- [ ] Existe uma tarefa em `tasks.md`.
- [ ] O arquivo `wwwroot/app.css` existe.
- [ ] Os tokens base estão definidos em `:root`.
- [ ] A aplicação possui uma barra lateral esquerda em desktop.
- [ ] O conteúdo principal é amplo e legível. - [ ] Os cards são brancos, com cantos arredondados e sombra suave.
- [ ] A grade de propriedades utiliza 3 cards por linha em desktops.
- [ ] A grade passa para 2 colunas em tablets.
- [ ] A grade passa para 1 coluna em dispositivos móveis.
- [ ] Não há estilos inline.
- [ ] Não há cores definidas diretamente no código (hardcoded) nos componentes Razor.
- [ ] Nenhum framework CSS foi adicionado.
