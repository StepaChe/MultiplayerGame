using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] LayerMask movementMask;

    Character character;
    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    public void SetCharacter(Character character, bool isLocalPlayer)
    {
        this.character = character;
        if (isLocalPlayer)
        {
            cam.GetComponent<CameraController>().target = character.transform;
            SkillsPanel.instance.SetSkills(character.unitSkills);
        }
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            if (character != null)
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    // при нажатии на правую кнопку мыши перемещаемся 
                    // в указанную точку
                    if (Input.GetMouseButtonDown(1))
                    {
                        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, 100f, movementMask))
                        {
                            CmdSetMovePoint(hit.point);
                        }
                    }
                    // при нажатии на левую кнопку мыши взаимодействуем 
                    // с объектами
                    if (Input.GetMouseButtonDown(0))
                    {
                        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, 100f,
                            ~(1 << LayerMask.NameToLayer("Player"))))
                        {
                            Interactable interactable =
                                hit.collider.GetComponent<Interactable>();
                            if (interactable != null)
                            {
                                CmdSetFocus(
                                    interactable.GetComponent<NetworkIdentity>()
                                );
                            }
                        }
                    }
                }
                // использование навыков
                if (Input.GetButtonDown("Skill1")) CmdUseSkill(0);
                if (Input.GetButtonDown("Skill2")) CmdUseSkill(1);
                if (Input.GetButtonDown("Skill3")) CmdUseSkill(2);
            }
        }
    }

    [Command]
    void CmdSetMovePoint(Vector3 point)
    {
        if (!character.unitSkills.inCast) character.SetMovePoint(point);
    }

    [Command]
    void CmdSetFocus(NetworkIdentity newFocus)
    {
        if (!character.unitSkills.inCast)
            character.SetNewFocus(newFocus.GetComponent<Interactable>());
    }

    [Command]
    void CmdUseSkill(int skillNum)
    {
        if (!character.unitSkills.inCast) character.UseSkill(skillNum);
    }

    private void OnDestroy()
    {
        if (character != null) Destroy(character.gameObject);
    }
}
