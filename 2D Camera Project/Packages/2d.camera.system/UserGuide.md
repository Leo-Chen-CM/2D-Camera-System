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
Currently in order to fade in and out of the screen you'll need to attach the "Activator Fade" script to an object. Said object requries a trigger collider.

### Axis Locking
The X or Y axis can be locked through a toggle. In the camera component you'll be able to lock the X or Y axis. This will prevent the camera from moving along the respective axis.

### Vignette Damage Effect
The Vignette damage effect requires an image to be used in the Camera System 2D script object. You'll need to attach the "Activator Damage" script to an object. Said object requires a trigger collider.

### 