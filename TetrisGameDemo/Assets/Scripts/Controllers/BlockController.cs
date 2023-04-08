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

    [SerializeField] GameObject sprite;
    [SerializeField] GameObject[,] sprites=new GameObject[10,20];

    float _time;
    InputReader _inputReader;
    float temp;
    Block _currentBlock;

    [SerializeField, Range(.5f, 2.5f)] float _waitToDown;
    [SerializeField] GameObject _currentObject;
    [SerializeField] List<GameObject> _blocks = new(); // j-block, t-block, s-block...

    public float WaitToDown => _waitToDown;
    public GameObject CurrentObject { get => _currentObject; set => _currentObject = value; }
    public List<GameObject> Blocks => _blocks;


    private void Awake()
    {
        _inputReader = GetComponent<InputReader>();
    }

    private void Start()
    {
        
        temp = _waitToDown;
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                print("a");
                sprites[i,j] = Instantiate(sprite,new Vector3(i,j,0),Quaternion.identity);
            }
        }
        SpawnBlock();
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


            if(_inputReader.IsPressedRotate)
                _currentBlock.Rotate();
        }

        
    }

    public void SpawnBlock()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                if (cells[i, j] == null)
                {
                    sprites[i, j].GetComponent<SpriteRenderer>().material.color = Color.red;
                }
                else
                    sprites[i, j].GetComponent<SpriteRenderer>().material.color = Color.green;

            }
        }

        CurrentObject = Instantiate(GetRandomItemFromList(Blocks), transform);

        _currentBlock = CurrentObject.GetComponent<Block>();

        StartCoroutine(_currentBlock.MoveDown());
    }

    private GameObject GetRandomItemFromList(List<GameObject> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    
}
