# Кейс 002 — Oklick GP-315M + The Wind Waker (Dolphin / GameCube)

**English:** [README.md](README.md)

**Дата:** 2026-09  
**Платформа игры:** Nintendo GameCube  
**Эмуляция:** RetroArch (~1.22) + ядро Dolphin (`dolphin_libretro`)  
**Геймпад:** Oklick GP-315M (клон Twin Shock / DragonRise, DInput)  
**Обёртка:** XOutput + ViGEmBus → виртуальный Xbox 360  
**Скрытие сырого пада:** HidHide  
**Итог:** играет нормально (ходьба, камера, щит на LB)

**Машина:** i5-12400F, RTX 5060 Ti, 32 GB, Win11, 1080p@144 — см. [hardware/lab-pc.ru.md](../../hardware/lab-pc.ru.md).  
Тот же стек ввода, что в [кейсе 001](../001-oklick-gp315m-sotc-lrps2/); у Dolphin добавляются **аналоговые триггеры GameCube** и **конфиг под имя ISO**.

---

## Симптомы (как это выглядело)

| # | Симптом | Ощущение в игре |
|---|---------|-----------------|
| A | «Управление не работает» / в меню только клавиатура | Линк не ходит; двигается только WASD из глобального retroarch.cfg |
| B | Dolphin просит слот карты памяти | нельзя продолжить после экрана сохранения |
| C | Вперёд/назад ~50% скорости; влево/вправо нормально | стик вперёд до упора — медленная ходьба; стрейф — обычный бег |
| D | Камера на C-stick зеркалит влево/вправо | ось X правого стика наоборот |
| E | «Нет кнопки прыжка» | ждали прыжок на A как в Mario — в WW прыжка нет (B = перекат) |

Проблемы A и C кажутся разными, но обе из **того, как RetroArch + Dolphin читают этот пад**.

---

## Железо и софт (обезличено)

Та же цепочка, что в кейсе 001 — меняется только ядро:

```
[Oklick GP-315M USB] → XOutput → ViGEm Xbox 360 → RetroArch (xinput) → Dolphin → Wind Waker (GC)
```

**Mode LED = красный** (аналоговый правый стик). См. [cheatsheets/oklick-gp315m.ru.md](../../cheatsheets/oklick-gp315m.ru.md).

Запуск **без Steam Input** (как в кейсе 001).

---

## Корневые причины по слоям

### 1) Глобальные клавиатурные бинды, нет gamepad override

В `retroarch.cfg` движение было на **клавиатуре** (`WASD`), а кнопки геймпада — `"nul"`.  
Одного autoconfig мало, если **конфиг контента** не переопределяет player 1 для этого ISO.

Dolphin подхватывает конфиг так:

```text
<RetroArch>/config/dolphin-emu/<ИМЯ_ISO>.cfg
```

Пример: `Zelda_Wind_Waker.iso` → `Zelda_Wind_Waker.iso.cfg`

**Фикс:** content `.cfg` с полными `input_player1_*` и обнулённой клавиатурой — см. [`configs/dolphin-emu/Wind-Waker.cfg.example`](../../configs/dolphin-emu/Wind-Waker.cfg.example).

### 2) Нет карты памяти GC (первый запуск)

Dolphin libretro ждёт файлы сохранений в своей user-папке. Пустой слот → экран «нет карты памяти».

**Фикс:** один раз дать Dolphin создать/отформатировать карту, или положить пустой `MemoryCardA.USA.raw` (2 MB). **Не коммитить** сейвы в репозиторий.

### 3) Медленно вперёд/назад, быстро в стороны — «полу-L» на триггере

На GameCube **L** и **R** — **анalogовые** курки. Dolphin мапит:

- ось **L2** (Xbox LT) → аналог L на GC  
- полу-L → щит / более медленное движение в Wind Waker

