using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    [SerializeField]
    public GameObject StartCanvas;
    [SerializeField]
    public GameObject TargetCanvas;
    [SerializeField]
    public GameObject ExitCanvas;

    public void Exit()
    {
        #if UNITY_EDITOR
			//then stop the play mode
				UnityEditor.EditorApplication.isPlaying = false;
			// if the editor isn't open
			#else
			//that means you're running the build, so close the app
				Application.Quit();
			#endif
    }
}
