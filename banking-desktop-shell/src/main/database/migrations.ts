import type { SQLiteDatabase } from './database';

interface Migration {
  version: number;
  name: string;
  up(database: SQLiteDatabase): void;
}

interface MigrationRow {
  version: number;
}

const MIGRATIONS: Migration[] = [
  {
    version: 1,
    name: 'create-device-state-tables',

    up(database): void {
      database.exec(`
        CREATE TABLE remembered_account (
          account_reference TEXT PRIMARY KEY,
          display_name TEXT NOT NULL,
          masked_cpf TEXT NOT NULL,

          remembered_at TEXT NOT NULL,
          last_accessed_at TEXT
        );

        CREATE TABLE pending_journey (
          singleton_id INTEGER PRIMARY KEY
            CHECK (singleton_id = 1),

          journey_reference TEXT NOT NULL UNIQUE,
          updated_at TEXT NOT NULL
        );

        CREATE TABLE app_installation (
          singleton_id INTEGER PRIMARY KEY
            CHECK (singleton_id = 1),

          installation_id TEXT NOT NULL UNIQUE,

          access_state TEXT NOT NULL
            CHECK (
              access_state IN (
                'UNLINKED',
                'ONBOARDING_PENDING',
                'ACCOUNT_REMEMBERED_LOCKED',
                'AUTHENTICATED'
              )
            ),

          active_account_reference TEXT,

          created_at TEXT NOT NULL,
          updated_at TEXT NOT NULL,

          FOREIGN KEY (active_account_reference)
            REFERENCES remembered_account(account_reference)
            ON DELETE SET NULL
        );
      `);
    },
  },
];

export function runMigrations(
  database: SQLiteDatabase,
): void {
  database.exec(`
    CREATE TABLE IF NOT EXISTS schema_migrations (
      version INTEGER PRIMARY KEY,
      name TEXT NOT NULL,
      applied_at TEXT NOT NULL
    );
  `);

  const appliedRows = database
    .prepare(`
      SELECT version
      FROM schema_migrations
    `)
    .all() as MigrationRow[];

  const appliedVersions = new Set(
    appliedRows.map((row) => row.version),
  );

  for (const migration of MIGRATIONS) {
    if (appliedVersions.has(migration.version)) {
      continue;
    }

    const applyMigration = database.transaction(() => {
      migration.up(database);

      database
        .prepare(`
          INSERT INTO schema_migrations (
            version,
            name,
            applied_at
          )
          VALUES (?, ?, ?)
        `)
        .run(
          migration.version,
          migration.name,
          new Date().toISOString(),
        );
    });

    applyMigration();
  }
}