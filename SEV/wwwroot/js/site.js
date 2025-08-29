// Sistema de Notificações Toast
class ToastNotification {
    constructor() {
        this.createToastContainer();
    }

    createToastContainer() {
        if (!document.getElementById('toast-container')) {
            const container = document.createElement('div');
            container.id = 'toast-container';
            container.className = 'toast-container position-fixed top-0 end-0 p-3';
            container.style.zIndex = '9999';
            document.body.appendChild(container);
        }
    }

    show(message, type = 'info', duration = 5000) {
        const toastId = 'toast-' + Date.now();
        const toast = document.createElement('div');
        toast.className = `toast align-items-center text-white bg-${type} border-0`;
        toast.id = toastId;
        toast.setAttribute('role', 'alert');
        toast.setAttribute('aria-live', 'assertive');
        toast.setAttribute('aria-atomic', 'true');

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
                <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
            </div>
        `;

        document.getElementById('toast-container').appendChild(toast);
        
        const bsToast = new bootstrap.Toast(toast, {
            autohide: true,
            delay: duration
        });
        
        bsToast.show();

        // Auto-remove após o toast ser escondido
        toast.addEventListener('hidden.bs.toast', () => {
            toast.remove();
        });
    }
}

// Inicializar sistema de notificações
const toast = new ToastNotification();

// Funções de utilidade para o sistema
const SEVUtils = {
    // Formatação de moeda brasileira
    formatCurrency: (value) => {
        return new Intl.NumberFormat('pt-BR', {
            style: 'currency',
            currency: 'BRL'
        }).format(value);
    },

    // Formatação de data brasileira
    formatDate: (date) => {
        return new Intl.DateTimeFormat('pt-BR').format(new Date(date));
    },

    // Formatação de número com separadores
    formatNumber: (value) => {
        return new Intl.NumberFormat('pt-BR').format(value);
    },

    // Validação de CPF
    validateCPF: (cpf) => {
        cpf = cpf.replace(/[^\d]/g, '');
        if (cpf.length !== 11) return false;
        
        let sum = 0;
        for (let i = 0; i < 9; i++) {
            sum += parseInt(cpf.charAt(i)) * (10 - i);
        }
        let remainder = (sum * 10) % 11;
        if (remainder === 10 || remainder === 11) remainder = 0;
        if (remainder !== parseInt(cpf.charAt(9))) return false;
        
        sum = 0;
        for (let i = 0; i < 10; i++) {
            sum += parseInt(cpf.charAt(i)) * (11 - i);
        }
        remainder = (sum * 10) % 11;
        if (remainder === 10 || remainder === 11) remainder = 0;
        if (remainder !== parseInt(cpf.charAt(10))) return false;
        
        return true;
    },

    // Validação de CNPJ
    validateCNPJ: (cnpj) => {
        cnpj = cnpj.replace(/[^\d]/g, '');
        if (cnpj.length !== 14) return false;
        
        // Verifica se todos os dígitos são iguais
        if (/^(\d)\1+$/.test(cnpj)) return false;
        
        let sum = 0;
        let weight = 2;
        for (let i = 11; i >= 0; i--) {
            sum += parseInt(cnpj.charAt(i)) * weight;
            weight = weight === 9 ? 2 : weight + 1;
        }
        let remainder = sum % 11;
        let digit1 = remainder < 2 ? 0 : 11 - remainder;
        
        sum = 0;
        weight = 2;
        for (let i = 12; i >= 0; i--) {
            sum += parseInt(cnpj.charAt(i)) * weight;
            weight = weight === 9 ? 2 : weight + 1;
        }
        remainder = sum % 11;
        let digit2 = remainder < 2 ? 0 : 11 - remainder;
        
        return parseInt(cnpj.charAt(12)) === digit1 && parseInt(cnpj.charAt(13)) === digit2;
    },

    // Máscara para campos de entrada
    applyMask: (input, mask) => {
        input.addEventListener('input', function(e) {
            let value = e.target.value.replace(/\D/g, '');
            let result = mask;
            
            for (let i = 0; i < value.length && i < mask.length; i++) {
                if (mask[i] === '#') {
                    result = result.replace('#', value[i]);
                }
            }
            
            e.target.value = result.replace(/#/g, '');
        });
    },

    // Confirmação de exclusão
    confirmDelete: (message = 'Tem certeza que deseja excluir este item?') => {
        return new Promise((resolve) => {
            const modal = document.createElement('div');
            modal.className = 'modal fade';
            modal.innerHTML = `
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">
                                <i class="bi bi-exclamation-triangle text-warning me-2"></i>Confirmação
                            </h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                        </div>
                        <div class="modal-body">
                            <p class="mb-0">${message}</p>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                            <button type="button" class="btn btn-danger" id="confirmDeleteBtn">Excluir</button>
                        </div>
                    </div>
                </div>
            `;
            
            document.body.appendChild(modal);
            
            const bsModal = new bootstrap.Modal(modal);
            bsModal.show();
            
            document.getElementById('confirmDeleteBtn').addEventListener('click', () => {
                bsModal.hide();
                resolve(true);
            });
            
            modal.addEventListener('hidden.bs.modal', () => {
                modal.remove();
                resolve(false);
            });
        });
    }
};

// Melhorias de UX para formulários
document.addEventListener('DOMContentLoaded', function() {
    // Aplicar máscaras em campos específicos
    const cpfInputs = document.querySelectorAll('input[data-mask="cpf"]');
    cpfInputs.forEach(input => {
        SEVUtils.applyMask(input, '###.###.###-##');
    });

    const cnpjInputs = document.querySelectorAll('input[data-mask="cnpj"]');
    cnpjInputs.forEach(input => {
        SEVUtils.applyMask(input, '##.###.###/####-##');
    });

    const phoneInputs = document.querySelectorAll('input[data-mask="phone"]');
    phoneInputs.forEach(input => {
        SEVUtils.applyMask(input, '(##) #####-####');
    });

    const cepInputs = document.querySelectorAll('input[data-mask="cep"]');
    cepInputs.forEach(input => {
        SEVUtils.applyMask(input, '#####-###');
    });

    // Validação em tempo real
    const formInputs = document.querySelectorAll('.form-control, .form-select');
    formInputs.forEach(input => {
        input.addEventListener('blur', function() {
            validateField(this);
        });
        
        input.addEventListener('input', function() {
            if (this.classList.contains('is-invalid')) {
                this.classList.remove('is-invalid');
                const feedback = this.parentNode.querySelector('.invalid-feedback');
                if (feedback) feedback.remove();
            }
        });
    });

    // Auto-complete para CEP
    const cepInput = document.querySelector('input[name="Cep"]');
    if (cepInput) {
        cepInput.addEventListener('blur', async function() {
            const cep = this.value.replace(/\D/g, '');
            if (cep.length === 8) {
                try {
                    const response = await fetch(`https://viacep.com.br/ws/${cep}/json/`);
                    const data = await response.json();
                    
                    if (!data.erro) {
                        const logradouroInput = document.querySelector('input[name="Logradouro"]');
                        const bairroInput = document.querySelector('input[name="Bairro"]');
                        const cidadeInput = document.querySelector('input[name="Cidade"]');
                        const estadoInput = document.querySelector('input[name="Estado"]');
                        
                        if (logradouroInput) logradouroInput.value = data.logradouro;
                        if (bairroInput) bairroInput.value = data.bairro;
                        if (cidadeInput) cidadeInput.value = data.localidade;
                        if (estadoInput) estadoInput.value = data.uf;
                        
                        toast.show('Endereço preenchido automaticamente!', 'success');
                    }
                } catch (error) {
                    console.error('Erro ao buscar CEP:', error);
                }
            }
        });
    }

    // Melhorar botões de exclusão
    const deleteButtons = document.querySelectorAll('a[asp-action="Delete"]');
    deleteButtons.forEach(button => {
        button.addEventListener('click', async function(e) {
            e.preventDefault();
            
            const confirmed = await SEVUtils.confirmDelete();
            if (confirmed) {
                window.location.href = this.href;
            }
        });
    });

    // Sistema de loading simples para formulários
    const forms = document.querySelectorAll('form');
    forms.forEach(form => {
        form.addEventListener('submit', function(e) {
            const submitBtn = this.querySelector('button[type="submit"], input[type="submit"]');
            if (submitBtn) {
                // Adicionar classe de loading ao botão
                submitBtn.classList.add('btn-loading');
                
                // Salvar texto original se não existir
                if (!submitBtn.getAttribute('data-original-text')) {
                    submitBtn.setAttribute('data-original-text', submitBtn.innerHTML);
                }
                
                // Mostrar spinner
                submitBtn.innerHTML = `
                    <span class="btn-text">${submitBtn.getAttribute('data-original-text')}</span>
                    <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
                `;
                
                // Desabilitar botão
                submitBtn.disabled = true;
                
                // Reabilitar após 15 segundos para evitar travamento
                setTimeout(() => {
                    if (submitBtn.disabled) {
                        submitBtn.disabled = false;
                        submitBtn.classList.remove('btn-loading');
                        submitBtn.innerHTML = submitBtn.getAttribute('data-original-text');
                    }
                }, 15000);
            }
        });
    });

    // Animações de entrada para cards
    const cards = document.querySelectorAll('.card');
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };

    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('animate-fade-in-up');
                observer.unobserve(entry.target);
            }
        });
    }, observerOptions);

    cards.forEach(card => {
        observer.observe(card);
    });

    // Tooltips e popovers
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    const popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl);
    });
});

