using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VoicelineState
{
    string name;
    AudioClip[] stateSounds;
    public float soundInterval;
    public float soundIntervallRandomiser;
}

[CreateAssetMenu(fileName = "New VoicelinesData", menuName = "VoicelinesData")]
public class VoicelinesSO : ScriptableObject
{
   public VoicelineState[] states; //we can have different states mostly on outside combat and one inside combat

}
