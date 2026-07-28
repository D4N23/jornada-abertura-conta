import { BankingDeviceApi } from "../shared/device-context";

declare global{
    interface Window{
        bankingDevice: BankingDeviceApi;
    }
}

export{};