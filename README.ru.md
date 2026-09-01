# Retro gaming notes

Журнал практических кейсов: дешёвые геймпады, обёртки DInput→XInput, RetroArch / ядра эмуляторов, widescreen-хаки и всё, что ломается между «купил пад» и «нормально играется».

**English:** [README.md](README.md)

## Зачем это

У ретрогейминга на PC часто нет одной кнопки «сделать хорошо». Проблемы копятся слоями:

1. железо геймпада (Mode LED, оси-клоны)
2. Windows / HidHide / дубли устройств
3. XOutput / ViGEm / x360ce
4. Steam Input
5. RetroArch joypad driver + autoconfig
6. опции ядра (PCSX2/LRPS2 analog mode, axis scale, invert; Dolphin content `.cfg`, триггеры GC)
7. сама игра (порог бега, crouch на R1, in-game 16:9)

Этот репозиторий фиксирует **симптомы → причину → фикс**, без личных путей и дампов игр.

## Lab PC

Референсная машина (чтобы понимать, на чём снимались кейсы):

**i5-12400F · RTX 5060 Ti · 32 GB RAM · Windows 11 · 1080p 144 Hz · Oklick GP-315M + XOutput/ViGEm/HidHide**

Подробности: [`hardware/lab-pc.ru.md`](hardware/lab-pc.ru.md).

## Кейсы

| # | Кейс | Стек | Статус |
|---|------|------|--------|
| [001](cases/001-oklick-gp315m-sotc-lrps2/) | Oklick GP-315M + Shadow of the Colossus | XOutput, HidHide, RetroArch LRPS2 | решено |
| [002](cases/002-oklick-gp315m-wind-waker-dolphin/) | Oklick GP-315M + The Wind Waker | XOutput, HidHide, RetroArch Dolphin | решено |

Шаблон нового кейса: [`cases/_TEMPLATE.ru.md`](cases/_TEMPLATE.ru.md).

## Быстрые ссылки

- [Lab PC / железо](hardware/lab-pc.ru.md)
- [Шпаргалка Oklick GP-315M](cheatsheets/oklick-gp315m.ru.md)
- [Скрипты диагностики стиков](scripts/)
- [Примеры конфигов](configs/)

## Что сюда не кладём

- ISO / ROM / BIOS-файлы
- абсолютные пути вида `D:\Users\...`
- Steam Guard / логины / ключи

## Лицензия

Тексты и скрипты — на твоё усмотрение при публикации (рекомендуется MIT или CC-BY для заметок). Игры и BIOS — права правообладателей.
