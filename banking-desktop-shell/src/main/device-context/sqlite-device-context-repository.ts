import { randomUUID } from 'node:crypto';

import {
  DEVICE_ACCESS_STATES,
  isDeviceAccessState,
  type DeviceAccessState,
  type DeviceContext,
  type PendingJourney,
  type RememberedAccount,
} from '../../shared/device-context';

import type { SQLiteDatabase } from '../database/database';
import type {
  DeviceContextRepository,
} from './device-context-repository';

interface InstallationRow {
  installation_id: string;
  access_state: string;
  active_account_reference: string | null;
}

interface RememberedAccountRow {
  account_reference: string;
  display_name: string;
  masked_cpf: string;
}

interface PendingJourneyRow {
  journey_reference: string;
  updated_at: string;
}

export class SQLiteDeviceContextRepository
implements DeviceContextRepository {
  private readonly loadOrCreateTransaction:
    () => DeviceContext;

  constructor(
    private readonly database: SQLiteDatabase,
  ) {
    this.loadOrCreateTransaction =
      this.database.transaction(() => {
        return this.loadOrCreateInternal();
      });
  }

  loadOrCreate(): DeviceContext {
    return this.loadOrCreateTransaction();
  }

  private loadOrCreateInternal(): DeviceContext {
    this.ensureInstallationExists();

    const installation = this.findInstallation();

    if (!installation) {
      throw new Error(
        'A instalação não foi encontrada após sua criação.',
      );
    }

    return {
      installationId: installation.installation_id,

      accessState: this.parseAccessState(
        installation.access_state,
      ),

      activeAccount: this.findActiveAccount(
        installation.active_account_reference,
      ),

      pendingJourney: this.findPendingJourney(),
    };
  }

  private ensureInstallationExists(): void {
    const now = new Date().toISOString();

    this.database
      .prepare(`
        INSERT OR IGNORE INTO app_installation (
          singleton_id,
          installation_id,
          access_state,
          active_account_reference,
          created_at,
          updated_at
        )
        VALUES (
          1,
          @installationId,
          @accessState,
          NULL,
          @createdAt,
          @updatedAt
        )
      `)
      .run({
        installationId: randomUUID(),
        accessState: DEVICE_ACCESS_STATES.UNLINKED,
        createdAt: now,
        updatedAt: now,
      });
  }

  private findInstallation():
    InstallationRow | undefined {
    return this.database
      .prepare(`
        SELECT
          installation_id,
          access_state,
          active_account_reference
        FROM app_installation
        WHERE singleton_id = 1
      `)
      .get() as InstallationRow | undefined;
  }

  private findActiveAccount(
    accountReference: string | null,
  ): RememberedAccount | null {
    if (!accountReference) {
      return null;
    }

    const row = this.database
      .prepare(`
        SELECT
          account_reference,
          display_name,
          masked_cpf
        FROM remembered_account
        WHERE account_reference = ?
      `)
      .get(accountReference) as
        | RememberedAccountRow
        | undefined;

    if (!row) {
      throw new Error(
        'A conta ativa não existe no armazenamento local.',
      );
    }

    return {
      accountReference: row.account_reference,
      displayName: row.display_name,
      maskedCpf: row.masked_cpf,
    };
  }

  private findPendingJourney():
    PendingJourney | null {
    const row = this.database
      .prepare(`
        SELECT
          journey_reference,
          updated_at
        FROM pending_journey
        WHERE singleton_id = 1
      `)
      .get() as PendingJourneyRow | undefined;

    if (!row) {
      return null;
    }

    return {
      journeyReference: row.journey_reference,
      updatedAt: row.updated_at,
    };
  }

  private parseAccessState(
    value: string,
  ): DeviceAccessState {
    if (isDeviceAccessState(value)) {
      return value;
    }

    throw new Error(
      `Estado de dispositivo inválido: ${value}`,
    );
  }
}