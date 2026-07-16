"use client";

import { FormEvent, useState } from "react";

import { JourneyNextAction } from "@/contracts/journey";
import { resolveJourneyEntryPoint } from "@/services/journey-api";

interface ResolutionMessage {
  title: string;
  description: string;
}

const resolutionMessages: Record<JourneyNextAction, ResolutionMessage> = {
  AUTHENTICATION_REQUIRED: {
    title: "Autenticação necessária",
    description:
      "Encontramos um cadastro para este CPF. Será necessário acessar sua conta.",
  },

  ONBOARDING_RESUME_REQUIRED: {
    title: "Cadastro em andamento",
    description:
      "Encontramos uma abertura de conta em andamento que poderá ser retomada.",
  },

  NEW_ONBOARDING_ALLOWED: {
    title: "Vamos abrir sua conta",
    description:
      "Seu CPF está disponível para iniciar uma nova abertura de conta.",
  },

  ONBOARDING_UNAVAILABLE: {
    title: "Abertura indisponível",
    description:
      "Não foi possível iniciar ou continuar a abertura de conta neste momento.",
  },
};

function formatCpf(value: string): string {
  const digits = value.replace(/\D/g, "").slice(0, 11);

  return digits
    .replace(/^(\d{3})(\d)/, "$1.$2")
    .replace(/^(\d{3})\.(\d{3})(\d)/, "$1.$2.$3")
    .replace(/\.(\d{3})(\d)/, ".$1-$2");
}

export function AccountOpeningForm() {
  const [cpf, setCpf] = useState("");
  const [nextAction, setNextAction] =
    useState<JourneyNextAction | null>(null);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();

    setErrorMessage(null);
    setNextAction(null);

    const cpfDigits = cpf.replace(/\D/g, "");

    if (cpfDigits.length !== 11) {
      setErrorMessage("Informe um CPF com 11 números.");
      return;
    }

    try {
      setIsSubmitting(true);

      const response = await resolveJourneyEntryPoint(cpf);

      setNextAction(response.nextAction);
    } catch (error) {
      const message =
        error instanceof Error
          ? error.message
          : "Ocorreu um erro inesperado.";

      setErrorMessage(message);
    } finally {
      setIsSubmitting(false);
    }
  }

  const resolution = nextAction
    ? resolutionMessages[nextAction]
    : null;

  return (
    <section className="account-card">
      <div className="account-card__header">
        <span className="account-card__eyebrow">Cloud Labs Bank</span>

        <h1>Abra sua conta</h1>

        <p>
          Para começar, informe seu CPF. É rápido, seguro e leva
          apenas alguns minutos.
        </p>
      </div>

      <form className="account-form" onSubmit={handleSubmit}>
        <div className="form-field">
          <label htmlFor="cpf">CPF</label>

          <input
            id="cpf"
            name="cpf"
            type="text"
            inputMode="numeric"
            autoComplete="off"
            placeholder="000.000.000-00"
            value={cpf}
            onChange={(event) => {
              setCpf(formatCpf(event.target.value));
              setErrorMessage(null);
              setNextAction(null);
            }}
            aria-describedby="cpf-help cpf-error"
            aria-invalid={Boolean(errorMessage)}
          />

          <span id="cpf-help" className="form-help">
            Digite somente os números. A formatação é automática.
          </span>
        </div>

        <button
          type="submit"
          className="primary-button"
          disabled={isSubmitting}
        >
          {isSubmitting ? "Consultando..." : "Continuar"}
        </button>
      </form>

      {errorMessage && (
        <div
          id="cpf-error"
          className="feedback feedback--error"
          role="alert"
        >
          <strong>Não foi possível continuar</strong>
          <span>{errorMessage}</span>
        </div>
      )}

      {resolution && (
        <div className="feedback feedback--success" role="status">
          <strong>{resolution.title}</strong>
          <span>{resolution.description}</span>

          <small>Próxima ação: {nextAction}</small>
        </div>
      )}
    </section>
  );
}