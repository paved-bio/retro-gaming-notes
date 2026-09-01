# XOutput — заметки по маппингу стиков (Oklick / Twin Shock клоны)

**English:** [stick-mapping.notes.md](stick-mapping.notes.md)

Не храним полный `settings.json` с GUID устройств — они уникальны на каждой машине.

## Рекомендуемый способ

1. Mode LED = красный  
2. XOutput → Edit профиля  
3. Для **RX**: Configure → правый стик влево-вправо до упора  
4. Для **RY**: Configure → правый стик вверх-вниз  
5. То же для LX/LY левым стиком  
6. Save → Start  

## Инверсия оси в XOutput

Если ось зеркальная, поменяй местами `MinValue` и `MaxValue` у маппера:

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

Для «крадётся в игре» поставь `Deadzone: 0.0` на LX/LY/RX/RY.

## Community-ориентир (DarkScorpion ini → смысл)

| Xbox ось | Типично на клоне |
|----------|------------------|
| LX | Axis X |
| LY | Axis Y (invert) |
| RX | Axis Z |
| RY | Axis Rz (invert) |

Номера InputType в XOutput зависят от версии/драйвера — Configure надёжнее хардкода.

## HidHide

- Cloak: ON  
- Hidden devices: сырой Oklick  
- Whitelisted applications: только `XOutput.exe` (полный путь на твоей машине)
