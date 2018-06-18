using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapDestroyer : MonoBehaviour {


	public delegate void PlayerDelegate (string tappedTag);//Basılan nesnenin etiketini GameManager.2a göndermek için kullanılan delege.
	public static event PlayerDelegate OnPlayerTap;//GameManager'a basılan nesnenin etiketini gönderen olay.

	public RaycastHit2D hit;
	public GameObject oneUp; // Animasyon nesnesi
	public  Spawner locationRemover; // Spawner'da lokasyonları kaldırmak için kullanılacak olan nesne
	public string tappedTag; // Üzerine basılan nesnenin etiketi
	public Transform tappedObjectTransform; // Üzerine basılan nesnenin transformu, pozisyonu liste havuzundan çıkarmak için kullanılıyor


	void OnEnable()
	{
		GameManager.OnPlayerFailed += OnPlayerFailed;
	}

	void OnDisable()
	{
		GameManager.OnPlayerFailed -= OnPlayerFailed;
	}

	void Start () 
	{
		locationRemover = Spawner.Instance; // Spawner sınıfına erişmek için kullandığımız nesne(Basılan nesnenin konumu silinir)
	}
		
	void Update () 
	{
		
		if (Input.GetMouseButtonDown(0)) 
		{ 
			hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);

			// Ekrana dokunulduğunda herhangi bir collider'a sahip nesneye dokunup dokunmadığımızın tespit edilmesi
			if (hit.collider != null) 
			{ 
				tappedObjectTransform = hit.collider.gameObject.transform; // Dokunulan nesnenin transformu
				tappedTag = hit.collider.gameObject.tag; // Dokunulan nesnenin etkieti
				Destroy (hit.collider.gameObject); // Dokunulan nesnenin yok edilmesi
				locationRemover.RemoveLocation (tappedObjectTransform.position); // Dokunulan nesnenin pozisyon havuzundan kaldırılması

				GameObject scoreText = Instantiate (oneUp, tappedObjectTransform.position, Quaternion.identity); // Skor animasyonunun oluşturulması
				Destroy(scoreText,1f); // Skor animasyonunun yok edilmesi
				OnPlayerTap(tappedTag);//Basılan nesnenin etiketi GameManager'a gönderilir.
			}
		}
	}

	void OnPlayerFailed()//Oyun bittiğinde kalan klonlar yok edilir.
	{
		Destroy (this.gameObject);
	}
}