// Função de validação de campos
function validateField(field) {
    const value = field.value.trim();
    let isValid = true;
    let message = '';

    // Validações específicas por tipo
    if (field.hasAttribute('required') && !value) {
        isValid = false;
        message = 'Este campo é obrigatório.';
    } else if (field.type === 'email' && value && !isValidEmail(value)) {
        isValid = false;
        message = 'Digite um e-mail válido.';
    } else if (field.hasAttribute('data-min-length') && value.length < parseInt(field.getAttribute('data-min-length'))) {
        isValid = false;
        message = `Mínimo de ${field.getAttribute('data-min-length')} caracteres.`;
    } else if (field.hasAttribute('data-max-length') && value.length > parseInt(field.getAttribute('data-max-length'))) {
        isValid = false;
        message = `Máximo de ${field.getAttribute('data-max-length')} caracteres.`;
    }

    if (!isValid) {
        field.classList.add('is-invalid');
        
        // Remover feedback anterior se existir
        const existingFeedback = field.parentNode.querySelector('.invalid-feedback');
        if (existingFeedback) existingFeedback.remove();
        
        // Adicionar novo feedback
        const feedback = document.createElement('div');
        feedback.className = 'invalid-feedback';
        feedback.textContent = message;
        field.parentNode.appendChild(feedback);
    }
}

// Função para validar e-mail
function isValidEmail(email) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}

// Função para mostrar notificação de sucesso
function showSuccess(message) {
    toast.show(message, 'success');
}

// Função para mostrar notificação de erro
function showError(message) {
    toast.show(message, 'danger');
}

// Função para mostrar notificação de aviso
function showWarning(message) {
    toast.show(message, 'warning');
}

// Função para mostrar notificação de informação
function showInfo(message) {
    toast.show(message, 'info');
}

// Exportar funções para uso global
window.SEVUtils = SEVUtils;
window.showSuccess = showSuccess;
window.showError = showError;
window.showWarning = showWarning;
window.showInfo = showInfo;
