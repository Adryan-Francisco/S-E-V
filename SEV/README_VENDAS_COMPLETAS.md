# Sistema de Vendas Unificado - SEV

## Visão Geral

Este sistema unifica as funcionalidades de **Venda** e **ItemVenda** em uma única interface, proporcionando uma experiência mais fluida e eficiente para o usuário.

## 🚀 Principais Melhorias Implementadas

### 1. **Interface Unificada**
- **Antes**: Duas páginas separadas (Venda + ItemVenda)
- **Agora**: Uma única página que combina ambas as funcionalidades
- Interface mais intuitiva e menos confusa para o usuário

### 2. **Criação de Vendas em Tempo Real**
- Adição dinâmica de itens à venda
- Cálculo automático de subtotais e total geral
- Validação em tempo real de estoque disponível
- Interface responsiva com cards organizados

### 3. **Gestão Inteligente de Estoque**
- Verificação automática de disponibilidade
- Atualização automática do estoque ao finalizar venda
- Devolução automática ao estoque em caso de exclusão/edição

### 4. **Experiência do Usuário Aprimorada**
- Design moderno com Bootstrap 5
- Ícones intuitivos (Bootstrap Icons)
- Feedback visual imediato
- Validações em tempo real
- Confirmações para ações críticas

### 5. **Funcionalidades Avançadas**
- **Criação**: Interface para adicionar múltiplos itens
- **Visualização**: Detalhes completos da venda com resumo
- **Edição**: Modificação de vendas existentes
- **Exclusão**: Remoção segura com confirmações
- **Recibo**: Geração de recibo para impressão

## 🏗️ Arquitetura Técnica

### **Novos Arquivos Criados**

#### **Models**
- `VendaCompletaViewModel.cs` - ViewModel unificado para vendas
- `ItemVendaViewModel.cs` - ViewModel para itens individuais

#### **Controllers**
- `VendasCompletasController.cs` - Controlador unificado com todas as operações CRUD

#### **Views**
- `Index.cshtml` - Lista todas as vendas com resumo
- `Create.cshtml` - Criação de vendas com múltiplos itens
- `Details.cshtml` - Visualização detalhada com opções de recibo
- `Edit.cshtml` - Edição de vendas existentes
- `Delete.cshtml` - Confirmação de exclusão com detalhes

### **Funcionalidades do Controlador**

```csharp
// Operações principais
public async Task<IActionResult> Index()           // Listar vendas
public async Task<IActionResult> Create()          // Formulário de criação
public async Task<IActionResult> Create([FromBody] VendaCompletaViewModel viewModel) // Salvar venda
public async Task<IActionResult> Details(int? id)  // Ver detalhes
public async Task<IActionResult> Edit(int? id)     // Formulário de edição
public async Task<IActionResult> Edit(int id, [FromBody] VendaCompletaViewModel viewModel) // Atualizar
public async Task<IActionResult> Delete(int? id)   // Confirmação de exclusão
public async Task<IActionResult> DeleteConfirmed(int id) // Executar exclusão

// Funcionalidades auxiliares
public async Task<IActionResult> GetProdutoInfo(int produtoId) // Info do produto via AJAX
```

## 💡 Características Técnicas

### **Transações de Banco**
- Uso de transações para garantir consistência
- Rollback automático em caso de erro
- Atualização atômica de venda e estoque

### **Validações**
- Validação de estoque disponível
- Verificação de dados obrigatórios
- Validação de quantidades mínimas
- Prevenção de vendas sem itens

### **Interface Responsiva**
- Layout adaptável para diferentes dispositivos
- Cards organizados para melhor visualização
- Formulários intuitivos com feedback visual

### **JavaScript Avançado**
- Manipulação dinâmica do DOM
- Cálculos em tempo real
- Validações client-side
- Comunicação AJAX com o servidor

## 🔄 Fluxo de Trabalho

### **1. Criação de Venda**
```
1. Usuário seleciona cliente e data
2. Adiciona produtos dinamicamente
3. Sistema valida estoque automaticamente
4. Cálculos em tempo real (subtotais, total)
5. Finalização com transação atômica
6. Atualização automática do estoque
```

### **2. Edição de Venda**
```
1. Carregamento dos dados existentes
2. Interface para modificação de itens
3. Validação de estoque atualizado
4. Processamento com rollback em caso de erro
5. Atualização consistente de dados
```

### **3. Exclusão de Venda**
```
1. Confirmação com detalhes da venda
2. Devolução automática ao estoque
3. Remoção segura dos dados
4. Feedback visual para o usuário
```

## 🎨 Interface do Usuário

### **Cores e Estilos**
- **Primário**: Azul (#0d6efd) - Informações principais
- **Sucesso**: Verde (#198754) - Ações positivas
- **Aviso**: Amarelo (#ffc107) - Edições
- **Perigo**: Vermelho (#dc3545) - Exclusões
- **Info**: Azul claro (#0dcaf0) - Resumos

### **Componentes Visuais**
- Cards com sombras suaves
- Badges para identificadores
- Ícones contextuais
- Alertas informativos
- Tabelas responsivas

## 📱 Responsividade

- **Desktop**: Layout em duas colunas (4/8)
- **Tablet**: Layout adaptativo
- **Mobile**: Layout empilhado
- **Componentes**: Tabelas com scroll horizontal

## 🔒 Segurança

- Validação de tokens anti-forgery
- Validação server-side
- Transações de banco seguras
- Confirmações para ações críticas

## 🚀 Como Usar

### **Acesso**
1. Navegue para o menu "Vendas"
2. Clique em "Nova Venda" para criar
3. Use as ações disponíveis para gerenciar vendas existentes

### **Criação de Venda**
1. Preencha cliente e data
2. Adicione produtos clicando em "Adicionar Item"
3. Selecione produto e quantidade
4. Visualize cálculos automáticos
5. Clique em "Finalizar Venda"

### **Gerenciamento**
- **Ver detalhes**: Clique no ícone de olho
- **Editar**: Clique no ícone de lápis
- **Excluir**: Clique no ícone de lixeira

## 🔧 Configuração

### **Dependências**
- ASP.NET Core 6+
- Entity Framework Core
- PostgreSQL
- Bootstrap 5
- jQuery
- Bootstrap Icons

### **Arquivos de Configuração**
- `appsettings.json` - Conexão com banco
- `Program.cs` - Configuração de serviços
- `_Layout.cshtml` - Navegação principal

## 📈 Benefícios

### **Para o Usuário**
- Interface mais intuitiva
- Menos cliques para completar tarefas
- Feedback visual imediato
- Menos chance de erro

### **Para o Sistema**
- Código mais organizado
- Menor duplicação de funcionalidades
- Melhor gestão de transações
- Interface mais moderna

### **Para a Manutenção**
- Código centralizado
- Menos arquivos para gerenciar
- Lógica unificada
- Facilidade de implementar melhorias

## 🔮 Próximas Melhorias

- [ ] Relatórios de vendas
- [ ] Exportação para PDF
- [ ] Integração com sistema de pagamentos
- [ ] Notificações por email
- [ ] Dashboard de vendas em tempo real
- [ ] Sistema de cupons e descontos

---

**Desenvolvido para o Sistema SEV**  
*Versão 2.0 - Sistema de Vendas Unificado*
