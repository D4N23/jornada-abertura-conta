#!/usr/bin/env bash
set -euo pipefail

SUBJECT_KEY="${1:?usage: ./scripts/query-read-model.sh <subject-key>}"
READ_URL="${JOURNEY_READ_URL:-http://localhost:8081}"

curl --silent --show-error \
  --request POST \
  --header 'Content-Type: application/json' \
  --data "{\"subjectKey\":\"${SUBJECT_KEY}\"}" \
  --write-out '\nHTTP %{http_code}\n' \
  "${READ_URL}/internal/v1/journey-resolutions/resolve"
