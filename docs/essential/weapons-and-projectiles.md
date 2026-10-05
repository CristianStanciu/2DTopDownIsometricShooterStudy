# Weapons & Projectiles

[Index](index.md) · [Player System](player.md) · [Architecture](architecture.md)

## Overview

The weapon framework lives in `Assets/Scripts/Weapons/`:

| Class | Base | Role |
|---|---|---|
| `Weapon` | `MonoBehaviour` | Base class: use-rate cooldown + virtual action hooks. |
| `WeaponHandler` | `MonoBehaviour` | Bridge between player and weapons (on the Player GameObject). |
| `Weapon_ShootProjectileCanCharge` | `Weapon` | The only implemented weapon behaviour (×2 in scenes). |
| `Weapon_ChargeContinuousShooting` | `Weapon` | **WIP stub** — `OnChargeShooting()` override is empty. |
| `WeaponAnim_ShootProjectileCanCharge` | `AnimationHandler` | Weapon sprite animations (Idle/BasicShot/Charging/ChargedShot). |
| `Projectile` | `MonoBehaviour` | Travelling bullet: Translate movement + wall-only trigger collision. |

## `Weapon` base class

- **Use-rate cooldown**: `_UseRate` seconds must pass between actions. `Update()` counts
  `t += Time.deltaTime` while `!canUse`; on expiry sets `canUse = true` and calls `virtual OnCanUse()`.
- **Fire-rate presets**: `useRateValues` array + `SwitchUseRate(SwitchUseRateType Next|Previous|ByIndex)`
  cycles values with `ArraysHandler.GetNextIndex/GetPreviousIndex` (circular modulo). The concrete
  weapon seeds `{ 0.125f, 0.05f }` in `Awake()`.
- **Actions**: `virtual PrimaryAction(bool value)` / `virtual SecondaryAction(bool value)` — the
  `bool` lets callers cancel mid-action (e.g., release charge button). Base implementations only
  debug-log.

## `WeaponHandler` (the bridge)

- Serialized `public Weapon[] weapons[]` — scene wiring: index **0 = LaserRifle_Auto-Charge**,
  index **1 = LaserBeam_Rifle_Body**.
- `SwitchWeapon(Next|Previous|ByIndex)`: disables **all** weapon GameObjects, computes the new
  index circularly, re-activates the selected one. Called on `Start()` (defaults to index 0), by
  `PlayerController` on `1`/`2`, `Q`/`E` (`PreviousWeapon`/`NextWeapon`) and mouse scroll.
- `SwitchUseRate(type, index, applyToAllWeapons = true)` — by default changes the fire rate on
  **every** weapon; called on `Start()` and on `C`/`V` (`PreviousUseRate`/`NextUseRate`).
- `UseWeapon(ActionType Primary|Secondary, bool value = true)`:
  - Mutual exclusion: primary is ignored while secondary is active and vice-versa
    (`isUsingPrimaryAction`/`isUsingSecondaryAction` flags).
  - `PlayerController` semantics: primary is **held** (`GetKey` → `Use(…, true)`,
    `GetKeyUp` → `Use(…, false)`); secondary is **press/release** (`GetKeyDown`/`GetKeyUp`).
- `CheckWeaponsAvailability()` logs a warning if any array slot is null (but does **not** stop the
  subsequent code — see [Known Issues](known-issues-and-roadmap.md)).

## `Weapon_ShootProjectileCanCharge` — the charge state machine

Serialized wiring: `basicProjectilePrefab`, `chargedProjectilePrefab`, `projectileSpawnPoint`,
`chargingPFX` (GameObject), `chargingSFX` (`SoundHandlerLocal`). Private `anim`
(`WeaponAnim_ShootProjectileCanCharge`) resolved with `TryGetComponent`.

State fields: `isReceivingInput` (secondary held), `isCharging`, `chargingTime`,
`CHARGE_DURATION = 2f`.

