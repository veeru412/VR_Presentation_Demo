//Created by Dilmer Valecillos, amended by Alex Coulombe @ibrews to signal presses and releases and log controller

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class InputManager : MonoBehaviour
{
  [SerializeField]
  private XRNode xRNode = XRNode.LeftHand;
  [SerializeField]
  private XRRayInteractor leftRay, rightRay; 

  private List<InputDevice> devices = new List<InputDevice>();

  private InputDevice device;

  private bool primaryButtonIsPressed;


  void GetDevice()
  {
    InputDevices.GetDevicesAtXRNode(xRNode, devices);
    device = devices.FirstOrDefault();
  }

  void OnEnable()
  {
    if (!device.isValid)
    {
      GetDevice();
    }
  }

  void Update()
  {
    if (!device.isValid)
    {
      return;
    }


    bool primaryButtonValue = false;
    InputFeatureUsage<bool> primaryButtonUsage = CommonUsages.primaryButton;

    if (device.TryGetFeatureValue(primaryButtonUsage, out primaryButtonValue) && primaryButtonValue && !primaryButtonIsPressed)
    {
      primaryButtonIsPressed = true;
      Logger.Debug($"PrimaryButton activated {primaryButtonValue} on {xRNode}");
      leftRay.enabled = !leftRay.enabled;
      rightRay.enabled = !rightRay.enabled;
    }
    else if (!primaryButtonValue && primaryButtonIsPressed)
    {
      primaryButtonIsPressed = false;
      Logger.Debug($"PrimaryButton deactivated {primaryButtonValue} on {xRNode}");
    }
  }
  public void DisableRayInteraction()
  {
    leftRay.enabled = false;
    rightRay.enabled = false;
    this.enabled = Constants.isProffesor;
  }
}
