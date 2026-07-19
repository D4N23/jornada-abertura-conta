#!/usr/bin/env bash
set -euo pipefail

TOPIC="${1:?usage: ./scripts/publish-event.sh <topic> <event-json-file>}"
EVENT_FILE="${2:?usage: ./scripts/publish-event.sh <topic> <event-json-file>}"
KAFKA_CONTAINER="${KAFKA_CONTAINER:-account-opening-local-kafka-1}"

if [[ ! -f "$EVENT_FILE" ]]; then
  echo "Event file not found: $EVENT_FILE" >&2
  exit 1
fi

SUBJECT_KEY="$(sed -n 's/.*"subjectKey"[[:space:]]*:[[:space:]]*"\([^"]*\)".*/\1/p' "$EVENT_FILE" | head -n 1)"

if [[ -z "$SUBJECT_KEY" ]]; then
  echo "Could not read subjectKey from $EVENT_FILE" >&2
  exit 1
fi

{
  printf '%s:' "$SUBJECT_KEY"
  tr -d '\n' < "$EVENT_FILE"
  printf '\n'
} | docker exec -i "$KAFKA_CONTAINER" \
  /opt/kafka/bin/kafka-console-producer.sh \
  --bootstrap-server kafka:29092 \
  --topic "$TOPIC" \
  --property parse.key=true \
  --property key.separator=:

echo "Published $EVENT_FILE to $TOPIC using key $SUBJECT_KEY"
