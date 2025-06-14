const { app, BrowserWindow, ipcMain } = require('electron');
const path = require('path');
const { aplicarFiltroWindows, resetarFiltro } = require('./colorFilter');

let mainWindow;

function createWindow() {
    mainWindow = new BrowserWindow({
        width: 1920,
        height: 1080,
        fullscreenable: true,
        autoHideMenuBar: true,
        webPreferences: {
            preload: path.join(__dirname, 'preload.js'),
            contextIsolation: false,
            nodeIntegration: true
        }
    });

    const htmlPath = path.join(__dirname, 'SightSync_App', 'sightsync.html');

    console.log("HTML Path:", htmlPath);

    mainWindow.loadFile(htmlPath).then(() => {
        mainWindow.maximize();
        mainWindow.webContents.setZoomFactor(0.55);
    }).catch(err => {
        console.error("Erro ao carregar o HTML:", err);
    });

    mainWindow.webContents.on('did-finish-load', () => {
        mainWindow.maximize();
        mainWindow.webContents.setZoomFactor(0.55);
    });
}

app.whenReady().then(() => {
    createWindow();

    app.on('activate', () => {
        if (BrowserWindow.getAllWindows().length === 0) {
            createWindow();
        }
    });
});

app.on('window-all-closed', () => {
    if (process.platform !== 'darwin') {
        app.quit();
    }
});

ipcMain.on('aplicar-filtro', (_, tipo) => {
    if (typeof tipo === "number" && tipo >= 0 && tipo <= 6) {
        aplicarFiltroWindows(tipo);
    } else {
        console.error("Tipo de filtro inválido recebido:", tipo);
    }
});

ipcMain.on('resetar-filtro', () => {
    resetarFiltro();
});