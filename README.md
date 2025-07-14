# WildBurnt (not definitive name)

## Basics

This is a **Unity** game project.
The objective is to create a kind of **multiplayer dungeon crawler**.

From 1 to 4 players.
Each player controls a character with their own stats, basic things like Health, Strength, Defense, etc... and an inventory.
Each character can equip up to 3 abilities, one being a default attack.
The goal is for teh players to reach the end of the dungeon.

## Devlog part

### Online

Starting with the basics. I needed 2 clients to communicate to make the game a multiplayer online game.
I used a peer to peer connection. Using TCP since I already used it for another project. [My Network lib for Unity](https://github.com/BourgoisAlexis/LamagzLib?tab=readme-ov-file#network)
One client will be the host of the game and the second one will be a guest.

### Basic map interface

I choose to prototype it mostly with UI elements, since it makes it easier to scale with screen size and make it interactable with basic actions like click, drag, etc...
I ended up with a basic map layout where i spawn some randomly generated tiles.

<br>*Images coming soonish*

### Vote system

Every time the players land on the map screen, they can vote for the next tile to explore.

<br>*Images coming soonish*
