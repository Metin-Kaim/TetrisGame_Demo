using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Block : MonoBehaviour
{
    GameObject _gameController;
    BlockController _blockController;
    InputReader _inputReader;
    [SerializeField] bool _canRotate;

    //[SerializeField] Vector3 birVector;
    //[SerializeField] Vector3 ikiVector;
    //[SerializeField] Vector3 ucVector;
    //[SerializeField] Vector3 dortVector;

    public bool CanRotate { get => _canRotate; private set => _canRotate = value; }

    private void Awake()
    {
        _gameController = GameObject.FindGameObjectWithTag("GameController");

        _blockController = _gameController.GetComponent<BlockController>();

        _inputReader = _gameController.GetComponent<InputReader>();
    }

    private void Update()
    {

        //birVector.x = Mathf.RoundToInt(transform.GetChild(0).transform.position.x);
        //ikiVector.x = Mathf.RoundToInt(transform.GetChild(1).transform.position.x);
        //ucVector.x = Mathf.RoundToInt(transform.GetChild(2).transform.position.x);
        //dortVector.x = Mathf.RoundToInt(transform.GetChild(3).transform.position.x);

        //birVector.y = Mathf.RoundToInt(transform.GetChild(0).transform.position.y);
        //ikiVector.y = Mathf.RoundToInt(transform.GetChild(1).transform.position.y);
        //ucVector.y = Mathf.RoundToInt(transform.GetChild(2).transform.position.y);
        //dortVector.y = Mathf.RoundToInt(transform.GetChild(3).transform.position.y);
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

    public bool CheckForRotate()
    {
        foreach (Transform child in transform)
        {
            //world positions
            int wX = Mathf.RoundToInt(child.position.x);
            int wY = Mathf.RoundToInt(child.position.y);

            //local positions
            int lX = Mathf.RoundToInt(child.localPosition.x);
            int lY = Mathf.RoundToInt(child.localPosition.y);

            //local y = -x , x = y
            int temp = lY;
            lY = -lX;
            lX = temp;
            //print($"�nce --> wX: {wX}, wY: {wY}");

            //world positions simulation after rotate
            wX = wX + lX + lY;
            wY = wY - lX + lY;

            //print($"sonra --> wX: {wX}, wY: {wY}");

            if (wX < 0 || wX >= BlockController.BOUNDARY_X)
            {
                return false;
            }// X boundary check

            if (wY < 0 || wY >= BlockController.BOUNDARY_Y)
            {
                return false;
            }// Y bottom boundary check

            if (_blockController.cells[wX, wY] != null)
            {
                return false;
            }

        }
        return true;
    }


    public void Rotate()
    {
        if (!CheckForRotate()) return;

        foreach (Transform child in transform)
        {
            int lX = Mathf.RoundToInt(child.localPosition.x);
            int lY = Mathf.RoundToInt(child.localPosition.y);

            //local y = -x , x = y
            int temp = lY;
            lY = -lX;
            lX = temp;
            child.transform.localPosition = new Vector2(lX, lY);
        }
    }

    public void BlockFaller()
    {
        foreach (Transform child in transform)
        {
            _blockController.cells[Mathf.RoundToInt(child.position.x), Mathf.RoundToInt(child.position.y)] = null;
        }
        StartCoroutine(MoveDown(false));
    }

    public IEnumerator MoveDown(bool canSpawn)
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
                if (canSpawn)
                    _blockController.PrepareToSpawn();
                break;
            }

            Vector2 position = GetPosition();
            position.y = Mathf.RoundToInt(--position.y);
            SetPosition(position);
        }
    }
}
