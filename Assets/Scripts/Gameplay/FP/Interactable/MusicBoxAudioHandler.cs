using UnityEngine;

public class MusicBoxAudioHandler : MonoBehaviour
{
    [SerializeField] private AudioSource _music;
    [SerializeField] private AudioClip _keyMove;
    [SerializeField] private AudioClip _open;

    public void PlayOpen()
    {
        _music.PlayOneShot(_open);
    }

    public void PlayRotate()
    {
        _music.PlayOneShot(_keyMove);
    }

}
