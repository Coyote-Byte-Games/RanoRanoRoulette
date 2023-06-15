
using UnityEngine;

[CreateAssetMenu(menuName="My Assets/SFXManager")]
public class SFXManagerSO : ScriptableObject
{
public AudioClip[] clips;
public enum Sound
{
    boom,
    gunShot,
    portalOpen,
    whoosh,
    boing,
    click,
    mechanical,
    AOL,
    bugInMyTurtleNeck,
    dodgeball,
    ZAWARDO,
    Timemovesagain,
    chainsaw
}
public AudioClip GetClip(Sound sound)
{
    return clips[((int)sound)];
}
public void PlayClip(Sound sound)
{
    FindObjectOfType<AudioSource>().PlayOneShot (clips[((int)sound)]);
}
}