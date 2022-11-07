# 2D Camera Effects
This component adds 2D camera effects to the main camera.
It is currently aimed at 2D side scrollers.


## Setup Component
You need to add the component to your project using the Package Manager. Open the Package Manager (Windows > Package Manager), click on the + icon and select "Add package from git URL..." and enter:

X

Note that the URL specifies the complete path to the package and a git tag. The package should now be visible in your project.


## Using the component
The component requires you to drag the appropriate script onto the main camera.
You will also need to tag your main character with the "Player" tag.

Once the component is attached to the main camera and the player tag is designated, you'll then need to toggle which setting you want to use.

### Camera zoom dependent on speed
The player's speed changes how large the zoom will be. If the player slows down or stops, the camera zoom will change back to its original size.