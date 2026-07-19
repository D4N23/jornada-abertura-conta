#!/usr/bin/env bash
set -euo pipefail

CPF="${1:?usage: ./scripts/subject-key.sh <cpf> [secret]}"
SECRET="${2:-${JOURNEY_SUBJECT_KEY_SECRET:-local-development-secret-change-me}}"
NORMALIZED_CPF="$(printf '%s' "$CPF" | tr -cd '0-9')"

if [[ ${#NORMALIZED_CPF} -ne 11 ]]; then
  echo "CPF must contain 11 digits after normalization" >&2
  exit 1
fi

printf '%s' "$NORMALIZED_CPF" \
  | openssl dgst -sha256 -mac HMAC -macopt "key:${SECRET}" -binary \
  | openssl base64 -A \
  | tr '+/' '-_' \
  | tr -d '='
