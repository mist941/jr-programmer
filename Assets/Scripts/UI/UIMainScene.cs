using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainScene : MonoBehaviour
{
    public static UIMainScene Instance { get; private set; }
    
    public interface IUIInfoContent
    {
        string GetName();
        string GetData();
        void GetContent(ref List<Building.InventoryEntry> content);
    }
    
    public InfoPopup InfoPopup;
    public ResourceDatabase ResourceDB;

    protected IUIInfoContent m_CurrentContent;
    protected List<Building.InventoryEntry> m_ContentBuffer = new List<Building.InventoryEntry>();


    private void Awake()
    {
        Instance = this;
        InfoPopup.gameObject.SetActive(false);
        ResourceDB.Init();
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    private void Update()
    {
        if (m_CurrentContent == null)
            return;

        InfoPopup.Data.text = m_CurrentContent.GetData();
        
        InfoPopup.ClearContent();
        m_ContentBuffer.Clear();
        
        m_CurrentContent.GetContent(ref m_ContentBuffer);
        foreach (var entry in m_ContentBuffer)
        {
            Sprite icon = null;
            if (ResourceDB != null)
                icon = ResourceDB.GetItem(entry.ResourceId)?.Icone;
            
            InfoPopup.AddToContent(entry.Count, icon);
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void SetNewInfoContent(IUIInfoContent content)
    {
        if (content == null)
        {
            InfoPopup.gameObject.SetActive(false);
        }
        else
        {
            InfoPopup.gameObject.SetActive(true);
            m_CurrentContent = content;
            InfoPopup.Name.text = content.GetName();
        }
    }
}
