# 🏪 SEV - Sistema de Vendas

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-blue.svg)](https://dotnet.microsoft.com/apps/aspnet)
[![Entity Framework](https://img.shields.io/badge/Entity%20Framework-9.0-green.svg)](https://docs.microsoft.com/en-us/ef/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-15+-blue.svg)](https://www.postgresql.org/)
[![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-blue.svg)](https://getbootstrap.com/)
[![Chart.js](https://img.shields.io/badge/Chart.js-4.0+-yellow.svg)](https://www.chartjs.org/)

## 📋 Descrição

**SEV (Sistema de Vendas)** é uma aplicação web moderna desenvolvida em ASP.NET Core MVC para gerenciamento completo de vendas, produtos, clientes e fornecedores. O sistema oferece uma interface intuitiva e responsiva com funcionalidades avançadas de dashboard, relatórios e gestão de estoque.

## ✨ Funcionalidades Principais

### 🎯 Dashboard Interativo
- **Estatísticas em Tempo Real**: Total de produtos, vendas, faturamento e clientes ativos
- **Gráficos Dinâmicos**: Vendas por mês e produtos com baixo estoque usando Chart.js
- **Atividades Recentes**: Sistema de monitoramento de atividades do sistema
- **Atualizações Automáticas**: Refresh automático a cada 5 minutos

### 📦 Gestão de Produtos
- **Cadastro Completo**: Nome, descrição, preço, estoque e categoria
- **Controle de Estoque**: Monitoramento de produtos com baixo estoque
- **Busca e Filtros**: Sistema avançado de busca por nome, categoria e status
- **Exportação CSV**: Exportação de dados para análise externa
- **Gestão de Categorias**: Organização hierárquica de produtos

### 👥 Gestão de Clientes
- **Cadastro de Clientes**: Dados pessoais, contato e endereço
- **Histórico de Compras**: Acompanhamento de todas as transações
- **Status de Atividade**: Controle de clientes ativos/inativos

### 🚚 Gestão de Fornecedores
- **Cadastro de Fornecedores**: CNPJ, contato e endereço
- **Produtos Fornecidos**: Relacionamento com produtos
- **Controle de Qualidade**: Avaliação e status dos fornecedores

### 💰 Sistema de Vendas
- **Processo de Venda**: Interface intuitiva para criação de vendas
- **Itens de Venda**: Controle detalhado de produtos vendidos
- **Cálculo Automático**: Totais, impostos e descontos
- **Histórico Completo**: Rastreamento de todas as transações

### 📊 Relatórios e Analytics
- **Dashboard Executivo**: Visão geral do negócio
- **Relatórios de Vendas**: Análise temporal e por categoria
- **Gestão de Estoque**: Alertas de produtos com baixo estoque
- **Métricas de Performance**: Indicadores de crescimento e eficiência

## 🛠️ Tecnologias Utilizadas

### Backend
- **ASP.NET Core 8.0**: Framework web moderno e de alto desempenho
- **Entity Framework Core 9.0**: ORM para acesso a dados
- **PostgreSQL**: Banco de dados relacional robusto
- **C# 12.0**: Linguagem de programação moderna

### Frontend
- **Bootstrap 5.3**: Framework CSS responsivo
- **Bootstrap Icons**: Biblioteca de ícones
- **Chart.js**: Biblioteca para gráficos interativos
- **jQuery**: Biblioteca JavaScript para manipulação do DOM
- **CSS3**: Estilos customizados com variáveis CSS e animações

### Arquitetura
- **MVC Pattern**: Separação clara de responsabilidades
- **Repository Pattern**: Abstração de acesso a dados
- **Dependency Injection**: Injeção de dependências nativa
- **Async/Await**: Programação assíncrona para melhor performance

## 🚀 Como Executar

### Pré-requisitos
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL 15+](https://www.postgresql.org/download/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)

### Configuração
1. **Clone o repositório**
   ```bash
   git clone https://github.com/Adryan-Francisco/S-E-V.git
   cd S-E-V
   ```

2. **Configure o banco de dados**
   - Crie um banco PostgreSQL
   - Atualize a string de conexão em `appsettings.json`

3. **Execute as migrações**
   ```bash
   cd SEV
   dotnet ef database update
   ```

4. **Execute o projeto**
   ```bash
   dotnet run
   ```

5. **Acesse a aplicação**
   - URL: `https://localhost:5001` ou `http://localhost:5000`

## 📁 Estrutura do Projeto

```
SEV/
├── Controllers/          # Controladores MVC
│   ├── DashboardController.cs
│   ├── ProdutosController.cs
│   ├── ClientesController.cs
│   ├── VendasController.cs
│   └── ...
├── Models/               # Modelos de dados
│   ├── Dashboard.cs
│   ├── Produto.cs
│   ├── Cliente.cs
│   ├── Venda.cs
│   └── ...
├── Views/                # Views Razor
│   ├── Dashboard/
│   ├── Produtos/
│   ├── Clientes/
│   └── ...
├── Data/                 # Contexto do banco
│   └── ApplicationDbContext.cs
├── wwwroot/             # Arquivos estáticos
│   ├── css/
│   ├── js/
│   └── lib/
└── Migrations/          # Migrações do Entity Framework
```

## 🎨 Interface e Design

### Design System
- **Tema Azul**: Paleta de cores profissional e moderna
- **Glassmorphism**: Efeitos visuais contemporâneos
- **Responsividade**: Adaptação perfeita para todos os dispositivos
- **Animações**: Transições suaves e feedback visual

### Componentes
- **Cards Estatísticos**: Exibição clara de métricas importantes
- **Tabelas Interativas**: Ordenação, filtros e paginação
- **Formulários Inteligentes**: Validação e feedback em tempo real
- **Modais e Notificações**: Interface não-intrusiva para ações

## 📈 Funcionalidades Avançadas

### Dashboard em Tempo Real
- **Auto-refresh**: Atualizações automáticas a cada 5 minutos
- **Simulação de Dados**: Atualizações simuladas para demonstração
- **Notificações Toast**: Sistema de alertas não-intrusivo
- **Loading States**: Indicadores visuais de processamento

### Sistema de Atividades
- **Monitoramento**: Rastreamento de todas as ações do sistema
- **Categorização**: Produtos, vendas, clientes e categorias
- **Timeline**: Histórico cronológico de atividades
- **Ações Rápidas**: Acesso direto a detalhes e edição

### Exportação e Relatórios
- **CSV Export**: Exportação de dados para análise externa
- **Filtros Avançados**: Busca por múltiplos critérios
- **Paginação**: Controle de volume de dados exibidos
- **Responsividade**: Adaptação para diferentes tamanhos de tela

## 🔧 Configurações

### Banco de Dados
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=SEV;Username=seu_usuario;Password=sua_senha"
  }
}
```

### Variáveis de Ambiente
- `ASPNETCORE_ENVIRONMENT`: Ambiente de execução
- `ASPNETCORE_URLS`: URLs de acesso à aplicação

## 📊 Métricas de Desenvolvimento

- **Linhas de Código**: +2.500 linhas
- **Arquivos**: 11 arquivos modificados/criados
- **Funcionalidades**: 8 módulos principais
- **Tecnologias**: 6 tecnologias principais
- **Padrões**: 4 padrões arquiteturais

## 🤝 Contribuição

1. **Fork** o projeto
2. **Crie** uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. **Commit** suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. **Push** para a branch (`git push origin feature/AmazingFeature`)
5. **Abra** um Pull Request

## 📝 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

## 👨‍💻 Desenvolvedor

**Adryan Francisco**
- GitHub: [@Adryan-Francisco](https://github.com/Adryan-Francisco)
- Projeto: SEV - Sistema de Vendas

## 🚀 Roadmap

### Versão 1.1 (Próxima)
- [ ] Sistema de usuários e autenticação
- [ ] Relatórios em PDF
- [ ] API REST para integração
- [ ] Dashboard mobile otimizado

### Versão 1.2 (Futura)
- [ ] Integração com sistemas de pagamento
- [ ] Notificações por email
- [ ] Backup automático do banco
- [ ] Sistema de auditoria completo

## 📞 Suporte

Para dúvidas, sugestões ou problemas:
- **Issues**: [GitHub Issues](https://github.com/Adryan-Francisco/S-E-V/issues)
- **Discussions**: [GitHub Discussions](https://github.com/Adryan-Francisco/S-E-V/discussions)

---

⭐ **Se este projeto foi útil, considere dar uma estrela no GitHub!**
