# Case 001 — Oklick GP-315M + Shadow of the Colossus (LRPS2)

**Date:** 2026-09  
**Game platform:** PS2  
**Emulation:** RetroArch (~1.22) + LRPS2 core (`pcsx2_libretro`)  
**Gamepad:** Oklick GP-315M (Twin Shock / DragonRise clone, DInput)  
**Wrapper:** XOutput + ViGEmBus → virtual Xbox 360  
**Raw pad hidden with:** HidHide  
**Result:** plays correctly (run, camera, widescreen)

**Machine:** i5-12400F, RTX 5060 Ti, 32 GB, Win11, 1080p@144 — see [hardware/lab-pc.md](../../hardware/lab-pc.md).  
On this hardware, 3× native + widescreen for SotC had no FPS issues; almost all pain in this case was **input**, not performance.

**Русский:** [README.ru.md](README.ru.md)

---

## Symptoms (what it looked like)

Problems came in waves — fix one thing, the next broke.

| # | Symptom | In-game feel |
|---|---------|--------------|
| A | “Nothing moves / camera dead” | right stick seems to not exist |
| B | Broken again after Steam launch | axes/buttons drift or duplicate |
| C | Wander “sneaks and feels sluggish” | left stick at full tilt = walk, not run |
| D | Mirrored axes | down = forward; right on right stick = camera left |
| E | Black bars / edge seam at 16:9 | picture not full screen or artifact on the right |

Important: this is **not one** bad setting. It is a chain.

---

## Hardware and software (sanitized)

```
[Oklick GP-315M USB]
        |
        +--(raw DInput)-- Windows / Steam / RetroArch   ← HIDE THIS
        |
        v
   [XOutput]  --maps-->  [ViGEm]  -->  "Controller (XBOX 360 For Windows)"
                                              |
                                              v
                                    RetroArch (joypad: xinput)
                                              |
                                              v
                                         LRPS2 / SotC
```

### Mode LED on Oklick (critical)

On these pads, Mode is **hardware** — there is no firmware to “make it Xbox forever.”

| LED | Mode | Right stick |
|-----|------|-------------|
| **Red** | analog | RX/RY axes → camera |
| **Green** | digital | right stick emulates face buttons 1–4 |

If LED is green — camera “lags,” button duplicates, diagnostics lie. Play only on **red**.

Community axis map (DarkScorpion / Twin Shock clones):

- Left: X / Y (Y often inverted)
- Right: Z / Rz (in XOutput these are usually separate InputTypes; Y inverted)

