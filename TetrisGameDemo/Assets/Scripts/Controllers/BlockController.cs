using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

//Bloklarin hareketini saðlamak
//Bloklarin parent objelerini degistirmek
public class BlockController : MonoBehaviour
{
    public const int BOUNDARY_X = 10, BOUNDARY_Y = 20;

    public GameObject[,] cells = new GameObject[BOUNDARY_X, BOUNDARY_Y];

    public List<List<int>> locations = new();

    [SerializeField, Range(.5f, 2.5f)] float _waitToDown;
    [SerializeField] GameObject _currentObject;
    [SerializeField] List<GameObject> _blocks = new(); // j-block, t-block, s-block...

    #region Test
    [SerializeField] GameObject _sprite;
    [SerializeField] GameObject[,] _sprites = new GameObject[10, 20];
    [SerializeField] Transform _testObject;
    [SerializeField] bool _isTest;
    #endregion

    float _time;
    InputReader _inputReader;
    float temp;
    Block _currentBlock;
    bool _isDestroyed;

    public float WaitToDown => _waitToDown;
    public GameObject CurrentObject { get => _currentObject; set => _currentObject = value; }
    public List<GameObject> Blocks => _blocks;


    private void Awake()
    {
        _inputReader = GetComponent<InputReader>();
    }

    private void Start()
    {
        if (_isTest)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    _sprites[i, j] = Instantiate(_sprite, new Vector3(i, j, 0), Quaternion.identity, _testObject);
                }
            }
        }

        temp = _waitToDown;
        SpawnObject();
    }

    private void Update()
    {
        if (_time < Time.time && CurrentObject != null)
        {
            _time = Time.time + .2f;//.2f saniyede bir hareket et
            _currentBlock.HorizontalMove();


            if (_inputReader.IsPressedDown)//when press button "S"
                _waitToDown = .1f;
            else
                _waitToDown = temp;


            if (_currentBlock.CanRotate && _inputReader.IsPressedRotate)
                _currentBlock.Rotate();
        }


    }

    public void SpawnObject()
    {

        if (_isTest)
        {
            TestFunction();
        }

        CheckNDestroyFullRow();
        BlockFaller();

        if (_isTest)
        {
            TestFunction();
        }
        StartCoroutine(SpawnObjectWaiter(0.1f));
    }

    private void BlockFaller()
    {
        while (_isDestroyed)
        {
            if (_isTest)
            {
                TestFunction();
            }

            _isDestroyed = false;
            _waitToDown = 0;

            //foreach (Transform child in transform)
            //{
            //    //child.GetComponent<Block>().BlockFaller();
            //}
            for (int y = 0; y < BOUNDARY_Y; y++)
            {
                for (int x = 0; x < BOUNDARY_X; x++)
                {
                    GameObject currentObject = cells[x, y];
                    if (currentObject == null)
                        continue;

                    //düsme islemi
                    int posX = Mathf.RoundToInt(currentObject.transform.position.x);
                    int posY = Mathf.RoundToInt(currentObject.transform.position.y);
                    cells[posX, posY] = null;

                    while (true)
                    {
                        posY--;
                        if (posY < 0)
                        {
                            break;
                        }// Y bottom boundary check
                        else if (cells[posX, posY] != null)
                        {
                            break;
                        }
                    }
                    posY++;
                    currentObject.transform.position = new Vector2(posX, posY);
                    cells[posX, posY] = currentObject;
                }


                CheckNDestroyFullRow();
            }
        }
    }

    private IEnumerator SpawnObjectWaiter(float second)
    {

        yield return new WaitForSeconds(second);

        _waitToDown = temp;

        CurrentObject = Instantiate(GetRandomItemFromList(Blocks), transform);

        if (cells[Mathf.RoundToInt(CurrentObject.transform.position.x), Mathf.RoundToInt(CurrentObject.transform.position.y)] != null)
        {
            GameManager.Instance.GameOver();
        }

        _currentBlock = CurrentObject.GetComponent<Block>();

        StartCoroutine(_currentBlock.MoveDown(true));
    }

    private void TestFunction()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                if (cells[i, j] == null)
                {
                    _sprites[i, j].GetComponent<SpriteRenderer>().material.color = Color.black;
                }
                else
                    _sprites[i, j].GetComponent<SpriteRenderer>().material.color = Color.grey;
            }
        }
    }

    private GameObject GetRandomItemFromList(List<GameObject> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    public void CheckNDestroyFullRow()
    {
        bool isFull;

        for (int y = 0; y < BOUNDARY_Y; y++)
        {
            isFull = true;
            for (int x = 0; x < BOUNDARY_X; x++)
            {
                if (cells[x, y] == null)
                {
                    isFull = false;
                    break;
                }
            }
            if (isFull)
            {
                for (int x = 0; x < BOUNDARY_X; x++)
                {
                    _isDestroyed = true;
                    Destroy(cells[x, y]);
                    cells[x, y] = null;
                }
            }

        }
    }



}

