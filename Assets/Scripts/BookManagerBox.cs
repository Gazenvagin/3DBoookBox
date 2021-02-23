using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using DG.Tweening;

[RequireComponent(typeof(LoadCover))]
[RequireComponent(typeof(LoadUCover))]
[RequireComponent(typeof(LoadPagesBook0))]
[RequireComponent(typeof(LoadPagesBook1))]
[RequireComponent(typeof(LoadPagesBook2))]
[RequireComponent(typeof(LoadPagesBook3))]
[RequireComponent(typeof(LoadPagesBook4))]
[RequireComponent(typeof(LoadPagesBook5))]
public class BookManagerBox : MonoBehaviour
{
    #region ** VARRIABLES **
    
    public static BookManagerBox Ctrl;
    
    [Header("BOOK 3D")]
    [Space]
    public List<GameObject> Books3D = new List<GameObject>();
    public List<AnimationClip> listClipReadyBook = new List<AnimationClip>();
    public List<AnimationClip> listClipGetBook = new List<AnimationClip>();
    public List<AnimationClip> listClipOpenBook = new List<AnimationClip>();
    public List<AnimationClip> listClipCloseBook = new List<AnimationClip>();
    public Image BoxBook;
    public Sprite BoxBookDef;
    public Sprite BoxBookFull;
    public GameObject ButBack;
    public float TimerBlink = 1.5f;
    [HideInInspector] public float TimeReload = 900f; //-> locked timer reload content
    [HideInInspector] public bool IsBlink = true;
    [HideInInspector] public bool IsBookOpen = false;
    [HideInInspector] public bool IsButBack = false;

    [Header("BOOK OPEN PAGES")]
    [Space]
    public GameObject BookOpenPagesGo;
    public Book BookConteiner;
    public Image BookOpenShelf;
    public Image RightNext;
    public Image LeftNext;
    public Sprite TrpGrayBackBook;
    public Button ButtonClose;

    private LoadCover _loadCover;
    private LoadUCover _loadUCover;
    private LoadPagesBook0 _loadPagesBook0;
    private LoadPagesBook1 _loadPagesBook1;
    private LoadPagesBook2 _loadPagesBook2;
    private LoadPagesBook3 _loadPagesBook3;
    private LoadPagesBook4 _loadPagesBook4;
    private LoadPagesBook5 _loadPagesBook5;
    
    private List<Sprite> _listUCoverBook = new List<Sprite>();
    private List<Material> _listMatBook = new List<Material>();

    private List<Sprite> _listBook3DPages0 = new List<Sprite>();
    private List<Sprite> _listBook3DPages1 = new List<Sprite>();
    private List<Sprite> _listBook3DPages2 = new List<Sprite>();
    private List<Sprite> _listBook3DPages3 = new List<Sprite>();
    private List<Sprite> _listBook3DPages4 = new List<Sprite>();
    private List<Sprite> _listBook3DPages5 = new List<Sprite>();

    private PointerEventData _click;
    private List<ClickReceiver> _listClickBook = new List<ClickReceiver>();

    private Animator _anim;
    private List<Animator> _animList = new List<Animator>();
    //private GameObject _bookMeshInside;
    private Material _defMatBookCover;
    private Sprite[] _defPages;
    private Sprite _defOpenShelfBook;
    private Sprite _openShelfBook;
    
    private float _bookArea1;
    private float _bookArea2;
    private float _bookArea3;

    private float _matBlink;
    private bool _isBlink = false;

    private static readonly int Diffuse = Shader.PropertyToID("_Diffuse");
    private static readonly int Close = Animator.StringToHash("Close");

    #endregion

