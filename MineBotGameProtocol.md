### Protocol

# Глобальные параметры игрока

- максимальное количество юнитов
- текущее количество юнитов
- максимальное количество баз
- текущее количество баз
- максимальное количество ресурсов
- текущее количество ресурсов
- количество зданий?

## Протокол (клиент)

| **Название**        | **Аргументы** | **Описание** |
| ---                 | ---           | ---          |
| **UpdateState**     | **UnitMaxCount** : int, **UnitCount:** int, **BaseMaxCount:** int, **BaseCount:** int, **ResMaxCount:** int, **ResCount:** int, **EnergyFlow** int| Возникает при изменении глобальных параметров игрока, до его хода. **UnitMaxCount** – актуальное максимальное количество юнитов игрока, **UnitCount -**  актуальное текущее количество юнитов игрока, **BaseMaxCount -**  актуальное максимальное количество баз игрока,  **BaseCount -** актуальное текущее количество баз игрока, **ResMaxCount -** актуальное максимальное количество ресурсов игрока, **ResCount -** актуальное текущее количество ресурсов игрока, **EnergyFlow** - актуальный вырабатываемый поток энергии игрока, **EnergyUse** - актуальный потребляемый поток энергии игрока |
| **OpponentActions** | **ActionList** : object | Возникает после хода каждого оппонента. **ActionList** - список действий оппонента |

# Действия игрока

- Добыть ресурс
- Построить здание
- Построить юнита
- Сделать апгрейд
- Создать модуль
- Просканировать комнату
- Простой _(не будет описан. Если от объекта не была получена команда, значит автоматически простой)_
- Саморазрушение

_Как мне кажется, все должно контролиться серваком. Т.е. там же должна быть инфа о том, сколько ходов длится каждое из действий. Соответственно клиент на каждом ходу будет &quot;подтверждать&quot; продолжение длительного действия (скорее всего передавая ту же команду), сервак же в свою очередь будет считать и кидать уведомление о завершении действия + результат (увеличение кол-ва ресурсов и все такое)._

## Протокол (сервер)

| **Название**   | **Аргументы** | **Описание** |
| ---            | ---           | ---          |
| **Mine**       | **UnitId** : int, **X:** int, **Y** : int | Возникает, когда игрок добывает ресурс. **UnitId** – идентификатор юнита, который майнит; **X** , **Y** – координаты ресурса. |
| **Build**      | **Type** : int, **X:** int, **Y** : int, **UnitId:** int | Возникает, когда игрок строит здание. **Type** – тип постройки, **X** , **Y**– координаты какого-то угла постройки; **UnitId** - идентификатор юнита, который создает постройку. |
| **CreateObject** | **ObjectType** : int, **ObjectId** : int, **BuildingId:** int | Возникает, когда игрок создает юнита. **ObjectType** – тип объекта, **ObjectId** - идентификатор объекта, **Building**  **Id**  **–** идентификатор постройки, в которой производится объект. |
| **Upgrade**    | **Type** : int, **ObjectType:** int, **ObjectId** : int | Возникает, когда игрок делает апгрейд. **Type** – тип апгрейда, **Object**  **Type** – тип объекта, для которого производится апгрейд, **Object**  **Id**  **–** идентификатор объекта, для которого производится апгрейд |
| **Move**       | **Type:**  int, **ObjectId:** int, **Direction:** int, **Count:** int | Возникает, когда игрок перемещает объект. **Type** - тип объекта, **ObjectId** - идентификатор объекта, **Direction** - направление движения, **Count** - количество шагов в данном направлении. |
| **Hit**        | **ObjectType:**  int, **ObjectId:** int, **TargetId:** int, **Type:** int | Возникает при ударе(починке) объектом игрока другого объекта. **ObjectType** - тип объекта, **ObjectId** - идентификатор объекта, **TargetId** - идентификатор цели, **Type** - тип удара(починки). |
| **SelfDestruction** | **ObjectId** : int | Возникает при самоуничтожении объекта игрока. **ObjectId** - идентификатор объекта |
| **Do**  (?)    | **CommandList**  {	**Id**: int,   **Object{}**} | Ход игрока. **CommandList** - список команд, **Id** - идентификатор команды, **Object** - объект, содержащий параметры команды. |

## Протокол (клиент)

