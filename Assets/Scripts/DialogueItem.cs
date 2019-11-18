using UnityEngine;

public class DialogueItem
{
    string m_text;
    string m_talker_name;
    Sprite m_talker_image;

    public DialogueItem(string text, string talker_name, Sprite talker_image)
    {
        m_text = text;
        m_talker_name = talker_name;
        m_talker_image = talker_image;
    }

    public string GetTalkerName()
    {
        return m_talker_name;
    }

    public Sprite GetTalkerImage()
    {
        return m_talker_image;
    }

    public string NextTextSegment()
    {
        int cut_num = Mathf.Min(119, m_text.Length);
        string outstring = m_text.Substring(0, cut_num);
        m_text = m_text.Remove(0, cut_num);
        return outstring;
    }

    public bool HasMoreText()
    {
        return m_text.Length > 0;
    }

}
