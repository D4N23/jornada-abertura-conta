import path from 'node:path';
import {app,BrowserWindow,ipcMain,type IpcMainInvokeEvent} from 'electron';
import started from 'electron-squirrel-startup';
import type { DeviceContext } from './shared/device-context';
import { DEVICE_ACCESS_STATES } from './shared/device-context';
import { IPC_CHANNELS } from './shared/ipc-channels';
import {openDatabase,type SQLiteDatabase} from './main/database/database';
import {runMigrations} from './main/database/migrations';
import type {DeviceContextRepository} from './main/device-context/device-context-repository';
import {SQLiteDeviceContextRepository} from './main/device-context/sqlite-device-context-repository';

if (started) {
  app.quit();
}

let mainWindow: BrowserWindow | null = null;
let database: SQLiteDatabase | null = null;
let deviceContextRepository: DeviceContextRepository | null = null;

function createMainWindow(): void {
  mainWindow = new BrowserWindow({
    title: 'Cloud Banking',

    width: 430,
    height: 932,

    resizable: false,
    maximizable: false,
    fullscreenable: false,

    backgroundColor: '#f5f6fa',
    autoHideMenuBar: true,

    webPreferences: {
      preload: path.join(__dirname, 'preload.js'),
      nodeIntegration: false,
      contextIsolation: true,
      sandbox: true,
    },
  });

  loadRenderer(mainWindow);
  preventUncontrolledWindows(mainWindow);

  if (!app.isPackaged) {
    mainWindow.webContents.openDevTools({
      mode: 'detach',
    });
  }

  mainWindow.on('closed', () => {
    mainWindow = null;
  });
}

function loadRenderer(window: BrowserWindow): void {
  if (MAIN_WINDOW_VITE_DEV_SERVER_URL) {
    void window.loadURL(MAIN_WINDOW_VITE_DEV_SERVER_URL);
    return;
  }

  void window.loadFile(
    path.join(
      __dirname,
      `../renderer/${MAIN_WINDOW_VITE_NAME}/index.html`,
    ),
  );
}

function preventUncontrolledWindows(window: BrowserWindow): void {
  window.webContents.setWindowOpenHandler(() => {
    return {
      action: 'deny',
    };
  });
}

function registerIpcHandlers(): void {
  /*
   * Evita registrar o mesmo handler duas vezes caso o processo
   * seja recarregado durante o desenvolvimento.
   */
  ipcMain.removeHandler(IPC_CHANNELS.loadDeviceContext);

  ipcMain.handle(
    IPC_CHANNELS.loadDeviceContext,
    (event): DeviceContext => {
      validateIpcSender(event);
        if(!deviceContextRepository){
          throw new Error('O repositório do dispositivo não foi inicializado.');
        }
      return deviceContextRepository.loadOrCreate();
    },
  );
}

function validateIpcSender(event: IpcMainInvokeEvent): void {
  const senderUrl = event.senderFrame?.url;

  if (!senderUrl) {
    throw new Error('Não foi possível identificar a origem da chamada.');
  }

  if (isTrustedRendererUrl(senderUrl)) {
    return;
  }

  throw new Error('Origem não autorizada para comunicação IPC.');
}

function isTrustedRendererUrl(senderUrl: string): boolean {
  try {
    const parsedSenderUrl = new URL(senderUrl);

    if (!app.isPackaged && MAIN_WINDOW_VITE_DEV_SERVER_URL) {
      const developmentServerUrl = new URL(
        MAIN_WINDOW_VITE_DEV_SERVER_URL,
      );

      return (
        parsedSenderUrl.origin === developmentServerUrl.origin
      );
    }

    return parsedSenderUrl.protocol === 'file:';
  } catch {
    return false;
  }
}

function initializeDeviceState(): void{
  const databaseDirectory = path.join( app.getPath('userData'), 'baking-device');
  const databasePath = path.join(databaseDirectory, 'device-state-sqlite3');
  database = openDatabase(databasePath);
  runMigrations(database);
  deviceContextRepository = new SQLiteDeviceContextRepository(database);
  if (!app.isPackaged) {
    console.info(`[device-state] SQLite disponivel em: ${databasePath}`)
  }

}

void app.whenReady().then(() => {
  initializeDeviceState();
  registerIpcHandlers();
  createMainWindow();

  app.on('activate', () => {
    if (BrowserWindow.getAllWindows().length === 0) {
      createMainWindow();
    }
  });
});

app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') {
    app.quit();
  }
});

app.on('before-quit', () => {
  database?.close();

  database = null;
  deviceContextRepository = null;
});