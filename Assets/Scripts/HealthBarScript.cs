using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    private RectTransform subtractionBar;
    private CharacterDataController cdc;
    private Character character;
    private float current_goal;
    private Coroutine interpolater = null;
    private bool interpolater_is_running = false;
    private float framerate;

    public void Awake()
    {
        
    }

    void Start()
    {
        subtractionBar = GameObject.Find("Canvas/StatusBar/SubtractionBar").GetComponent<RectTransform>();
        cdc = GameObject.Find(Character.PLAYER).GetComponent<CharacterDataController>();
        character = cdc.character;
    }

    private void SetNewGoal(float goal)
    {
        current_goal = goal;
    }

    public void BeginHealthInterpolate(float goal)
    {
        current_goal = goal;
        if (!interpolater_is_running)
        {
            interpolater = StartCoroutine("InterpolateHealth");
            interpolater_is_running = true;
        }
        else
        {
            SetNewGoal(goal);
        }
            

    }

    public IEnumerator InterpolateHealth()
    {
        while (subtractionBar.localScale.x <= current_goal)
        {
            float goal_division = Mathf.MoveTowards(subtractionBar.localScale.x, current_goal, current_goal * Time.deltaTime);
            Debug.Log(Time.deltaTime + " " + Time.fixedDeltaTime);
            subtractionBar.localScale = new Vector3(goal_division, 1f, 1f);
            yield return new WaitForSeconds(0.01f);
        }
        interpolater_is_running = false;
    }

    void Update()
    {
        
    }
}
