# Oklick GP-315M — cheat sheet

**Русский:** [oklick-gp315m.ru.md](oklick-gp315m.ru.md)

## Mode indicator

| LED | Play? | Right stick |
|-----|-------|-------------|
| Red | yes | axes (camera) |
| Green | no | face buttons |

## Stack

1. ViGEmBus installed  
2. XOutput: Oklick profile → Start  
3. HidHide: hide raw Oklick, whitelist `XOutput.exe`  
4. Steam Input for emulator: Disable / launch without Steam  

## If “nothing works again”

1. LED red?  
2. XOutput Start?  
3. Not via Steam?  
4. In joy.cpl only one Xbox 360, raw Oklick not visible?  

## Axes (community)

Left X/Y, Right Z/Rz; Y often inverted.  
Better: XOutput → Edit → RX/RY → **Configure** and move the stick than guessing InputType by hand.

## SotC / LRPS2 (common toggles)

```ini
pcsx2_analog_mode1 = "enabled"
pcsx2_axis_scale1 = "200%"
pcsx2_axis_scale2 = "200%"
pcsx2_axis_deadzone1 = "0%"
pcsx2_axis_deadzone2 = "0%"
pcsx2_invert_left_stick1 = "y_axis"   ; if up/down mirrored
pcsx2_invert_right_stick1 = "x_axis"  ; if left/right mirrored
pcsx2_widescreen_hint = "enabled (16:9)"
```

In-game: Options → Screen → 16:9. R1 = crouch — do not confuse with “sneaking because of stick range.”
