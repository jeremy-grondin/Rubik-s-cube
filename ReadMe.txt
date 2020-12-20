 [ProjectName] :  Rubick's Cube
///////////////////////////////
 [Author] :   TODOROV Anatole
	      GRONDIN Jeremy
///////////////////////////////
 [Techs] :   - c#
             - Unity
///////////////////////////////
 [To execute] :   - open Unity's project and click "Play"
		  - Open RubicksCube.exe
///////////////////////////////
 [Inputs] :  - "Mouse Right Hold + Mouse motion" : Rotate the Rubick's Cube
	     - "Mouse Left Hold + Mouse motion" : otate a slice of the Rubick's Cube
	     - "A", "Z", "E", "R", "T" and "Y" : correspond to the 6 buttons below in Game
	     - "S" : Shuffle the Rubick's Cube once
///////////////////////////////
 [Features] :   - Rotation of the cube (Quat * rotation in this order to rotate in World)
		- Rotation of a slice (rotation * Quat in this order to rotate in Local)
		- Generation of a cube depending on size given (initialize only the faces needed)		
 		- Shuffle depending on number given (Shuffle a random Slice by a random angle)
///////////////////////////////
 [Additionnal Notes] : -In the Build, it can happen that the Victory don't show up even if the requirement are met.

