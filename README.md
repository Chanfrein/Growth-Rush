# Growth-Rush
Growth Rush is a project that I made over the course of 5 months during my free time after work. <br>
The knoweldge comes mostly from my own research and also two online lessons that I took on Udemy.<br>


<strong>The intended design:</strong>
The final goal would be a single-player moba, where the player starts in a base on a symetric map and the other base is controlled by AI.<br>
The player must help his troops push against the AI's troop, either through direct combat or by purchasing upgrades.<br>
<br>
<br>
<p align="center">
<strong>The troops.</strong><br>
 
<img src="./GameplayGifs/TroopsSpawning.gif"><br>
The script responsible for spawning the troops is Scripts/Core/TroopSpawner.cs.<br>
In this script I decided to try my hand at asynchronous functions instead of coroutines, mostly for fun but also because coroutines are native to unity.<br>
<br>
<br>
<img src="./GameplayGifs/PatrolPath.gif"><br>
Here we see the original line of troops that spawn from a base dividing into two. Each ai combatant has a patrol path prafab assgined to it.<br>
I have a simple patrol path script that draw gizmos to facilitate design as well as a contain few methods used by the ai controller.<br>
More in Scripts/Control/PatrolPath.cs<br>
 <br>
 <br>
<img src="./GameplayGifs/TroopsCombat.gif"><br>
Fast forward a minute later and the opposing troops meet in the middle of the map, on one of the two roads.<br>
If you look carefully, you can see that I have no talent for art.<br>
 <br>
 <br>
 <br>
 <strong>The weapons.</strong><br>
 <img src="./GameplayGifs/PlayerSword.gif"><br>
 <img src="./GameplayGifs/PlayerBow.gif"><br>
 <img src="./GameplayGifs/PlayerMagic.gif"><br>
 <br>
 Currently there are three weapons: a sword, a bow and a holy blast. Weapons are a scriptable object, with ranged weapons having an additional Projectile monobehaviour attached to them. More in Scripts/Combat/Weapons.cs<br>
 The weapons, combat mechanics and animation are shared between the player and troops. The Combatant class is flexible and can be used by both the PlayerController and AiController.<br>
 Each weapon prefab also has an animation override controller to replace the current attack animation by its own.<br>
 I also have a text popup to display the damage dealt and give the player some feedback for the hit.<br>
 <br>
 <br>
 <br>
 <strong>The merchants</strong><br>
 The merchants are NPCs meant to purchase upgrades and weapons. Currently the upgrades only include adding swordsmen and archers to the troop spawner.<>
</p>
