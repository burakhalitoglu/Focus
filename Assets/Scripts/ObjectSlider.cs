using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSlider : MonoBehaviour {

	//Kayan nesnelerin bulunacağı pozisyonlar
	Vector3[] positions = { new Vector3(-4,4,0), new Vector3(-2,4,0), new Vector3(0,4,0), 
							new Vector3(2,4,0), new Vector3 (10, 4, 0) }; 

	//Kayan nesnelerin pozisyonlara karşılık gelen scale değerleri
	Vector3 defaultScale = new Vector3 (0.1f, 0.1f, 0.1f);
	Vector3 midScale = new Vector3(0.2f, 0.2f, 0.2f);
	Vector3 scale;

	public int posIndex; // SlidingObjectSpawner'dan gelen, nesnenin pozisyon indisi
	float speed = 10f; // Lerp içerisinde kullanılan kayma hızı çarpanı
	Rotator rotatorScriptComponent; // Nesnenin Rotator komponentini almak için kullanılan değişken 

	bool isSlideActive = false;//Kayma olayının gerçekleşip gerçekleşmeyeceğini belirleyen durum değişkeni

	void OnEnable()
	{
		SlidingObjectSpawner.OnObjectSpawned += OnObjectSpawned;
	}

	void OnDisable()
	{
		SlidingObjectSpawner.OnObjectSpawned -= OnObjectSpawned;
	}

	void Start () 
	{
		// Nesnenin Rotator komponenti alınır
		rotatorScriptComponent = this.gameObject.GetComponent <Rotator> ();
	}

	void Update () 
	{
		LerpObjects ();	// Nesnelerin kaymasını sağlayan fonksiyon
	}

	// SlidingObjectSpawner'dan gelen pozisyon indisini almak için kullanılan fonksiyon
	void GetPosIndex(int posIn)
	{
		posIndex = posIn;
	}

	// Nesnelerin kaymasını sağlayan fonksiyon
	void LerpObjects()
	{
		// Pozisyonun mevcut indisine göre bir sonraki pozisyona kaydırma işlemi
		if (isSlideActive) 
		{
			if (posIndex == positions.Length - 1)
				Destroy (this.gameObject); // Son pozisyondaki nesnenin yok edilmesi
			else
				posIndex++;  // Mevcut pozisyondan bir sonrakine geçiş için pozisyon indisinin arttırılması
		}

		if (posIndex == 2) 
		{
			rotatorScriptComponent.enabled = true; // Nesne orta konumdaysa geldiğinde Rotator komponenti aktif edilir
			scale = midScale;//Ortaya kayacak olan nesnenin scale değeri.
			this.gameObject.name = "TargetObject";//Ortaya kayacak olan nesnenin ismi(GameManager'da karşılaştırmak için kullanılır)
		} 
		else 
		{
			rotatorScriptComponent.enabled = false; // Nesne ortaya harici bir konumdaysa Rotator komponenti pasif edilir
			transform.rotation = Quaternion.identity; // Nesne ortaya harici bir konumdaysa rotasyonu ilk haline getirilir
			scale = defaultScale;//Orta konum dışında olan nesnelerin scale değeri.
			this.gameObject.name = "NonTargetObject";//Orta konum dışında olan nesnelerin etiketi değeri.
		}

		// Pozisyon değeri mevcut konumdan bir sonrakine kaydırılır ve bu pozisyonlara karşılık gelen scale değeri ayarlanır
		transform.position = Vector2.Lerp (transform.position, positions [posIndex], speed*Time.deltaTime);
		transform.localScale = Vector3.Lerp (transform.localScale, scale, speed*Time.deltaTime);
		isSlideActive = false;

	}

	void OnObjectSpawned()//Yeni klon oluştuktan sonra kayma olayını aktif etmek için kullanılan olay
	{
		isSlideActive = true;
	}
}