| **Название**           | **Аргументы** | **Описание** |
| ---                    | ---           | ---          |
| **ChangeResCount**     | **Type:** int, **Count:** int/double? | Возникает, когда у игрока увеличивается/уменьшается количество ресурса. **Type**  **–** тип ресурса, **Count** – количество, на которое увеличился ресурс (при уменьшении количества ресурса передается отрицательное значение). |
| **ChangeUnitResCount** | **Type:** int, **UnitId:** int, **Count:** int/double? | Возникает, когда у юнита игрока увеличивается/уменьшается количество ресурса. **Type**  **–** тип ресурса, **UnitId** - идентификатор юнита, **Count** – количество, на которое увеличился ресурс (при уменьшении количества ресурса передается отрицательное значение). |
| **ChangeObjectHP**     | **ObjectId:** int, **Count** : int/double | Возникает при изменении очков здоровья у объекта игрока. **ObjectId** - идентификатор объекта, **Count** - количество добавленных HP (может быть отрицательным). |
| **AddUnit**            | **Type:** int, **X:** int, **Y:** it, **Id:** int | Возникает, когда у игрока добавляется юнит. **Type**  **–** тип юнита, **X** , **Y**– координатыпоявления юнита;** Id** – идентификатор юнита |
| **AddModule**          | **Type:** int, **BuildingId:** int | Возникает, когда у игрока добавляется модуль. **Type**  **–** тип модуля, **Building**  **Id**  **–** идентификатор постройки, в которой добавился модуль. |
| **AddBuilding**        | **Type:** int, **X:** int, **Y:** int, **Id:** int | Возникает, когда у игрока добавляется постройка. **Type**  **–** тип постройки, **X** , **Y**– координаты какого-то угла постройки;** Id** – идентификатор постройки. |
| **AddUpgrade**         | **Type** : int, **ObjectType:** int, **ObjectId** : int | Возникает, когда у игрока добавляется апгрейд. **Type** – тип апгрейда, **Object**  **Type** – тип объекта, для которого производится апгрейд, **Object**  **Id**  **–** идентификатор объекта, для которого производится апгрейд |
| **Hit**                | **PlayerId:** int, **ObjectId:** int, **TargetId:** int, **Damage:** int | Возникает при ударе (починке) объектом игрока другого объекта. **PlayerId** - идентификатор игрока, чей объект наносит урон(чинит) объекту игрока. **ObjectId** - идентификатор объекта, **TargetId** - идентификатор цели, **Damage** - наносимый урон (в случае починки значение будет отрицательным). |
| **DestroyObject**      | **ObjectId:** int | Возникает при уничтожении объекта. **ObjectId** - идентификатор объекта |


## My version (Updated)

### Data Types

Name        | Description
------------|-----------------
Vector      | Аналог - Vector2 в c#
IVector     | Целочисленный вектор
Id          | int
Type        | int (имеет соответствуующий enum)
int         | 32х битный целочисленный знаковый численный тип

### Client to server commands

CommandName          | Parameters        | Type                   | Description
---------------------|-------------------|------------------------|--------------
Idle                 | Nothin'           | PlayerAction           | Ну собсно.... Зачем? хз.
Move                 | Id, IVector       | PlayerActionVectorized | Толи волю тратить, то ли замораживать..
RangeHit             | Id, IVector       | PlayerActionVectorized | Толи волю тратить, то ли замораживать..
MeleeHit             | Id, IVector       | PlayerActionVectorized | Толи волю тратить, то ли замораживать..
BuildStart           | Id, IVector, Type | PlayerActionBuild      | Приступаем к строительству... Юнит обездвиживается
BuildEnd             | Id                | PlayerActionObject     | Бросаем строительство, теперь можно двигаться 
StartResearch        | Id, Type          | PlayerActionOperation  | Здание начинает исследование (помещает в очередь), ресурсы берём сразу
StartUnitSpawn       | Id, Type          | PlayerActionOperation  | Здание начинает создание юнита (помещает в оцередь), 
StartUnitUpgrade     | Id, Id, Type      | PlayerActionUpgrade    | Здание начинает апгрейд юнита (помещает в очередь), ресурсы берём сразу. Юнит должен быть рядом, обездвиживается (сразу, при помещении в очередь). 
CancelBuildingAction | Id, int           | PlayerActionCancel     | Здание отменает действие, ресурсы высвобождаются, и прочая фигня
SelfDestruct         | Id                | PlayerActionObject     | Бум


# Возможные значения переменных

## Тип ресурса (00)

| Идентификатор | Название |
| ---           | ---      |
| 0000          | Iron     |
| 0001          | Silicon  |
| 0002          | Uranium  |
| 0003          | Quartz   |

## Тип постройки (01)

| Идентификатор | Название      |
| ---           | ---           |
| 0100          | Base          |
| 0101          | Storage       |
| 0102          | ModuleFactory |
| 0103          | Workshop      |

## Тип объекта (02)

| Идентификатор | Название |
| ---           | ---      |
| 0200          | Building |
| 0201          | Unit     |
| 0202          | Module   |

## Тип модуля (03)

| Идентификатор | Название     |
| ---           | ---          |
|               |              |

## Тип юнита (04)

| Идентификатор | Название     |
| ---           | ---          |
|               |              |

## Тип апгрейда (05)

| Идентификатор | Название     |
| ---           | ---          |
|               |              |

## Тип удара (06)

| Идентификатор | Название     |
| ---           | ---          |
|               |              |