using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private GraphicRaycaster graphicRaycaster;
    [SerializeField] private Skill[] skills;

    private List<RaycastResult> raycastResults;
    private PointerEventData pointerEventData;

    private void Awake()
    {
        raycastResults = new List<RaycastResult>();
        pointerEventData = new PointerEventData(null);
    }

    private void Update()
    {
        NumberUse();
        NumPadUse();
        ClickUse();
    }

    private void NumberUse()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                skills[i].UseSkill();
            }
        }
    }

    private void NumPadUse()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1)) { skills[0].UseSkill(); }
        if (Input.GetKeyDown(KeyCode.Keypad2)) { skills[1].UseSkill(); }
        if (Input.GetKeyDown(KeyCode.Keypad3)) { skills[2].UseSkill(); }
        if (Input.GetKeyDown(KeyCode.Keypad4)) { skills[3].UseSkill(); }
    }

    private void ClickUse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            raycastResults.Clear();

            pointerEventData.position = Input.mousePosition;
            graphicRaycaster.Raycast(pointerEventData, raycastResults);

            if (raycastResults.Count > 0)
            {
                Skill cool = raycastResults[0].gameObject.GetComponent<Skill>() as Skill;

                if (cool != null)
                {
                    cool.UseSkill();
                }
            }
        }
    }
}