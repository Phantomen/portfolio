using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro gunText;

    List<GameObject> listOfTargets = new List<GameObject>();
    public List<GameObject> movementLevels;

    [SerializeField]
    private GameObject[] doors = new GameObject[2];

    [SerializeField]
    private TextMeshPro[] levelText = new TextMeshPro[2];

    public int currentLevel = 0;
    //public int maxLevel = 0;
    private int maxLevelReached = 0;

    int currentTargetsShot = 0;

    [SerializeField]
    TeleportBack teleportPoint;

    private bool forcedTeleport = false;

    // Start is called before the first frame update
    void Start()
    {
        //Disable all movement levels and activate the one one that has been set to start with
        for(int i = 0; i < movementLevels.Count; i++)
        {
            movementLevels[i].SetActive(false);
        }

        if (movementLevels[currentLevel] != null)
        { movementLevels[currentLevel].SetActive(true); }
        maxLevelReached = currentLevel;

        gunText.text = 0 + "/" + listOfTargets.Count;

        for (int i = 0; i < levelText.Length; i++)
        {
            levelText[i].text = "Current Level:" + "\n" + (currentLevel + 1) + "/" + movementLevels.Count;
        }
    }

    //The red targets call this when they awake so they add them selves to this list
    public void AddTarget(GameObject target)
    {
        listOfTargets.Add(target);
    }

    //When a red target has been shot they call this
    public void targetShot()
    {
        currentTargetsShot++;
        if(currentTargetsShot == listOfTargets.Count)
        {
            if(currentLevel == maxLevelReached)
            {
                maxLevelReached++;
            }
            OpenDoor();
            forcedTeleport = true;
        }

        gunText.text = currentTargetsShot + "/" + listOfTargets.Count;
    }

    void OpenDoor()
    {
        doors[0].SetActive(false);
        doors[1].SetActive(false);
    }

    void CloseDoor()
    {
        doors[0].SetActive(true);
        doors[1].SetActive(true);
    }

    public void NextLevel()
    {
        currentLevel++;
        for(int i = 0; i < listOfTargets.Count; i++)
        {
            listOfTargets[i].SetActive(true);
        }

        currentTargetsShot = 0;

        if(currentLevel == maxLevelReached && currentLevel < movementLevels.Count)
        {
            CloseDoor();
        }
        //If player was on last level
        else if(currentLevel == movementLevels.Count)
        {
            currentLevel--;

            if (movementLevels[currentLevel - 1] != null)
            { movementLevels[currentLevel - 1].SetActive(false); }
            if (movementLevels[currentLevel] != null)
            { movementLevels[currentLevel].SetActive(true); }

            gunText.text = 0 + "/" + listOfTargets.Count;

            for (int i = 0; i < levelText.Length; i++)
            {
                levelText[i].text = "CONGRATULATIONS!!!";
            }

            return;
        }

        //Disable the previues movement system and enable the new one
        if (movementLevels[currentLevel - 1] != null)
        { movementLevels[currentLevel-1].SetActive(false); }
        if (movementLevels[currentLevel] != null)
        { movementLevels[currentLevel].SetActive(true); }

        gunText.text = 0 + "/" + listOfTargets.Count;

        for (int i = 0; i < levelText.Length; i++)
        {
            levelText[i].text = "Current Level:" + "\n" + (currentLevel + 1) + "/" + movementLevels.Count;
        }

        forcedTeleport = true;
    }

    public void PreviousLevel()
    {
        currentLevel--;
        for (int i = 0; i < listOfTargets.Count; i++)
        {
            listOfTargets[i].SetActive(true);
        }

        currentTargetsShot = 0;

        if(currentLevel < 0)
        {
            currentLevel = 0;
            gunText.text = 0 + "/" + listOfTargets.Count;
            for (int i = 0; i < levelText.Length; i++)
            {
                levelText[i].text = "Current Level:" + "\n" + (currentLevel + 1) + "/" + movementLevels.Count;
            }

            forcedTeleport = true;
            return;
        }

        if (movementLevels[currentLevel + 1] != null)
        { movementLevels[currentLevel + 1].SetActive(false); }
        if (movementLevels[currentLevel] != null)
        { movementLevels[currentLevel].SetActive(true); }

        OpenDoor();

        gunText.text = 0 + "/" + listOfTargets.Count;

        for (int i = 0; i < levelText.Length; i++)
        {
            levelText[i].text = "Current Level:" + "\n" + (currentLevel + 1) +  "/" + movementLevels.Count;
        }

        forcedTeleport = true;
    }
}