    #region ** AWAKE **
    private void Awake()
    { 
        Ctrl = this;

        if (!_loadCover)
            _loadCover = GetComponent<LoadCover>();
        
        if (!_loadUCover)
            _loadUCover = GetComponent<LoadUCover>();
        
        if (!_loadPagesBook0)
            _loadPagesBook0 = GetComponent<LoadPagesBook0>();
        
        if (!_loadPagesBook1)
            _loadPagesBook1 = GetComponent<LoadPagesBook1>();
        
        if (!_loadPagesBook2)
            _loadPagesBook2 = GetComponent<LoadPagesBook2>();
        
        if (!_loadPagesBook3)
            _loadPagesBook3 = GetComponent<LoadPagesBook3>();
        
        if (!_loadPagesBook4)
            _loadPagesBook4 = GetComponent<LoadPagesBook4>();
        
        if (!_loadPagesBook5)
            _loadPagesBook5 = GetComponent<LoadPagesBook5>();
        
        _defPages = BookConteiner.bookPages;
        _defOpenShelfBook = BookOpenShelf.sprite;

        ButBack.SetActive(false);
        BookOpenPagesGo.SetActive(false);

        foreach (var books in Books3D)
        {
            _animList.Add(books.GetComponent<Animator>());
            _listClickBook.Add(books.GetComponent<ClickReceiver>());
        }

        foreach (var anim in _animList)
        {
            anim.speed = 0;
        }

        TimerBlink = 3f;
    }
    
    #endregion

    #region ** START **
    private void Start()
    {
        //StartCoroutine(StartReloadContent()); //-> timer reload content
        LoadExtContent();
        LoadBookArea();
    }
    
    #endregion

    #region ** UPDATE **

    private void Update()
    {
        if (IsBlink && _isBlink)
            foreach (var mat in _listMatBook)
                mat.DOFloat(2f, "_RimPower", TimerBlink).OnComplete(() => { _isBlink = false;});
            

        if (IsBlink && !_isBlink)
            foreach (var mat in _listMatBook)
                mat.DOFloat(0.55f, "_RimPower", TimerBlink).OnComplete(() => { _isBlink = true;});
        
        if (!AutoFlip.isFlipping)
        {
            ButtonClose.interactable = true;
        }
        else
        {
            ButtonClose.interactable = false;
        }
            
        if (LukoilProcessor.X != 0 && !IsBookOpen)
        {
            var i = LukoilProcessor.X;
            var s = Math.Round(i, 2, MidpointRounding.AwayFromZero);
            
            /*var s = i.ToString().Substring(0, 2);
            i = Convert.ToSingle(s);*/

            if (s <= _bookArea1)
            {
                SelectBook(3);
                _listClickBook[3].OnPointerDown(_click);
                LukoilProcessor.X = 0;
                s = 0;
            }

            if (s > _bookArea1 && s <= _bookArea2)
            {
                SelectBook(4);
                _listClickBook[4].OnPointerDown(_click);
                LukoilProcessor.X = 0;
                s = 0;
            }

            if (s > _bookArea2 && s <= _bookArea3)
            {
                SelectBook(5);
                _listClickBook[5].OnPointerDown(_click);
                LukoilProcessor.X = 0;
                s = 0;
            }
        }
    }

    #endregion
    
    #region ** LOAD EXT CONTENT **

    private void LoadExtContent()
    {
        foreach (var nw in Books3D.Zip(_loadCover.CoverBooks, Tuple.Create))
            LoadCoverBook(nw.Item1, nw.Item2);

        foreach (var nw in Books3D.Zip(_loadUCover.CoverUBooks, Tuple.Create))
            LoadUCoverBook(nw.Item1, nw.Item2);

        foreach (var path in _loadPagesBook0.PagesBook0)
            LoadPagesBook0(path);
        
        foreach (var path in _loadPagesBook1.PagesBook1)
            LoadPagesBook1(path);
        
        foreach (var path in _loadPagesBook2.PagesBook2)
            LoadPagesBook2(path);
        
        foreach (var path in _loadPagesBook3.PagesBook3)
            LoadPagesBook3(path);
        
        foreach (var path in _loadPagesBook4.PagesBook4)
            LoadPagesBook4(path);
        
        foreach (var path in _loadPagesBook5.PagesBook5)
            LoadPagesBook5(path);
    }

    #endregion

