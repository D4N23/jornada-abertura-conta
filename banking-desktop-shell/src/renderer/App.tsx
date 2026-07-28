export function App() {
  return (
    <div className="banking-shell">
      <header className="shell-header">
        <div className="brand">
          <div className="brand-symbol" aria-hidden="true">
            CB
          </div>

          <div>
            <span className="brand-name">Cloud Banking</span>
            <span className="environment-label">Laboratório</span>
          </div>
        </div>
      </header>

      <main className="shell-content">
        <section className="welcome-card">
          <span className="eyebrow">Seu banco digital</span>

          <h1>Bem-vindo ao Cloud Banking</h1>

          <p>
            Estamos preparando seu dispositivo para acessar sua jornada
            bancária.
          </p>
        </section>

        <section
          className="initialization-status"
          aria-live="polite"
          aria-busy="true"
        >
          <div className="loading-indicator" aria-hidden="true" />

          <div>
            <strong>Inicializando dispositivo</strong>
            <span>Carregando configurações locais...</span>
          </div>
        </section>
      </main>

      <footer className="shell-footer">
        <span>Ambiente de desenvolvimento</span>
      </footer>
    </div>
  );
}