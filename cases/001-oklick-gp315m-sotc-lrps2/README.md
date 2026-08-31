# Кейс 001 — Oklick GP-315M + Shadow of the Colossus (LRPS2)

**Дата:** 2026-09  
**Платформа игры:** PS2  
**Эмуляция:** RetroArch (~1.22) + ядро LRPS2 (`pcsx2_libretro`)  
**Геймпад:** Oklick GP-315M (клон Twin Shock / DragonRise, DInput)  
**Обёртка:** XOutput + ViGEmBus → виртуальный Xbox 360  
**Скрытие сырого пада:** HidHide  
**Итог:** играет нормально (бег, камера, widescreen)

**Машина:** i5-12400F, RTX 5060 Ti, 32 GB, Win11, 1080p@144 — см. [hardware/lab-pc.md](../../hardware/lab-pc.md).  
На этом железе 3× native + widescreen для SotC — без упирания в FPS; почти все боли кейса были в **вводе**, не в производительности.

---

## Симптомы (как это выглядело)

Проблемы шли волнами — чинили одно, всплывало следующее.

| # | Симптом | Ощущение в игре |
|---|---------|-----------------|
| A | «Ничего не ходит / камера мёртвая» | правый стик будто не существует |
| B | После запуска из Steam снова сломано | оси/кнопки плывут или дублируются |
| C | Wander «крадётся и тупит» | левый стик в край — всё равно шаг, не бег |
| D | Зеркальные оси | вниз = вперёд; вправо на правом стике = камера влево |
| E | Чёрные полосы / шов по краю при 16:9 | картинка не на весь экран или артефакт справа |

Важно: это **не одна** баговая настройка. Это цепочка.

---

## Железо и софт (обезличено)