Reference: [DarkScorpion/Oklick_GP-315m](https://github.com/DarkScorpion/Oklick_GP-315m)

---

## Root causes by layer

### 1) Two controllers at once

Windows sees both raw Oklick and virtual Xbox. RetroArch/game may read the wrong device or mix axes.

**Fix:** HidHide — cloak ON, hide raw Oklick (`VID_04D9` / sometimes `VID_11FF` on revisions), whitelist only `XOutput.exe`.

### 2) Steam Input

Steam periodically resets controller policy for RetroArch and layers its own mapping on top of XInput.

**Fix:**

- Steam → RetroArch → Controller → **Disable Steam Input** / Force Off  
- or launch RetroArch **outside Steam** (see `scripts/launch-game-without-steam.bat`)

### 3) DualShock analog mode in LRPS2

Core override had:

```ini
pcsx2_analog_mode1 = "disabled"
```

Without analog mode, many PS2 games (including SotC) poorly or never accept sticks.

**Fix:**

```ini
pcsx2_analog_mode1 = "enabled"
```

### 4) Incomplete stick range → “sneaking”

XInput measurement showed roughly **±16383** at full tilt instead of **±32767** (~50% travel).

SotC (and some other PS2 titles) require near-full stick deflection to **run**. Half travel = walk/creep.

Known PCSX2 class of issues: circular sticks / incomplete range / diagonals → walk instead of run  
(see [PCSX2#6230](https://github.com/PCSX2/pcsx2/issues/6230), threads on Analog Sensitivity / Axis Scale).

**Fix in LRPS2 override:**

```ini
pcsx2_axis_scale1 = "200%"
pcsx2_axis_scale2 = "200%"
pcsx2_axis_deadzone1 = "0%"
pcsx2_axis_deadzone2 = "0%"
```

Plus deadzone in XOutput on LX/LY/RX/RY = `0`.

If still walking after 200% — recalibrate sticks in XOutput → Configure (move to full deflection), check Mode = red, R1 not held (in SotC R1 = crouch/grab).

### 5) Axis inversion

After range was fixed:

- left stick: down = forward, up = backward  
- right stick: right = camera left (and vice versa)

**Fix for this game only (`.opt`):**

```ini
pcsx2_invert_left_stick1 = "y_axis"
pcsx2_invert_right_stick1 = "x_axis"
```

Core values: `disabled` | `x_axis` | `y_axis` | `all`.

Alternative — swap Min/Max in XOutput or axis signs in RetroArch binds; for one game, invert in `.opt` is simpler.

### 6) Xbox 360 autoconfig / Wireless PID

ViGEm presents as wired Xbox 360. RetroArch sometimes picks the **Wireless** profile with a different `input_product_id` and wrong binds.

**Fix:** explicit autoconfig with `input_product_id = "654"` (wired) — see `configs/retroarch/`.

### 7) Widescreen / edge seam

Separate from the pad:

- in-game: Options → Screen → **16:9**
- core: `pcsx2_widescreen_hint = "enabled (16:9)"`
- RetroArch: aspect ~16:9; SotC upscale has known seams — community workaround Zoom ≈ **102.6%** (via custom viewport)

BIOS not included in examples; you need a compatible dump (often USA). Do not commit BIOS/ISO files to the repo.

---

## Pre-session checklist

1. Pad LED **red**
2. XOutput running, Oklick profile → **Start**
3. HidHide cloak ON, raw pad hidden
4. Steam Input for RetroArch off **or** launch without Steam
5. In-game Screen → 16:9
6. If “sneaking” — check R1 and axis scale

---

## Files in this case

| Path | Purpose |
|------|---------|
| [`configs/lrps2/Shadow of the Colossus.opt.example`](../../configs/lrps2/Shadow%20of%20the%20Colossus.opt.example) | analog + scale 200% + invert |
| [`configs/lrps2/Shadow of the Colossus.cfg.example`](../../configs/lrps2/Shadow%20of%20the%20Colossus.cfg.example) | aspect / viewport / binds |
| [`configs/retroarch/Controller (XBOX 360 For Windows) XOutput.cfg`](../../configs/retroarch/Controller%20(XBOX%20360%20For%20Windows)%20XOutput.cfg) | ViGEm autoconfig |
| [`configs/xoutput/stick-mapping.notes.md`](../../configs/xoutput/stick-mapping.notes.md) | how to map axes |
| [`scripts/`](../../scripts/) | launch without Steam, CaptureSteps, PadTest |
| [`cheatsheets/oklick-gp315m.md`](../../cheatsheets/oklick-gp315m.md) | short cheat sheet |

---

## How we diagnosed (method)

Do not guess JSON blindly:

1. `PadTest` — live LX/LY/RX/RY stream for 10 sec  
2. `CaptureSteps` — step log “do action → what XInput saw”  
3. Compare max |axis| to 32767 → decide if axis scale is needed  
4. Red vs green Mode — separate CaptureSteps run

Expected at full stick tilt: values around **20000–32767**. If stable ~16000 — SotC will walk without scale.

---

## One-line takeaway

Oklick stays a DInput clone forever: **red Mode + XOutput + HidHide + no Steam Input + analog mode + tune sensitivity/invert per game**.

Long term, a native XInput pad (Xbox / 8BitDo in Xbox mode) is simpler. This case is about squeezing a clone.
