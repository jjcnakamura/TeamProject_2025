using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MouseManager : Singleton<MouseManager>
{
    //�}�E�X�̍��W
    public Vector3 mousePos { get; private set; }
    public Vector3 worldPos { get; private set; }

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

        //Tag��ClickObj�̃I�u�W�F�N�g�̂ݎQ��
        var clickables = hits
            .Where(h => h.collider.CompareTag("ClickObj"))
            .OrderBy(h => h.distance)  //��O�i�������Z���j���Ƀ\�[�g
            .ToArray();

        //OnMouseEnter
        if (clickables.Length > 0)
        {
            mouseRayHits = clickables[0];
        }
        //OnMouseExit
        else
        {
            mouseRayHits = new RaycastHit();
        }
    }
}
