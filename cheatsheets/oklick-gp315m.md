# Oklick GP-315M — шпаргалка

## Индикатор Mode

| LED | Играть? | Правый стик |
|-----|---------|-------------|
| Красный | да | оси (камера) |
| Зелёный | нет | face-кнопки |

## Стек

1. ViGEmBus установлен  
2. XOutput: профиль Oklick → Start  
3. HidHide: скрыть сырой Oklick, whitelist `XOutput.exe`  
4. Steam Input для эмулятора: Disable / запуск без Steam  

## Если «снова ничего не работает»

1. LED красный?  
2. XOutput Start?  
3. Не через Steam?  
4. В joy.cpl один Xbox 360, сырого Oklick не видно?  

## Оси (community)

Left X/Y, Right Z/Rz; Y часто инвертировать.  
Лучше: XOutput → Edit → RX/RY → **Configure** и поводить стиком, чем руками угадывать InputType.

## SotC / LRPS2 (частые тумблеры)

```ini
pcsx2_analog_mode1 = "enabled"
pcsx2_axis_scale1 = "200%"
pcsx2_axis_scale2 = "200%"
pcsx2_axis_deadzone1 = "0%"
pcsx2_axis_deadzone2 = "0%"
pcsx2_invert_left_stick1 = "y_axis"   ; если вверх/вниз зеркало
pcsx2_invert_right_stick1 = "x_axis"  ; если влево/вправо зеркало
pcsx2_widescreen_hint = "enabled (16:9)"
```

В игре: Options → Screen → 16:9. R1 = crouch — не путать с «крадётся из‑за стика».
