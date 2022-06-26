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
    public VideoPlayer player;
    public List<buttonIcons> panelButtons;
    public GameObject playButton;
    public buttonIcons videoBackButton;
    

    bool showingMainPanel = false;
    bool showingVideoPanel = false;
    bool showingImagesPanel = false;
    private Vector2 playerSmallSize;

    private void Awake()
    {
        //Vector3 p = Vector3.right * mainPanel.rect.width;
        mainPanel.position += Vector3.right * mainPanel.rect.width;
        videoPanel.position += Vector3.down * videoPanel.rect.height;
        imagesPanel.position += Vector3.down * imagesPanel.rect.height;
        playerSmallSize = player.GetComponent<RectTransform>().sizeDelta;
    }

    public void SetMainPanel(bool open)
    {
        if (showingMainPanel == open) return;
        showingMainPanel = open;
        mainPanel.gameObject.SetActive(true);
        float x = open ? 0 : mainPanel.rect.width;
        mainPanel.DOMoveX(Screen.width + x, 1).OnComplete(()=> mainPanel.gameObject.SetActive(open));
    }

    public void SetVideoPanel(bool open)
    {
        if (showingVideoPanel == open) return;
        showingVideoPanel = open;
        videoPanel.gameObject.SetActive(true);
        float y = open ? videoPanel.rect.height * 0.5f : -videoPanel.rect.height * 1.5f;
        videoPanel.DOMoveY(y, 1).OnComplete(() => videoPanel.gameObject.SetActive(open));
    }

    public void SetImagesPanel(bool open)
    {
        if (showingImagesPanel == open) return;
        showingImagesPanel = open;
        imagesPanel.gameObject.SetActive(true);
        float y = open ? imagesPanel.rect.height * 0.5f : -imagesPanel.rect.height * 1.5f;
        imagesPanel.DOMoveY(y, 1).OnComplete(() => imagesPanel.gameObject.SetActive(open));
    }

    public void GoToPanel(string panel)
    {
        SetMainPanel(panel.Equals("Main"));
        SetVideoPanel(panel.Equals("Video"));
        SetImagesPanel(panel.Equals("Images"));
        
        foreach (buttonIcons bi in panelButtons)
        {
            bi.im.sprite = 
                bi.im.name.Equals(panel) ? bi.on : bi.off;
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

}
