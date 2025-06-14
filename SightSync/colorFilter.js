const { exec } = require('child_process');
const robot = require('robotjs');

function aplicarFiltroWindows(tipo) {
    console.log(`Aplicando filtro tipo ${tipo}...`);
    
    // Configura os filtros diretamente via registro
    const comandos = [
        `reg add "HKCU\\Software\\Microsoft\\ColorFiltering" /v Active /t REG_DWORD /d 1 /f`,
        `reg add "HKCU\\Software\\Microsoft\\ColorFiltering" /v FilterType /t REG_DWORD /d ${tipo} /f`,
        `reg add "HKCU\\Software\\Microsoft\\ColorFiltering" /v HotkeyEnabled /t REG_DWORD /d 1 /f`
    ];

    // Executa os comandos de registro sem abrir janelas
    exec(comandos.join(' && '), (error, stdout, stderr) => {
        if (error) {
            console.error(`Erro ao configurar registro para filtro: ${error.message}`);
            return;
        }
        
        console.log("Registro configurado com sucesso");

        // Atualiza parâmetros do sistema para aplicar as mudanças imediatamente
        exec('rundll32 user32.dll,UpdatePerUserSystemParameters 1, 1', (err) => {
            if (err) {
                console.error("Falha na atualização dos parâmetros:", err);
                
                // Se falhar o método de atualização, tenta diretamente o atalho como fallback
                activateFilterWithHotkey();
            } else {
                console.log("Parâmetros do sistema atualizados");
                
                // Garantir que o filtro seja aplicado com atalho após a atualização dos parâmetros
                activateFilterWithHotkey();
            }
        });
    });
}

function activateFilterWithHotkey() {
    try {
        // Pressiona o atalho Windows+Ctrl+C que ativa/desativa filtros de cor no Windows
        robot.keyToggle('control', 'down');
        robot.keyToggle('command', 'down');
        robot.keyTap('c');
        robot.keyToggle('command', 'up');
        robot.keyToggle('control', 'up');
        
        // Pressiona novamente para garantir que está realmente ativado
        setTimeout(() => {
            robot.keyToggle('control', 'down');
            robot.keyToggle('command', 'down');
            robot.keyTap('c');
            robot.keyToggle('command', 'up');
            robot.keyToggle('control', 'up');
            console.log("Ativação do filtro concluída via atalho");
        }, 500);
    } catch (robotErr) {
        console.error("Erro no atalho de teclado:", robotErr);
        
        // Fallback para método alternativo em caso de falha
        try {
            // Alternativa: Ctrl+Alt+C (caso configurado no sistema)
            robot.keyToggle('control', 'down');
            robot.keyToggle('alt', 'down');
            robot.keyTap('c');
            robot.keyToggle('alt', 'up');
            robot.keyToggle('control', 'up');
            console.log("Atalho de teclado alternativo enviado");
        } catch (e) {
            console.error("Erro no atalho de teclado alternativo:", e);
        }
    }
}

function resetarFiltro() {
    console.log("Desativando filtros de cor...");

    const comandos = [
        `reg add "HKCU\\Software\\Microsoft\\ColorFiltering" /v Active /t REG_DWORD /d 0 /f`
    ];

    exec(comandos.join(' && '), (error) => {
        if (error) return console.error(`Erro ao desativar filtro: ${error.message}`);
        console.log("Filtro desativado no registo");

        forcarAtualizacaoSistema(() => {
            console.log("Desativando filtro com atalho...");
            try {
                robot.keyToggle('control', 'down');
                robot.keyToggle('command', 'down');
                robot.keyTap('c');
                robot.keyToggle('command', 'up');
                robot.keyToggle('control', 'up');
                console.log("Filtro desativado via atalho");
            } catch (err) {
                console.error("Erro ao desativar com atalho:", err);
            }
        });
    });
}

function forcarAtualizacaoSistema(callback) {
    exec('rundll32 user32.dll,UpdatePerUserSystemParameters 1, 1', (err) => {
        if (err) {
            console.error("Erro ao forçar atualização do sistema:", err);
        } else {
            console.log("Sistema atualizado via user32.dll");
        }

        exec('DisplaySwitch.exe /internal', (dsErr) => {
            if (dsErr) {
                console.error("Erro no DisplaySwitch:", dsErr);
            } else {
                console.log("Forçado refresh com DisplaySwitch");
            }

            if (callback) callback();
        });
    });
}

module.exports = { aplicarFiltroWindows, resetarFiltro };