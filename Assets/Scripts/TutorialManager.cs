using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public TutorialTarget[] targets;
    public string NextScene;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckTargets());
    }

    IEnumerator CheckTargets()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);

            bool noTargetsActive = true;
            
            foreach (TutorialTarget t in targets)
            {
                if (t != null)
                {
                    noTargetsActive = false;
                }
            }

            if (noTargetsActive)
            {
                SceneManager.LoadScene(NextScene);
            }
        }
    }

 }

