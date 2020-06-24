using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonNavigation : MonoBehaviour
{
    [SerializeField] private Button btnConfirm, btnBack;

    [SerializeField] private MenuPlayer playerOne, playerTwo, playerThree, playerFour;
    private List<Button> buttonsPlayerOne, buttonsPlayerTwo, buttonsPlayerThree, buttonsPlayerFour;

    private List<Button> buttonsPlayerOneLeft, buttonsPlayerOneRight;
    private List<Button> buttonsPlayerTwoLeft, buttonsPlayerTwoRight;
    private List<Button> buttonsPlayerThreeLeft, buttonsPlayerThreeRight;
    private List<Button> buttonsPlayerFourLeft, buttonsPlayerFourRight;

    public void UpdateNavigation(bool start, MenuPlayer player)
    {
        FindButtons();

        if(buttonsPlayerOne != null)
        {
            buttonsPlayerOneRight = buttonsPlayerOne;
            buttonsPlayerOneLeft = buttonsPlayerOne;
        }

        if(buttonsPlayerTwo != null)
        {
            buttonsPlayerTwoRight = buttonsPlayerTwo;
            buttonsPlayerTwoLeft = buttonsPlayerTwo;
        }

        if(buttonsPlayerThree != null)
        {
            buttonsPlayerThreeRight = buttonsPlayerThree;
            buttonsPlayerThreeLeft = buttonsPlayerThree;
        }

        if(buttonsPlayerFour != null)
        {
            buttonsPlayerFourRight = buttonsPlayerFour;
            buttonsPlayerFourLeft = buttonsPlayerFour;
        }

        if(buttonsPlayerOne != null)
            UpdateButtons(buttonsPlayerOne, null, buttonsPlayerTwoRight, btnConfirm);
        else
        {
            UpdateButtons(buttonsPlayerOneLeft, buttonsPlayerOneRight, buttonsPlayerTwoRight, btnConfirm);
            UpdateButtons(buttonsPlayerOneRight, null, buttonsPlayerOneLeft, btnConfirm);
        }

        if(buttonsPlayerTwo != null)
            UpdateButtons(buttonsPlayerTwo, buttonsPlayerOneLeft, buttonsPlayerThreeRight, btnConfirm);
        else
        {
            UpdateButtons(buttonsPlayerTwoLeft, buttonsPlayerTwoRight, buttonsPlayerThreeRight, btnConfirm);
            UpdateButtons(buttonsPlayerTwoRight, buttonsPlayerOneLeft, buttonsPlayerTwoLeft, btnConfirm);
        }

        if(buttonsPlayerThree != null)
            UpdateButtons(buttonsPlayerThree, buttonsPlayerTwoLeft, buttonsPlayerFourRight, btnBack);
        else
        {
            UpdateButtons(buttonsPlayerThreeLeft, buttonsPlayerThreeRight, buttonsPlayerFourRight, btnBack);
            UpdateButtons(buttonsPlayerThreeRight, buttonsPlayerTwoLeft, buttonsPlayerThreeLeft, btnBack);
        }

        if(buttonsPlayerFour != null)
            UpdateButtons(buttonsPlayerFour, buttonsPlayerThreeLeft, null, btnBack);
        else
        {
            UpdateButtons(buttonsPlayerFourLeft, buttonsPlayerFourRight, null, btnBack);
            UpdateButtons(buttonsPlayerFourRight, buttonsPlayerThreeLeft, buttonsPlayerFourLeft, btnBack);
        }

        
        Navigation navigation = btnConfirm.navigation;
        navigation.mode = Navigation.Mode.Explicit;

        if(buttonsPlayerOne != null)
            navigation.selectOnLeft = buttonsPlayerOne[buttonsPlayerOne.Count - 1];
        else
        {
            if(buttonsPlayerOneRight.Count > 3)
                navigation.selectOnLeft = buttonsPlayerOneLeft[buttonsPlayerOneLeft.Count - 1];
            else
                navigation.selectOnLeft = buttonsPlayerOneRight[buttonsPlayerOneRight.Count - 1];
        }

        if (buttonsPlayerTwo != null)
            navigation.selectOnUp = buttonsPlayerTwo[buttonsPlayerTwo.Count - 1];
        else
        {
            if (buttonsPlayerTwoRight.Count > 3)
                navigation.selectOnUp = buttonsPlayerTwoLeft[buttonsPlayerTwoLeft.Count - 1];
            else
                navigation.selectOnUp = buttonsPlayerTwoRight[buttonsPlayerTwoRight.Count - 1];
        }

        navigation.selectOnRight = btnBack;
        btnConfirm.navigation = navigation;

        navigation = btnBack.navigation;
        navigation.mode = Navigation.Mode.Explicit;
        navigation.selectOnLeft = btnConfirm;

        if (buttonsPlayerThree != null)
            navigation.selectOnUp = buttonsPlayerThree[buttonsPlayerThree.Count - 1];
        else
        {
            if (buttonsPlayerThreeRight.Count > 3)
                navigation.selectOnLeft = buttonsPlayerThreeLeft[buttonsPlayerThreeLeft.Count - 1];
            else
                navigation.selectOnLeft = buttonsPlayerThreeRight[buttonsPlayerThreeRight.Count - 1];
        }

        if (buttonsPlayerFour != null)
            navigation.selectOnRight = buttonsPlayerFour[buttonsPlayerFour.Count - 1];
        else
        {
            if (buttonsPlayerFourRight.Count > 3)
                navigation.selectOnLeft = buttonsPlayerFourLeft[buttonsPlayerFourLeft.Count - 1];
            else
                navigation.selectOnLeft = buttonsPlayerFourRight[buttonsPlayerFourRight.Count - 1];
        }

        btnBack.navigation = navigation;

        Button selectButton = null;

        if (start) selectButton = buttonsPlayerOne[0];
        else if (player == playerOne)
        {
            if (buttonsPlayerOne != null)
                selectButton = buttonsPlayerOne[0];
            else if (buttonsPlayerOneRight.Count > 1)
                selectButton = buttonsPlayerOneRight[buttonsPlayerOneRight.Count - 1];
            else
                selectButton = buttonsPlayerOneLeft[buttonsPlayerOneLeft.Count - 1];
        }
        else if (player == playerTwo)
        {
            if (buttonsPlayerTwo != null)
                selectButton = buttonsPlayerTwo[0];
            else if (buttonsPlayerTwoRight.Count > 1)
                selectButton = buttonsPlayerTwoRight[buttonsPlayerTwoRight.Count - 1];
            else
                selectButton = buttonsPlayerTwoLeft[buttonsPlayerTwoLeft.Count - 1];
        }
        else if (player == playerThree)
        {
            if (buttonsPlayerThree != null)
                selectButton = buttonsPlayerThree[0];
            else if (buttonsPlayerThreeRight.Count > 1)
                selectButton = buttonsPlayerThreeRight[buttonsPlayerThreeRight.Count - 1];
            else
                selectButton = buttonsPlayerThreeLeft[buttonsPlayerThreeLeft.Count - 1];
        }
        else if (player == playerFour)
        {
            if (buttonsPlayerFour != null)
                selectButton = buttonsPlayerFour[0];
            else if (buttonsPlayerFourRight.Count > 1)
                selectButton = buttonsPlayerFourRight[buttonsPlayerFourRight.Count - 1];
            else
                selectButton = buttonsPlayerFourLeft[buttonsPlayerFourLeft.Count - 1];
        }

        if (selectButton != null) selectButton.Select();
    }

    private void FindButtons()
    {
        buttonsPlayerOne = playerOne.FindState();
        buttonsPlayerTwo = playerTwo.FindState();
        buttonsPlayerThree = playerThree.FindState();
        buttonsPlayerFour = playerFour.FindState();

        if (buttonsPlayerOne == null)
        {
            CharacterMenu menuCharacter = playerOne.FindCharacterMenu();
            List<Button> buttons = menuCharacter.FindButtons();

            buttonsPlayerOneLeft = new List<Button>();
            buttonsPlayerOneRight = new List<Button>();

            int nr = 0;
            foreach (Button btn in buttons)
            {
                if (nr == 1)
                {
                    if (buttons.Count == 3)
                        buttonsPlayerOneLeft.Add(btn);
                    else
                        buttonsPlayerOneRight.Add(btn);
                }
                else if (nr < 1)
                    buttonsPlayerOneRight.Add(btn);
                else
                    buttonsPlayerOneLeft.Add(btn);

                nr++;
            }
        }

        if (buttonsPlayerTwo == null)
        {
            CharacterMenu menuCharacter = playerTwo.FindCharacterMenu();
            List<Button> buttons = menuCharacter.FindButtons();

            buttonsPlayerTwoLeft = new List<Button>();
            buttonsPlayerTwoRight = new List<Button>();

            int nr = 0;
            foreach (Button btn in buttons)
            {
                if (nr == 1)
                {
                    if (buttons.Count == 3)
                        buttonsPlayerTwoLeft.Add(btn);
                    else
                        buttonsPlayerTwoRight.Add(btn);
                }
                else if (nr < 1)
                    buttonsPlayerTwoRight.Add(btn);
                else
                    buttonsPlayerTwoLeft.Add(btn);

                nr++;
            }
        }

        if (buttonsPlayerThree == null)
        {
            CharacterMenu menuCharacter = playerThree.FindCharacterMenu();
            List<Button> buttons = menuCharacter.FindButtons();

            buttonsPlayerThreeLeft = new List<Button>();
            buttonsPlayerThreeRight = new List<Button>();

            int nr = 0;
            foreach (Button btn in buttons)
            {
                if (nr == 1)
                {
                    if (buttons.Count == 3)
                        buttonsPlayerThreeLeft.Add(btn);
                    else
                        buttonsPlayerThreeRight.Add(btn);
                }
                else if (nr < 1)
                    buttonsPlayerThreeRight.Add(btn);
                else
                    buttonsPlayerThreeLeft.Add(btn);

                nr++;
            }
        }

        if (buttonsPlayerFour == null)
        {
            CharacterMenu menuCharacter = playerFour.FindCharacterMenu();
            List<Button> buttons = menuCharacter.FindButtons();

            buttonsPlayerFourLeft = new List<Button>();
            buttonsPlayerFourRight = new List<Button>();

            int nr = 0;
            foreach (Button btn in buttons)
            {
                if (nr == 1)
                {
                    if (buttons.Count == 3)
                        buttonsPlayerFourLeft.Add(btn);
                    else
                        buttonsPlayerFourRight.Add(btn);
                }
                else if (nr < 1)
                    buttonsPlayerFourRight.Add(btn);
                else
                    buttonsPlayerFourLeft.Add(btn);

                nr++;
            }
        }
    }

    private void UpdateButtons(List<Button> updateList, List<Button> leftButtons, List<Button> rightButtons, Button btnDown)
    {
        if (updateList == null) return;

        int count = 0;
        foreach (Button btn in updateList)
        {
            // get the Navigation data
            Navigation navigation = btn.navigation;
            navigation.mode = Navigation.Mode.Explicit;

            // Set the buttons
            if (count != 0)
                navigation.selectOnUp = updateList[count - 1];

            if (count == (updateList.Count - 1))
                navigation.selectOnDown = btnDown;
            else
                navigation.selectOnDown = updateList[count + 1];

            if(leftButtons != null && leftButtons.Count > 0)
            {
                Button left = null;
                int minLeft = leftButtons.Count - updateList.Count;
                if(minLeft == 0)
                {
                    left = leftButtons[count];
                }
                else if (minLeft < 0)
                {
                    if (count < (minLeft * -1))
                        left = leftButtons[0];
                    else
                    {
                        int nr = count + minLeft;
                        left = leftButtons[nr];
                    }
                }
                else
                {
                    int nr = count + minLeft;
                    left = leftButtons[nr];
                }
                navigation.selectOnLeft = left;
            }

            if(rightButtons != null)
            {
                Button right = null;
                int minRight = rightButtons.Count - updateList.Count;

                if(minRight >= 0)
                {
                    right = rightButtons[count];
                }
                else if (minRight < 0)
                {
                    if (count < (minRight * -1))
                        right = rightButtons[0];
                    else
                    {
                        int nr = count + minRight;
                        right = rightButtons[nr];
                    }
                }
                else
                {
                    int nr = count + minRight;
                    right = rightButtons[nr];
                }

                navigation.selectOnRight = right;
            }

            // reassign the struct data to the button
            btn.navigation = navigation;

            count++;
        }
    }
}
