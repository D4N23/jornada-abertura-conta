#!/usr/bin/env bash
set -euo pipefail

CPF_AUTH="${CPF_AUTH:-52998224725}"
CPF_RESUME="${CPF_RESUME:-11144477735}"
SECRET="${JOURNEY_SUBJECT_KEY_SECRET:-local-development-secret-change-me}"
ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"

AUTH_KEY="$($ROOT_DIR/scripts/subject-key.sh "$CPF_AUTH" "$SECRET")"
RESUME_KEY="$($ROOT_DIR/scripts/subject-key.sh "$CPF_RESUME" "$SECRET")"

sed "s/__SUBJECT_KEY__/${AUTH_KEY}/g" \
  "$ROOT_DIR/events/customer-active.template.json" \
  > "$ROOT_DIR/events/customer-active.json"

sed "s/__SUBJECT_KEY__/${RESUME_KEY}/g" \
  "$ROOT_DIR/events/onboarding-in-progress.template.json" \
  > "$ROOT_DIR/events/onboarding-in-progress.json"

cat <<SUMMARY
Generated test events:
  Authentication CPF: $CPF_AUTH
  Authentication subjectKey: $AUTH_KEY

  Resume CPF: $CPF_RESUME
  Resume subjectKey: $RESUME_KEY
SUMMARY
