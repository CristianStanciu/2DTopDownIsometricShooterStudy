# Asset Licence Receipt — `[YYYY-MM-DD_slug]`

> **Template (P0.E8.T5).** One receipt per acquisition (purchase, download, permission e-mail).
> Copy this file into `docs/legal/receipts/`, fill it in, attach proof, then update the ledger.
> Not legal advice; keep the original invoice/e-mail as the authoritative record.

## What was acquired

- **Asset(s):** `[ledger path(s) or pack name]`
- **Source:** `[marketplace / author page URL]`
- **Invoice or order #:** `[ ]` · **Amount:** `[amount, currency]` · **Date:** `[YYYY-MM-DD]`

## Grant of rights

- **License / grant type:** `[e.g. CC0 1.0, CC BY 4.0, Royalty-Free, exclusive license,
  work-for-hire + signed IP assignment (docs/legal/templates/ip-assignment.md)]`
- **Commercial use in a paid game:** `[ ] yes  [ ] no (then do not ship) — evidence: [quote/URL]`
- **Attribution required:** `[ ] no  [ ] yes → credit text: `[...]` (→ in-game credits P3.E3.T7)`
- **Exclusivity / transfer:** `[ ] non-exclusive  [ ] assigned/exclusive → signed doc attached`
- **Territory / duration:** `[worldwide, perpetual / other]`
- **Restrictions:** `[none / list — e.g. no resale of raw assets]`

## Proof attached

`[filename or link — invoice PDF, license page screenshot, e-mail thread]`

## Record updates (all required before merge)

- [ ] Ledger row added/updated in `docs/legal/asset-ledger.md` (origin, license, commercial)
- [ ] `scripts/get-asset-ledger.ps1 -Check` green
- [ ] `THIRD-PARTY.md` mirrored if attribution or a new third-party component is involved
- [ ] Replacement-brief checklist item marked (if this receipt closes a blocker)
