using UnityEngine.UI;

public class RaycastImage : Image
{
    protected override void OnPopulateMesh(VertexHelper toFill) {
        toFill.Clear();
    }
}