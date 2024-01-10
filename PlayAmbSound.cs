using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class PlayAmbSound : MonoBehaviour
{
    [System.Serializable]
    class GridInfos
    {
        public int[] gridPosInfo;
        public string materialInfo;
    }
    [System.Serializable]
    class AudioPointData
    {
        public GridInfos[] gridInfos;
    }

    ArrayList materialList = new ArrayList();
    ArrayList materialList2 = new ArrayList();
    void Start()
    {
        string jsonData = ReadData();
        AudioPointData audioPointData = JsonUtility.FromJson<AudioPointData>(jsonData);
        foreach(GridInfos item in audioPointData.gridInfos)
        {
            Debug.Log(item.materialInfo);
            materialList.Add(item.materialInfo);
        }
        materialList2 = RemixArrayList(materialList);
        for(int i = 0; i < materialList2.Count; i++)
        {
            GameObject gameObject = new GameObject((string)materialList2[i]);
            //AkMultiPositionType
            //AkSoundEngine.SetMultiplePositions(gameObject,);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ArrayList RemixArrayList(ArrayList arrayList)
    {
        HashSet<object> hashSet = new HashSet<object>();
        foreach (var item in arrayList)
        {
            hashSet.Add(item);
        }

        // 将HashSet转换为数组
        object[] array = new object[hashSet.Count];
        hashSet.CopyTo(array);

        // 使用数组来初始化新的ArrayList
        ArrayList newArrayList = new ArrayList(array);
        return newArrayList;
    }



    public string ReadData()
    {
        string readData = null;
        string fileUrl = Application.streamingAssetsPath + "\\mapJson.json";
        if (fileUrl != null)
        {
            StreamReader streamReader = File.OpenText(fileUrl);
            readData = streamReader.ReadToEnd();
            streamReader.Close();
        }
        return readData;
    }
}
