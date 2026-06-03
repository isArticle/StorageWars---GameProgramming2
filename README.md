# Storage Wars

**Game Programming II - Final Exam Project**

**Team Gold Diggers:**
* **Arda Erenerdi (2305041074):** Lead Programmer, 2D Art & Architecture
* **Kuzey İbrahim Amaç (2305041037):** Game Designer & UI Integration

**IMPORTANT
Dear Selam Hocam, we fix the bugs. Bugs because the sound quality should be a maximum of 1.0f, but since we set it to 1.2f, the game was crashing.
The build and code we sent you has this bug, but the GitHub version is up-to-date.

---

## Game Objective
*Storage Wars* is a 2D tactical auction game. Two players compete against each other and an AIBot to win storage units. You must manage your inventory, sell items under a dynamic inflation system, and pay off debt to increase your maximum health. 

On the **15th Round**, the game shifts from competitive PvP to Co-Op. Both players must pool their money to outbid the Final Boss.
* **Win Condition:** Outbid the Boss by the end of the round. The player with the highest final net worth wins.
* **Loss Condition:** Losing all HP to debt/boss penalties, or failing to outbid the Boss.

---

## ⌨️ Controls

**System & Menus:**
* `Enter`: Confirm / Start Game
* `Space`: Next Phase / Advance
* `Backspace`: Go Back (Just in Menus)
* `Escape`: Exit Game

**Player 1 (Left Side):**
* `Left Shift`: Place Bid
* `Left Alt`: Pass / Fold
* `Q` / `W` / `E`: Use Skills (Auction Phase)
* `W` `A` `S` `D`: Move Inventory Cursor
* `Q` (Sell) / `E` (Take Debt) / `R` (Pay Debt): Inventory Actions

**Player 2 (Right Side):**
* `Right Shift`: Place Bid
* `Right Alt`: Pass / Fold
* `J` / `K` / `L`: Use Skills (Auction Phase)
* `Arrow Keys`: Move Inventory Cursor
* `I` (Sell) / `O` (Take Debt) / `P` (Pay Debt): Inventory Actions

---

## 🛠️ Technical Notes & Credits
* **Architecture:** The project relies on a custom **State Machine** pattern to handle game loops and strict OOP principles, clearly separating business logic (Managers) from presentation (Renderers).
* **AI & Economy:** Features a dynamic inflation system and a custom bot AI that scales its bidding logic based on the current round.
* **Assets:** No AI-generated assets were used. All 2D sprites and UI elements were created by the team. Audio elements are royalty-free sound effects implemented with dynamic pitch shifting.
