using Sonity;
using UnityEngine;

public class MusicPlayList : MonoBehaviour
{
    public SoundEvent[] playlist;

    private EventSettings.EventInfo eventInfo;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int song = Random.Range(0, playlist.Length);

        eventInfo.SoundEvent = playlist[song];
        eventInfo._action = EventSettings.Action.Play;
        eventInfo.allowFadeOut = true;
        eventInfo.stopAllOtherMusic = true;
        
        MusicManager.musicManager.InvokeAudioSettings(eventInfo);
    }
}
