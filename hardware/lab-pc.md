# Lab PC — reference machine

The config used to run cases in this repo (2026). Not “minimum requirements,” but **what was actually in use** so you can compare: weaker / similar / stronger than yours.

**Русский:** [lab-pc.ru.md](lab-pc.ru.md)

No personal paths, logins, or serial numbers are published.

## Hardware

| Component | Model / value |
|-----------|---------------|
| CPU | Intel Core **i5-12400F** (6P / 12 threads, Alder Lake) |
| GPU | NVIDIA GeForce **RTX 5060 Ti** |
| RAM | **32 GB** |
| OS | **Windows 11** x64, build **26200** |
| Monitor | **1920×1080 @ 144 Hz** (RetroArch fullscreen) |
| Storage | two SSD/HDD volumes ~465 GB + ~894 GB (games/dumps separate from OS) |

### Why this matters for emulation

- **i5-12400F + MTVU / multi-thread VU** — comfortable headroom for PS2 (LRPS2) at 2–3× native; bottleneck is usually GPU/upscale and the input stack, not CPU.
- **RTX 5060 Ti** — Vulkan/D3D in RetroArch/PCSX2 is fine at 1080p; 3× native (~1080p) for SotC is light load.
- **32 GB RAM** — plenty for Steam + RetroArch + XOutput + browser; 16 GB would be enough for LRPS2 alone.
- **1080p 144 Hz** — viewport/zoom in case 001 was tuned for this mode; on 1440p/4K you need to recalculate `custom_viewport_*`.

## Input peripherals

| Device | Role |
|--------|------|
| **Oklick GP-315M** (USB, DInput / Twin Shock clone) | primary gamepad |
| **XOutput** + **ViGEmBus** | DInput → virtual Xbox 360 (XInput) |
| **HidHide** | hides raw Oklick so you do not get two pads at once |

Mode LED: **red = analog** (play), green = digital (right stick = buttons). See [cheatsheets/oklick-gp315m.md](../cheatsheets/oklick-gp315m.md).

## Emulation software (at time of case 001)

| Software | Notes |
|----------|-------|
| **RetroArch** | ~1.22.x (Steam build on the machine; games often launched **outside Steam** so Steam Input does not break the pad) |
| Core | **LRPS2** / `pcsx2_libretro.dll` |
| Joypad driver | `xinput` |
| Case 001 target | Shadow of the Colossus (PS2), widescreen 16:9, 3× upscale |

Other emulators on disk (not documented as separate cases yet): PPSSPP, Dolphin, etc. — will appear as `cases/NNN-…` when there is something worth recording.

## Typical launch order (SotC)

1. Pad Mode → red  
2. XOutput → Start  
3. HidHide cloak ON  
4. RetroArch **without** Steam Input (or bat from `scripts/`)  
5. In-game: Options → Screen → 16:9  

## If your system is different

| Your setup | What to watch |
|------------|---------------|
| Weaker CPU (old dual/quad) | lower upscale, check MTVU, EE cycle; input fixes stay the same |
| Another DInput clone | same XOutput+HidHide stack; map axes via Configure / CaptureSteps |
| Native Xbox / 8BitDo Xbox mode | XOutput often unnecessary; Steam Input can still interfere |
| 1440p / 4K | do not copy viewport from case 001 verbatim |
| Laptop + iGPU | SotC at 2–3× may stutter — that is not a pad bug |

## Updates

When hardware or the stack changes significantly, update this page and the date below.

*Last snapshot update: 2026-09-01*
