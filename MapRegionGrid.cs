using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.WSA;
using static Sound.MapRegionGrid;

namespace Sound
{
    public class MapRegionGrid : MonoBehaviour
    {
        [System.Serializable]
        public class GridCell
        {
            public bool isInt;
            public int soundLevel = 0;
            public string materialInfo = "dirt";

            // 可以添加更多属性和方法
        }
        public int width = 10;
        public int height = 10;
        public GridCell[,] grid;
        public float cellSize = 1.0f;

        void Start()
        {
            //InitializeGrid();
        }

        void Update()
        {
            
        }

        void InitializeGrid()
        {
            grid = new GridCell[width * 2, height * 2];
            for (int x = 0; x < width * 2; x++)
            {
                for (int y = 0; y < height * 2; y++)
                {
                    grid[x, y] = new GridCell
                    {
                        isInt = true,
                    };
                }
            }
        }

        public void UpdateMaterialInfo(int x, int y, string material)
        {
            grid[x, y].materialInfo = material;
            int[] gridPos = { x, y };
            SaveGridInfo(gridPos, grid[x, y].materialInfo);
        }

        private void OnDrawGizmos()
        {
            if (grid == null)
            {
                InitializeGrid(); // 确保grid被初始化
            }
            Gizmos.color = Color.blue;
            for (int x = -10; x < width; x++)
            {
                for (int y = -10; y < height; y++)
                {
                    // 绘制格子边界
                    Vector3 pos = new Vector3(x, 0, y);
                    Vector3 posRemix = new Vector3(0.5f, 0, 0.5f);
                    Gizmos.DrawWireCube(pos + posRemix, new Vector3(1, 0, 1));
                    Handles.Label(pos + posRemix, $"{x + 10},{y + 10}\n{grid[x + 10, y + 10].materialInfo}");
                }
            }
        }
        [System.Serializable]
        public class GridInfo
        {
            public int[] gridPosInfo;
            public string materialInfo;
        }

        [System.Serializable]
        public class GridInfoList
        {
            public List<GridInfo> gridInfos = new List<GridInfo>();
        }

        public void SaveGridInfo(int[] gridPosInfo, string materialInfo)
        {
            GridInfo gridInfo = new GridInfo
            {
                gridPosInfo = gridPosInfo,
                materialInfo = materialInfo
            };
            SaveToFile(gridInfo);
        }

        public void SaveToFile(GridInfo gridInfo)
        {
            string path = UnityEngine.Application.streamingAssetsPath + "\\mapJson.json";
            GridInfoList gridInfoList;
            if (File.Exists(path))
            {
                // 读取并反序列化现有的JSON文件
                //Debug.Log(File.ReadAllText(path));
                string oldJson = File.ReadAllText(path);
                gridInfoList = JsonUtility.FromJson<GridInfoList>(oldJson);
            }
            else
            {
                // 创建新的列表
                gridInfoList = new GridInfoList();
            }
            if (gridInfoList.gridInfos.Count != 0)
            {
                int gridInfoListLen = gridInfoList.gridInfos.Count;
                bool isNew = true;
                for (int i = 0; i < gridInfoListLen; i++)
                {
                    //Debug.Log(gridInfoListLen);
                    if ((gridInfoList.gridInfos[i].gridPosInfo[0] == gridInfo.gridPosInfo[0]) && (gridInfoList.gridInfos[i].gridPosInfo[1] == gridInfo.gridPosInfo[1]))
                    {
                        isNew = false;
                        gridInfoList.gridInfos[i].materialInfo = gridInfo.materialInfo;
                    }
                }
                //Debug.Log(isNew);
                if (isNew)
                {
                    gridInfoList.gridInfos.Add(gridInfo);
                }
                string newJson = JsonUtility.ToJson(gridInfoList);
                //Debug.Log(newJson);
                Debug.Log("Save Json To" + path);
                File.WriteAllText(path, newJson);
            }
            //json为空
            else {
                    gridInfoList.gridInfos.Add(gridInfo);
                    string newJson = JsonUtility.ToJson(gridInfoList);
                    //Debug.Log(newJson);
                    Debug.Log("Save Json To" + path);
                    File.WriteAllText(path, newJson);
            }

        }
    }

}

