using UnityEngine;
using UnityEditor;

public class ShowHideWireframeExample : EditorWindow
{

    [MenuItem("Example/Show WireFrame %s")]
    static void ShowWireframe()
    {
        foreach (GameObject s in Selection.gameObjects)
        {
            Renderer rend = s.GetComponent<Renderer>();

            if (rend)
                EditorUtility.SetSelectedRenderState(rend, EditorSelectedRenderState.Wireframe);
           // EditorUtility.
        }
    }



    [MenuItem("Example/Show WireFrame %s", true)]
    static bool ShowWireframeValidate()
    {
        return Selection.activeGameObject != null;
    }



    [MenuItem("Example/Hide WireFrame %h")]
    static void HideWireframe()
    {
        foreach (GameObject s in Selection.gameObjects)
        {
            Renderer[] rends = s.GetComponentsInChildren<Renderer>();

            foreach (Renderer rend in rends)
            {
                if (rend)
                    EditorUtility.SetSelectedRenderState(rend, EditorSelectedRenderState.Hidden);
            }
        }
    }



    [MenuItem("Example/Hide WireFrame %h", true)]
    static bool HideWireframeValidate()
    {
        return Selection.activeGameObject != null;
    }
}
