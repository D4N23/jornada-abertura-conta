export const DEVICE_ACCESS_STATES = {
  UNLINKED: 'UNLINKED',
  ONBOARDING_PENDING: 'ONBOARDING_PENDING',
  ACCOUNT_REMEMBERED_LOCKED: 'ACCOUNT_REMEMBERED_LOCKED',
  AUTHENTICATED: 'AUTHENTICATED',
}as const;

export type DeviceAccessState = (typeof DEVICE_ACCESS_STATES)[keyof typeof DEVICE_ACCESS_STATES];

export function isDeviceAccessState(value: string): value is DeviceAccessState {
  return Object.values(DEVICE_ACCESS_STATES).some((state) => state === value);
}

export interface RememberedAccount {
  accountReference: string;
  displayName: string;
  maskedCpf: string;
}

export interface PendingJourney {
  journeyReference: string;
  updatedAt: string;
}

export interface DeviceContext {
  installationId: string;
  accessState: DeviceAccessState;
  activeAccount: RememberedAccount | null;
  pendingJourney: PendingJourney | null;
}

export interface BankingDeviceApi {
  loadContext(): Promise<DeviceContext>;
}