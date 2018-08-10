using System.Collections;
using UnityEngine;

public class SceneChangeTrigger : MonoBehaviour {

    [SerializeField]
    private int sceneIndex;
	
    private void OnTriggerEnter2D(Collider2D other)
    {
        print("trigger entered by " + other.gameObject.name);
        StartCoroutine(endSceneRoutine(other.GetComponentInParent<PlayerBehaviours>().inputIndex));
    }
    IEnumerator endSceneRoutine(int initiatorIndex)
    {
        SceneInfo changeSceneInfo = new SceneInfo();
        changeSceneInfo.sceneIndex = sceneIndex;
        changeSceneInfo.initiatorIndex = initiatorIndex;
        GameEventManager.instance.endSceneEvent();
        yield return new WaitForSeconds(1);
        GameEventManager.instance.changeSceneEvent(changeSceneInfo);
    }
}
