# 3D-Enemy-Detection

Fastest/Lightweight implementation for detecting the closest enemies in a 3D space, using a data structure called K-Dimensional Tree (also known as K-D Tree) Ref: https://medium.com/@katyayanivemula90/what-is-a-k-dimensional-tree-8265cc737d77


## Architecture Overview


### PlayerSpawnerController
- Main Controller responsible for spawning players based on the user input, managing the instances by using object pooling, send the view updates based on the KDTree implementation.

### KdTree
- Implementation for the logic of controlling the logical data and calculating the nearest nodes. It also supports the logic for 2D only.


## Patterns to showcase

#### KdTree
Data structure approach to calculate and organizing closest nodes in the 3D space.

#### Object Pooling
Reusing Objects that already exist when they need to create new instances during runtime.
For this particular object, we're using this approach for the 3D players spawned and for the UI items during hitting scam.

#### Observable
Design pattern approach for setting listeners and updating the view every time:
- A new player spawned
- A player has a new update (e.g hitting scam)

#### Clean Architecture (Controller, Data, Presenters)

The layers of this project are separated by following the clean architecture project pattern:
- Data/Mode: (PlayerMovement is a scriptable object to manage parameters for each player instance)
- Controller: Each Game entity has the logic separately on its own Controller (e.g., Hud, Player)
- Presenters/View: Player has a view layer for updating all the visual changes (e.g Position)

![](Media/usage_1.gif)

![](Media/usage_2.gif)