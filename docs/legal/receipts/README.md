# Asset Licence Receipts

This folder holds **one completed receipt per asset acquisition** (P0.E8.T5): purchases,
downloads with license terms, permission e-mails, and signed IP-assignment agreements for
commissioned work.

**Workflow**

1. Copy [`../templates/license-receipt.md`](../templates/license-receipt.md) →
   `YYYY-MM-DD_slug.md` (e.g. `2026-10-07_pixel-wall-set.md`); attach or link the proof
   (invoice PDF, license-page screenshot, e-mail export).
2. Commissioned work additionally gets a signed
   [`../templates/ip-assignment.md`](../templates/ip-assignment.md) stored alongside its receipt.
3. Update [`../asset-ledger.md`](../asset-ledger.md) (origin / license / commercial columns) and
   run `scripts/get-asset-ledger.ps1 -Check` — it must stay green.
4. Mirror any attribution change into [`../../../THIRD-PARTY.md`](../../../THIRD-PARTY.md).

**Why it matters:** the final commercial-release audit (P8.E3.T4) proves every shipped asset is
in the ledger with commercial-use = yes — this folder is the evidence trail behind those rows.
