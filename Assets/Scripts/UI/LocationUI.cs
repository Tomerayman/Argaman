using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;
using UnityEngine.Video;

public class LocationUI : MonoBehaviour
{
    public RectTransform mainPanel;
    public RectTransform videoPanel;
    public RectTransform imagesPanel;
    public RectTransform storiesPanel;
    public VideoPlayer player;
    public List<buttonIcons> panelButtons;
    public GameObject playButton;
    public buttonIcons videoBackButton;
    public RectTransform storyElementsParent;
    public Button prevStoryButton;
    public Button nextStoryButton;
    private Dictionary<string, RectTransform> panels;
    
    private Vector2 playerSmallSize;
    private List<RectTransform> _stories;
    private int _currStory = 0;


    private void Awake()
    {
        playerSmallSize = player.GetComponent<RectTransform>().sizeDelta;
        panels = new Dictionary<string, RectTransform>()
        {
            {"Main", mainPanel},
            {"Video", videoPanel},
            {"Images", imagesPanel},
            {"Stories", storiesPanel},
        };
        foreach (var panel in panels)
        {
            if (panel.Value == null) continue;
            panel.Value.position += panel.Key.Equals("Main") ?
                Vector3.right * panel.Value.rect.width :
                Vector3.down * Screen.width;
            panel.Value.gameObject.SetActive(false);
        }

        // stories Init;
        if (storyElementsParent == null) return;
        _stories = new List<RectTransform>();
        foreach (RectTransform story in storyElementsParent)
        {
            _stories.Add(story);
            if (_stories.Count > 1) story.position += Vector3.right * Screen.width;
        }
        prevStoryButton.interactable = false;
    }

    public void SetMainPanel(bool open)
    {
        if (mainPanel.gameObject.activeSelf == open) return;
        mainPanel.gameObject.SetActive(true);
        float x = open ? 0 : mainPanel.rect.width;
        mainPanel.DOMoveX(Screen.width + x, 1).OnComplete(()=> mainPanel.gameObject.SetActive(open));
    }


    public void SetContentPanel(RectTransform panel, bool open)
    {
        if (panel.gameObject.activeSelf == open) return;
        panel.gameObject.SetActive(true);
        float y = open ? Screen.height * 0.5f : -Screen.height * 1.5f;
        panel.DOMoveY(y, 1).OnComplete(() => panel.gameObject.SetActive(open));
    }

    public void GoToPanel(string nextPanel)
    {
        foreach (var kv in panels)
        {
            if (kv.Key.Equals("Main")) SetMainPanel(nextPanel.Equals("Main"));
            else if (kv.Value != null) SetContentPanel(kv.Value, nextPanel.Equals(kv.Key));
        }
        
        foreach (buttonIcons bi in panelButtons)
        {
            bi.im.sprite = 
                bi.im.name.Equals(nextPanel) ? bi.on : bi.off;
        }
    }

    public void StartVideo()
    {
        player.Play();
        player.GetComponent<RectTransform>()
            .DOSizeDelta(new Vector2(Screen.width, Screen.height), 1);
        playButton.SetActive(false);

        videoBackButton.im.sprite = videoBackButton.on;
        Button b = videoBackButton.im.GetComponent<Button>();
        b.onClick.RemoveAllListeners();
        b.onClick.AddListener(ClosePlayer);
    }

    public void ClosePlayer()
    {
        player.Stop();
        player.GetComponent<RectTransform>()
            .DOSizeDelta(playerSmallSize, 1);
        playButton.SetActive(true);

        videoBackButton.im.sprite = videoBackButton.off;
        Button b = videoBackButton.im.GetComponent<Button>();
        b.onClick.RemoveAllListeners();
        b.onClick.AddListener(() => GoToPanel("Main"));
    }


    [Serializable]
    public class buttonIcons
    {
        public Image im;
        public Sprite on;
        public Sprite off;
    }

    public void PlayPuaseVideo()
    {
        if (player.isPlaying) player.Pause();
        else player.Play();
    }

    public void MoveStory(bool next)
    {
        if (next && _currStory == _stories.Count - 1) return;
        else if (!next && _currStory == 0) return;
        int moveFactor = next ? 1 : -1;
        float movement = Screen.width * moveFactor;
        _stories[_currStory].DOMoveX(_stories[_currStory].position.x - movement, 1);
        _currStory += moveFactor;
        _stories[_currStory].DOMoveX(_stories[_currStory].position.x - movement, 1);
        prevStoryButton.interactable = (_currStory > 0);
        nextStoryButton.interactable = (_currStory < _stories.Count - 1);
    }

    public void ResetVideo()
    {
        player.time = 0;
    }

}
