# 2D Camera Effects
This component adds 2D camera effects to the main camera.
It is currently aimed at 2D side-scrollers.


## Setup Component
You need to add the component to your project using the Package Manager. Open the Package Manager (Windows > Package Manager), click on the + icon and select "Add package from git URL..." and enter:

https://github.com/Leo-Chen-CM/2D-Camera-System.git?path=/2D%20Camera%20Project/Packages/2d.camera.system

## Using the component
The component requires you to add the appropriate script onto the main camera through the Add Component.
You will also need to tag your main character with the "Player" tag.

Once the component is attached to the main camera and the player tag is designated, you'll then need to toggle which setting you want to use.

You will also need to create a camera script that will enable other features included in the package.

For a list of components that are in the functions, the CameraSystem2D script in the inspector will show what can be toggled.

<img src="https://github.com/Leo-Chen-CM/2D-Camera-System/blob/main/GifsnImages/InspectorVariables.png" alt="CameraSystem2D Inspector" />

## Features 

### Pan Camera
Holding Left Control and one of the designated keys will pan the camera to X position
away from the player. The designated keys by default are I,J,L,M.

<img src="https://github.com/Leo-Chen-CM/2D-Camera-System/blob/main/GifsnImages/PanCamera.gif" alt="Pan Camera Example" />

#### Adding the component
In a camera script in the start function call the various functions for assigning keys.

For example:

CameraScript.cs

    void Start()
    {
        CameraSystem2D.Instance.SetCameraPanKeyCode();
    }

In the SetCameraPanKeyCode function you are able to rebind the keys to what you need them to be.
SetCameraPanKeyCode(Keycode.LeftCtrl, Keycode.T, Keycode.G, Keycode.F, Keycode.H);


### Camera zooms in and out
By pressing U or O, the camera will zoom in or out.
Alternatively, by using the mouse's scroll wheel to zoom in or out.

<img src="https://github.com/Leo-Chen-CM/2D-Camera-System/blob/main/GifsnImages/ZoomInnOut.gif" alt="Zoom in and out Example" />

#### Adding the component
In a camera script in the start function call the various functions for assigning keys.

For example:

CameraScript.cs

    void Start()
    {
        SetCameraZoomKeysSetCameraZoomKeys();
    }

In the SetCameraZoomKeys function you are able to rebind the keys to what you need them to be.

SetCameraZoomKeys(Keycode.U, Keycode.O);

### Camera Fade to black
To use the fade in and out to black feature you need to call the following function in a script. Whether this is upon the death of your character or a transition to another level.

<img src="https://github.com/Leo-Chen-CM/2D-Camera-System/blob/main/GifsnImages/FadeToBlack.gif" alt="Fade to black Example"/>

#### Adding the component
Example:

Acitvator.cs

    void OnTriggerEnter2D()
    {
        CameraSystem2D.Instance.FadeInOut();
    }

An example of the function can be used by attaching the "Activator Fade" script to an object. Said object requires a trigger collider.
By having a player tagged object collide with the activator will the fade to black occur.

### Camera Position Locking
Upon pressing a certain button or a certain event occurs, the camera’s position will lock either to its current point or towards an important object. It can also simply lock its x and y position as well.

<img src="https://github.com/Leo-Chen-CM/2D-Camera-System/blob/main/GifsnImages/CameraPositionLocking.gif" alt="Camera Position Locking Example" />

<img src="https://github.com/Leo-Chen-CM/2D-Camera-System/blob/main/GifsnImages/CameraPointOfInterest.gif" alt="Point of Interest" />

#### Adding the component
Acitvator.cs

    void Function()
    {
        //This will lock the camera in place
        CameraSystem2D.Instance.LockCameraPosition();
    }


### Axis Locking
The X or Y axis can be locked through a toggle. In the camera component you'll be able to lock the X or Y axis. This will prevent the camera from moving along the respective axes.

Additionally you are able to call the SetXAxisLock() and SetYAxisLock() and pass in a bool to lock it that way.

<img src="https://github.com/Leo-Chen-CM/2D-Camera-System/blob/main/GifsnImages/AxisLocking.gif" alt="Axis Locking Example" />

#### Adding the component
Acitvator.cs

    void Function()
    {
        //This will lock or unlock the X axis from moving the camera.
        CameraSystem2D.Instance.ToggleXAxisLock();
    }

### Camera zoom dependent on speed
The player's speed changes how large the zoom will be. If the player slows down or stops, the camera zoom will change back to its original size.

