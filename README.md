# Vectorio | Source Code
Welcome to the source code for Vectorio! This is where you can make contributions to the game, or just view the project. Before making any contributions, please read through the whole README document.

**Important Links!** 
- [Steam Page](https://store.steampowered.com/app/1462470/Vectorio__Early_Access/)
- [Discord](https://discord.gg/auDgRJqtT9)
- [Trello Board](https://trello.com/b/BfiySKBr/vectorio)

# Core Refactor 
Though the current setup of the game works fine, it does not support extensive modding or multiplayer. It is both outdated and inefficient, so over the course of v0.2 multiple changes will be made to the base code to improve on these things as well as help streamline the development process. 

If you are interested in contributing to the core refactor, heres a list of what needs to be done.

**Turret Refactor**
- [x] Create a new script, `BaseTurret`, which contains stats any turret will have (implements `IDamageable`)
- [x] Create a new script, `DefaultTurret`, which contains basic turret logic (extends `BaseTurret`)
- [x] Replace the singular turret scripts with the `DefaultTurret` script and set variables
- [ ] Replace methods that require external calls with events (ex. `onDamage event`)
- [x] Create interfaces to replace methods inside `DefaultTurret` (ex. `iDamageable`)
- [ ] Have turret rotation be handled by a `TurretHandler` script

**Bullet Refactor**
- [ ] Remove all bullet prefabs and instead create one prefab for all turrets
- [ ] Create new variable inside `BaseTurret` called `bulletSize` 
- [ ] Have bullets set size when spawned 

**Enemy Refactor**
- [ ] Create a new script, `BaseEnemy`, which contains stats any enemy will have
- [ ] Create a new script, `DefaultEnemy`, which contains basic enemy logic (extends `BaseEnemy`)
- [ ] Replace the `EnemyClass` script with the `DefaultEnemy` script
- [ ] Replace methods that require external calls with events (ex. `onSpawn event`)
- [ ] Create interfaces to replace methods inside `DefaultEnemy` (ex. `iDamageable`)

**Boss Refactor**
- [ ] Create a new script, `DefaultBoss`, which contains basic boss logic (extends `BaseEnemy`)
- [ ] Replace the `BossAI` script with the `DefaultBoss` script
- [ ] Replace methods that require external calls with events (ex. `onDestroyed event`)
- [ ] Create interfaces to replace methods inside `DefaultEnemy` (ex. `iDamageable`)

**Other Refactors (will be expanded in future)**
- [ ] Survival full refactor 
- [ ] Research full refactor
- [ ] Technology full refactor
- [ ] Interface full refactor
- [ ] Spawner full refactor

# Contributing
If you're interested in contributing, please make sure you understand the following rules / guidelines.

- When you make a contribution, you are giving me the right to use whatever it may be. (code/art/sounds etc)
- Any PR's that contain Unity object changes will be denied. (Instead, please send me a link to your fork.)
- Always check the Trello board before making a contribution, as it may already be in the works. 
- NEVER use any sort of external Unity asset except the ones listed below. 

**Tutorials coming soon!**

# Required Assets
In order to fully build the game, you'll need the following assets. If you do not want to pay for these assets, you can modify your fork to work without them, but make sure when you make a PR you do not include these changes (it will be denied), or simply just send us a link to your fork. If you have obtained these assets, create a folder called `Blacklist` inside the `External` folder and place the asset folders inside of it. 

**NOTE:** We are talking with the asset developers to see if we can include the required parts of each asset so the game can be built without them, but until then, this is a temporary workaround. Sorry!

- [**Modern UI Pack |** $29.99 (Required)](https://assetstore.unity.com/packages/tools/gui/modern-ui-pack-150824)
- [**MK Glow Lite |** $9.99 (Optional)](https://assetstore.unity.com/packages/vfx/shaders/fullscreen-camera-effects/mk-glow-lite-155643)
