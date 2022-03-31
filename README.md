# 2D-Arkaden-Shooter
 My 2D (Mobile) Arcade Shooter Project

TODO:

---Gameplay---

Player Weaponry
-Damage enemies
-Don't hit the player
-Weapons produce heat; when you get heat capped, you can't shoot until you drop down to 50% heat
-Heat is dissipated over time
-Whenever you shoot anything, you trigger a timer equal to the cooldown of the weapon that stops all heat dissipation -> so you have to stop shooting sometimes...
-When adjusting the timer, the timer is always set with a Max(CurrentTimer, Cooldown), instead of being added on top of current timer.
-Timer ticks down based on deltatime every frame

Player Gadgets
-For example, bombs, no heat generation but have a "reserve". Regenerate slowly, but can be used in bursts
-Not limited to bombs, could be other kind of gadget for a "bomb" slot too?
-Rockets, fired automatically when shooting main weaponry, produce no heat and don't effect the heat timer
-Usually auto-aimish
-Time stop?
-Shield generator? Trades dmg->heat, if overheated (dissipation back to 50% till you can shoot again), stops working during this time
-Nanobots? Heal the player when heat is under certain treshold (0%?)
-APS? Shoots down (annoying)projectiles with a cooldown.

Health pickups
-Basic repair kit, heals % of max HP
-No heat pickup: x seconds of weapons not generating heat
-Quad damage? Double damage?

Enemy functionality
-Spawning logic (and removal logic)
-Behaviour such as flying patterns, movement
-Ramming interaction between player&enemy

Scoring
-Enemies die -> score (doesn't matter how they die, unless expiring)

Checkpoints
-Remember player stats when we hit a checkpoint (health value first in mind, heat don't matter)
-Need to keep track of enemies we already killed until this point? Most likely, to keep scoring same.

---Levels---

Terrain
-Figure out SpriteShape
-Make a demo level...
-When making destructible objects, use scripts such as health from enemies, and also use scoring logic. We might want a bit of score for destroyed terrain

---Menus---

Main Menu
-Access to game->level select etc
-Options
-Close application

Options
-Adjust volume
-Brightness, Gamma?
-Adjust binds (PC only?)