<img src="https://github.com/Leo-Chen-CM/2D-Camera-System/blob/main/GifsnImages/CameraSpeedZoom.gif" alt="Camera Zoomed Based On Speed Example" />

#### Adding the component
For activating this feature all you need to to is activate the toggle in the inspector.

### Directional Camera
Toggling this option on will have the camera be offset by X amount.
Moving the character it is attached to will have the camera offset be moved towards where the character it is facing.

<img src="https://github.com/Leo-Chen-CM/2D-Camera-System/blob/main/GifsnImages/DirectionalCamera.gif" alt="Directional Camera Example" />

### Camera zooms out based on the height/Y axis
The camera zooms out if the player character has moved above a certain threshold. The higher
they go the wider the field of view for the camera will be.

<img src="https://github.com/Leo-Chen-CM/2D-Camera-System/blob/main/GifsnImages/ZoomOutYAxis.gif" alt="Camera zooms out based on Y axis Example" />

#### Adding the component
Toggle the component in the Camera System 2D and adjust the minimum and maximum height thresholds.

<img src="https://github.com/Leo-Chen-CM/2D-Camera-System/blob/main/GifsnImages/CameraZoomHeightVars.png" alt="Camera zooms out based on Y axis Example" />

### Vignette Damage Effect
The Vignette damage effect requires an image to be used in the Camera System 2D script.

An example of this effect taking place is attaching the "Activator Damage" script to an object. Said object requires a trigger collider.
By having a player tagged object collide with the activator will increase the transparency of the image. In the inspector for the script, you'll be able to set the "damage" of the object to increase the effect. By default it is set to 10.

<img src="https://github.com/Leo-Chen-CM/2D-Camera-System/blob/main/GifsnImages/VignetteEffect.gif" alt="Vignette Damage Effect Example" />

#### Adding the component
Create a script that has the AssignVignetteValue function in it and attach it to an object.

Acitvator.cs

    void Function()
    {
        //This will increase or decrease the alpha value of the image.
        CameraSystem2D.Instance.AssignVignetteValue(float t_value);       
    }

### Camera Shake Effect
A simple camera shaking effect.

<img src="https://github.com/Leo-Chen-CM/2D-Camera-System/blob/main/GifsnImages/CameraShakeEffect.gif" alt="Camera Shake Effect Example" />

#### Adding the component
Create a script that has the AssignVignetteValue function in it and attach it to an object.

Acitvator.cs

    void Function()
    {
            //Depending on the size of the trembling/shaking it will either be a very big shake or a very small one

            int randomInt = Random.Range(0, 1);

            if (randomInt == 0)
            {
                m_trembleSmall = false;
            }
            else
            {
                m_trembleSmall = true;
            }

            CameraSystem2D.Instance.EnableTremble(m_trembleSmall);   
    }

### Erratic Camera Movement
Causes the camera to move and rotate in locations that are tied to the player object.

<img src="https://github.com/Leo-Chen-CM/2D-Camera-System/blob/main/GifsnImages/RandomCameraMovement.gif" alt="Random Camera Movement" />

#### Adding the component
Create a script that has the RandomMovementCameraEffect function in it and attach it to an object. It also requires two variables passed into it that determine how long the effect lasts and how intense it is.

Acitvator.cs

    void Function()
    {
        CameraSystem2D.Instance.RandomMovementCameraEffect(duration, intensity);
    }

### Camera Rotating
A function that rotates the camera's Z axis by X amount. Either triggered by a game object or manually called.

<img src="https://github.com/Leo-Chen-CM/2D-Camera-System/blob/main/GifsnImages/CameraRotation.gif" alt="Camera Rotation Example" />

#### Adding the component
Create a script that has the CallRotateCamera function in it and attach it to an object. It also requires two variables passed into it that determine the speed of the rotation and degrees to rotate by.

Acitvator.cs

    void Function()
    {
        CameraSystem2D.Instance.CallRotateCamera(speed, rotationAmount);
    }

### Damage/Color Flicker
A simple color flash of the screen that signifies something has occured. Whether you've takeb damage or gained health.

<img src="https://github.com/Leo-Chen-CM/2D-Camera-System/blob/main/GifsnImages/ColorFlicker.gif" alt="Screen Flash Example" />

#### Adding the component
Create a script that has the FlashColour function in it and attach it to an object. It also requires two variables passed into it the color of the flashing and duration of the flash.

Acitvator.cs

    void Function()
    {
         CameraSystem2D.Instance.FlashColour(new Color(1, 0, 0, 0), flashDuration);
    }
