using UnityEngine;

public class TransitionDemo : MonoBehaviour
{
    public float transitionDuration = 3f;

    [Header("Number")]
    public float number =  0.0f;
    public float toNumber = 1.0f;
    
    [Header("Vector")]
    public Vector3 vector = new Vector3();
    public Vector3 toVector = new Vector3(1f, 2f, 3f);
    
    [Header("Color")]
    public Color color = Color.red;
    public Color toColor = Color.blue;

    void Start()
    {
        StartCoroutine(UnityUtils.Transitions.LerpFloat(  () => number, (x) => number = x, toNumber, transitionDuration)  );
        StartCoroutine(UnityUtils.Transitions.LerpVector3(  () => vector, (x) => vector = x, toVector, transitionDuration)  );
        StartCoroutine(UnityUtils.Transitions.LerpColor(  () => color, (x) => color = x, toColor, transitionDuration)  );
    }

}