    #region ** SELECT BOOK **
    public void SelectBook(int count)
    {
        if (count == 0)
        {
            BookOpenShelf.sprite = _listUCoverBook[0];
            BookConteiner.bookPages = new Sprite[_listBook3DPages0.Capacity];
            BookConteiner.bookPages = _listBook3DPages0.ToArray();
            BookConteiner.currentPage = 0;
            RightNext.sprite = _listBook3DPages0[0];
            LeftNext.sprite = TrpGrayBackBook;
        }
        
        if (count == 1)
        {
            BookOpenShelf.sprite = _listUCoverBook[1];
            BookConteiner.bookPages = new Sprite[_listBook3DPages1.Capacity];
            BookConteiner.bookPages = _listBook3DPages1.ToArray();
            BookConteiner.currentPage = 0;
            RightNext.sprite = _listBook3DPages1[0];
            LeftNext.sprite = TrpGrayBackBook;
        }
        
        if (count == 2)
        {
            BookOpenShelf.sprite = _listUCoverBook[2];
            BookConteiner.bookPages = new Sprite[_listBook3DPages2.Capacity];
            BookConteiner.bookPages = _listBook3DPages2.ToArray();
            BookConteiner.currentPage = 0;
            RightNext.sprite = _listBook3DPages2[0];
            LeftNext.sprite = TrpGrayBackBook;
        }
        
        if (count == 3)
        {
            BookOpenShelf.sprite = _listUCoverBook[3];
            BookConteiner.bookPages = new Sprite[_listBook3DPages3.Capacity];
            BookConteiner.bookPages = _listBook3DPages3.ToArray();
            BookConteiner.currentPage = 0;
            RightNext.sprite = _listBook3DPages3[0];
            LeftNext.sprite = TrpGrayBackBook;
        }
        
        if (count == 4)
        {
            BookOpenShelf.sprite = _listUCoverBook[4];
            BookConteiner.bookPages = new Sprite[_listBook3DPages4.Capacity];
            BookConteiner.bookPages = _listBook3DPages4.ToArray();
            BookConteiner.currentPage = 0;
            RightNext.sprite = _listBook3DPages4[0];
            LeftNext.sprite = TrpGrayBackBook;
        }
        
        if (count == 5)
        {
            BookOpenShelf.sprite = _listUCoverBook[5];
            BookConteiner.bookPages = new Sprite[_listBook3DPages5.Capacity];
            BookConteiner.bookPages = _listBook3DPages5.ToArray();
            BookConteiner.currentPage = 0;
            RightNext.sprite = _listBook3DPages5[0];
            LeftNext.sprite = TrpGrayBackBook;
        }
    }

    #endregion
    
    #region ** LOAD JSON BOOK AREA **

    public void LoadBookArea()
    {
        string loadPath = Application.streamingAssetsPath + "/data/BoxSaveData.json";
        string jsonString = File.ReadAllText(loadPath);
        var saveData = JsonUtility.FromJson<BoxLowSaveData>(jsonString);
        _bookArea1 = saveData.BookArea1;
        _bookArea2 = saveData.BookArea2;
        _bookArea3 = saveData.BookArea3;
    }

    #endregion
    
    #region ** LOAD JSON COVER BOOK **

    void LoadCoverBook(GameObject book, String path)
    {
        _defMatBookCover = book.transform.Find("Book_Cover").gameObject.GetComponent<SkinnedMeshRenderer>().material;
        
        byte[] coverdata = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(1600, 1068);
        tex.LoadImage(coverdata);

        if(tex != null)
            _defMatBookCover.SetTexture(Diffuse, tex);
        
        _listMatBook.Add(_defMatBookCover);

        IsBlink = true;
        _isBlink = true;
    }

    #endregion
    
    #region ** LOAD JSON UCOVER BOOK **

    void LoadUCoverBook(GameObject book, String path)
    {
        byte[] coverdata = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(1600, 1068);
        tex.LoadImage(coverdata);
        Sprite fromtex = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);
        