У Oklick **LT/RT в XOutput — цифровые кнопки**, но мы сначала повесили `input_player1_l2_axis = "+4"` как на обычном Xbox. Вместе с шумом по оси Y Dolphin мог видеть **частичный L** при движении вперёд/назад — Линк идёт медленно. Стрейф (ось X) не давал тот же эффект → «вбок быстро, вперёд медленно».

**Фикс для этой игры:**

```ini
input_player1_l2_axis = "nul"
input_player1_r2_axis = "nul"
# L/R только цифровые на LB/RB:
input_player1_l_btn = "4"
input_player1_r_btn = "5"
```

Плюс чувствительность стика для порога walk/run:

```ini
input_analog_sensitivity = "2.000000"
```

Oklick часто отдаёт ~**±16383** вместо ±32767 на полном наклоне (как в кейсе 001). Wind Waker по **наклону** стика различает шаг и бег; половина диапазона по Y = постоянная «ходьба» вперёд/назад.

Если после null LT/RT и 2× всё ещё медленно — перепроверить LY в XOutput ([stick-mapping.notes.ru.md](../../configs/xoutput/stick-mapping.notes.ru.md)).

### 4) C-stick: ось X камеры инвертирована

**Фикс только в content `.cfg`:**

```ini
input_player1_r_x_plus_axis = "-2"
input_player1_r_x_minus_axis = "+2"
```

Поменяй знаки, если у тебя наоборот.

### 5) «Нет прыжка» — это норма

В Wind Waker (GameCube) **нет кнопки прыжка**. **B** = перекат/ползание; **A** = меч/действие. Это не баг маппинга.

(Игры Wii вроде Mario Galaxy требуют тип **Wiimote + Nunchuk** — отдельный кейс.)

---

## Xbox → GameCube (в игре)

| Xbox | GameCube | В Wind Waker |
|------|----------|--------------|
| A (зелёная) | A | меч / действие / ветер |
| B (красная) | B | перекат (не прыжок) |
| X | X | предмет |
| Y | Y | вторичное |
| LB | L (цифровой) | щит |
| RB | R | контекст / камера |
| Back | Z | использовать предмет |
| Start | Start | пауза |
| Левый стик | Control stick | движение |
| Правый стик | C-stick | камера |

---

## Чеклист перед сессией

1. LED пада **красный**
2. XOutput запущен; HidHide прячет сырой Oklick
3. RetroArch **без Steam Input**
4. Content `.cfg` лежит под **именем твоего ISO**
5. LB = щит; **не** вешать оси LT/RT на Oklick для GC, если не нужен analog L/R

---

## Файлы в кейсе

| Путь | Назначение |
|------|------------|
| [`configs/dolphin-emu/Wind-Waker.cfg.example`](../../configs/dolphin-emu/Wind-Waker.cfg.example) | бинды геймпада, null клавиатуры, без LT/RT, sensitivity 2×, инверт C-stick |
| [`configs/retroarch/Controller (XBOX 360 For Windows) XOutput.cfg`](../../configs/retroarch/Controller%20(XBOX%20360%20For%20Windows)%20XOutput.cfg) | autoconfig ViGEm (общий с кейсом 001) |
| [`cases/001-oklick-gp315m-sotc-lrps2/`](../001-oklick-gp315m-sotc-lrps2/) | общий стек пада (HidHide, XOutput, Steam) |
| [`scripts/`](../../scripts/) | PadTest, CaptureSteps, запуск без Steam |

---

## Как диагностировали

1. В игре двигается только клавиатура → нет или неверное имя content `.cfg`  
2. `PadTest` / `CaptureSteps`: сравнить max \|LX\| и \|LY\| на полном наклоне  
3. Если медленно только вперёд/назад: проверить **L-Analog** в Dolphin — обнулить `l2_axis` / `r2_axis`  
4. Крутить `input_analog_sensitivity` **на игру**, не ломая глобальный `retroarch.cfg`

---

## Вывод в одну строку

Тот же Oklick, что и на PS2, но для **GameCube нужен content `.cfg`**, **L/R на LB/RB без LT/RT осей** и часто **2× analog sensitivity** — и помни: в Wind Waker **нет прыжка**.
