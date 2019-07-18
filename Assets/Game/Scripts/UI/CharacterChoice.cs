using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using YummyGame.Framework;

namespace YummyGame.CubeWar
{

    public class CharacterChoice : UIWindow
    {
        public override string ResourcePath => "UI/CharacterChoice";

        [Inst("Occupation/OccupationName")]
        public Text occupationName;

        [UIClick("BtnOccuUP")]
        [Inst("Occupation/BtnOccuUP")]
        public Button btnOccuUP;

        [UIClick("BtnOccuDown")]
        [Inst("Occupation/BtnOccuDown")]
        public Button btnDown;

        [UIClick("BtnEnter")]
        [Inst("Occupation/BtnEnter")]
        public Button btnEnter;

        [UIClick("BtnModeUP")]
        [Inst("ChoiceMode/BtnModeUP")]
        public Button btnModeUP;

        [UIClick("BtnModeDown")]
        [Inst("ChoiceMode/BtnModeDown")]
        public Button btnModeDown;

        [Inst("Occupation/CharacterData/HpNum")]
        Text hpNum;

        [Inst("Occupation/CharacterData/MpNum")]
        Text mpNum;

        [Inst("Occupation/CharacterData/ArmNum")]
        Text armNum;

        TableManager tableManager;
        int occupationIndex = 1;
        List<Transform> characterModes = new List<Transform>();
        int modeIndex = 0;

        #region 重载
        public override void OnCreate()
        {
            base.OnCreate();
            tableManager = CubeWarManager.Instance.MTableManager;
            Transform showMode = root.transform.Find("ChoiceMode/ShowMode");
            for (int i = 0; i < showMode.childCount; i++)
            {
                characterModes.Add(showMode.GetChild(i));
            }
        }

        public override void OnShow()
        {
            base.OnShow();
            occupationIndex = 1;
            SetOccupationInfo();
        }
        #endregion

        #region Btn
        void BtnOccuUP()
        {
            occupationIndex--;
            occupationIndex = occupationIndex <= 0 ? (int)OccupationType.Max - 1 : occupationIndex;
            SetOccupationInfo();
        }

        void BtnOccuDown()
        {
            occupationIndex++;
            occupationIndex = occupationIndex >= (int)OccupationType.Max ? 1 : occupationIndex;
            SetOccupationInfo();
        }

        void BtnEnter()
        {
            Debug.Log("选择职业完成，进入下一关场景");
        }

        void BtnModeUP()
        {
            modeIndex--;
            modeIndex = modeIndex < 0 ? characterModes.Count - 1 : modeIndex;
        }

        void BtnModeDown()
        {
            modeIndex++;
            modeIndex = modeIndex >= characterModes.Count ? 0 : modeIndex;
        }
        #endregion

        /// <summary>
        /// 设置显示职业信息的Text
        /// </summary>
        void SetOccupationInfo()
        {
            occupationName.text = tableManager.GetTableElement<string>(TableType.OccupationTable, occupationIndex, Consts.OccupationTable_Name);
            hpNum.text = tableManager.GetTableElement<int>(TableType.OccupationTable, occupationIndex, Consts.OccupationTable_Hp).ToString();
            mpNum.text = tableManager.GetTableElement<int>(TableType.OccupationTable, occupationIndex, Consts.OccupationTable_Mp).ToString();
            armNum.text = tableManager.GetTableElement<int>(TableType.OccupationTable, occupationIndex, Consts.OccupationTable_Armor).ToString();

            //显示对应模型
            for (int i = 0; i < characterModes.Count; i++)
            {
                if (i != occupationIndex - 1)
                {
                    characterModes[i].gameObject.SetActive(false);
                    continue;
                }
                characterModes[i].gameObject.SetActive(true); 
            }
        }
    }
}
