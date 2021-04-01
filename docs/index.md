## Eyecam: The Open-Hardware Human-eye webcam.  

This is the open-source repository for Eyecam. The [repository](https://github.com/marcteys/eyecam) contains all the files needed to reproduce the device, from the 3D-printed hardware parts to the control software. 

To know more about the projct, visit https://marcteyssier.com/projects/eyecam/

**I am currently editing a video tutorial of the whole build process (*45minutes*).** This documentation lack step-by-step tutorials. 


![Eyecam](https://raw.githubusercontent.com/marcteys/eyecam/main/Mechanical/pictures/eyecam.jpg)


## Mechanical Design

In the folder [Mechanical](https://github.com/marcteys/eyecam/tree/main/Mechanical) , you will find the .stl files as well as the source CAD file. I modelled the mechanical parts with Blender, don't be surprised it's not perfect. 


## Software 
The [Software](https://github.com/marcteys/eyecam/tree/main/Software) folder of this repo contains the 3 main parts to run and move eyecam.
1. **Firmware:**  Arduino program to control the 6 servo motors used to move the eyeball, eyelids and eyebrows. It is optimized to run on an Arduino Leonardo Pro Micro. 
2. **Raspberry Pi as Camera:** How to turn a Raspberry Pi Zero + Pi Cam as a standard USB camera
3. **Unity Control Interface:** Additional interface to visually control the motors and  run the computer vision processing




## Licence
Eyecam is under MIT licence. See LICENCE file. 
