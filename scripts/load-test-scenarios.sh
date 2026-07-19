#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"

"$ROOT_DIR/scripts/render-test-events.sh"
"$ROOT_DIR/scripts/publish-event.sh" \
  customer.lifecycle.v1 \
  "$ROOT_DIR/events/customer-active.json"
"$ROOT_DIR/scripts/publish-event.sh" \
  onboarding.lifecycle.v1 \
  "$ROOT_DIR/events/onboarding-in-progress.json"

echo
cat <<'SUMMARY'
Scenarios loaded.

Test through the BFF:
  ./scripts/query-bff.sh 529.982.247-25  # AUTHENTICATION_REQUIRED
  ./scripts/query-bff.sh 111.444.777-35  # ONBOARDING_RESUME_REQUIRED
  ./scripts/query-bff.sh 123.456.789-09  # NEW_ONBOARDING_ALLOWED (no projection)
SUMMARY
