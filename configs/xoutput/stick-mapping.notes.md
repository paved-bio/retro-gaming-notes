# XOutput — stick mapping notes (Oklick / Twin Shock clones)

**Русский:** [stick-mapping.notes.ru.md](stick-mapping.notes.ru.md)

Do not store full `settings.json` with device GUIDs — they are unique per machine.

## Recommended approach

1. Mode LED = red  
2. XOutput → Edit profile  
3. For **RX**: Configure → move right stick left-right to full deflection  
4. For **RY**: Configure → move right stick up-down  
5. Same for LX/LY with left stick  
6. Save → Start  

## Axis inversion in XOutput

If an axis is mirrored, swap `MinValue` and `MaxValue` on the mapper:

```json
"LY": {
  "Mappers": [
    {
      "InputType": "<detected>",
      "MinValue": 1.0,
      "MaxValue": 0.0,
      "Deadzone": 0.0
    }
  ],
  "CenterPoint": 0.5
}
```

For “sneaking in game,” set `Deadzone: 0.0` on LX/LY/RX/RY.

## Community reference (DarkScorpion ini → meaning)

| Xbox axis | Typical on clone |
|-----------|------------------|
| LX | Axis X |
| LY | Axis Y (invert) |
| RX | Axis Z |
| RY | Axis Rz (invert) |

InputType numbers in XOutput depend on version/driver — Configure is more reliable than hardcoding.

## HidHide

- Cloak: ON  
- Hidden devices: raw Oklick  
- Whitelisted applications: only `XOutput.exe` (full path on your machine)
