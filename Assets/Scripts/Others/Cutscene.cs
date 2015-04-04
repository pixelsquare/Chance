using UnityEngine;
using System.Collections;

public delegate void CutsceneFunc();

public class Cutscene : MonoBehaviour {

	# region Public Variables

	[SerializeField]
	private string animationTrigger;

	# endregion Public Variables

	# region Private Variables

	private Animator cutsceneAnimator;
	private AnimatorStateInfo animatorStateInfo;
	private int animationID;

	private CutsceneFunc endFunc;

	# endregion Private Variables

	private void Start() {
		cutsceneAnimator = GetComponent<Animator>();
		animatorStateInfo = cutsceneAnimator.GetCurrentAnimatorStateInfo(0);
		animationID = Animator.StringToHash("Base Layer.Cutscene");
	}

	public void RunCutscene(CutsceneFunc end) {
		endFunc = end;
		cutsceneAnimator.SetTrigger(animationTrigger);
		StartCoroutine("CutsceneAnimation");
	}

	public void StopCutscene() {
		if (IsAnimating()) {
			StartCoroutine(StopAnimation());
			StopCoroutine("CutsceneAnimation");

			if (endFunc != null) {
				endFunc();
			}
		}
	}

	private IEnumerator CutsceneAnimation() {
		yield return new WaitForSeconds(0.2f);
		animatorStateInfo = cutsceneAnimator.GetCurrentAnimatorStateInfo(0);
		while (IsAnimating()) {
			animatorStateInfo = cutsceneAnimator.GetCurrentAnimatorStateInfo(0);
			yield return null;
		}

		if (endFunc != null) {
			endFunc();
		}
	}

	private IEnumerator StopAnimation() {
		yield return new WaitForSeconds(1f);
		cutsceneAnimator.SetTrigger("Skip");
	}

	private bool IsAnimating() {
		return animatorStateInfo.nameHash == animationID;
	}
}