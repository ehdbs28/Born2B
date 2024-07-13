using UnityEngine;

public class TitleBoardUI : UIComponent
{
    [Space(15f)]
    [SerializeField] float startDelay = 5f;
    [SerializeField] float loopDelay = 5f;

    private readonly int PLAY_TRIGGER_HASH = Animator.StringToHash("Play");
    private Animator animator = null;
    private float timer = 0f;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        timer = startDelay;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            animator.SetTrigger(PLAY_TRIGGER_HASH);
            timer = loopDelay;
        }
    }
}
