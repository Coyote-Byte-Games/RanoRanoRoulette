using UnityEditor;
using UnityEngine;

//for now we'll just put in sprite
[CustomPropertyDrawer(typeof(ArrayLayout<>))]
public class MatrixInspector : PropertyDrawer
{

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        //    return property.FindPropertyRelative("rowCount").intValue + 2;
        return (property.FindPropertyRelative("rowCount").intValue+2) * 18;
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        #region Constants
        // int increment = ;
        #endregion



        EditorGUI.PrefixLabel(position, label);

        Rect currentElemPos = position;
        // currentElemPos.y += 18 * 5f;

        int rowCount = property.FindPropertyRelative("rowCount").intValue;
        int colCount = property.FindPropertyRelative("colCount").intValue;
        SerializedProperty matrix = property.FindPropertyRelative("rows");
        Debug.Log(matrix is null);
        for (int j = 0; j < rowCount; j++)
        {

            currentElemPos.height = 18;
            SerializedProperty row = matrix.GetArrayElementAtIndex(j).FindPropertyRelative("elements");
            if (row.arraySize != rowCount)
            {
                row.arraySize = rowCount;
            }

            for (int i = 0; i < colCount; i++)
            {

                currentElemPos.width = position.width / colCount;
                currentElemPos.y = position.height * (j+1f) / rowCount;

                EditorGUI.PropertyField(currentElemPos, row.GetArrayElementAtIndex(i), GUIContent.none);
                currentElemPos.x += currentElemPos.width;


            }
            currentElemPos.x = position.x;
            currentElemPos.y += 18;



        }


    }
}
