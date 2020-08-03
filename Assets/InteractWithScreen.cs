using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class InteractWithScreen : MonoBehaviour
{
    float detectionRadius;
    public List<GameObject> candidates;
    public GameObject canvas;
    public RawImage screen;
    public GameObject resultCandidate;
    VideoPlayer videoPlayer;
    void Start()
    {
        candidates = new List<GameObject>();
        detectionRadius = GetComponent<SphereCollider>().radius;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.X))
        {
            if (candidates.Count <= 0) return;
            float minDistance = detectionRadius;
            resultCandidate = candidates[0];
            foreach (GameObject candidate in candidates)
            {
                float distance = Vector3.Distance(candidate.transform.position, transform.position);
                if (distance < minDistance)
                {
                    resultCandidate = candidate;
                }
            }

            videoPlayer = resultCandidate.GetComponent<VideoPlayer>();

            screen.texture = resultCandidate.GetComponent<VideoPlayer>().targetTexture;

            StartCoroutine(FadeScreen(true));

        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            resultCandidate = null;
            StartCoroutine(FadeScreen(false));
        }

    }

    private IEnumerator FadeScreen(bool fadeIn)
    {
        if (fadeIn)
        {
            while (screen.color.a < 0.95f)
            {
                screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, screen.color.a + 0.1f);
                yield return new WaitForSeconds(0.033f);
            }
            screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, 1f);
            yield return null;

        }
        else
        {
            while (screen.color.a > 0.05f)
            {
                screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, screen.color.a - 0.1f);
                yield return new WaitForSeconds(0.033f);
            }
            screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, 0f);
            screen.texture = null;
            yield return null;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject != other.gameObject)
        {
            candidates.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        candidates.Remove(other.gameObject);
    }
}
