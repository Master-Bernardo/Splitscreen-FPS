using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VoicelineState
{
    public string name;
    public AudioClip[] stateSounds;
    public float soundInterval;
    [Tooltip("the next sound playTime is time time + soundInterval + - or + randomiser")]
    public float soundIntervallRandomiser;
}

[CreateAssetMenu(fileName = "New VoicelinesData", menuName = "VoicelinesData")]
public class VoicelinesSO : ScriptableObject
{
   public VoicelineState[] states; //we can have different states mostly on outside combat and one inside combat

}
