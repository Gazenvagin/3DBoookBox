using UnityEngine;
using UnityEngine.EventSystems;

using DG.Tweening;

public class ClickReceiver : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private Material _mat;
    private AnimatorStateInfo _animStateInfo;
    private float _timeAnim;
    private static readonly int RimOffset = Shader.PropertyToID("_RimOffset");
    
    protected Animator _anim;
    protected AnimatorOverrideController _animOverrideCtrl;
    protected int _clipBookIndex;

    #region ** START **
    private void Start()
    {
        _mat = transform.Find("Book_Cover").gameObject.GetComponent<SkinnedMeshRenderer>().material;
        
        _clipBookIndex = BookManagerBox.Ctrl.Books3D.FindIndex(x => x.name == gameObject.name);
        _anim = gameObject.GetComponent<Animator>();
        _animOverrideCtrl = new AnimatorOverrideController(_anim.runtimeAnimatorController);
        _anim.runtimeAnimatorController = _animOverrideCtrl;
        _animOverrideCtrl["ReadyBook_0"] = BookManagerBox.Ctrl.listClipReadyBook[_clipBookIndex];
        _animOverrideCtrl["GetBook_0"] = BookManagerBox.Ctrl.listClipGetBook[_clipBookIndex];
        _animOverrideCtrl["OpenBook"] = BookManagerBox.Ctrl.listClipOpenBook[0];
        _animOverrideCtrl["CloseBook_0"] = BookManagerBox.Ctrl.listClipCloseBook[_clipBookIndex];
        //Debug.Log(_clipBookIndex);
    }
    #endregion
    
    #region ** UPDATE **

    private void Update()
    {
        _animStateInfo = _anim.GetCurrentAnimatorStateInfo(0);
        _timeAnim = _animStateInfo.normalizedTime;

        if (!BookManagerBox.Ctrl.IsButBack && _animStateInfo.IsName("GetBook") && _timeAnim >= 1f)
        {    
            BookManagerBox.Ctrl.ButBack.SetActive(true);
            _timeAnim = _animStateInfo.normalizedTime % 1;
            BookManagerBox.Ctrl.IsButBack = true;
        }

        if (BookManagerBox.Ctrl.IsButBack && _animStateInfo.IsName("CloseBook") && _timeAnim < 1f)
        {
            _timeAnim = _animStateInfo.normalizedTime % 1;
            BookManagerBox.Ctrl.IsButBack = false;
        }

        if (BookManagerBox.Ctrl.IsButBack && _animStateInfo.IsName("OpenBook") && _timeAnim < 1f)
        {
            _timeAnim = _animStateInfo.normalizedTime % 1;
            BookManagerBox.Ctrl.IsButBack = false;
        }

        if (_animStateInfo.IsName("CloseBook") && _timeAnim >= 0.99f)
        {
            for (int i = 0; i < BookManagerBox.Ctrl.Books3D.Count; i++)
            {
                if (BookManagerBox.Ctrl.Books3D[i].activeSelf)
                   BookManagerBox.Ctrl.Books3D[i].GetComponent<Collider>().enabled = true;
            }
        }

        if (_mat.GetFloat(RimOffset) > 0f && _animStateInfo.IsName("CloseBook"))
            _mat.SetFloat(RimOffset, 0f);
    }
    
    #endregion

    #region ** CLICK ACTIONS **

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_animStateInfo.IsName("ReadyBook"))
        {
            BookManagerBox.Ctrl.IsBookOpen = true;
            BookManagerBox.Ctrl.SelectBook(_clipBookIndex);

            _anim.speed = 1f;
            _anim.SetTrigger("Get");
            _mat.SetFloat(RimOffset, 3f);

            for (int i = 0; i < BookManagerBox.Ctrl.Books3D.Count; i++)
            {
                if (BookManagerBox.Ctrl.Books3D[i].name != gameObject.name)
                {
                    BookManagerBox.Ctrl.Books3D[i].GetComponent<Collider>().enabled = false;
                    BookManagerBox.Ctrl.Books3D[i].SetActive(false);
                }
            }
            
            BookManagerBox.Ctrl.BoxBook.sprite = BookManagerBox.Ctrl.BoxBookFull;
            BookManagerBox.Ctrl.BoxBook.DOColor(new Color(0.14f, 0.14f, 0.14f), 2f);
            BookManagerBox.Ctrl.MatBlinkOff(false);
        }

        if (_animStateInfo.IsName("GetBook") && _timeAnim >= 1f)
        {
            _anim.SetTrigger("Open");
            BookManagerBox.Ctrl.ButBack.SetActive(false);
            
            for (int i = 0; i < BookManagerBox.Ctrl.Books3D.Count; i++)
                if (BookManagerBox.Ctrl.Books3D[i].activeSelf)
                    BookManagerBox.Ctrl.Books3D[i].GetComponent<Collider>().enabled = false;
        }

        /*if (_animStateInfo.IsName("OpenBook"))
        {
            _timeAnim = _animStateInfo.normalizedTime % 1;
            _anim.SetTrigger("Close");
        }*/
        
        Debug.Log("Click To Book");
    }
    
    #endregion

    #region ** METHOD TO EVENT **

    public void OffMeshAnimEvent()
    {
        BookManagerBox.Ctrl.BookOpenPagesGo.SetActive(true);
        BookManagerBox.Ctrl.BookOpenShelf.DOColor(Color.clear, 0.5f).From();
    }

    #endregion
}
