using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : Singleton<MouseManager>
{
    //�}�E�X�̍��W
    public Vector3 mousePos { get; private set; }
    public Vector3 worldPos { get; private set; }

    //���݃}�E�X���������Ă���I�u�W�F�N�g
    HashSet<GameObject> currentMouseHits = new HashSet<GameObject>();

    //�}�E�X��Ray�ɏՓ˂��Ă���ClickObj�^�O�t���I�u�W�F�N�g
    public RaycastHit mouseRayHits { get; private set; }

    void Update()
    {
        //�}�E�X�J�[�\���̍��W���i�[
        mousePos = Input.mousePosition;
        //�X�N���[�����W�����[���h���W�ɕϊ�
        worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));

        ////////////////////////////////////////////////////////////////////////////////////

        //�}�E�X�̈ʒu����Ray���΂�
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 100f);
        //���t���[���Ńq�b�g����Ray�Փ˃I�u�W�F�N�g
        HashSet<GameObject> newHits = new HashSet<GameObject>();

        //OnMouseEnter
        foreach (RaycastHit hit in hits)
        {
            //�N���b�N�\�I�u�W�F�N�g�Ƀ}�E�X��������ꍇ
            if (hit.collider.CompareTag("ClickObj"))
            {
                newHits.Add(hit.collider.gameObject);

                //�V�����}�E�X��������I�u�W�F�N�g�i�O�t���[���ɂ͂Ȃ������j
                if (!currentMouseHits.Contains(hit.collider.gameObject))
                {
                    mouseRayHits = hit;
                }
            }
        }
        //OnMouseExit
        foreach (GameObject old in currentMouseHits)
        {
            //�N���b�N�\�I�u�W�F�N�g����}�E�X�����ꂽ�ꍇ
            if (!newHits.Contains(old))
            {
                mouseRayHits = new RaycastHit();
            }
        }

        //��Ԃ��X�V
        currentMouseHits = newHits;
    }
}
