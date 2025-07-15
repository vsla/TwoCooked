using NUnit.Framework;
using UnityEngine;

public class StoveBurningFlashingBarUI : MonoBehaviour
{
    private const string IS_BURNING = "IsFlashing";
    [SerializeField] private StoveCounter stoveCounter;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounter__OnProgressChanged;
        animator.SetBool(IS_BURNING, false);
    }

    private void StoveCounter__OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burnShowProgressAmount = 0.3f;
        bool show = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;

        animator.SetBool(IS_BURNING, show);
    }


}
