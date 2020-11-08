    ////////////////////////////////////////////////////////////////
    // If you have any trouble with the asset, please email me at //
    //                  lukebox@hailgames.net                     //
    //               or on Discord Lukebox#8482                   //
    //         I will get back to you within 12 hours             //
    ////////////////////////////////////////////////////////////////

Video tutorial:
https://www.youtube.com/watch?v=01MXkUUXoOg

How-to:

1. Add a new layer called "FOW"
2. Select the FoWCamera prefab and drag it under your main camera
3. In the FoW camera culling mask, disable everything except "FOW"
4. Find Prefab_FoWPlane and set its layer to "FOW"
5. Select your main camera and deselect "FOW" in the culling mask
6. Add FowUnit script to all gameobjects you wish to have vision
7. Start the scene
8. In the FoWCamera script, tweak the fog plane scale and offset values until the plane fits the camera view
9. Right click the script, select Copy Component
10. Stop the scene
11. Right click the script again and select Paste Component Values
12. Done!


Important to remember:

- The default fog height is at y=0. If your terrain is higher, remember to change the fog plane offset y value to match your terrain height, otherwise the fog will be inaccurate
- FowCamera should be a child object to your main camera, and its local position and rotation should be zero
- FowCamera should always have same settings as your main camera (field of view, orthographic view size, clipping planes)
- FowCamera camera depth should be larger than the main camera's depth
