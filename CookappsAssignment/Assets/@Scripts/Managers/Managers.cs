using UnityEngine;

public class Managers : MonoBehaviour
{
	public static bool Initialized { get; set; } = false;

	private static Managers s_instance; // 유일성이 보장된다
    private static Managers Instance { get { Init(); return s_instance; } } // 유일한 매니저를 갖고온다

    #region Contents

    private GameManager _game = new GameManager();
    private ObjectManager _object = new ObjectManager();
    
    public static GameManager Game { get { return Instance?._game; } }
    public static ObjectManager Object { get { return Instance?._object; } }
    
    #endregion

    #region Core

    private DataManager _data = new DataManager();
    private PoolManager _pool = new PoolManager();
    private ResourceManager _resource = new ResourceManager();
    private UIManager _ui = new UIManager();


    public static DataManager Data { get { return Instance?._data; } }
    public static PoolManager Pool { get { return Instance?._pool; } }
    public static ResourceManager Resource { get { return Instance?._resource; } }
    public static UIManager UI { get { return Instance?._ui; } }

    #endregion

    
    public static void Init()
    {
        if (s_instance == null && Initialized == false)
        {
            Initialized = true;

			GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();
        }		
	}

    public static void Clear()
    {
        UI.Clear();
        Pool.Clear();
    }


}
