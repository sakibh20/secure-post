# 🎲 Bluff Game

## Introduction

The **Bluff Game** is a strategic 1v1 multiplayer game built around deception, quick thinking, and reading your opponent. Players take turns **rolling a die** and **declaring a result** (1–6). The twist? You're allowed to **lie**. The opposing player must then decide whether to **believe** or **call bluff**.

* If the opponent **calls bluff** correctly (and you're lying), they earn a point.
* If the opponent **calls bluff** incorrectly (and you're honest), you earn a point.
* If the opponent **believes** and you're lying, **you** get the point.
* First to the max score wins!

It's a test of confidence, trickery, and psychological gameplay—ideal for quick, tense, and fun multiplayer showdowns.

---

## Report

[Click here to view the full project report](/Documentation/Bulff%20Game_Final%20Report.docx) <!-- Replace with actual link -->

---

## 💻 Executable

[Download Executable](/Executable) <!-- Replace with actual link -->
- Inside the Executable folder there is a **Game.exe**
- Run that **Game.exe**
- Get the whole Executable flder to run the game properly

---

## Deployment Instructions

### Requirements

* [Docker](https://www.docker.com/)
* Unity Executable (provided above)

### Step-by-step:

1. **Start Backend Server with Docker Compose**

   Run the following in the root directory:

   ```bash
   docker-compose up --build
   ```

2. **Launch Two Game Instances**

   Run two instances of the `Game.exe` to simulate two players (e.g., on the same machine).

3. **Registration & Login**

   * Register two unique users (e.g., `PlayerA`, `PlayerB`).
   * Login with the respective credentials in both game instances.

4. **Enter Lobby**

   * After login, click the lobby button to enter the lobby.
   * Here, you'll see all **available players**.

5. **Challenge & Accept**

   * One player sends a **challenge** request.
   * The other instance/player will receive an invite and can **accept**.

6. **Gameplay**

   * The game begins.
   * Turns are handled via server commands.
   * You roll, claim a number, and the other player decides to **Believe** or **Call Bluff**.

   The system ensures turn-based flow and tracks score securely via server logic.
