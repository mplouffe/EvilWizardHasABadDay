using lvl_0;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    private bool m_startClicked = false;

    public void OnStartClicked()
    {
        if (!m_startClicked)
        {
            m_startClicked = true;
            LevelAttendant.Instance.LoadGameState(GameState.GameStart);
        }
        m_startClicked = true;
    }
}
