import {useCallback,useEffect, useState} from 'react';
import type { DeviceContext } from '../shared/device-context';
import { DEVICE_ACCESS_STATES } from '../shared/device-context';

type DeviceContextViewState =
  | {
      status: 'loading';
    }
  | {
      status: 'success';
      context: DeviceContext;
    }
  | {
      status: 'error';
      message: string;
    };

export function App() {
  const [viewState, setViewState] =
    useState<DeviceContextViewState>({
      status: 'loading',
    });

  const loadDeviceContext = useCallback(async (): Promise<void> => {
    setViewState({
      status: 'loading',
    });

    try {
      const context =
        await window.bankingDevice.loadContext();

      setViewState({
        status: 'success',
        context,
      });
    } catch (error: unknown) {
      setViewState({
        status: 'error',
        message: getErrorMessage(error),
      });
    }
  }, []);

  useEffect(() => {
    void loadDeviceContext();
  }, [loadDeviceContext]);

  return (
    <div className="banking-shell">
      <ShellHeader />

      <main className="shell-content">
        {viewState.status === 'loading' && (
          <LoadingDeviceContext />
        )}

        {viewState.status === 'error' && (
          <DeviceContextError
            message={viewState.message}
            onRetry={loadDeviceContext}
          />
        )}

        {viewState.status === 'success' && (
          <DeviceContextLoaded
            context={viewState.context}
          />
        )}
      </main>

      <footer className="shell-footer">
        <span>Ambiente de desenvolvimento</span>
      </footer>
    </div>
  );
}

function ShellHeader() {
  return (
    <header className="shell-header">
      <div className="brand">
        <div
          className="brand-symbol"
          aria-hidden="true"
        >
          CB
        </div>

        <div>
          <span className="brand-name">
            Cloud Banking
          </span>

          <span className="environment-label">
            Laboratório
          </span>
        </div>
      </div>
    </header>
  );
}

function LoadingDeviceContext() {
  return (
    <section
      className="initialization-status"
      aria-live="polite"
      aria-busy="true"
    >
      <div
        className="loading-indicator"
        aria-hidden="true"
      />

      <div>
        <strong>Inicializando dispositivo</strong>

        <span>
          Carregando configurações locais...
        </span>
      </div>
    </section>
  );
}

interface DeviceContextErrorProps {
  message: string;
  onRetry(): Promise<void>;
}

function DeviceContextError({
  message,
  onRetry,
}: DeviceContextErrorProps) {
  return (
    <section
      className="state-card error-card"
      role="alert"
    >
      <span className="state-label">
        Falha de inicialização
      </span>

      <h1>Não foi possível carregar o dispositivo</h1>

      <p>{message}</p>

      <button
        className="primary-button"
        type="button"
        onClick={() => void onRetry()}
      >
        Tentar novamente
      </button>
    </section>
  );
}

interface DeviceContextLoadedProps {
  context: DeviceContext;
}

function DeviceContextLoaded({
  context,
}: DeviceContextLoadedProps) {
  if (
    context.accessState ===
    DEVICE_ACCESS_STATES.UNLINKED
  ) {
    return (
      <section className="state-card">
        <span className="state-label">
          Dispositivo preparado
        </span>

        <h1>Nenhuma conta vinculada</h1>

        <p>
          Este é o primeiro acesso neste dispositivo.
          Na próxima etapa, iniciaremos a identificação
          por CPF.
        </p>

        <dl className="device-information">
          <div>
            <dt>Estado</dt>
            <dd>{context.accessState}</dd>
          </div>

          <div>
            <dt>Instalação</dt>
            <dd>{context.installationId}</dd>
          </div>
        </dl>
      </section>
    );
  }

  return (
    <section className="state-card">
      <span className="state-label">
        Contexto carregado
      </span>

      <h1>Estado reconhecido</h1>

      <p>
        O dispositivo foi inicializado no estado:
      </p>

      <strong>{context.accessState}</strong>
    </section>
  );
}

function getErrorMessage(error: unknown): string {
  if (error instanceof Error) {
    return error.message;
  }

  return 'Ocorreu um erro desconhecido.';
}