using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player player;
    public GameObject finish;
    public Text game_over;
    public Text game_won;
    public Canvas health_root;
    public Image health_image;
    public float finish_dist_thresh = 1;

    public List<GameObject> curr_health;
    private int active_healthbars;

    private bool finished = false;

    private void InitializeHealthbar()
    {
        curr_health = new List<GameObject>();
        float width = health_image.gameObject.GetComponent<RectTransform>().sizeDelta.x;
        RectTransform parent = health_root.GetComponent<RectTransform>();
        for (int i = 0; i < player.health; ++i)
        {
            GameObject health = Instantiate(health_image.gameObject);
            RectTransform rt = health.GetComponent<RectTransform>();
            rt.SetParent(parent);
            rt.position = new Vector3(width / 2 + width * i, parent.position.y, 0);
            curr_health.Add(health);
        }

        active_healthbars = curr_health.Count;
    }

    private void UpdateHealthbars()
    {
        if(active_healthbars != player.health)
        {
            int dif = Mathf.Abs(active_healthbars - Mathf.Clamp(player.health, 0, curr_health.Count));
            if (active_healthbars < player.health)
            {
                for(int i = 0; i < dif; ++i)
                {
                    curr_health[i + active_healthbars].SetActive(true);
                }
            }
            else
            {
                for(int i = 0; i < dif; ++i)
                {
                    curr_health[active_healthbars - dif].SetActive(false);
                }
            }

            active_healthbars = Mathf.Clamp(player.health, 0, curr_health.Count);
        }
    }

    void Start()
    {
        InitializeHealthbar();
    }

    void Update()
    {
        if (finished)
        {
            return;
        }

        UpdateHealthbars();
        if(player.health <= 0)
        {
            game_over.gameObject.SetActive(true);
        }

        if((player.gameObject.transform.position - finish.transform.position).magnitude < finish_dist_thresh)
        {
            game_won.gameObject.SetActive(true);
        }
    }
}
