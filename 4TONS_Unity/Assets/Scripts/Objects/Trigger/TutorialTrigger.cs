using System.Collections;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour {

    TutorialLevelManager tutorialLevelManager;
    public int tutorialIndex;
    public GameObject wallCollider;

    private void Start()
    {
        tutorialLevelManager = GameObject.Find("LevelManager").GetComponent<TutorialLevelManager>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        print("tutorial trigger entered by " + other.gameObject.name);
        tutorialLevelManager.loadTutorial(tutorialIndex);
        StartCoroutine(openDoor());
    }
    private IEnumerator openDoor()
    {
        yield return new WaitForSeconds(3f);
        Destroy(wallCollider);
    }
}
