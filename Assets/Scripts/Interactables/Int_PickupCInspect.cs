using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(Int_Pickup))]
public class Int_PickupCInspect : Editor
{
    override public void OnInspectorGUI()
    {

        base.OnInspectorGUI();

        Int_Pickup myScript = target as Int_Pickup;  //this code causes an "a scripted object has a different serialization layout when loading" - error upon build

        if (myScript.pickupType == PickupType.Ammo)
        {
            myScript.ammoType = (AmmoType)EditorGUILayout.EnumPopup("Ammo Type", myScript.ammoType);
        }
    }
}
#endif

