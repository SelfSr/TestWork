using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthView : MonoBehaviour
{
    [SerializeField] private Image fill;
    [SerializeField] private TMP_Text counterTxt;
    [SerializeField] private TMP_Text damagePopupTxt;

    private Tween filling;
    private Sequence damagePopup;
    private float maxValue;
    private float currentValue;

    public void Initialize(float max)
    {
        maxValue = max;
        currentValue = max;

        fill.fillAmount = 1f;
        counterTxt.text = maxValue.ToString();
    }

    public void UpdateValue(float value)
    {
        if (value < 0)
            value = 0;

        float damage = currentValue - value;
        currentValue = value;

        float targetFill = Mathf.Clamp01(value / maxValue);

        if (filling != null && filling.active)
            filling.Complete();

        filling = fill.DOFillAmount(targetFill, 0.5f);
        counterTxt.text = value.ToString();

        if (damage > 0)
            ShowDamagePopup(damage);
    }

    private void ShowDamagePopup(float damage)
    {
        damagePopupTxt.text = damage.ToString();

        Vector2 randomOffset = new(Random.Range(-100f, 100f), 0f);
        damagePopupTxt.rectTransform.anchoredPosition = randomOffset;
        damagePopupTxt.alpha = 1f;

        damagePopup?.Kill();

        damagePopup = DOTween.Sequence()
            .Append(damagePopupTxt.DOFade(0f, 1f).SetEase(Ease.InQuad))
            .Join(damagePopupTxt.rectTransform.DOAnchorPos(randomOffset + Vector2.up * 50f, 0.5f));
    }
}