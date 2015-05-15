using UnityEngine;
using System.Collections;
using UnityEditor;

[CanEditMultipleObjects/*, CustomEditor(typeof(GameObject), true)*/]
public class UnitInfoEditor : Editor
{
    void DISABLED_OnSceneGUI()
    {
        var targetGo = target as GameObject;
        if(targetGo != null)
        {
            var targetGoGUIPosition = HandleUtility.WorldToGUIPoint(targetGo.transform.position);
            if (targetGo.CompareTag("Actor"))
            {
                var guiStyle = GUI.skin.FindStyle("GroupBox");

                Handles.BeginGUI();
                {
                    GUI.Label(new Rect(targetGoGUIPosition.x, targetGoGUIPosition.y, 200, 100), "Actor", guiStyle);
                }
                Handles.EndGUI();
            }
        }
    }
}
