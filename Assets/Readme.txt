

--------------- Notes ---------------

I tried making the game as modular as possible and i think i have done a pretty good job with that.
The AI system i used is maybe a littlebit overkill for what this game is since the AI needs MAX 2 states if not even one state of chasing and firing in one,
but i wanted to try this system out. It is based on this Tutorial by Unity -> " https://www.youtube.com/watch?v=cHUXh5biQMg "

The controls system can be improved but i wanted to focus on making the actual game not a Input system.

I also set the game up to have local coop so to implement that not alot would have to change
other then how cameras are displayed depending on how many players there are.

All the navigation scripts are from Unity. So i could use the Navmesh on a tilted plane and did not have to write my own navigation for 2D.


--------------- Other ---------------

Projectiles keep moving when the game is paused was not really itended but i thought the idea was cool,
so i stopped ships from taking damage if the game is paused but the projectiles still keep moving

I gave the player some immunity time after being hit so he could not instantly die if multiple enemies are shooting at him.

The way i set the UI up is probobly not the best and i am still improving in that area. The reason i seperated the Settings/Options on to another canvas
is to have the abbility to add In-Game Settings/Options


--------------- Improvements ---------------

Performance:
	Stop spawning everything at runtime and create a pooling system that either spawns all the stuff at scene load and add a loading screen
	or spawn everything in the editor before the build.

Polish:
	Make it noticable to the player if he beat his previous highscore by making the text bigger and/or color it for example
	Make the Score text change size when the player gets more score so he sees something happening

Gameplay:
	Add more variations of powerups/missile on hit effects/shooting variations
	Maybe stop bullets from traveling while the game is paused? I think its a cool thing but then again not really what players expect to happen.