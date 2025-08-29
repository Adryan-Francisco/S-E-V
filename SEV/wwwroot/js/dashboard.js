// Dashboard.js - Funcionalidades específicas do Dashboard SEV

class DashboardManager {
    constructor() {
        this.charts = {};
        this.refreshInterval = null;
        this.initializeDashboard();
    }

    initializeDashboard() {
        this.setupEventListeners();
        this.setupAutoRefresh();
        this.setupRealTimeUpdates();
        this.initializeCharts();
    }

    setupEventListeners() {
        // Botão de atualizar dashboard
        const refreshBtn = document.getElementById('refreshDashboardBtn');
        if (refreshBtn) {
            refreshBtn.addEventListener('click', (e) => {
                e.preventDefault();
                this.refreshDashboard();
            });
        }

        // Botão de atualizar atividades
        const refreshAtividadesBtn = document.getElementById('refreshAtividadesBtn');
        if (refreshAtividadesBtn) {
            refreshAtividadesBtn.addEventListener('click', (e) => {
                e.preventDefault();
                this.refreshAtividades();
            });
        }

        // Botões de ação nas atividades
        document.addEventListener('click', (e) => {
            if (e.target.closest('.btn-detalhes-atividade')) {
                e.preventDefault();
                const button = e.target.closest('.btn-detalhes-atividade');
                const tipo = button.getAttribute('data-tipo');
                const id = parseInt(button.getAttribute('data-id'));
                this.verDetalhesAtividade(tipo, id);
            }
        });
    }

    setupAutoRefresh() {
        // Atualizar dashboard a cada 5 minutos
        this.refreshInterval = setInterval(() => {
            this.refreshDashboard();
        }, 5 * 60 * 1000);
    }

    setupRealTimeUpdates() {
        // Simular atualizações em tempo real
        setInterval(() => {
            this.updateRandomStat();
        }, 30 * 1000); // A cada 30 segundos
    }

    updateRandomStat() {
        const statCards = document.querySelectorAll('.stats-card h3');
        if (statCards.length > 0) {
            const randomCard = statCards[Math.floor(Math.random() * statCards.length)];
            const currentValue = parseInt(randomCard.textContent.replace(/\D/g, ''));
            const newValue = currentValue + Math.floor(Math.random() * 5) + 1;
            
            // Animar a mudança
            randomCard.style.transform = 'scale(1.1)';
            randomCard.style.color = '#10b981';
            randomCard.style.transition = 'all 0.3s ease';
            
            setTimeout(() => {
                randomCard.textContent = newValue;
                randomCard.style.transform = 'scale(1)';
                randomCard.style.color = '';
            }, 150);
        }
    }

    initializeCharts() {
        // Inicializar gráficos se existirem
        if (typeof Chart !== 'undefined') {
            this.setupChartAnimations();
        }
    }

    setupChartAnimations() {
        // Adicionar animações aos gráficos
        const chartContainers = document.querySelectorAll('.chart-container');
        chartContainers.forEach((container, index) => {
            setTimeout(() => {
                container.style.opacity = '0';
                container.style.transform = 'translateY(20px)';
                container.style.transition = 'all 0.6s ease';
                
                setTimeout(() => {
                    container.style.opacity = '1';
                    container.style.transform = 'translateY(0)';
                }, 100);
            }, index * 200);
        });
    }

    refreshDashboard() {
        // Mostrar indicador de loading
        this.showLoadingState();
        
        // Recarregar a página após um breve delay
        setTimeout(() => {
            location.reload();
        }, 500);
    }

    refreshAtividades() {
        // Animar linhas de atividades
        const atividadeRows = document.querySelectorAll('.atividade-row');
        atividadeRows.forEach((row, index) => {
            setTimeout(() => {
                row.style.opacity = '0.5';
                row.style.transform = 'translateX(-20px)';
                row.style.transition = 'all 0.3s ease';
            }, index * 100);
        });
        
        // Recarregar após animação
        setTimeout(() => {
            location.reload();
        }, atividadeRows.length * 100 + 300);
    }

