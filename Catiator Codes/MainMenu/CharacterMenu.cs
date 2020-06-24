using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    public string name;
    public Color clr;
    private bool chosenColor = false;
    [SerializeField] private Text nameText;

    [SerializeField] private Transform uiParent;
    [SerializeField] private GameObject menuClassItem;

    private int index;
    [SerializeField] List<GameObject> characters = new List<GameObject>();
    private List<GameObject> menuItems;

    private GameObject current;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private Button last, next;
    [SerializeField] private GameObject btnReturn, btnConfirm, btnCancel;

    [Header("Circle")]
    [SerializeField] private Sprite ranger;
    [SerializeField] private Sprite berserker, mage, rogue;

    private bool chosen;
    public bool ChosenCharacter
    {
        get { return chosen; }
        set { chosen = value; }
    }


    void Start()
    {
        menuItems = new List<GameObject>();

        int i = 0;
        foreach (GameObject character in characters)
        {
            GameObject menuItem = Instantiate(menuClassItem, uiParent);
            PlayerUI uiPlayer = menuItem.GetComponent<PlayerUI>();
            if (uiPlayer != null) uiPlayer.SetPlayer(character.name, clr);
            menuItems.Add(menuItem);
        }

        last.onClick.AddListener(LastItem);
        next.onClick.AddListener(NextItem);

        index = 0;
        GoToCharacter();

        Button confirm = btnConfirm.GetComponent<Button>();
        confirm.onClick.AddListener(delegate { ChooseCharacter(); });

        Button cancel = btnCancel.GetComponent<Button>();
        cancel.onClick.AddListener(CancelCharacter);

        ChosenCharacter = false;
    }

    public void LastItem()
    {
        if (index != 0)
            index = index - 1;
        else
            index = menuItems.Count - 1;

        GoToCharacter();
    }

    public void NextItem()
    {
        if (index != characters.Count - 1)
            index = index + 1;
        else
            index = 0;

        GoToCharacter();
    }

    public void PhoneCharacter()
    {
        last.gameObject.SetActive(false);
        next.gameObject.SetActive(false);
    }

    private void GoToCharacter()
    {
        int nr = 0;
        foreach (GameObject menuItem in menuItems)
        {
            if (nr == index)
                menuItem.SetActive(true);
            else
                menuItem.SetActive(false);

            nr++;
        }

        if (current != null)
        {
            Destroy(current);
            current = null;
        }

        current = Instantiate(characters[index], spawnPoint.position, spawnPoint.rotation);
        current.transform.parent = this.transform;
        current.transform.localScale = new Vector3(100, 100, 100);

        BasicMovement character = current.GetComponent<BasicMovement>();
        character.SetBasics(name, clr);
    }

    public GameObject ChooseCharacter()
    {
        btnReturn.SetActive(false);
        btnConfirm.SetActive(false);
        btnCancel.SetActive(true);

        next.gameObject.SetActive(false);
        last.gameObject.SetActive(false);

        ChosenCharacter = true;
        return characters[index];
    }

    public GameObject ChooseCharacter(string charName)
    {
        int count = 0;
        foreach (GameObject character in characters)
        {
            if (character.name.Equals(charName))
            {
                index = count;
                GoToCharacter();

                btnReturn.SetActive(false);
                btnConfirm.SetActive(false);
                btnCancel.SetActive(true);

                return character;
            }
            count++;
        }

        next.gameObject.SetActive(false);
        last.gameObject.SetActive(false);

        ChosenCharacter = true;
        return null;
    }

    public void CancelCharacter()
    {
        btnReturn.SetActive(true);
        btnConfirm.SetActive(true);
        btnCancel.SetActive(false);

        next.gameObject.SetActive(true);
        last.gameObject.SetActive(true);

        ChosenCharacter = false;
    }

    public void SetName(string _name)
    {
        // name = _name;
    }

    public void SetColor(Color _clr)
    {
        clr = _clr;
        chosenColor = true;
    }

    public void Update()
    {
        if (chosenColor)
        {
            foreach (GameObject item in menuItems)
            {
                Transform circle = item.transform.Find("CentreCircle");
                if (circle != null)
                {
                    Transform image = circle.transform.Find("ColorCircle");

                    Image imgCircle = image.GetComponent<Image>();
                    imgCircle.color = clr;
                }
            }

            chosenColor = false;
        }
    }

    public List<Button> FindButtons()
    {
        List<Button> buttons = new List<Button>();

        buttons.Add(last);

        if (!ChosenCharacter)
        {
            Button buttonConfirm = btnConfirm.GetComponent<Button>();
            if (buttonConfirm != null) buttons.Add(buttonConfirm);

            buttons.Add(next);

            Button buttonReturn = btnReturn.GetComponent<Button>();
            if (buttonReturn != null) buttons.Add(buttonReturn);

        }
        else
        {
            buttons.Add(next);

            Button buttonCancel = btnCancel.GetComponent<Button>();
            if (buttonCancel != null) buttons.Add(buttonCancel);
        }

        return buttons;
    }
}
