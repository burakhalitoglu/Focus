using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Spawner : MonoBehaviour {

	public static Spawner Instance; // TapDestroyer scriptinde kullanılması için bu sınıfın bir örneği
	GameManager game;

	private float minPosX = -2.35f; // Nesnenin oluşacağı min x koord.
	private float maxPosX = 2.35f; // Nesnenin oluşacağı max x koord.
	private float minPosY = -4.2f; // Nesnenin oluşacağı min y koord.
	private float maxPosY = 2.05f; // Nesnenin oluşacağı max y koord.

	public List<Vector3> shapeLocations = new List<Vector3>(); // Klonların pozisyonlarının tutulduğu liste
	public Vector3 newPos; // Yeni oluşturulacak klonun pozisyonu

	public GameObject[] shapePrefab; // Klonlarımız için kullanılacak prefablar
	public int objectIndex; // Rastgele şekilde klon oluşturulması için kullanılan indis

	public GameObject referenceGeometry; // Ekran içerisindeki kenar sınırları belirlemek için kullanılan referans kare nesnesi
	public float referenceRadius; // Ekran içerisindeki kenar sınırları belirlemek için kullanılan referans nesnenin yarıçapı
	public float borderCoefficient = 2f; // Ekran sınırları için belirlenen değişken

	public float spawnTime = 1f; // Bir sonraki klonlama işlemi için geçen süre
	private bool isOverlapped = false;// newPos değişkeninin mevcut pozisyonlarla çakışıp çakışmadığının gösteren değişken

	void Start () 
	{
		game = GameManager.Instance;//GameManager'a erişmek için kullanılan örnek.
		referenceGeometry.AddComponent <CircleCollider2D>(); // Referans geometriye CircleCollider eklenmesi

		/* 	Referans kare'ye ait CircleCollider'ın yarıçap değeri alınır, bu yarıçap değeri karenin iç teğet çemberinin 
			yarıçapıdır. Sınır hesaplamasında dış teğet çemberin yarıçapı kullanılacağından kök 2 ile çarpılmıştır. */
		referenceRadius = (referenceGeometry.GetComponent <CircleCollider2D> ().radius) * Mathf.Sqrt (2f)* referenceGeometry.transform.localScale.x;
		Destroy (referenceGeometry.GetComponent<CircleCollider2D> ()); // Eklenen Collider bir daha kullanılmacağından yok edilmiştir. 
		StartCoroutine ("ShapeSpawner"); // Sürekli olarak klonlama yapılması için coroutine başlatılır. 
	}

	void Awake() 
	{
		Instance = this; // Bu sınıfın bir örneği (Singleton)
	}
		
		
	public IEnumerator ShapeSpawner()
	{
		while (true) 
		{
			if (game.GameOver)//Eğer oyun kaybedildiyse klonalama işlemi kesilir.
				break ;
			
			yield return new WaitForSeconds (spawnTime); // spawnTime süresi boyunca bekleme
			isOverlapped = true; 
			objectIndex = Random.Range(0,shapePrefab.Length); // 0 ile maksimum nesne sayısı arasında rastgele indis belirleme

			while (isOverlapped)
			{
				/* Min ve maks x-y değerleri arasında yeni pozisyon belirlenir ve pozisyon listesinde olup 
				olmadığı kontrol edilir*/
				newPos = new Vector3 (Random.Range (minPosX, maxPosX), Random.Range (minPosY, maxPosY), 0);
				isOverlapped = CheckOverlap (newPos);
			}
			shapeLocations.Add (newPos); // Klonun pozisyonunu pozisyon havuzuna ekleme
			Instantiate (shapePrefab[objectIndex], newPos, Quaternion.identity); // Yeni rastgele pozisyona göre klonlama
		}
	}

	// Ekrandan yok edilen klonların pozisyonlarının pozisyon havuzundan çıkarılması
	public void RemoveLocation(Vector3 pos)
	{
		shapeLocations.Remove (pos);
	}

	// Oluşturulan rastgele pozisyonun mevcut pozisyon havuzundaki pozisyonlarla karşılaştırılması
	public bool CheckOverlap(Vector3 newPosition)
	{
		for(int i=0; i < shapeLocations.Count; i++)// Pozisyon listesinin boyutu kadar döngü çalışsın
		{
			if((newPosition.x <=shapeLocations[i].x + borderCoefficient * referenceRadius) && (newPosition.x >= shapeLocations[i].x - borderCoefficient * referenceRadius))	// oldPos.x-borderCoefficient*radius < newPos.x < oldPos.x+borderCoefficient*radius
				if((newPosition.y <=shapeLocations[i].y + borderCoefficient * referenceRadius) && (newPosition.y >= shapeLocations[i].y - borderCoefficient * referenceRadius)) // oldPos.y-borderCoefficient*radius < newPos.y < oldPos.x+borderCoefficient*radius
				{
					return true; // Eğer yeni pozisyon mevcut havuzdaki pozisyonlardan herhangi biriyle çakışıyorsa true döndürür
				}
		}
		return false; // Eğer yeni pozisyon mevcut havuzdaki pozisyonlardan herhangi biriyle çakışmıyorsa false döndürür 
	}
}
