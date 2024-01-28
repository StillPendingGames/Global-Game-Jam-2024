using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnStart : MonoBehaviour
{
    [SerializeField] private string soundName;

    void Start()
    {
        AudioManager.Instance.StopCurrentSongPlayNew(soundName);
    }

}
