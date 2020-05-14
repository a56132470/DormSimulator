using Excel;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;

public class ExcelAccess : MonoBehaviour
{
    public static string Excel = "\\Excel\\RoommateAction";

    public static List<CharacterAction>[] selectActionTable(string sheetName = "sheet1")
    {
        string excelName = Excel + ".xlsx";
        DataRowCollection collect = ReadExcel(excelName, sheetName);

        List<CharacterAction>[] characterActionArray = new List<CharacterAction>[3];
        List<CharacterAction> studyActions = new List<CharacterAction>();
        List<CharacterAction> amusementActions = new List<CharacterAction>();
        List<CharacterAction> laborActions = new List<CharacterAction>();
        for (int i = 1; i < collect.Count; i++)
        {
            if (collect[i][1].ToString().Equals("")) continue;

            CharacterAction characterAction = new CharacterAction
            {
                Name = collect[i][1].ToString(),
                Logic = (int)collect[i][2],
                Talk = (int)collect[i][3],
                Athletics = (int)collect[i][4],
                Creativity = (int)collect[i][5],
                Money = (int)collect[i][6],
                SelfControl = (int)collect[i][7],
                SuccessRate = (float)collect[i][8],
                NeedLogic = (int)collect[i][9],
                NeedTalk = (int)collect[i][10],
                NeedAthletics = (int)collect[i][11],
                NeedCreativity = (int)collect[i][12],
                Captions = new string[3]
                {
                    collect[i][13].ToString(),
                    collect[i][14].ToString(),
                    collect[i][15].ToString()
                }
            };
            string type = collect[i][0].ToString().Split('_')[1];

            switch (type)
            {
                case "Study":
                    characterAction.Type = ActionType.Study;
                    studyActions.Add(characterAction);
                    break;

                case "Amusement":
                    characterAction.Type = ActionType.Amusement;
                    amusementActions.Add(characterAction);
                    break;

                case "Labor":
                    characterAction.Type = ActionType.Labor;
                    laborActions.Add(characterAction);
                    break;
            }

            Debug.Log(characterAction.Name);
        }
        characterActionArray[0] = studyActions;
        characterActionArray[1] = amusementActions;
        characterActionArray[2] = laborActions;

        return characterActionArray;
    }

    private static DataRowCollection ReadExcel(string excelName, string sheetName)
    {
        string path = Application.streamingAssetsPath + "/" + excelName;
        FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        DataSet result = excelReader.AsDataSet();

        return result.Tables[sheetName].Rows;
    }
}