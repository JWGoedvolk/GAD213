using UnityEngine;

public class ApplicationCloser : MonoBehaviour
{
    [SerializeField] KeyCode closeKey = KeyCode.Escape;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Cancel") > 0 || Input.GetKeyDown(closeKey))
        {
            Application.Quit();
        }
    }

    public void Close()
    {
        Application.Quit();
    }
}