    verDetalhesAtividade(tipo, id) {
        let url = '';
        
        switch(tipo) {
            case 'Produto':
                url = `/Produtos/Details/${id}`;
                break;
            case 'Cliente':
                url = `/Clientes/Details/${id}`;
                break;
            case 'Venda':
                url = `/Vendas/Details/${id}`;
                break;
            case 'Categoria':
                url = `/Categorias/Details/${id}`;
                break;
            default:
                console.log('Tipo de atividade não reconhecido:', tipo);
                return;
        }
        
        if (url) {
            // Abrir em nova aba
            window.open(url, '_blank');
            
            // Mostrar notificação
            this.showNotification(`Visualizando detalhes de ${tipo}`, 'info');
        }
    }

    showLoadingState() {
        // Criar overlay de loading
        const loadingOverlay = document.createElement('div');
        loadingOverlay.id = 'dashboard-loading';
        loadingOverlay.style.cssText = `
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(255, 255, 255, 0.9);
            display: flex;
            align-items: center;
            justify-content: center;
            z-index: 9999;
            backdrop-filter: blur(5px);
        `;
        
        loadingOverlay.innerHTML = `
            <div class="text-center">
                <div class="spinner-border text-primary" style="width: 3rem; height: 3rem;" role="status">
                    <span class="visually-hidden">Carregando...</span>
                </div>
                <p class="mt-3 text-muted">Atualizando dashboard...</p>
            </div>
        `;
        
        document.body.appendChild(loadingOverlay);
    }

    showNotification(message, type = 'info') {
        // Criar notificação toast
        const toast = document.createElement('div');
        toast.className = `toast align-items-center text-white bg-${type} border-0`;
        toast.style.cssText = `
            position: fixed;
            top: 20px;
            right: 20px;
            z-index: 10000;
            min-width: 300px;
        `;
        
        const iconMap = {
            'success': 'bi-check-circle-fill',
            'danger': 'bi-exclamation-triangle-fill',
            'warning': 'bi-exclamation-triangle-fill',
            'info': 'bi-info-circle-fill'
        };
        
        toast.innerHTML = `
            <div class="d-flex">
                <div class="toast-body">
                    <i class="bi ${iconMap[type] || iconMap.info} me-2"></i>
                    ${message}
                </div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto" onclick="this.parentElement.parentElement.remove()"></button>
            </div>
        `;
        
        document.body.appendChild(toast);
        
        // Auto-remover após 5 segundos
        setTimeout(() => {
            if (toast.parentElement) {
                toast.remove();
            }
        }, 5000);
    }

    // Método para pausar atualizações automáticas
    pauseAutoRefresh() {
        if (this.refreshInterval) {
            clearInterval(this.refreshInterval);
            this.refreshInterval = null;
        }
    }

    // Método para retomar atualizações automáticas
    resumeAutoRefresh() {
        if (!this.refreshInterval) {
            this.setupAutoRefresh();
        }
    }

    // Método para destruir o dashboard manager
    destroy() {
        this.pauseAutoRefresh();
        // Limpar outros listeners se necessário
    }
}

// Inicializar dashboard quando o DOM estiver pronto
document.addEventListener('DOMContentLoaded', function() {
    // Verificar se estamos na página do dashboard
    if (document.querySelector('.stats-card')) {
        window.dashboardManager = new DashboardManager();
    }
});

// Funções globais para compatibilidade
function refreshDashboard() {
    if (window.dashboardManager) {
        window.dashboardManager.refreshDashboard();
    } else {
        location.reload();
    }
}

function refreshAtividades() {
    if (window.dashboardManager) {
        window.dashboardManager.refreshAtividades();
    } else {
        location.reload();
    }
}

function verDetalhesAtividade(tipo, id) {
    if (window.dashboardManager) {
        window.dashboardManager.verDetalhesAtividade(tipo, id);
    } else {
        // Fallback para navegação direta
        let url = '';
        switch(tipo) {
            case 'Produto': url = `/Produtos/Details/${id}`; break;
            case 'Cliente': url = `/Clientes/Details/${id}`; break;
            case 'Venda': url = `/Vendas/Details/${id}`; break;
            case 'Categoria': url = `/Categorias/Details/${id}`; break;
        }
        if (url) window.open(url, '_blank');
    }
}
