# Case 002 — Oklick GP-315M + The Wind Waker (Dolphin / GameCube)

**Date:** 2026-09  
**Game platform:** Nintendo GameCube  
**Emulation:** RetroArch (~1.22) + Dolphin core (`dolphin_libretro`)  
**Gamepad:** Oklick GP-315M (Twin Shock / DragonRise clone, DInput)  
**Wrapper:** XOutput + ViGEmBus → virtual Xbox 360  
**Raw pad hidden with:** HidHide  
**Result:** plays correctly (move, camera, shield on LB)

**Machine:** i5-12400F, RTX 5060 Ti, 32 GB, Win11, 1080p@144 — see [hardware/lab-pc.md](../../hardware/lab-pc.md).  
Same input stack as [Case 001](../001-oklick-gp315m-sotc-lrps2/); Dolphin adds **GameCube trigger semantics** and **content-specific `.cfg`**.

**Русский:** [README.ru.md](README.ru.md)

---

## Symptoms (what it looked like)

| # | Symptom | In-game feel |
|---|---------|--------------|
| A | “Controls do nothing” / keyboard works in menus only | Link won't move; only WASD from global RetroArch binds |
| B | Dolphin asks for memory card slot | can't continue past save prompt |
| C | Forward and back ~50% speed; left/right full speed | stick fully forward = slow walk; strafe = normal run |
| D | Camera left/right reversed on C-stick | right stick X mirrored |
| E | “No jump button” | expected Mario-style A jump — WW has no jump (B = roll/crawl) |

Problems A and C look unrelated but both come from **how RetroArch + Dolphin interpret this pad**.

---

## Hardware and software (sanitized)

Same chain as Case 001 — only the core changes:

```
[Oklick GP-315M USB] → XOutput → ViGEm Xbox 360 → RetroArch (xinput) → Dolphin → Wind Waker (GC)
```

**Mode LED = red** (analog right stick). See [cheatsheets/oklick-gp315m.md](../../cheatsheets/oklick-gp315m.md).

Launch **without Steam Input** (same as Case 001).

---

## Root causes by layer

### 1) Global RetroArch keyboard binds, no gamepad override

`retroarch.cfg` had movement on **keyboard** (`WASD`) and joypad buttons set to `"nul"`.  
Autoconfig alone is not enough if the **content config** does not re-bind player 1 for this ISO.

Dolphin loads per-game config from:

```text
<RetroArch>/config/dolphin-emu/<ISO_FILENAME>.cfg
```

Example: `Zelda_Wind_Waker.iso` → `Zelda_Wind_Waker.iso.cfg`

**Fix:** content `.cfg` with full `input_player1_*` Xbox binds and keyboard keys nulled — see [`configs/dolphin-emu/Wind-Waker.cfg.example`](../../configs/dolphin-emu/Wind-Waker.cfg.example).

### 2) GC memory card missing (first run)

Dolphin libretro expects GC save files under its user folder (created on first run). Empty slot → “no memory card” style prompt.

**Fix:** let Dolphin create/format cards once, or pre-create empty `MemoryCardA.USA.raw` (2 MB) in the saves tree. Do **not** commit saves to this repo.

### 3) Slow forward/back, fast strafe — analog L trigger ghost

GameCube **L** and **R** are **analog** triggers. Dolphin maps:

- RetroPad **L2 axis** (Xbox LT) → GC L analog  
- Half-press L → shield / slower movement in Wind Waker

Oklick **LT/RT are digital buttons** in XOutput, but we initially mapped `input_player1_l2_axis = "+4"` like a standard Xbox pad. Combined with stick Y noise or trigger bleed, Dolphin could read **partial L** while moving forward/back — Link walks slowly. Strafe (mostly X axis) did not trigger the same bleed → felt “fast sideways, slow forward.”

**Fix for this game:**

```ini
input_player1_l2_axis = "nul"
input_player1_r2_axis = "nul"
# keep digital L/R on LB/RB only:
input_player1_l_btn = "4"
input_player1_r_btn = "5"
```

Plus boost stick sensitivity for walk/run threshold:

```ini
input_analog_sensitivity = "2.000000"
```

Oklick often reports ~**±16383** instead of ±32767 at full tilt (same as Case 001). Wind Waker uses stick **tilt** for walk vs run; half range on Y feels like permanent “walking speed” on forward/back.

If still slow after LT/RT null + 2× sensitivity — re-check XOutput LY mapping ([stick-mapping.notes.md](../../configs/xoutput/stick-mapping.notes.md)).

### 4) C-stick camera X inverted

**Fix in content `.cfg` only (per game):**

```ini
input_player1_r_x_plus_axis = "-2"
input_player1_r_x_minus_axis = "+2"
```

Swap signs if your pad differs.

### 5) “No jump” is normal

Wind Waker (GameCube) has **no jump button**. **B** = roll/crawl; **A** = sword/action. Not a mapping bug.

(Wii games like Mario Galaxy need **Wiimote + Nunchuk** device type — different case.)

---

## Xbox → GameCube (in game)

| Xbox | GameCube | In Wind Waker |
|------|----------|---------------|
| A (green) | A | sword / action / wind |
| B (red) | B | roll / crawl (not jump) |
| X | X | item |
| Y | Y | secondary |
| LB | L (digital) | shield |
| RB | R | context / camera |
| Back | Z | use item / talk |
| Start | Start | pause |
| Left stick | Control stick | move |
| Right stick | C-stick | camera |

---

## Pre-session checklist

1. Pad LED **red**
2. XOutput running; HidHide hides raw Oklick
3. RetroArch **without Steam Input**
4. Content `.cfg` present for **your ISO file name**
5. LB = shield; **do not** map LT/RT axes for Oklick on GC titles unless you need analog L/R

---

## Files in this case

| Path | Purpose |
|------|---------|
| [`configs/dolphin-emu/Wind-Waker.cfg.example`](../../configs/dolphin-emu/Wind-Waker.cfg.example) | gamepad binds, null keyboard, no LT/RT, sensitivity 2×, C-stick invert |
| [`configs/retroarch/Controller (XBOX 360 For Windows) XOutput.cfg`](../../configs/retroarch/Controller%20(XBOX%20360%20For%20Windows)%20XOutput.cfg) | ViGEm autoconfig (shared with Case 001) |
| [`cases/001-oklick-gp315m-sotc-lrps2/`](../001-oklick-gp315m-sotc-lrps2/) | shared pad stack (HidHide, XOutput, Steam) |
| [`scripts/`](../../scripts/) | PadTest, CaptureSteps, launch without Steam |

---

## How we diagnosed (method)

1. Confirm wrong input: in-game only keyboard moves Link → content `.cfg` missing or not named after ISO  
2. `PadTest` / `CaptureSteps`: compare max \|LX\| vs \|LY\| at full tilt  
3. If forward/back slow only: check Dolphin GC **L-Analog** — try nulling `l2_axis` / `r2_axis`  
4. Tune `input_analog_sensitivity` per game before touching global `retroarch.cfg`

---

## One-line takeaway

Same Oklick stack as PS2, but **GameCube needs a content `.cfg`**, **digital L/R on LB/RB (not LT/RT axes)**, and often **2× analog sensitivity** — plus remember Wind Waker **does not jump**.
