using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour {

	private SpriteRenderer sr; // Nesnenin SpriteRenderer komponenti için kullanılacak olan değişken
	private Color rendColor = Color.black; // Nesenin rengi
	public float fadingTime = 0.025f; // Ekranda belirip-kaybolda süresi
	public  Spawner locationRemover; // Nesenin ekrandan yok olduktan sonra pozisyonunun havuzdan kaldırılması için kullanılan örnek

	void Start () 
	{
		sr = GetComponent<SpriteRenderer> (); // Nesenin SpriteRenderer komponentinin sr değişkenine atanması
		sr.color = rendColor; // Nesenin rengine rendColor'ın atanması
		locationRemover = Spawner.Instance; // Spawner sınıfına erişmek için kullandığımız nesne (Singleton)
		StartCoroutine ("FadingObject"); // Fading olayının başlatılması

	}
		
	public IEnumerator FadingObject()
	{
		//FadeIn, alpha değerimiz 0'dan 0.05er olarak 1'e doğru artıyor.
		for (float alpha = 0f; alpha <= 1f; alpha += 0.05f) {
			rendColor.a = alpha;
			sr.color = rendColor;
			yield return new WaitForSeconds (fadingTime);
		}

		//FadeOut, alfa değerimiz 1'den 0.05er olarak 0'a doğru azalıyor.
		for (float alpha = 1f; alpha >= -0.05f; alpha -= 0.05f) {
			rendColor.a = alpha;
			sr.color = rendColor;
			yield return new WaitForSeconds (fadingTime);
		}

		// Nesne fade out olduktan sonra pozisyon havuzundan nesenin pozisyonu çıkarılıyor ve nesne yok ediliyor
		//GameObject go = this.gameObject;
		locationRemover.RemoveLocation (this.gameObject.transform.position);
		Destroy (this.gameObject);
	}
}
