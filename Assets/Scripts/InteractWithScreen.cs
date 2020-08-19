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
    public GameObject introduction;
    VideoPlayer videoPlayer;
    bool focusScreen = false;

    public VideoPlayer[] allVideoPlayer;
    void Start()
    {
        candidates = new List<GameObject>();
        detectionRadius = GetComponent<SphereCollider>().radius;
        // StartCoroutine(LoadVideos());
        StartCoroutine(HideIntroduction());
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) && !focusScreen)
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

            // video player
            videoPlayer = resultCandidate.GetComponentInChildren<VideoPlayer>();
            foreach (VideoPlayer player in allVideoPlayer)
            {
                if (videoPlayer != player)
                {
                    player.SetDirectAudioMute(0, true);
                }
                else
                {
                    player.SetDirectAudioVolume(0, player.GetDirectAudioVolume(0) + 0.5f);
                }
            }


            screen.texture = resultCandidate.GetComponentInChildren<VideoPlayer>().targetTexture;
            StartCoroutine(FadeScreen(true));
        }
        else if (Input.GetKeyUp(KeyCode.Space) && focusScreen)
        {
            // video player
            foreach (VideoPlayer player in allVideoPlayer)
            {
                if (videoPlayer != player)
                {
                    player.SetDirectAudioMute(0, false);
                }
                else
                {
                    player.SetDirectAudioVolume(0, player.GetDirectAudioVolume(0) - 0.5f);
                }

            }
            resultCandidate = null;
            StartCoroutine(FadeScreen(false));
        }
    }

    private IEnumerator LoadVideos()
    {
        bool ready = false;
        bool[] allLoaded = new bool[8];
        for (int i = 0; i < 8; i++)
        {
            allLoaded[i] = false;
        }
        while (!ready)
        {
            for (int i = 0; i < 8; i++)
            {
                VideoPlayer player = allVideoPlayer[i];
                if (player.isPrepared)
                {
                    allLoaded[i] = true;
                    ready = true;
                    Debug.Log("video" + (i + 1).ToString() + " done");
                }

            }
            for (int i = 0; i < 8; i++)
            {
                if (allLoaded[i] == false)
                {
                    ready = false;
                }
            }
            Debug.Log("LOADING");
            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log("DONE");
        yield return null;
    }
    private IEnumerator HideIntroduction()
    {
        yield return new WaitForSeconds(10f);

        while (introduction.activeSelf)
        {
            foreach (Text text in introduction.GetComponentsInChildren<Text>())
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - 0.01f);
                if (text.color.a < 0.05f)
                {
                    introduction.SetActive(false);
                }
            }
            yield return new WaitForSeconds(0.033f);
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
            focusScreen = true;
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
            focusScreen = false;
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
