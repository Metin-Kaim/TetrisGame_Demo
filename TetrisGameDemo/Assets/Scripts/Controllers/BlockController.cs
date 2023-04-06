using System.Collections;
using System.Collections.Generic;
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

    float _time;
    InputReader _inputReader;
    float temp;

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
        SpawnBlock();
        temp = _waitToDown;
    }

    private void Update()
    {
        if (_time < Time.time && CurrentObject != null)
        {
            _time = Time.time + .2f;//.2f saniyede bir hareket et
            CurrentObject.GetComponent<Block>().HorizontalMove();

            
            if (_inputReader.IsPressedDown)//when press button "S"
            {
                _waitToDown = .1f;
            }
            else
                _waitToDown = temp;
        }
    }

    public void SpawnBlock()
    {
        CurrentObject = Instantiate(GetRandomItemFromList(Blocks), transform);
        StartCoroutine(CurrentObject.GetComponent<Block>().MoveDown());
    }

    private GameObject GetRandomItemFromList(List<GameObject> list)
    {
        return list[Random.Range(0, list.Count)];
    }
}
