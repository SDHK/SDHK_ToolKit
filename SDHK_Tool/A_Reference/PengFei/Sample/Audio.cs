using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public string wavPath;

    public string savePath;

    public AudioSource source;

    public List<AudioClip> audioClips;

	void Update ()
    {
        if (source !=null)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (source.clip != null)
                {
                    source.Play();
                }
            }

            if (Input.GetKeyUp(KeyCode.Q))
            {
                if (wavPath != null && wavPath != "")
                {
                    source.clip = AudioTool.Wav2Clip(wavPath);
                }
                else
                {
                    Debug.Log("error");
                }
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                if (savePath != null && savePath != "")
                {
                    AudioTool.Clip2Wav(source.clip, savePath);
                }
                else
                {
                    Debug.Log("error");
                }
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                if (audioClips != null && audioClips.Count > 0)
                {
                    source.clip = AudioTool.Combine(audioClips);  
                }
                else
                {
                    Debug.Log("error");
                }
            }
        }
	}
}
