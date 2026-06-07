using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;

public class CSVReader : MonoBehaviour
{
    //미션 csv 파일로 받아서 파싱, 리스트에 저장하여 관리하는 스크립트

    [Header("CSV File")]
    [SerializeField] private TextAsset _missionCsvFile = null;
    private List<Mission> missionList = null;
    private bool isSolved = false;

    //문자열 파싱 위한 변수 지정
    #region For CSV read
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };
    #endregion

    
    //미션 리스트 반환
    public List<Mission> GetMissionList()
    {
        missionList = readMission();
        return missionList;
    }

    //csv 파일 내 문자열 파싱, mission들 리스트로 저장, 관리
    private List<Mission> readMission()
    {
        var lines = Regex.Split(_missionCsvFile.text, LINE_SPLIT_RE);
        List<Mission> list = new List<Mission>();

        if (lines.Length <= 1) return null;

        var header = Regex.Split(lines[0], SPLIT_RE);

        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;
            if(values[3] == "0")
            {
                isSolved = false;
            }
            Mission msn = new Mission(int.Parse(values[0]), int.Parse(values[1]), values[2], isSolved);
            list.Add(msn);
            //Debug.Log(list[i-1].missionNum + " " +list[i-1].mission);
        }
        return list;
    }
    
}
