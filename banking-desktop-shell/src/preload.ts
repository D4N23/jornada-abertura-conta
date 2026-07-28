import { contextBridge, ContextBridge, ipcRenderer } from "electron";
import type{BankingDeviceApi,DeviceContext} from './shared/device-context';
import { IPC_CHANNELS } from "./shared/ipc-channels";

const bankingDeviceApi : BankingDeviceApi = {
    loadContext: ():Promise<DeviceContext> => {return ipcRenderer.invoke(IPC_CHANNELS.loadDeviceContext);}
};

contextBridge.exposeInMainWorld('bankingDevice', bankingDeviceApi);