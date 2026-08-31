# Retro gaming notes

Журнал практических кейсов: дешёвые геймпады, обёртки DInput→XInput, RetroArch / ядра эмуляторов, widescreen-хаки и всё, что ломается между «купил пад» и «нормально играется».

## Зачем это

У ретрогейминга на PC часто нет одной кнопки «сделать хорошо». Проблемы копятся слоями:

1. железо геймпада (Mode LED, оси-клоны)
2. Windows / HidHide / дубли устройств
3. XOutput / ViGEm / x360ce
4. Steam Input
5. RetroArch joypad driver + autoconfig
6. опции ядра (PCSX2/LRPS2 analog mode, axis scale, invert)
7. сама игра (порог бега, crouch на R1, in-game 16:9)

Этот репозиторий фиксирует **симптомы → причину → фикс**, без личных путей и дампов игр.

## Кейсы

| # | Кейс | Стек | Статус |
|---|------|------|--------|
| [001](cases/001-oklick-gp315m-sotc-lrps2/) | Oklick GP-315M + Shadow of the Colossus | XOutput, HidHide, RetroArch LRPS2 | решено |

Шаблон нового кейса: [`cases/_TEMPLATE.md`](cases/_TEMPLATE.md).

## Быстрые ссылки

- [Шпаргалка Oklick GP-315M](cheatsheets/oklick-gp315m.md)
- [Скрипты диагностики стиков](scripts/)
- [Примеры конфигов](configs/)

## Что сюда не кладём

- ISO / ROM / BIOS-файлы
- абсолютные пути вида `D:\Users\...`
- Steam Guard / логины / ключи

## Лицензия

Тексты и скрипты — на твоё усмотрение при публикации (рекомендуется MIT или CC-BY для заметок). Игры и BIOS — права правообладателей.
