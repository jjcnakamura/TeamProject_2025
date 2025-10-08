using UnityEngine;

namespace SaveData_Settings
{
    //���ʐݒ�̃f�[�^�����[�h
    public  class Load : MonoBehaviour
    {
        public static int bgm, se;          //Audio�p

        //���ʐݒ�̃��[�h
        public static void Audio()
        {
            Debug.Log("���ʐݒ�����[�h���܂���");

            bgm = PlayerPrefs.GetInt("Vol_BG", 8);
            se = PlayerPrefs.GetInt("Vol_SE", 8);

            SoundManager.Instance.VolumeChange(bgm, se);
        }
    }

    //�ۑ��f�[�^���Z�[�u
    public class Save : MonoBehaviour
    {
        //���ʐݒ�̕ۑ�
        public static void Audio(int b, int s)
        {
            Debug.Log("���ʐݒ���Z�[�u���܂���");

            PlayerPrefs.SetInt("Vol_BG", b);
            PlayerPrefs.SetInt("Vol_SE", s);
            PlayerPrefs.Save();

            Load.bgm = b;
            Load.se = s;
        }
    }

    //�ۑ��f�[�^�̏�����
    public class InitializeSaveData : MonoBehaviour
    {
        public static void All()
        {
            Debug.Log("�S�f�[�^�����������܂���");
            PlayerPrefs.DeleteAll();

            Load.Audio();       //������Ԃ̉��ʐݒ�����[�h
        }
    }
}