```
OnEnable() / OnCanUse()  → SpawnProjectiles(): lazily Instantiate both prefabs at the spawn
                           point (parented, SetActive(false)) — one instance reused per weapon;
                           plays Idle animation when the cooldown resets.

Update() while isReceivingInput:
  first frame → OnChargingStart(): if !canUse → return (cooldown gates charging);
               anim → Charging; isCharging = true;
               CameraShake.Shake(duration: CHARGE_DURATION, shakeAmount: 0.065, decreaseFactor: 1);
               chargingPFX.SetActive(true); chargingSFX.PlaySound()
  every frame → chargingTime += deltaTime; OnCharging(t): PFX localScale = one * t (anticipation)
  t >= 2s    → OnChargingEnd(): anim → ChargedShot; isCharging = false; canUse = false;
               chargingTime = 0; CameraShake.Shake(0.2s, amount 1, decreaseFactor 3);
               secondaryProjectile.SetActive(true); secondaryProjectile.Fire();
               secondaryProjectile = null; PFX reset+off

Update() when !isReceivingInput && isCharging → OnChargeCancel(): anim → Idle; reset flags/timer;
               CameraShake.Shake(0, 0, 0) stops shake; chargingSFX.StopSound(); PFX reset+off
```

Author's own `NOTE` in code: the charging shake *overrides* the release shake; fixing it needs a
timer so the charge option can't be rapidly re-used. Listed under
[Known Issues](known-issues-and-roadmap.md).

Primary action calls `Fire()` on the primary projectile when `canUse` (rate-limited by the base
cooldown). Both scene weapons use this same class — differences are Inspector data (prefabs, PFX,
SFX, animations) only.

## `Projectile`

- Fields: `Speed = 20`, `LifeTime = 1`, `hitPFX` prefab; caches `BoxCollider2D`, `SpriteRenderer`,
  `SoundHandlerLocal` via `TryGetComponent` (warns in `Start()` if any missing).
- **Lifecycle**: kept alive *inside* the weapon (instantiated once, `SetActive(false)` toggles
  renderer+collider). `Fire()` sets `hasLaunched`, plays SFX slot 0, chooses travel direction:
  `PlayerBodyPartsHandler.isRightDirection ? +Vector3.right : -Vector3.right`, **unparents**
  (`transform.parent = null`) and schedules `Destroy(gameObject, LifeTime)`.
- **Movement**: `Travel()` runs while `hasLaunched`:
  `transform.Translate(dir.normalized × Speed × deltaTime, Space.Self)`.
  (Code comment: "Remove destroy actions if a pool system is implemented".)
- **Collision**: `OnTriggerEnter2D` reacts only to tag **`Wall`**; an `impactCount` guard ensures a
  single hit; spawns `hitPFX` at `collision.ClosestPoint(transform.position)` then `Destroy(gameObject)`.
  There is **no damage system** — projectiles only interact with walls.

## Projectile / particle content

- Projectile prefabs (`Assets/Prefabs/Projectiles/`): `pfab_Bullet_Laser_Huge_Yellow` (charged),
  `pfab_Bullet_Laser_MidSize_Blue`, `pfab_Bullet_Laser_Small_Cyan` (basic).
- Particles (`Assets/Prefabs/Particles/`): impacts `pfx_Bullet_Laser_Impact_Big_Yellow`,
  `_Impact_Mid_Blue`, `_Impact_Small_Cyan`, `_Impact_Small_Green`; charge FX
  `pfx_Bullet_Laser_Charge_Yellow`; `pfx_Warp_Yellow`.
- Materials: `mat_Bullet_Laser_{Yellow,Blue,Cyan,Green,Magenta}` under `Assets/Materials/Projectiles/`.

## Scene placement

Both weapons exist **in the scene** (children of the player's hands), each with
`Weapon_ShootProjectileCanCharge` + `WeaponAnim_ShootProjectileCanCharge` (counts: 2 + 2 in
`Level_00`, same in `SingleWall_Test`). `Projectile` components are never placed in scenes — they
are instantiated at runtime by `SpawnProjectiles()`. See [Scene Inventory](scenes.md).

## Known gaps (weapons)

- `Weapon_ChargeContinuousShooting.OnChargeShooting()` is an empty override (dead code path).
- No reload, spread, ammo, damage, enemies, or hit feedback beyond particle + sound.
- `CheckWeaponsAvailability` doesn't actually abort (see [Known Issues](known-issues-and-roadmap.md)).
- Charge/shoot camera-shake override (author's own NOTE).

## Related pages

- [Player System](player.md) — how actions are triggered from input.
- [Input System](input.md) — the bindings that reach `WeaponHandler`.
- [Assets Pipeline](assets.md) — weapon animation clips and sprite sheets.

