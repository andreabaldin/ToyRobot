# ToyRobot Simulator #  

C# console basic OOP programming exercise, including xUnit.Net unit test.

5 units x 5 units table defined as constant values.  Origin 0,0 is at SOUTH-WEST

Supported commands:
PLACE X,Y,F         Eg. PLACE 0,0,NORTH / PLACE 0,4,EAST / PLACE 1,2,3 (Notice 0=N / 1=E / 2=S / 3=W )
MOVE
LEFT
RIGHT
REPORT

Unrecognised command stops the task.
At start, only PLACE X,Y,F command is accepted.

Output is at system console, however the toyrobot current status is persisted to a text file:  .\ToyRobot_Status.txt
At restart latest status is loaded if any. Delete the status file to return at undefined positioning.


## How to use it ##

`ToyRobot` has 2 running mode: interactive and commands-file.
At command line:
- ToyRobot.exe -h                   help 
- ToyRobot.exe                      - interactive mode -  Standard input commands accepted.
- ToyRobot.exe <CommandsFileName>   - commands-file mode -  Text file listing a command per line.  First must be a valid PLACE X,Y,F



## Licensing ##
`ToyRobot` is MIT Licensed !  
