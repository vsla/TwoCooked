using System;
using UnityEngine;

public class StoveBurningWarningUI : MonoBehaviour
{
   private const string IS_BURNING = "IsBurning";
   [SerializeField] private StoveCounter stoveCounter;

   private Animator animator;

   private void Awake()
   {
      animator = GetComponent<Animator>();
   }

   private void Start()
   {
      stoveCounter.OnProgressChanged += StoveCounter__OnProgressChanged;
      Hide();
   }

   private void StoveCounter__OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
   {
      float burnShowProgressAmount = 0.3f;
      bool show = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;

      if (show)
      {
         Show();
      }
      else
      {
         Hide();
      }
   }

   private void Show()
   {
      gameObject.SetActive(true);
      animator.SetBool(IS_BURNING, true);
   }

   private void Hide()
   {
      gameObject.SetActive(false);
      animator.SetBool(IS_BURNING, false);
   }
}
