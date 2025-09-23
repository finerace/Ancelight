<p align="right">
  <img src="https://flagpedia.net/data/flags/w20/gb.png" width="20" alt="English">
  <a href="#hi-this-is-ancelightmy-old-project-where-i-implemented-all-the-mechanics-of-a-classic-fps-from-scratch"> English Version</a>
</p>

<p align="center">
  <!-- Вставь сюда логотип или баннер проекта, если есть -->
  <img src="Assets/UI/Sprites/Other/icon.png" width="300" alt="Ancelight Logo">
</p>

<h3 align="center">Привет! Это "Ancelight" — мой старый проект, в котором я с нуля реализовал все механики классического FPS-шутера.</h3>

<p align="center">
  <a href="https://drive.google.com/file/d/16jbOWAHsmiOoD8lNHU3QDHs1_p4yVWv1/view?usp=sharing" target="_blank">
    <img src="https://img.shields.io/badge/Download-Build-4285F4?style=for-the-badge&logo=google-drive&logoColor=white" alt="Download Build"/>
  </a>
</p>

<p align="center">
  <a href="https://www.youtube.com/watch?v=gD4C09PAJKU" target="_blank">
    <img src="https://img.shields.io/badge/YouTube-FF0000?style=for-the-badge&logo=youtube&logoColor=white" alt="Gameplay Video"/>
  </a>
  <a href="https://www.linkedin.com/in/finerace/" target="_blank">
    <img src="https://img.shields.io/badge/LinkedIn-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white" alt="LinkedIn"/>
  </a>
  <a href="https://t.me/finerace" target="_blank">
    <img src="https://img.shields.io/badge/Telegram-26A5E4?style=for-the-badge&logo=telegram&logoColor=white" alt="Telegram"/>
  </a>
</p>

---

<p align="center">
  <!-- Вставь сюда гифки, демонстрирующие геймплей, AI, систему сохранений -->
  <img src="docs/doc (1).png" width="400" alt="Gameplay Demo">
  <img src="docs/doc (2).png" width="400" alt="Save-Load System">
</p>

---

### TL;DR

