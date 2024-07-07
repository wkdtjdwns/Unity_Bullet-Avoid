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
        for (int i = 0; i < skills.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                skills[i].UseSkill();
            }
        }

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