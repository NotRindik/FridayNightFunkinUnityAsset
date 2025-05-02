# **Friday Night Funkin' Unity Asset**
### This asset was created so that anyone can make their own Friday Night Funkin' in Unity — for free.

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

---

Have fun making your own FNF game! 🎤
