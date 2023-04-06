using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    GameObject _gameController;
    BlockController _blockController;
    InputReader _inputReader;

    private void Awake()
    {
        _gameController = GameObject.FindGameObjectWithTag("GameController");

        _blockController = _gameController.GetComponent<BlockController>();

        _inputReader = _gameController.GetComponent<InputReader>();
    }

    public IEnumerator MoveDown()
    {
        while (true)
        {
            yield return new WaitForSeconds(_blockController.WaitToDown);

            if (!CheckVerMovable()) //sinirlari kontrol et
            {
                foreach (Transform child in transform)
                {
                    _blockController.cells[(int)child.position.x, (int)child.position.y] = child.gameObject;
                }

                _blockController.SpawnBlock();
                break;
            }

            Vector2 position = GetPosition();
            position.y--;
            SetPosition(position);
        }
    }
    public void HorizontalMove()
    {
        int horizontal = _inputReader.ReadHorizontalValue();

        if (!CheckHorMovable(horizontal)) return; //sinirlari kontrol et

        Vector2 position = GetPosition();
        position.x += horizontal;
        SetPosition(position);
    }

    private Vector2 GetPosition()
    {
        return gameObject.transform.position;
    }
    private void SetPosition(Vector2 position)
    {
        gameObject.transform.position = position;
    }

    public bool CheckHorMovable(int hozirontal)
    {
        foreach (Transform child in transform)
        {
            Vector2 position = child.position;
            position.x += hozirontal;

            if (position.x < 0 || position.x >= BlockController.BOUNDARY_X)
            {
                return false;
            }// X boundary check

            if (_blockController.cells[(int)position.x, (int)position.y] != null)
            {
                return false;
            }


        }
        return true;
    }
    public bool CheckVerMovable()
    {
        foreach (Transform child in transform)
        {
            Vector2 position = child.position;
            position.y--;

            if (position.y < 0)
            {
                return false;
            }// Y bottom boundary check

            if (_blockController.cells[(int)position.x, (int)position.y] != null)
            {
                return false;
            }
        }
        return true;
    }
}
