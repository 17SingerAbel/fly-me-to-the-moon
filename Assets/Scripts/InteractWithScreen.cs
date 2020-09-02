using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class InteractWithScreen : MonoBehaviour
{
    public XRNode inputSource;
    public float distance = 5f;
    float detectionRadius;
    public List<GameObject> candidates;
    public GameObject screen;
    public GameObject resultCandidate;
    public GameObject introduction;
    VideoPlayer videoPlayer;
    bool focusScreen = false;

    float timer = 0.0f;

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
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        if (device != null)
        {
            device.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
            if (triggerValue > 0.5 && timer == 0.0f && !focusScreen)
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
                screen.transform.position = transform.forward * distance + transform.position;
                screen.transform.position = new Vector3(screen.transform.position.x, 3.1f, screen.transform.position.z);
                Renderer renderer = screen.GetComponent<Renderer>();
                renderer.materials = resultCandidate.GetComponentInChildren<VideoPlayer>().transform.GetComponent<Renderer>().materials;

                screen.SetActive(true);

                focusScreen = true;
            }
            else if (triggerValue > 0.5 && timer == 0.0f && focusScreen)
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
                screen.SetActive(false);
                focusScreen = false;

            }
            if (triggerValue > 0.5)
            {
                timer += Time.deltaTime;
            }
            else if (triggerValue < 0.1 && timer > 0.01)
            {
                timer = 0;
            }
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
