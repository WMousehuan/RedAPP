using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiHintCase :Singleton_Base<UiHintCase>
{
    public override bool isDontDestroy => true;
    public Image case_Image;
    public Text content_Text;
    public IEnumerator ie;
    private void Start()
    {
        case_Image.gameObject.SetActive(false);
    }
    public void Show(string content)
    {
        case_Image.gameObject.SetActive(true);
        content_Text.color = new Color(content_Text.color.r, content_Text.color.g, content_Text.color.b, 0);
        case_Image.color = new Color(case_Image.color.r, case_Image.color.g, case_Image.color.b, 0);
        content_Text.text = content;
        case_Image.rectTransform.sizeDelta = new Vector2(Mathf.Clamp(GeneralTool_Ctrl.GetTextWidth(content_Text)+32, 128, 480), 0);
        case_Image.rectTransform.sizeDelta = new Vector2(case_Image.rectTransform.sizeDelta.x, content_Text.preferredHeight+32);
        IEPool_Manager.instance.KeepTimeToDo(ref ie, 0.32f, null, (_time) =>
        {
            content_Text.color = new Color(content_Text.color.r, content_Text.color.g, content_Text.color.b, Mathf.Lerp(0, 1, 1 - _time / 0.32f));
            case_Image.color = new Color(case_Image.color.r, case_Image.color.g, case_Image.color.b, Mathf.Lerp(0, 0.8f, 1 - _time / 0.32f));

            return true;
        }, () =>
        {
            IEPool_Manager.instance.WaitTimeToDo(ref ie, 1.5f, null, () =>
            {
                IEPool_Manager.instance.KeepTimeToDo(ref ie, 1, null, (_time) =>
                {
                    content_Text.color = new Color(content_Text.color.r, content_Text.color.g, content_Text.color.b, Mathf.Lerp(1, 0, 1 - _time / 1f));
                    case_Image.color = new Color(case_Image.color.r, case_Image.color.g, case_Image.color.b, Mathf.Lerp(0.8f, 0, 1 - _time / 1f));

                    return true;
                }, () =>
                {
                    case_Image.gameObject.SetActive(false);
                    content_Text.color = new Color(content_Text.color.r, content_Text.color.g, content_Text.color.b, 0);
                    case_Image.color = new Color(case_Image.color.r, case_Image.color.g, case_Image.color.b, 0);
                });
            });
        });



    }
}
