# **Friday Night Funkin' Unity Asset**
### This asset was created so that anyone can make their own Friday Night Funkin' in Unity — for free.

---

## 🎮 Demo Game  
Want to see the framework in action?  
Check out a game made using an earlier version of this framework:  
👉 [Night in Restaurant on itch.io](https://rindik.itch.io/night-in-restaurant)

---

## 🔧 Setup Instructions

Follow these steps to set up the asset correctly:

### 1. Install the release package
Download the `.unitypackage` release from GitHub or your source.

### 2. Import into Unity
Open your project in Unity and import the package:
> `Assets → Import Package → Custom Package...`

### 3. Fix initial errors by installing dependencies
After import, you will see several errors — **don’t worry!**  
Unity just needs the required packages. Install the following dependencies via **Package Manager**:

- ✅ **Input System**
- ✅ **Cinemachine**
- ✅ **Universal Render Pipeline (URP)**
- ✅ **Timeline** (comes preinstalled in recent Unity versions)

You can find all these in **Window → Package Manager → Unity Registry**.

---

### 4. Add Scenes to Build Settings

Go to `Assets/FNF/Scenes/` and add the following scenes to **File → Build Settings**:
- `MainMenu`
- `Tutorial` (located at `Assets/FNF/Levels/Tutorial/`)

### 5. Play the Game

Now press Play to test! If something breaks:

1. Locate the object named `FNFBOX` in the scene.
2. It contains a `ChartPlayback` component.
3. Press **"Reload Chart"** from that component.

After this, the level should load correctly — setup is complete.

---

## 📦 Dependencies

Make sure the following Unity packages are installed:

| Package           | Required |
|------------------|----------|
| Input System      | ✅ Yes   |
| Cinemachine       | ✅ Yes   |
| URP (Universal RP)| ✅ Yes   |
| Timeline          | ✅ Yes   |

> ℹ️ URP is mandatory. Built-in Render Pipeline is not supported.

---

## ✨ Features

### 🎼 1. Editor

- The chart editor is based on Unity Timeline, letting you create charts right in the editor:

  <img src="Timeline.gif" width="70%"><br><br>

- Add arrows via context menu, move them freely along the track, and delete with ease:

  <img src="ChartSpawn.gif" width="70%"><br><br>

- Use keyboard shortcuts to copy and paste arrows on the timeline:

  <img src="shortcuts.gif"><br><br>

---

### 🎮 2. Game

- Fully replicated gameplay with working player & bot logic:

  <img src="GamePlay.gif"><br><br>

---

### 🧭 3. Menus & Interface

- Includes fully working menus and UI similar to the original game:

  <img src="Menus.gif"><br><br>

---
<details>
  <summary>Инструкция на русском</summary>

## 📘 Русская инструкция (перевод)

  1. Установите `.unitypackage` файл с GitHub.
  2. Импортируйте в Unity через `Assets → Import Package → Custom Package...`
  3. Если Unity покажет ошибки — это нормально. Установите зависимости через `Package Manager`:
     - Input System
     - Cinemachine
     - URP
     - Timeline
  4. Добавьте сцены `MainMenu` и `Tutorial` в `File → Build Settings`.
  5. Нажмите Play. Если уровень не запускается — найдите объект `FNFBOX`, и в компоненте `ChartPlayback` нажмите **Reload Chart**.
</details>

---
## 🛠 Tips for Working with the Framework

Here are some useful tips to help you avoid common issues and get the most out of this asset:

<details>
  <summary>📉 <strong>Low FPS in Play Mode?</strong></summary>

If your FPS drops significantly during Play Mode, make sure the **Timeline tab is closed**.  
Even if you're not using it, Unity keeps it active and it can **consume a lot of performance** in the background.
</details>

<details>
  <summary>❓ <strong>Unexpected errors?</strong></summary>

If you encounter any **weird or unclear errors**, try using the **`Reload Chart`** button.  
It often helps fix issues related to desynchronization or missing references.
</details>

<details>
  <summary>🧠 <strong>Reload Chart didn’t help?</strong></summary>

If Reload Chart doesn’t fix the problem, chances are the error is on your side.  
But don’t worry — you can ask for help in our communities:

- 💬 **Telegram channel**: [[TELEGRAM]](https://t.me/RindikPodval)
- 💬 **Discord server**: [[DISCORD]](https://discord.com/invite/d9yuDXqF4r)

Let’s solve the issue together 💪
</details>

<details>
  <summary>📦 <strong>What is <code>FNFBOX</code>?</strong></summary>

`FNFBOX` is the main GameObject that stores everything needed for the FNF framework to work.  
To edit level data, use the editor window:

> **`FNFMaker > FNFLevelDataEditor`**

This window allows you to edit level settings like name, difficulty, chart, etc.
</details>

<details>
  <summary>🎯 <strong>Working with Markers?</strong></summary>

Markers can only be placed inside **`ArrowMarkerTrackAsset`**.  
It’s highly recommended to create two marker tracks:
- `Player`
- `Enemy`

This avoids confusion and keeps your timeline clean.  
Each marker also has an **enum** to choose its side (`Player`, `Enemy`, etc.).
</details>

<details>
  <summary>🎬 <strong>Creating a New Level?</strong></summary>

Always remember to **add the new scene to Build Settings**!  
Unity cannot load the scene via script unless it is added to the **Build Settings** list.
</details>

<details>
  <summary>🇷🇺 Нажмите, чтобы прочитать советы на русском</summary>

## 🛠 Советы по работе с фреймворком

Вот несколько полезных советов, которые помогут вам избежать распространённых ошибок и лучше понять, как пользоваться этим ассетом:

<details>
  <summary>📉 <strong>Мало FPS в режиме Play?</strong></summary>

Если у вас сильно проседает FPS во время Playmode, **уберите вкладку Timeline**.  
Даже если вы её не используете — Unity всё равно её рендерит и она **сильно ест производительность**.
</details>

<details>
  <summary>❓ <strong>Появляются странные ошибки?</strong></summary>

Если вы столкнулись с **непонятной ошибкой**, нажмите **`Reload Chart`**.  
Это часто помогает при ошибках синхронизации или потерянных ссылках.
</details>

<details>
  <summary>🧠 <strong>Reload Chart не помог?</strong></summary>

Если `Reload Chart` не помогает — возможно, ошибка с вашей стороны.  
Но не переживайте — вы можете обратиться за помощью в наши сообщества:

- 💬 **Телеграм-канал**: [[TELEGRAM]](https://t.me/RindikPodval)
- 💬 **Дискорд-сервер**: [[DISCORD]](https://discord.com/invite/d9yuDXqF4r)

Решим проблему вместе 💪
</details>

<details>
  <summary>📦 <strong>Что такое <code>FNFBOX</code>?</strong></summary>

`FNFBOX` — это основной объект на сцене, который содержит всё нужное для работы фреймворка.  
Чтобы редактировать параметры уровня, используйте окно редактора:

> **`FNFMaker > FNFLevelDataEditor`**

В этом окне можно менять название уровня, сложность, чарты и другие параметры.
</details>

<details>
  <summary>🎯 <strong>Работа с маркерами?</strong></summary>

Маркер можно размещать **только в `ArrowMarkerTrackAsset`**.  
Рекомендуется создавать два отдельных трека:
- `Player`
- `Enemy`

Так будет меньше путаницы, а таймлайн станет чище.  
У каждого маркера также есть **enum** для выбора стороны (`Player`, `Enemy` и т.д.).
</details>

<details>
  <summary>🎬 <strong>Создаёте новый уровень?</strong></summary>

Не забудьте **добавить сцену в Build Settings**!  
Unity не сможет открыть сцену через код, если она не добавлена в список сцен.
</details>

</details>


---

Have fun making your own FNF game! 🎤
