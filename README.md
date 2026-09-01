# Retro gaming notes

A practical case log for retro gaming on PC: cheap gamepads, DInput→XInput wrappers, RetroArch / emulator cores, widescreen hacks, and everything that breaks between “I bought a pad” and “it actually plays.”

**Русский:** [README.ru.md](README.ru.md)

## Why this exists

Retro gaming on PC rarely has a single “make it work” button. Problems stack in layers:

1. gamepad hardware (Mode LED, clone axis layouts)
2. Windows / HidHide / duplicate devices
3. XOutput / ViGEm / x360ce
4. Steam Input
5. RetroArch joypad driver + autoconfig
6. core options (PCSX2/LRPS2 analog mode, axis scale, invert; Dolphin content `.cfg`, GC triggers)
7. the game itself (run threshold, R1 crouch, in-game 16:9)

This repo records **symptoms → root cause → fix**, without personal paths or game dumps.

## Lab PC

Reference machine (so you know what hardware the cases were tested on):

**i5-12400F · RTX 5060 Ti · 32 GB RAM · Windows 11 · 1080p 144 Hz · Oklick GP-315M + XOutput/ViGEm/HidHide**

Details: [`hardware/lab-pc.md`](hardware/lab-pc.md).

## Cases

| # | Case | Stack | Status |
|---|------|-------|--------|
| [001](cases/001-oklick-gp315m-sotc-lrps2/) | Oklick GP-315M + Shadow of the Colossus | XOutput, HidHide, RetroArch LRPS2 | fixed |
| [002](cases/002-oklick-gp315m-wind-waker-dolphin/) | Oklick GP-315M + The Wind Waker | XOutput, HidHide, RetroArch Dolphin | fixed |

New case template: [`cases/_TEMPLATE.md`](cases/_TEMPLATE.md).

## Quick links

- [Lab PC / hardware](hardware/lab-pc.md)
- [Oklick GP-315M cheat sheet](cheatsheets/oklick-gp315m.md)
- [Stick diagnostic scripts](scripts/)
- [Sample configs](configs/)

## What we do not commit

- ISO / ROM / BIOS files
- absolute paths like `D:\Users\...`
- Steam Guard / logins / keys

## License

Notes and scripts — your choice when publishing (MIT or CC-BY recommended for docs). Games and BIOS files remain property of their rights holders.
