#!/usr/bin/env bash
set -euo pipefail

CPF="${1:?usage: ./scripts/query-bff.sh <cpf>}"
BFF_URL="${JOURNEY_BFF_URL:-http://localhost:8080}"

curl --silent --show-error \
  --request POST \
  --header 'Content-Type: application/json' \
  --data "{\"identifier\":{\"type\":\"CPF\",\"value\":\"${CPF}\"}}" \
  --write-out '\nHTTP %{http_code}\n' \
  "${BFF_URL}/v1/journeys/entry-point/resolve"
