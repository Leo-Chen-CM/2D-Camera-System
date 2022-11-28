# 2D Camera Effects
This component adds 2D camera effects to the main camera.
It is currently aimed at 2D side scrollers.


## Setup Component
You need to add the component to your project using the Package Manager. Open the Package Manager (Windows > Package Manager), click on the + icon and select "Add package from git URL..." and enter:

https://github.com/Leo-Chen-CM/2D-Camera-System.git?path=/2D%20Camera%20Project/Packages/2d.camera.system

Note that the URL specifies the complete path to the package and a git tag. The package should now be visible in your project.


## Using the component
The component requires you to add the appropriate script onto the main camera through the Add Component.
You will also need to tag your main character with the "Player" tag.

Once the component is attached to the main camera and the player tag is designated, you'll then need to toggle which setting you want to use.

### Camera zoom dependent on speed
The player's speed changes how large the zoom will be. If the player slows down or stops, the camera zoom will change back to its original size.

### Camera Fade to black
You will need to create a Unity Canvas attached to the Camera. In it will need to have an Image of a color of your choice. Set its alpha to zero.
Currently in order to fade in and out of the screen it is tied to the E key. Pressing it will fade the camera to black or transparent.