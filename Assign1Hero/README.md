# Assignment 1: Hero

## World Camera Settings

Height of 100

## Hero 

Size: 5x5

Control: Follows the mouse, or controlled by keyboard control
        (M toggles between the two)

Keyboard Control: W gradually increases the Hero's speed
                  S gradually decreases the Hero's speed

Hero Rotation:  A rotates the hero counter-clockwise
                D rotates the her clockwise
Rotation Speed: 45 degrees/second

Initial Speed: 20 units/second
Max Speed:     40 units/second

Space-Bar: Spawns an egg-projectile at a rate of one egg every 0.2 seconds


## Spawned Egg

Size: 1x1

Direction: Aligned with Hero direction at time of launch

Speed: 40 units/second

Expiration: 
    * Passes the Bounds of the World
    * At Collision with Enemy


## Enemy

Size: 5x5

Number of Enemies: 10

Location: Randomly spawned within 90% of the world boundaries

Expiration: 
    * At Collision with Hero
    * At 4th Collision with an Egg
        * Each egg costs 80 percent of enemy's current energy

Spawning: A new enemy is spawned so that the number and location 
          conditions are always met


## Application Status

The application prints out the following status:
    * Hero:
        * Control mode: mouse / keyboard
        * Number of times the hero touched the enemy
    * Egg:
        * number of eggs currently in the world
    * Enemy:
        * Total current enemy count
        * Total number destroyed
    * Q-key:
        * Quits the application