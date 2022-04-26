using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MicCall : MonoBehaviour
{
    public Button startBtn;
    public Button endBtn;
    public Button playBtn;
    public Button saveBtn;

    public string saveName;
	void Start ()
    {
        if (startBtn != null)
        {
            startBtn.onClick.AddListener(StartRecord);
        }
        if (endBtn != null)
        {
            endBtn.onClick.AddListener(EndRecord);
        }

        if (playBtn != null)
        {
            playBtn.onClick.AddListener(PlayRecord);
        }

        if (saveBtn != null)
        {
            saveBtn.onClick.AddListener(SaveRecord);
        }
    }
	

	void Update ()
    {
		
	}

    public void StartRecord()
    {
        Debug.Log("StartRecord");
        //MicroPhoneInput.getInstance().StartRecord();
        MicroPhoneInput.getInstance().SetIsStopRecord(false);
    }
    public void EndRecord()
    {
        Debug.Log("EndRecord");

        MicroPhoneInput.getInstance().SetIsStopRecord(true);
        //MicroPhoneInput.getInstance().StopClip();
    }
    public void PlayRecord()
    {
        Debug.Log("PlayRecord");
        MicroPhoneInput.getInstance().PlayRecord();

    }
    public void SaveRecord()
    {
        Debug.Log("SaveRecord");
        MicroPhoneInput.getInstance().SaveMusic();
    }
}
