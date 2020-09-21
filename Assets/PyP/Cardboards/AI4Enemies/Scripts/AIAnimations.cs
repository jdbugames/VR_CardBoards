using UnityEngine;
using System.Collections;

public class AIAnimations : MonoBehaviour
{

    public AnimationClip idleAnimation;
    public AnimationClip walkAnimation;
    public AnimationClip fightAnimation;
    public AnimationClip hitAnimation;
    public AnimationClip dieAnimation;

    private AIController aiController;

    //private string idleAnimationName = "idle";
    //private string walkAnimationName = "run";
    //private string fightAnimationName = "shoot";
    //private string hitAnimationName = "";
    //private string dieAnimationName = "";

    void Awake()
    {
        aiController = gameObject.GetComponentInChildren<AIController>() as AIController;

        aiController.idleAnimation = idleAnimation.name;
        aiController.walkAnimation = walkAnimation.name;
        aiController.fightAnimation = fightAnimation.name;

        if (hitAnimation != null)
        {
            aiController.hitAnimation = hitAnimation.name;
        }
        if (dieAnimation != null)
        {
            aiController.dieAnimation = dieAnimation.name;
        }
               
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