```
[Oklick GP-315M USB]
        |
        +--(сырой DInput)-- Windows / Steam / RetroArch   ← ХОТИМ СПРЯТАТЬ
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

### Mode LED на Oklick (критично)

На этих падах Mode — **аппаратный**, прошивки «сделать навсегда Xbox» нет.

| LED | Режим | Правый стик |
|-----|--------|-------------|
| **Красный** | analog | оси RX/RY → камера |
| **Зелёный** | digital | правый стик эмулирует face-кнопки 1–4 |

Если LED зелёный — камера «тупит», дубли кнопок, диагностика врёт. Играть только на **красном**.

Community-мап осей (DarkScorpion / клоны Twin Shock):

- Left: X / Y (Y часто инвертирован)
- Right: Z / Rz (в XOutput это обычно отдельные InputType; Y инверт)

Референс: [DarkScorpion/Oklick_GP-315m](https://github.com/DarkScorpion/Oklick_GP-315m)

---

## Корневые причины по слоям

### 1) Два контроллера одновременно

Windows видит и сырой Oklick, и виртуальный Xbox. RetroArch/игра могут читать не тот девайс или мешать оси.

**Фикс:** HidHide — cloak ON, спрятать сырой Oklick (`VID_04D9` / иногда `VID_11FF` у ревизий), whitelist только `XOutput.exe`.

### 2) Steam Input

Steam для RetroArch периодически сбрасывает политику контроллера и накладывает свою раскладку поверх XInput.

**Фикс:**

- Steam → RetroArch → Controller → **Disable Steam Input** / Force Off  
- или запуск RetroArch **мимо Steam** (см. `scripts/launch-game-without-steam.bat`)

### 3) Analog mode DualShock в LRPS2

В override ядра стояло:

```ini
pcsx2_analog_mode1 = "disabled"
```

Без analog mode многие PS2-игры (включая SotC) плохо/никак не принимают стики.

**Фикс:**

```ini
pcsx2_analog_mode1 = "enabled"
```

### 4) Неполный диапазон стика → «крадётся»

Замер через XInput показал примерно **±16383** при упоре вместо **±32767** (~50% хода).

SotC (и ряд других PS2) для **бега** требует почти полный ход. Половина = walk/creep.

Это известный класс проблем PCSX2: circular sticks / неполный range / диагонали → walk вместо run  
(см. [PCSX2#6230](https://github.com/PCSX2/pcsx2/issues/6230), треды про Analog Sensitivity / Axis Scale).

**Фикс в LRPS2 override:**

```ini
pcsx2_axis_scale1 = "200%"
pcsx2_axis_scale2 = "200%"
pcsx2_axis_deadzone1 = "0%"
pcsx2_axis_deadzone2 = "0%"
```

Плюс deadzone в XOutput на LX/LY/RX/RY = `0`.

Если после 200% всё ещё walk — перекалибровать стики в XOutput → Configure (поводить до упора), проверить Mode = красный, R1 не зажат (в SotC R1 = crouch/grab).

### 5) Инверсия осей

После того как диапазон «заиграл»:

- левый стик: вниз = вперёд, вверх = назад  
- правый стик: вправо = камера влево (и наоборот)

**Фикс только для этой игры (`.opt`):**

```ini
pcsx2_invert_left_stick1 = "y_axis"
pcsx2_invert_right_stick1 = "x_axis"
```

Значения ядра: `disabled` | `x_axis` | `y_axis` | `all`.

Альтернатива — крутить Min/Max в XOutput или знаки осей в RetroArch binds; для одной игры удобнее invert в `.opt`.

### 6) Autoconfig Xbox 360 / Wireless PID

ViGEm представляется как wired Xbox 360. Иногда RetroArch подхватывает профиль **Wireless** с другим `input_product_id` и кривыми binds.

**Фикс:** явный autoconfig с `input_product_id = "654"` (wired) — см. `configs/retroarch/`.

### 7) Widescreen / шов по краю

Отдельно от пада:

- in-game: Options → Screen → **16:9**
- ядро: `pcsx2_widescreen_hint = "enabled (16:9)"`
- RetroArch: aspect ~16:9; при upscale у SotC известны швы — community workaround Zoom ≈ **102.6%** (через custom viewport)

BIOS в примере не прилагаем; нужен совместимый дамп (часто USA). Файлы BIOS/ISO в репозиторий не класть.

---

## Рабочий чеклист перед сессией

1. LED геймпада **красный**
2. XOutput запущен, профиль Oklick → **Start**
3. HidHide cloak ON, сырой пад скрыт
4. Steam Input для RetroArch выключен **или** запуск без Steam
5. В игре Screen → 16:9
6. Если «крадётся» — проверить R1 и axis scale

---

## Файлы в этом кейсе

| Путь | Зачем |
|------|--------|
| [`configs/lrps2/Shadow of the Colossus.opt.example`](../../configs/lrps2/Shadow%20of%20the%20Colossus.opt.example) | analog + scale 200% + invert |
| [`configs/lrps2/Shadow of the Colossus.cfg.example`](../../configs/lrps2/Shadow%20of%20the%20Colossus.cfg.example) | aspect / viewport / binds |
| [`configs/retroarch/Controller (XBOX 360 For Windows) XOutput.cfg`](../../configs/retroarch/Controller%20(XBOX%20360%20For%20Windows)%20XOutput.cfg) | autoconfig ViGEm |
| [`configs/xoutput/stick-mapping.notes.md`](../../configs/xoutput/stick-mapping.notes.md) | как мапить оси |
| [`scripts/`](../../scripts/) | запуск без Steam, CaptureSteps, PadTest |
| [`cheatsheets/oklick-gp315m.md`](../../cheatsheets/oklick-gp315m.md) | короткая шпаргалка |

---

## Как диагностировали (метод)

Не гадать JSON вслепую:

1. `PadTest` — живой поток LX/LY/RX/RY 10 сек  
2. `CaptureSteps` — пошаговый лог «сделай действие → что увидел XInput»  
3. Сравнить max |оси| с 32767 → понять нужен ли axis scale  
4. Красный vs зелёный Mode — отдельный прогон CaptureSteps

Ожидание при упоре стика: величины порядка **20000–32767**. Если стабильно ~16000 — SotC будет walk без scale.

---

## Вывод одной фразой

Oklick остаётся DInput-клоном навсегда: **Mode красный + XOutput + HidHide + без Steam Input + analog mode + добить sensitivity/invert под конкретную игру**.

Долгосрочно для «просто работает» проще пад с родным XInput (Xbox / 8BitDo в Xbox mode). Этот кейс — про то, как выжать клон.
