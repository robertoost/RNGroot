## RNGroot
Tree generation using Space Colonization, altered to allow for branch pruning and recovery.

This project was created for my twin Master's thesis Artificial Intelligence and Game & Media Tech under the guidance of 

1st supervisor: Prof. Dr. M.J. (Marc) van Kreveld, m.j.vankreveld@uu.nl
2nd supervisor: Dr. ing. S.C.J. (Sander) Bakkes, s.c.j.bakkes@uu.nl

### Description
This project was created to determine whether the Space Colonization algorithm could be altered to capture tree regrowth behavior after branch loss. By manually or automatically pruning, tree shapes can be generated that would not have resulted from the classic space colonization algorithm.

_Two trees, manually pruned to different degrees._
![treesis comparison](https://github.com/robertoost/RNGroot/assets/33265853/06ec39a2-5b2a-4711-bd23-2d34e61e5083)

_One tree, generated step by step with an upward directional bias for branch growth._
![ezgif-6-f384e499c1](https://github.com/robertoost/RNGroot/assets/33265853/28ba4c2a-9516-4d52-bee3-45292cd63af2)

Nine models with two varying parameters were created, allowing for different tree shapes to be generated. 

### Implementation

![SpaceColonizationExplanation](https://github.com/robertoost/RNGroot/assets/33265853/e9022718-cabf-4205-aa37-9a96db80972e)

### Installation
Using Unity Hub, download Unity version 2022.3.2f1. Then, download this project to a local folder, and select the project folder from the Unity Hub menu.
27 generated trees were saved as prefabs, 3 for each model variant. These can be found in the prefab folder.
Three scenes are available for each variation of the pruning parameter. In each of these scenes, three tree generators are present. One for each of three possible directional bias settings.
