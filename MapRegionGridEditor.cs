using UnityEngine;
using UnityEditor;
using Sound;
using static UnityEditor.PlayerSettings;

[CustomEditor(typeof(MapRegionGrid))]
public class SoundRegionGridEditor : Editor
{
    public int width = 10;
    public int height = 10;
    private string[] options = new string[] { "dirt", "water", "magma", "grass", "stone", "wood" };
    private int selectedOption = 0;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // 添加材质选择器
        EditorGUILayout.LabelField("选择材质");
        selectedOption = EditorGUILayout.Popup(selectedOption, options);

        // 更新选中的材质
        //if (GUILayout.Button("更新材质"))

    }

    void OnSceneGUI()
    {
        // 处理Scene视图中的输入事件
        Event e = Event.current;
        if (e.type == EventType.MouseDown && e.button == 0)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 hitPoint = hit.point;
                Debug.Log("hitPoint " + hitPoint.x + " " +hitPoint.y);
                //Gizmos.color = Color.red;
                //Gizmos.DrawWireCube(hit.point, new Vector3(1, 0, 1));
                int gridX = Mathf.FloorToInt(hitPoint.x) + width;
                int gridY = Mathf.FloorToInt(hitPoint.z) + height;
                Debug.Log("grid " + gridX + " " + gridY);
                MapRegionGrid grid = (MapRegionGrid)target;
                if (gridX >= 0 && gridX < width*2 && gridY >= 0 && gridY < height*2)
                {
                    grid.UpdateMaterialInfo(gridX, gridY, options[selectedOption]); // 假设这是一个更新格子材质的方法
                    e.Use(); // 标记事件已被使用
                }
            }
        }
    }
}
