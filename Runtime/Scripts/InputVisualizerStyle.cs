using System;
using DartCore.Utilities;
using UnityEngine;

[CreateAssetMenu(fileName = "new Input Visualizer Style", menuName = "DartCore/UI/Input Visualizer Style", order = 1)]
public class InputVisualizerStyle : ScriptableObject
{
    [Header("Triggers")]
    public Sprite rightTrigger;
    public Sprite leftTrigger;
    
    [Header("Bumpers")]
    public Sprite rightBumper;
    public Sprite leftBumper;
    
    [Header("Sticks")]
    public Sprite leftStick;
    public Sprite leftStickButton;
    public Sprite rightStick;
    public Sprite rightStickButton;
    
    [Header("Dpad")]
    public Sprite dpad;
    public Sprite dpadTop;
    public Sprite dpadBottom;
    public Sprite dpadRight;
    public Sprite dpadLeft;
    
    [Header("Buttons")]
    public Sprite topButton;
    public Sprite bottomButton;
    public Sprite leftButton;
    public Sprite rightButton;

    public Sprite GetSpriteOfKey(ControllerKey key)
    {
        switch (key)
        {
            case ControllerKey.RightTrigger:
                return rightTrigger;
            case ControllerKey.LeftTrigger:
                return leftTrigger;
            case ControllerKey.RightBumper:
                return rightBumper;
            case ControllerKey.LeftBumper:
                return leftBumper;
            case ControllerKey.LeftStick:
                return leftStick;
            case ControllerKey.LeftStickButton:
                return leftStickButton;
            case ControllerKey.RightStick:
                return rightStick;
            case ControllerKey.RightStickButton:
                return rightStickButton;
            case ControllerKey.Dpad:
                return dpad;
            case ControllerKey.DpadTop:
                return dpadTop;
            case ControllerKey.DpadBottom:
                return dpadBottom;
            case ControllerKey.DpadRight:
                return dpadRight;
            case ControllerKey.DpadLeft:
                return dpadLeft;
            case ControllerKey.TopButton:
                return topButton;
            case ControllerKey.BottomButton:
                return bottomButton;
            case ControllerKey.LeftButton:
                return leftButton;
            case ControllerKey.RightButton:
                return rightButton;
            default:
                throw new ArgumentOutOfRangeException(nameof(key), key, null);
        }
    }
}
