using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingObjectSpawner : MonoBehaviour {

	public delegate void PlayerDelegate ();
	public static event PlayerDelegate OnObjectSpawned;//Yeni klon oluştuktan sonra kayma olayını aktif etmek için kullanılan olay(ObjectSlider'a gönderilir)

	public GameObject[] slidingShapes; // Kayan nesneler
	public static Vector3 startPos = new Vector3(-4,4,0); // Kayan nesnelerin oluşma pozisyonu
	public static Vector3 firstPos= new Vector3(-2,4,0); // Kayan nesnelerin görünen ilk pozisyonu
	public static Vector3 midPos= new Vector3(0,4,0); // Kayan nesnelerin görünen ikinci pozisyonu
	public Vector3 midScale; // Kayan nesnelerden orta pozisyondakinin scale değeri
	private GameObject go; // Oluşturulan klonları tutan nesne
	private int objectIndex; // Klonlanan nesnenin rastgele seçimi için kullanılan indis


	void OnEnable()
	{
		GameManager.OnPlayerScored += OnPlayerScored;
	}

	void OnDisable()
	{
		GameManager.OnPlayerScored -= OnPlayerScored;
	}

	void Start () 
	{
		CreateFirstClones (); // Oyun ilk başladığında ekranda görünecek nesnelerin oluşturulması
	}

	// Oyuncu skor yaptıysa rastgele yeni bir klon oluşturmak için kullanılan fonksiyon
	void OnPlayerScored()//GameManager'dan eğer oyuncu skor yaptıysa gelen olay.
	{
		objectIndex = Random.Range (0, slidingShapes.Length);
		Instantiate (slidingShapes [objectIndex], startPos, transform.rotation);
		OnObjectSpawned ();//Yeni klon oluştuktan sonra kayma olayını aktif etmek için kullanılan olay(ObjectSlider'a gönderilir)
	}

	void CreateFirstClones()
	{
		int posIndex = 1; // Kayan nesnelerin pozisyon indisi

		objectIndex = Random.Range (0, slidingShapes.Length);
		go = Instantiate (slidingShapes [objectIndex], firstPos, transform.rotation);
		go.SendMessage ("GetPosIndex",posIndex++);

		objectIndex = Random.Range (0, slidingShapes.Length);
		go = Instantiate (slidingShapes [objectIndex], midPos, transform.rotation);
		go.transform.localScale = midScale;
		go.SendMessage ("GetPosIndex",posIndex);
	}
}
