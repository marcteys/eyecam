# Eyecam Open-Hardware

**Edit April 1st: I am currently editing the video of the whole fabrication process, including how to connect the mechanical elements.**

The mechanical design of Eyecam result of a long iterative process. I used Blender 2.91.0 to model the 3D printer parts. The main technical challenge was to pack everything as much as possible. 

If you want to reproduce the design in a proper CAO software (Fusion360), feel free to do it and share the 

## 3D Printed Files 

The model has been printed in an Ender Pro 3 and an Ultimaker 5S. 

The folder `STL_export` contains all the different Eyecam pieces that you need to print. 

### Printing
The best printing settings are 0.16mm in height with a 0.4mm nozzle.  Pay attention to the orientation of the parts as well as adding some support exclusion to the tiny screw holes.



## 3D CAO Files 

Two main files are present in this folder. 

1. *Eyecam_OpenHardware_Clean.blend* is the cleaned source file of Eyecam. Each part is sorted in a collection (top right). To export, select one or several objects and `File> Export As > STL`. 
2. *Eyecam_OpenHardware_Dirty.blend* is the base file I used for modelling. I used a maximum of boolean (procedural modelling) to have a non-destructive control of the shape. Attention, this file is super messy and not cleaned. 


### Additional Blender Addons (optional)
- Measure It: Measure the dimension of the 3D parts. 
- [Multi Exporter](https://blenderartists.org/t/multi-exporter/1136641): Export the mesh/parts of the mesh at once
- Freestyle (for render) 



## Hardware List 


TBD