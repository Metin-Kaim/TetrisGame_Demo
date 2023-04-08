using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Block : MonoBehaviour
{
    GameObject _gameController;
    BlockController _blockController;
    InputReader _inputReader;

    [SerializeField] Vector3 birVector;
    [SerializeField] Vector3 ikiVector;
    [SerializeField] Vector3 ucVector;
    [SerializeField] Vector3 dortVector;
    private void Awake()
    {
        _gameController = GameObject.FindGameObjectWithTag("GameController");

        _blockController = _gameController.GetComponent<BlockController>();

        _inputReader = _gameController.GetComponent<InputReader>();
    }

    private void Update()
    {

        birVector.x = Mathf.RoundToInt(transform.GetChild(0).transform.position.x);
        ikiVector.x = Mathf.RoundToInt(transform.GetChild(1).transform.position.x);
        ucVector.x = Mathf.RoundToInt(transform.GetChild(2).transform.position.x);
        dortVector.x = Mathf.RoundToInt(transform.GetChild(3).transform.position.x);

        birVector.y = Mathf.RoundToInt(transform.GetChild(0).transform.position.y);
        ikiVector.y = Mathf.RoundToInt(transform.GetChild(1).transform.position.y);
        ucVector.y = Mathf.RoundToInt(transform.GetChild(2).transform.position.y);
        dortVector.y = Mathf.RoundToInt(transform.GetChild(3).transform.position.y);
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
                    _blockController.cells[Mathf.RoundToInt(child.position.x), Mathf.RoundToInt(child.position.y)] = child.gameObject;
                }
                _blockController.SpawnBlock();
                break;
            }

            Vector2 position = GetPosition();
            position.y = Mathf.RoundToInt(--position.y);
            SetPosition(position);
        }
    }
    public void HorizontalMove()
    {
        int horizontal = _inputReader.ReadHorizontalValue();
        horizontal = Mathf.RoundToInt(horizontal);
        if (!CheckHorMovable(horizontal)) return; //sinirlari kontrol et

        int x = Mathf.RoundToInt(transform.position.x);
        int y = Mathf.RoundToInt(transform.position.y);
        x += horizontal;

        Vector2 position = new Vector2(x, y);
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
            int x = Mathf.RoundToInt(child.position.x);
            int y = Mathf.RoundToInt(child.position.y);
            x += hozirontal;
            if (x < 0 || x >= BlockController.BOUNDARY_X)
            {
                return false;
            }// X boundary check

            if (_blockController.cells[x, y] != null)
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
            int x = Mathf.RoundToInt(child.position.x);
            int y = Mathf.RoundToInt(child.position.y);

            y--;

            if (y < 0)
            {
                return false;
            }// Y bottom boundary check

            if (_blockController.cells[x, y] != null)
            {
                return false;
            }
        }
        return true;
    }

    public void Rotate()
    {
        Vector3 current = transform.eulerAngles;
        current.z += -90;
        transform.eulerAngles = current;
    }
}