        if (fromtex != null)
            _listUCoverBook.Add(fromtex);
    }

    #endregion

    #region ** LOAD JSON PAGES BOOK #0 **
    
    void LoadPagesBook0(string path)
    {
        byte[] pagebytes = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(841, 1092);
        tex.LoadImage(pagebytes);
        Sprite fromtex = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);

        if (fromtex != null)
            _listBook3DPages0.Add(fromtex);
    }
    
    #endregion
    
    #region ** LOAD JSON PAGES BOOK #1 **
    
    void LoadPagesBook1(string path)
    {
        byte[] pagebytes = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(841, 1092);
        tex.LoadImage(pagebytes);
        Sprite fromtex = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);

        if (fromtex != null)
            _listBook3DPages1.Add(fromtex);
    }
    
    #endregion
    
    #region ** LOAD JSON PAGES BOOK #2 **
    
    void LoadPagesBook2(string path)
    {
        byte[] pagebytes = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(841, 1092);
        tex.LoadImage(pagebytes);
        Sprite fromtex = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);

        if (fromtex != null)
            _listBook3DPages2.Add(fromtex);
    }
    
    #endregion
    
    #region ** LOAD JSON PAGES BOOK #3 **
    
    void LoadPagesBook3(string path)
    {
        byte[] pagebytes = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(841, 1092);
        tex.LoadImage(pagebytes);
        Sprite fromtex = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);

        if (fromtex != null)
            _listBook3DPages3.Add(fromtex);
    }
    
    #endregion
    
    #region ** LOAD JSON PAGES BOOK #4 **
    
    void LoadPagesBook4(string path)
    {
        byte[] pagebytes = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(841, 1092);
        tex.LoadImage(pagebytes);
        Sprite fromtex = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);

        if (fromtex != null)
            _listBook3DPages4.Add(fromtex);
    }
    
    #endregion
    
    #region ** LOAD JSON PAGES BOOK #5 **
    
    void LoadPagesBook5(string path)
    {
        byte[] pagebytes = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(841, 1092);
        tex.LoadImage(pagebytes);
        Sprite fromtex = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);

        if (fromtex != null)
            _listBook3DPages5.Add(fromtex);
    }
    
    #endregion
    
    #region ** MAT BLINK OFF **

    public void MatBlinkOff(bool blink)
    {
        IsBlink = blink;
        foreach (var mat in _listMatBook)
        {
            if (mat != null)
                mat.DOFloat(5f, "_RimPower", TimerBlink);
        }
    }

    #endregion
    
    #region ** CLOSE BUTTON **
    public void ButClose()
    {
        BookOpenShelf.sprite = _defOpenShelfBook;
        BookConteiner.bookPages = _defPages;
        BookConteiner.currentPage = 0;
        RightNext.sprite = _defPages[0];
        LeftNext.sprite = TrpGrayBackBook;
        
        BoxBook.sprite = BoxBookDef;
        BookOpenPagesGo.SetActive(false);
        BoxBook.DOColor(new Color(0.5f, 0.47f, 0.42f), 0.7f);

        if (ButBack.activeSelf)
        {
            foreach (var anim in _animList)
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("GetBook"))
                    anim.SetTrigger(Close);
            
            ButBack.SetActive(false);
        }
        else
        {
            foreach (var anim in _animList)
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("OpenBook"))
                    anim.SetTrigger(Close);
        }

        for (int i = 0; i < Books3D.Count; i++)
        {
            if (!Books3D[i].activeSelf)
                Books3D[i].SetActive(true);
            
            /*if (Books3D[i].activeSelf)
                Books3D[i].GetComponent<Collider>().enabled = true;*/
        }
        
        LukoilProcessor.X = 0;
        
        IsBookOpen = false;
        IsBlink = true;
        _isBlink = false;
    }
    
    #endregion
    
    IEnumerator StartReloadContent()
    {
        rest:
        yield return new WaitForSeconds(TimeReload);
        LoadExtContent();
        goto rest;
    }
}
