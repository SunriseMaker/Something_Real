# Something_Real
2D platformer, Unity 5 and C#. I like to write code and want to show what I can.
Standart Unity assets and free assets from different sources are used in this project.

## Instruction
1. Download this project using your favorite Git client (I'm using TortoiseGit).
2. Run Unity, call "File -> Open project...", choose the project folder and hit "Select folder" button.
3. Wait for a couple of minutes - Unity will generate meta data.
4. Run "File -> Build & Run...", choose platform and hit "Build and run" button.
5. Enjoy :)

## History of Changes

###2016-10-11

####1. Skill switching
Click on skill icon to cycle available skills.

####2. Items
To switch: mousewheel or left/right controller d-pad arrows.
To use: mousewheel button or Back controller button.
Keycards, health potions (small, middle, large).
Particle system spawns when health potion is used.

####3. Doors
It works like a portal. To open locked door appropriate key must be used. You can see keys and doors in Jumps & Doors testground.

####4. AI
A little delay before attack.
Hero stands still before next attack. Then he attacks or pursues the target.

####5. Camera
More smooth camera. Look: W/S (up/down arrows) on keyboard or controller right stick.

####6. Pause menu
Image changed. Blur effect added.

####7. Loading screen
Static image appears when the level is loading.

####8. Slow Motion skill
Everything slows for a short period of time. Twirl effect used when casted.

###2016-09-27 First commit

#### Overview
1. 3 levels, 2 testgrounds.
2. Player can switch between available heroes.
3. Each hero has different movement speed, number of jumps, weapons, attack and defense skills.
4. There are AI enemies roaming around the level.
5. They will pursue and attack player if the player is in their sight radius.
6. Player always can pause the game, go to the start menu and choose a level or a testground to play.
7. Input devices: Keyboard and Mouse, Gamepad (I've tested on Xbox360 controller).

##AI (Artificial Intelligence)
1. Hero will mark target (ally or enemy) according to his own sight radius.
2. Every hero can be a leader. If the hero is not a leader, he will be searching for a leader. When the leader is found, hero follows.
If the enemy appears, he attacks.
3. AI hero will use random attack skill when attack distance is reached.
4. AI hero will use random defense skill as soon as his Health reaches the critical level.