*   🏃‍♂️ **Продвинутая система передвижения:** Игрок не просто ходит и прыгает. Реализованы ключевые "action-FPS" механики: **крюк-кошка** (`PlayerHookService`) с собственной физикой на `SpringJoint` и расходом ресурса, а также система **рывков (dashes)** (`PlayerDashsService`), ограниченная количеством зарядов и временем восстановления.
*   💾 **Полная система сохранений:** Реализована возможность сохранить и загрузить игру в **любой момент**. Система обрабатывает и восстанавливает состояние всех динамических объектов: от летящих пуль и врагов (включая их текущую фазу атаки) до положения дверей и состояния триггеров.
*   📈 **Система прокачки и кастомизации костюма:** В игру встроена полноценная система перманентных улучшений. Игрок собирает специальные предметы (`EnchancedSuitPointItem`) для получения очков прокачки, которые затем вкладывает в улучшение своих способностей (увеличение числа рывков, усиление защитного взрыва, улучшение крюка-кошки).
*   🤖 **Тактический ИИ с командным взаимодействием:** Создано более 15 уникальных типов врагов. Благодаря системе "тревоги" (`EnemysAiManager`), заметивший игрока противник может предупредить союзников в радиусе, заставляя всю группу атаковать скоординированно. Некоторые враги умеют стрелять на упреждение.
*   🔫 **Data-driven система оружия:** Арсенал из 10+ видов оружия полностью настраивается через `ScriptableObject`. Реализованы различные режимы стрельбы (автоматический, зарядка, дробовик, лазер), система боеприпасов и кастомная процедурная отдача (`PlayerWeaponRecoil`).
*   🎨 **Продвинутая работа с шейдерами:** Написаны с нуля комплексные шейдеры для визуальных эффектов, включая энергетический щит с эффектом искажения, динамическую воду, а также анимированные UI-шейдеры с переливающимися линиями.
*   🖌️ **Создание ассетов с нуля:** Значительная часть игровых ассетов, включая 3D-модели, текстуры, иконки и элементы UI, была создана самостоятельно, что демонстрирует комплексный подход к разработке и понимание всего пайплайна создания контента.
*   🎯 **Динамическая система заданий:** На уровнях работает система задач (`LevelTaskService`), которая ставит перед игроком цели (например, "уничтожить всех врагов в зоне") и отслеживает их выполнение, направляя геймплей.```

---

<p align="center">
  <!-- Вставь сюда гифки, демонстрирующие геймплей, AI, систему сохранений -->
  <img src="docs/doc (3).png" width="400" alt="Gameplay Demo">
  <img src="docs/doc (4).png" width="400" alt="Save-Load System">
</p>

---

### 💡 О проекте

Ancelight — это динамичный FPS-шутер, созданный как полигон для отработки и реализации сложных игровых систем с нуля. Основной фокус был сделан не на визуальной составляющей, а на глубокой проработке "подкапотных" механик: AI, система сохранений, физика и управление ресурсами.

> [!NOTE]
> <details>
>   <summary><strong>Подробнее о проекте...</strong></summary>
>
>   <br>
>
>   *   **🎮 Игровой процесс:** Классический шутер с разнообразными врагами, каждый из которых обладает уникальным поведением. Игрок может использовать различное оружие, перки и специальные способности для прохождения уровней.
>
>   *   **🎯 Цель проекта:** Разработать и организовать в единый, работоспособный механизм множество сложных и взаимосвязанных игровых систем: от продвинутого AI противников и обширного арсенала оружия до полной системы сохранений и кастомной физики. Основной задачей было создание стабильной и функциональной основы, способной поддерживать весь этот комплекс механик.
>
> </details>

---

### 🛠️ Реализованные ключевые системы

Проект построен на нескольких фундаментальных системах, написанных с нуля для обеспечения полного контроля над поведением игры. Каждая система проектировалась с упором на функциональность и решение конкретных геймплейных задач.

> [!NOTE]
> <details>
>   <summary><strong>Показать полный список систем...</strong></summary>
>
>   <br>
>
>   #### I. Системы Игрока (Player Core Systems)
>
>   *   **🏃‍♂️ Продвинутая система передвижения (Mobility Suite)**
>       *   **Ответственные классы:** `PlayerMovement.cs`, `PlayerDashsService.cs`, `PlayerHookService.cs`.
>       *   **Суть:** Помимо стандартного передвижения (ходьба, приседание, прыжок), система включает две ключевые action-механики:
>           *   **Крюк-кошка:** Реализован на `SpringJoint` для создания физически корректного ощущения притягивания. Имеет ограниченный ресурс (`Hook Strength`), который тратится во время использования и регенерируется со временем.
>           *   **Рывки (Dashes):** Игрок имеет несколько зарядов рывка, которые позволяют мгновенно уклоняться от атак. Заряды восстанавливаются по одному с определенной задержкой.
>
>   *   **🔫 Система оружия и способностей**
>       *   **Ответственные классы:** `PlayerWeaponsManager.cs`, `WeaponData.cs`, `PlayerWeaponRecoil.cs`, `PlayerImmediatelyProtectionService.cs`.
>       *   **Суть:** Полностью data-driven система, где все оружие (более 10 видов) настраивается через `ScriptableObject`. Поддерживает различные режимы стрельбы (автоматический, зарядка, дробовик, лазер), имеет кастомную систему отдачи и интегрирована с системой боеприпасов. Также включает защитную способность — мощный направленный взрыв (`Magnetic Shock`), работающий по принципу "ультимейта" с долгой перезарядкой.
>
>   *   **📈 Система здоровья и прокачки костюма**
>       *   **Ответственные классы:** `PlayerMainService.cs` (как хаб), `ImprovementItem.cs`, `DashImprovementItem.cs` (примеры).
>       *   **Суть:** Комплексная система, управляющая здоровьем, броней и сопротивлением урону. Включает в себя механику перманентных улучшений: игрок находит на уровнях специальные предметы, дающие "очки прокачки", которые можно потратить в специальном меню на апгрейд способностей (увеличение числа рывков, усиление защитного взрыва и т.д.).
> 
> ---
> 
>   #### II. AI и Игровые Сценарии
>
>   *   **🤖 Тактический ИИ с командным взаимодействием**
>       *   **Ответственные классы:** `DefaultBot.cs` (базовый класс), `EnemysAiManager.cs`, `FlyingDroneBot.cs`, `TeleporterBot.cs` и др.
>       *   **Суть:** Основа поведения врагов (более 15 типов). AI построен на базе конечного автомата (FSM), реализованного на корутинах. Каждый тип врага наследуется от `DefaultBot` и имеет уникальную логику атаки и передвижения.
>           <details>
>           <summary><strong>🧠 Особенности реализации...</strong></summary>
>           <br>
>           *   **Командная работа:** `EnemysAiManager` отслеживает всех "умных" ботов на сцене. Если один из них замечает игрока, он может "поднять тревогу", и все боты в радиусе также станут агрессивными и получат последнее известное местоположение игрока.
>           *   **Стрельба на упреждение:** Некоторые враги используют метод `CalculateSmartTargetPos`, который рассчитывает будущую позицию игрока на основе его текущей скорости и скорости полета снаряда.
>           </details>
>
>   *   **🎬 Система спавна врагов по сценариям**
>       *   **Ответственный класс:** `LevelSpawnScenario.cs`.
>       *   **Суть:** Мощный инструмент для левел-дизайна, позволяющий создавать сложные сценарии появления врагов. Поддерживает волны, задержки между ними, повторения, движение врагов к указанным точкам (`WayPoint`) и активацию `UnityEvent`'ов после зачистки всех врагов в сценарии.
>
> ---
>
>   #### III. Архитектурные и Базовые Системы
>
>   *   **💾 Полная система сохранений**
>       *   **Ответственные классы:** `LevelSaveLoadSystem.cs`, `LevelSaveData.cs`, `FixedJsonUtilityFunc.cs`.
>       *   **Суть:** Позволяет сериализовать и десериализовать состояние всей игровой сцены в любой момент времени. Сохраняет состояние игрока, каждого врага, летящего снаряда, предмета и триггера.
>           <details>
>           <summary><strong>🧠 Особенности реализации...</strong></summary>
>           <br>
>           Для сериализации несериализуемых по-умолчанию типов Unity (`Dictionary`, `Rigidbody`, `Transform`) был написан кастомный класс-хелпер `FixedJsonUtilityFunc`, который конвертирует их в "понятные" для `JsonUtility` форматы и обратно. Это демонстрирует способность обходить ограничения стандартных инструментов.
>           </details>
>
>   *   **🎶 Аудио-система с пулом объектов**
>       *   **Ответственный класс:** `AudioPoolService.cs`.
>       *   **Суть:** Для воспроизведения звуковых эффектов используется пул объектов `AudioSource` с поддержкой приоритетов. Это позволяет избежать постоянного создания/уничтожения объектов, что значительно снижает количество сборок мусора (GC) во время активного геймплея.
>
>   *   **📦 Система предметов и подбираемых объектов (Pickups)**
>       *   **Ответственные классы:** `OrdinaryPlayerItem.cs` (базовый класс), `WeaponGetItem.cs`, `PlayerAmmoItem.cs`, `PlasmaGetItem.cs`.
>       *   **Суть:** Полиморфная система, где базовый класс `OrdinaryPlayerItem` определяет общую логику подбора, а дочерние классы реализуют конкретное поведение: выдать оружие, пополнить боезапас, восполнить здоровье/броню или добавить плазму для крафта патронов.
>
> --- 
>
>   #### IV. UI и Обратная связь
>
>   *   ** HUD и Индикаторы**
>       *   **Ответственные классы:** `MainCircleUI.cs`, `DashsIndicatorService.cs`, `HookCircle.cs`, `BulletsIndicators.cs`.
>       *   **Суть:** Комплексный игровой интерфейс, который в реальном времени отображает всю важную информацию: здоровье, броню, выбранное оружие (в виде анимированного "колеса"), количество патронов, доступные рывки и энергию крюка-кошки.
>
>   *   **🎯 Контекстные подсказки и информация**
>       *   **Ответственные классы:** `AdditionalInformationPanel.cs`, `KeyboardKeyTip.cs`, `LevelTaskService.cs`.
>       *   **Суть:** Система предоставляет игроку обратную связь:
>           *   При наведении на интерактивный объект или врага появляется панель с информацией о нём (`AdditionalInformationPanel`).
>           *   Наэкранные подсказки по управлению (`KeyboardKeyTip`) автоматически обновляются при изменении раскладки в настройках.
>           *   Система задач (`LevelTaskService`) выводит на экран текущую цель миссии.
>
> ---
>
>   #### V. Меню и Настройки
>
>   *   **⚙️ Комплексное меню настроек**
>       *   **Ответственные классы:** `SettingsSaveLoadSystem.cs`, `SettingsSetSystem.cs`.
>       *   **Суть:** Полнофункциональное меню настроек, позволяющее игроку кастомизировать практически все аспекты игры. Все настройки сохраняются и загружаются из файла.
>           <details>
>           <summary><strong>🧠 Поддерживаемые опции...</strong></summary>
>           <br>
>           *   **Графика:** Качество текстур, анизотропная фильтрация, разрешение и дистанция теней, SSAO, MSAA, качество растительности, дистанция прорисовки, VSync, FOV, разрешение и формат экрана.
>           *   **Управление:** Полная перенастройка всех клавиш (`InputButtonField.cs`), включая действия мыши и колеса прокрутки, а также настройка чувствительности.
>           *   **Звук:** Раздельная регулировка громкости (мастер, музыка, эффекты, окружение, UI), настройка максимального количества звуковых источников.
>           </details>
>
>   *   **🌍 Система локализации**
>       *   **Ответственные классы:** `CurrentLanguageData.cs`, `StaticTextLanguageAdaptable.cs`.
>       *   **Суть:** Реализована система, позволяющая переводить весь игровой текст. Текстовые данные хранятся в `ScriptableObject` (`LanguageData`), а специальные компоненты на UI-элементах (`StaticTextLanguageAdaptable`) автоматически подгружают нужный перевод при смене языка в настройках.
>
> </details>

---

### 🚀 Кейс-стади: Проектирование Полной Системы Сохранений

Самой сложной и интересной задачей в проекте была реализация системы сохранений, которая не просто запоминает чекпоинт, а "замораживает" весь игровой мир в любой момент времени.

> [!NOTE]
> <details>
> <summary><strong>Показать полный разбор системы...</strong></summary>
> 
> # 📝 Проектирование системы сохранений "Save-Anywhere"
> 
> ## 1. 🧐 Проблема и Цели
> 
> В большинстве игр сохранение происходит в строго определенных точках. Цель была амбициознее: дать игроку возможность сохраниться в разгаре боя, во время полета ракеты или диалога, а после загрузки — вернуться в **абсолютно идентичное состояние мира**.
> 
> **Ключевые требования:**
> *   Сохранять состояние **всех** динамических сущностей.
> *   Работать с несериализуемыми типами Unity.
> *   Корректно восстанавливать сцену, избегая дубликатов и потерянных объектов.
> 
> ## 2. 🏗️ Архитектура и Реализация
> 
> Система состоит из трех ключевых компонентов:
> 
> *   **`LevelSaveData.cs`:** "Контейнер" для данных. Этот `MonoBehaviour` динамически собирает ссылки на все объекты, которые нужно сохранить: игрока, врагов, пули, предметы, триггеры. Перед сериализацией он "опрашивает" каждый объект и сохраняет его состояние в виде простого `Serializable` класса.
> 
> *   **`FixedJsonUtilityFunc.cs`:** "Переводчик". Стандартный `JsonUtility` не умеет работать с `Transform`, `Rigidbody`, `Dictionary` и другими сложными типами. Этот статический класс содержит методы, которые "разбирают" эти объекты на базовые типы (например, `Transform` на `Vector3` позиции, `Quaternion` вращения и `Vector3` скейла) и "собирают" их обратно при загрузке.
> 
> *   **`LevelSaveLoadSystem.cs`:** "Оркестратор". Этот класс управляет всем процессом:
>     1.  **При сохранении:** Вызывает метод `SaveToJson()` у `LevelSaveData`, который собирает все данные, конвертирует их в JSON и записывает в файл с помощью `BinaryFormatter` (для простоты).
>     2.  **При загрузке:**
>         *   Загружает сцену по ID, сохраненному в файле.
>         *   **(Ключевой шаг)** После загрузки сцены **уничтожает** всех динамических врагов, предметы и пули, которые были на сцене по умолчанию.
>         *   Читает JSON из файла.
>         *   Проходит по сохраненным данным и **воссоздает** каждый объект из префаба, а затем применяет к нему сохраненное состояние (позицию, здоровье, скорость и т.д.).
> 
> ## 3. 🎉 Результаты и Выводы
> 
> *   **Результат:** Была создана полностью рабочая система, позволяющая сохранять и загружать игру в любой момент.
> *   **Полученный опыт:** Этот процесс научил меня глубокой работе с сериализацией, рефлексией (для доступа к типам объектов), управлению жизненным циклом объектов и важности строгого порядка операций при восстановлении сложной сцены. Этот опыт стал фундаментом для моих дальнейших, более сложных архитектурных решений.
> 
> ## 4. 🤔 Компромиссы и Точки Роста
> 
> *   **Производительность:** Текущая реализация использует `JsonUtility`, который не является самым быстрым методом сериализации. В реальном ААА-проекте стоило бы использовать более производительные бинарные сериализаторы.
> *   **"Хрупкость":** Система сильно зависит от наличия префабов с нужными компонентами. Изменение префаба может "сломать" старые сохранения. В будущем можно было бы внедрить систему версионирования сохранений для обратной совместимости.
>
> </details>

---

### 📈 Что я вынес из этого проекта

Этот проект был моей "песочницей" для фундаментальных механик. Отсутствие строгой архитектуры и DI-фреймворков заставило меня решать многие проблемы "вручную", что дало бесценное понимание того, **как на самом деле работает Unity**. Этот опыт напрямую повлиял на мои последующие проекты, где я уже осознанно применял SOLID, DI и другие паттерны для решения проблем, с которыми столкнулся здесь.

<h3 align="center">Спасибо за внимание!</h3>

<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>
<br>

<p align="right">
  <img src="https://flagpedia.net/data/flags/w20/ru.png" width="20" alt="Russian">
  <a href="#привет-это-ancelight--мой-старый-проект-в-котором-я-с-нуля-реализовал-все-механики-классического-fps-шутера">Русская версия</a>
</p>

<p align="center">
  <!-- Insert your project logo or banner here, if available -->
  <img src="Assets/UI/Sprites/Other/icon.png" width="300" alt="Ancelight Logo">
</p>

<h3 align="center">Hi! This is "Ancelight"—my old project where I implemented all the mechanics of a classic FPS from scratch.</h3>

<p align="center">
  <a href="https://drive.google.com/file/d/16jbOWAHsmiOoD8lNHU3QDHs1_p4yVWv1/view?usp=sharing" target="_blank">
    <img src="https://img.shields.io/badge/Download-Build-4285F4?style=for-the-badge&logo=google-drive&logoColor=white" alt="Download Build"/>
  </a>
</p>

<p align="center">
  <a href="https://www.youtube.com/watch?v=gD4C09PAJKU" target="_blank">
    <img src="https://img.shields.io/badge/YouTube-FF0000?style=for-the-badge&logo=youtube&logoColor=white" alt="Gameplay Video"/>
  </a>
  <a href="https://www.linkedin.com/in/finerace/" target="_blank">
    <img src="https://img.shields.io/badge/LinkedIn-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white" alt="LinkedIn"/>
  </a>
  <a href="https://t.me/finerace" target="_blank">
    <img src="https://img.shields.io/badge/Telegram-26A5E4?style=for-the-badge&logo=telegram&logoColor=white" alt="Telegram"/>
  </a>
</p>

---

<p align="center">
  <!-- Insert GIFs demonstrating gameplay, AI, save system, etc. -->
  <img src="docs/doc (1).png" width="400" alt="Gameplay Demo">
  <img src="docs/doc (2).png" width="400" alt="Save-Load System">
</p>

---

### TL;DR

*   🏃‍♂️ **Advanced Movement System:** The player does more than just walk and jump. Key "action-FPS" mechanics have been implemented: a **grappling hook** (`PlayerHookService`) with its own physics based on `SpringJoint` and resource consumption, as well as a **dash system** (`PlayerDashsService`) limited by charges and cooldowns.
*   💾 **Comprehensive Save System:** Implemented the ability to save and load the game at **any moment**. The system processes and restores the state of all dynamic objects: from flying projectiles and enemies (including their current attack phase) to the position of doors and the state of triggers.
*   📈 **Suit Upgrade and Customization System:** The game features a full-fledged permanent upgrade system. The player collects special items (`EnchancedSuitPointItem`) to earn upgrade points, which can then be invested in improving abilities (increasing the number of dashes, enhancing the protective blast, upgrading the grappling hook).
*   🤖 **Tactical AI with Team Interaction:** Over 15 unique enemy types have been created. Thanks to an "alert" system (`EnemysAiManager`), an enemy that spots the player can warn nearby allies, causing the entire group to attack in a coordinated manner. Some enemies can shoot predictively.
*   🔫 **Data-Driven Weapon System:** An arsenal of 10+ weapons is fully configurable via `ScriptableObject`. Various firing modes (automatic, charged, shotgun, laser), an ammunition system, and a custom procedural recoil (`PlayerWeaponRecoil`) have been implemented.
*   🎨 **Advanced Shader Work:** Complex shaders for visual effects were written from scratch, including an energy shield with a distortion effect, dynamic water, and animated UI shaders with iridescent lines.
*   🖌️ **Asset Creation from Scratch:** A significant portion of the game's assets, including 3D models, textures, icons, and UI elements, was created independently, demonstrating a comprehensive approach to development and an understanding of the entire content creation pipeline.
*   🎯 **Dynamic Quest System:** The levels feature a task system (`LevelTaskService`) that sets goals for the player (e.g., "eliminate all enemies in the area") and tracks their completion, guiding the gameplay.

---

<p align="center">
  <!-- Insert GIFs demonstrating gameplay, AI, save system, etc. -->
  <img src="docs/doc (3).png" width="400" alt="Gameplay Demo">
  <img src="docs/doc (4).png" width="400" alt="Save-Load System">
</p>

---

### 💡 About the Project

"Ancelight" is a dynamic FPS created as a training ground for developing and implementing complex game systems from scratch. The main focus was not on the visual component, but on the in-depth development of "under-the-hood" mechanics: AI, the save system, physics, and resource management.

> [!NOTE]
> <details>
>   <summary><strong>More about the project...</strong></summary>
>
>   <br>
>
>   *   **🎮 Gameplay:** A classic shooter with a variety of enemies, each with unique behavior. The player can use different weapons, perks, and special abilities to complete levels.
>
>   *   **🎯 Project Goal:** To develop and organize a multitude of complex and interconnected game systems into a single, functional mechanism: from advanced enemy AI and an extensive arsenal of weapons to a comprehensive save system and custom physics. The main task was to create a stable and functional foundation capable of supporting this entire complex of mechanics.
>
> </details>

---

### 🛠️ Key Implemented Systems

The project is built on several fundamental systems, written from scratch to ensure full control over the game's behavior. Each system was designed with a focus on functionality and solving specific gameplay challenges.

> [!NOTE]
> <details>
>   <summary><strong>Show the full list of systems...</strong></summary>
>
>   <br>
>
>   #### I. Player Core Systems
>
>   *   **🏃‍♂️ Advanced Movement System (Mobility Suite)**
>       *   **Responsible Classes:** `PlayerMovement.cs`, `PlayerDashsService.cs`, `PlayerHookService.cs`.
>       *   **Essence:** In addition to standard movement (walking, crouching, jumping), the system includes two key action mechanics:
>           *   **Grappling Hook:** Implemented with a `SpringJoint` to create a physically accurate feeling of being pulled. It has a limited resource (`Hook Strength`) that is consumed during use and regenerates over time.
>           *   **Dashes:** The player has several dash charges that allow for instant evasion of attacks. Charges are restored one by one with a certain delay.
>
>   *   **🔫 Weapon and Ability System**
>       *   **Responsible Classes:** `PlayerWeaponsManager.cs`, `WeaponData.cs`, `PlayerWeaponRecoil.cs`, `PlayerImmediatelyProtectionService.cs`.
>       *   **Essence:** A completely data-driven system where all weapons (over 10 types) are configured via `ScriptableObject`. It supports various firing modes (automatic, charged, shotgun, laser), has a custom recoil system, and is integrated with an ammunition system. It also includes a defensive ability—a powerful directional blast (`Magnetic Shock`) that functions as an "ultimate" with a long cooldown.
>
>   *   **📈 Health and Suit Upgrade System**
>       *   **Responsible Classes:** `PlayerMainService.cs` (as a hub), `ImprovementItem.cs`, `DashImprovementItem.cs` (examples).
>       *   **Essence:** A comprehensive system that manages health, armor, and damage resistance. It includes a permanent upgrade mechanic: the player finds special items on levels that grant "upgrade points," which can be spent in a special menu to upgrade abilities (increasing the number of dashes, enhancing the protective blast, etc.).
> 
> ---
> 
>   #### II. AI and Game Scenarios
>
>   *   **🤖 Tactical AI with Team Interaction**
>       *   **Responsible Classes:** `DefaultBot.cs` (base class), `EnemysAiManager.cs`, `FlyingDroneBot.cs`, `TeleporterBot.cs`, etc.
>       *   **Essence:** The foundation of enemy behavior (over 15 types). The AI is built on a finite-state machine (FSM) implemented with coroutines. Each enemy type inherits from `DefaultBot` and has unique attack and movement logic.
>           <details>
>           <summary><strong>🧠 Implementation Details...</strong></summary>
>           <br>
>           *   **Teamwork:** `EnemysAiManager` tracks all "smart" bots on the scene. If one of them spots the player, it can "raise an alarm," and all bots within a certain radius will also become aggressive and receive the player's last known location.
>           *   **Predictive Shooting:** Some enemies use the `CalculateSmartTargetPos` method, which calculates the player's future position based on their current velocity and the projectile's speed.
>           </details>
>
>   *   **🎬 Enemy Spawning System via Scenarios**
>       *   **Responsible Class:** `LevelSpawnScenario.cs`.
>       *   **Essence:** A powerful tool for level design that allows creating complex enemy appearance scenarios. It supports waves, delays between them, repetitions, moving enemies to specified points (`WayPoint`), and activating `UnityEvent`s after clearing all enemies in a scenario.
>
> ---
>
>   #### III. Architectural and Core Systems
>
>   *   **💾 Comprehensive Save System**
>       *   **Responsible Classes:** `LevelSaveLoadSystem.cs`, `LevelSaveData.cs`, `FixedJsonUtilityFunc.cs`.
>       *   **Essence:** Allows serializing and deserializing the state of the entire game scene at any moment. It saves the state of the player, every enemy, flying projectile, item, and trigger.
>           <details>
>           <summary><strong>🧠 Implementation Details...</strong></summary>
>           <br>
>           To serialize non-serializable Unity types by default (`Dictionary`, `Rigidbody`, `Transform`), a custom helper class `FixedJsonUtilityFunc` was written to convert them into formats understandable by `JsonUtility` and back. This demonstrates the ability to work around the limitations of standard tools.
>           </details>
>
>   *   **🎶 Audio System with Object Pooling**
>       *   **Responsible Class:** `AudioPoolService.cs`.
>       *   **Essence:** An object pool of `AudioSource` components is used for playing sound effects, with support for priorities. This avoids constant instantiation and destruction of objects, significantly reducing garbage collection (GC) overhead during active gameplay.
>
>   *   **📦 Item and Pickup System**
>       *   **Responsible Classes:** `OrdinaryPlayerItem.cs` (base class), `WeaponGetItem.cs`, `PlayerAmmoItem.cs`, `PlasmaGetItem.cs`.
>       *   **Essence:** A polymorphic system where the base class `OrdinaryPlayerItem` defines the general pickup logic, and child classes implement specific behaviors: granting a weapon, replenishing ammo, restoring health/armor, or adding plasma for crafting ammo.
>
> --- 
>
>   #### IV. UI and Feedback
>
>   *   **HUD and Indicators**
>       *   **Responsible Classes:** `MainCircleUI.cs`, `DashsIndicatorService.cs`, `HookCircle.cs`, `BulletsIndicators.cs`.
>       *   **Essence:** A comprehensive game interface that displays all vital information in real-time: health, armor, selected weapon (in an animated "wheel"), ammo count, available dashes, and grappling hook energy.
>
>   *   **🎯 Contextual Hints and Information**
>       *   **Responsible Classes:** `AdditionalInformationPanel.cs`, `KeyboardKeyTip.cs`, `LevelTaskService.cs`.
>       *   **Essence:** The system provides feedback to the player:
>           *   When aiming at an interactive object or enemy, a panel with information about it appears (`AdditionalInformationPanel`).
>           *   On-screen control hints (`KeyboardKeyTip`) automatically update when the key layout is changed in the settings.
>           *   The task system (`LevelTaskService`) displays the current mission objective on the screen.
>
> ---
>
>   #### V. Menus and Settings
>
>   *   **⚙️ Comprehensive Settings Menu**
>       *   **Responsible Classes:** `SettingsSaveLoadSystem.cs`, `SettingsSetSystem.cs`.
>       *   **Essence:** A fully functional settings menu that allows the player to customize almost every aspect of the game. All settings are saved to and loaded from a file.
>           <details>
>           <summary><strong>🧠 Supported Options...</strong></summary>
>           <br>
>           *   **Graphics:** Texture quality, anisotropic filtering, shadow resolution and distance, SSAO, MSAA, vegetation quality, draw distance, VSync, FOV, screen resolution, and display mode.
>           *   **Controls:** Full rebinding of all keys (`InputButtonField.cs`), including mouse actions and scroll wheel, as well as sensitivity settings.
>           *   **Sound:** Separate volume controls (master, music, effects, ambient, UI), and configuration of the maximum number of sound sources.
>           </details>
>
>   *   **🌍 Localization System**
>       *   **Responsible Classes:** `CurrentLanguageData.cs`, `StaticTextLanguageAdaptable.cs`.
>       *   **Essence:** A system has been implemented to translate all in-game text. Text data is stored in a `ScriptableObject` (`LanguageData`), and special components on UI elements (`StaticTextLanguageAdaptable`) automatically load the correct translation when the language is changed in the settings.
>
> </details>

---

### 🚀 Case Study: Designing a Comprehensive Save System

The most challenging and interesting task in the project was implementing a save system that doesn't just remember a checkpoint but "freezes" the entire game world at any given moment.

> [!NOTE]
> <details>
> <summary><strong>Show the full system breakdown...</strong></summary>
> 
> # 📝 Designing a "Save-Anywhere" System
> 
> ## 1. 🧐 Problem and Goals
> 
> In most games, saving occurs at strictly defined points. The goal was more ambitious: to give the player the ability to save in the middle of a battle, during a rocket's flight, or in a dialogue, and after loading, to return to an **absolutely identical state of the world**.
> 
> **Key Requirements:**
> *   Save the state of **all** dynamic entities.
> *   Work with non-serializable Unity types.
> *   Correctly restore the scene, avoiding duplicates and lost objects.
> 
> ## 2. 🏗️ Architecture and Implementation
> 
> The system consists of three key components:
> 
> *   **`LevelSaveData.cs`:** The data "container." This `MonoBehaviour` dynamically collects references to all objects that need to be saved: the player, enemies, projectiles, items, triggers. Before serialization, it "polls" each object and saves its state as a simple `Serializable` class.
> 
> *   **`FixedJsonUtilityFunc.cs`:** The "translator." The standard `JsonUtility` cannot handle `Transform`, `Rigidbody`, `Dictionary`, and other complex types. This static class contains methods that "disassemble" these objects into basic types (e.g., a `Transform` into a `Vector3` for position, a `Quaternion` for rotation, and a `Vector3` for scale) and "reassemble" them upon loading.
> 
> *   **`LevelSaveLoadSystem.cs`:** The "orchestrator." This class manages the entire process:
>     1.  **On Save:** Calls the `SaveToJson()` method on `LevelSaveData`, which gathers all data, converts it to JSON, and writes it to a file using `BinaryFormatter` (for simplicity).
>     2.  **On Load:**
>         *   Loads the scene by the ID saved in the file.
>         *   **(Key Step)** After the scene loads, it **destroys** all dynamic enemies, items, and projectiles that were in the scene by default.
>         *   Reads the JSON from the file.
>         *   Iterates through the saved data and **recreates** each object from a prefab, then applies its saved state (position, health, velocity, etc.).
> 
> ## 3. 🎉 Results and Conclusions
> 
> *   **Result:** A fully functional system was created that allows saving and loading the game at any moment.
> *   **Experience Gained:** This process taught me in-depth work with serialization, reflection (for accessing object types), object lifecycle management, and the importance of a strict order of operations when restoring a complex scene. This experience became the foundation for my subsequent, more complex architectural decisions.
> 
> ## 4. 🤔 Compromises and Growth Points
> 
> *   **Performance:** The current implementation uses `JsonUtility`, which is not the fastest serialization method. In a real AAA project, more performant binary serializers would be preferable.
> *   **"Fragility":** The system heavily relies on the existence of prefabs with the necessary components. Changing a prefab can "break" old saves. In the future, a save versioning system could be implemented for backward compatibility.
>
> </details>

---

### 📈 What I Learned from This Project

This project was my "sandbox" for fundamental mechanics. The absence of a strict architecture and DI frameworks forced me to solve many problems "manually," which gave me an invaluable understanding of **how Unity actually works**. This experience directly influenced my subsequent projects, where I consciously applied SOLID, DI, and other patterns to solve the problems I encountered here.

<h3 align="center">Thank you for your attention!</h3>
