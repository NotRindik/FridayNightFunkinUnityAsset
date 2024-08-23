using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicLayerTesting : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(Running());
    }

    IEnumerator Running()
    {
        GraphicPanel panel = GraphicPanelManager.instance.GetPanel("Background");
        GraphicLayer layer0 = panel.GetLayer(0, true);
        GraphicLayer layer1 = panel.GetLayer(1, true);

        layer0.SetVideo("Graphics/BG Videos/Nebula");
        layer1.SetTexture("Graphics/BG Images/Spaceshipinterior");

        yield return null;
    }
}
