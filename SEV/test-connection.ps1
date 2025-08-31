# Script para testar conexão com PostgreSQL
Write-Host "Testando conexão com PostgreSQL..." -ForegroundColor Yellow

try {
    # Verificar se o módulo PostgreSQL está disponível
    $pgPath = Get-Command psql -ErrorAction SilentlyContinue
    if ($pgPath) {
        Write-Host "psql encontrado em: $($pgPath.Source)" -ForegroundColor Green
        
        # Testar conexão
        $env:PGPASSWORD = "110906"
        $result = psql -h localhost -U postgres -d SEV1 -c "SELECT version();" 2>&1
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "Conexão bem-sucedida!" -ForegroundColor Green
            Write-Host $result -ForegroundColor Cyan
        } else {
            Write-Host "Erro na conexão:" -ForegroundColor Red
            Write-Host $result -ForegroundColor Red
        }
    } else {
        Write-Host "psql não encontrado no PATH" -ForegroundColor Red
        Write-Host "Verifique se o PostgreSQL está instalado e configurado" -ForegroundColor Yellow
    }
} catch {
    Write-Host "Erro ao executar teste: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "Teste concluído." -ForegroundColor Yellow

