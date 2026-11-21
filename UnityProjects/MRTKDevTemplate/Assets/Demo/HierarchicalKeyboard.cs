using UnityEngine;
using TMPro;

public class HierarchicalKeyboard : MonoBehaviour
{
    public GazeDwellButton keyPrefab;
    public TMP_Text outputText;
    public float spacing = 0.12f;

    string[] groups = { "ABCDE", "FGHIJ", "KLMNO", "PQRST", "UVWXYZ" };

    GameObject groupRoot;
    GameObject letterRoot;
    GazeDwellButton[] letterKeys = new GazeDwellButton[6];

    void Start()
    {
        groupRoot = new GameObject("GroupLevel");
        groupRoot.transform.SetParent(transform, false);
        letterRoot = new GameObject("LetterLevel");
        letterRoot.transform.SetParent(transform, false);

        for (int i = 0; i < groups.Length; i++)
        {
            string group = groups[i];
            GazeDwellButton key = MakeKey(groupRoot.transform, i, 7);
            key.SetLabel(group);
            key.onSelected = () => ShowLetters(group);
        }

        GazeDwellButton space = MakeKey(groupRoot.transform, 5, 7);
        space.SetLabel("SPACE");
        space.onSelected = () => outputText.text += " ";

        GazeDwellButton del = MakeKey(groupRoot.transform, 6, 7);
        del.SetLabel("DEL");
        del.onSelected = DeleteLast;

        for (int i = 0; i < letterKeys.Length; i++)
        {
            letterKeys[i] = MakeKey(letterRoot.transform, i, 7);
        }

        GazeDwellButton back = MakeKey(letterRoot.transform, 6, 7);
        back.SetLabel("BACK");
        back.onSelected = ShowGroups;

        ShowGroups();
    }

    GazeDwellButton MakeKey(Transform parent, int index, int count)
    {
        GazeDwellButton key = Instantiate(keyPrefab, parent);
        float x = (index - (count - 1) / 2f) * spacing;
        key.transform.localPosition = new Vector3(x, 0f, 0f);
        key.transform.localRotation = Quaternion.identity;
        return key;
    }

    void ShowGroups()
    {
        letterRoot.SetActive(false);
        groupRoot.SetActive(true);
    }

    void ShowLetters(string group)
    {
        for (int i = 0; i < letterKeys.Length; i++)
        {
            if (i < group.Length)
            {
                char c = group[i];
                letterKeys[i].gameObject.SetActive(true);
                letterKeys[i].SetLabel(c.ToString());
                letterKeys[i].onSelected = () => TypeLetter(c);
            }
            else
            {
                letterKeys[i].gameObject.SetActive(false);
            }
        }

        groupRoot.SetActive(false);
        letterRoot.SetActive(true);
    }

    void TypeLetter(char c)
    {
        outputText.text += c;
        ShowGroups();
    }

    void DeleteLast()
    {
        string t = outputText.text;
        if (t.Length > 0) outputText.text = t.Substring(0, t.Length - 1);
    }
}
