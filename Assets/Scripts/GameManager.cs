using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
 
	string targetTag; //Orta pozisyondaki kayan nesnenin etiketi

	public delegate void PlayerDelegate ();
	public static event PlayerDelegate OnPlayerScored;//SlidingObjectSpawner'a gönderilir ve spawn olayı gerçekleşir.
	public static event PlayerDelegate OnPlayerFailed;//Oyun kaybedilince TapDestroyer'a gönderilir ve son kalan klonları yok eder.


	int score = 0;//Oyun sırasındaki skor
	public Text scoreText; //Oyun sırasındaki skoru gösteren metin
	public GameObject gameOverPage;//Oyun kaybedilince ortaya çıkan Game Over sayfası
	bool isGameOver = false;//Oyun durum kontrolü için kullanılan değişken
	public static GameManager Instance;//Singleton--- Oyun kaybedilince Spawner'da klonlamanın sonlanması için erişim sağlanır.

	public bool GameOver { get {  return isGameOver; } }
	public int Score { get { return score; } }

	void Awake()
	{
		Instance = this;
	}

	void Update()
	{
		//Oyun bittiyse OnPlayerFailed TapDestroyer'a gönderilir ve kalan klonlar temizlenir.
		if (isGameOver && (OnPlayerFailed !=null))
			OnPlayerFailed ();

	}

	void OnEnable()
	{
		TapDestroyer.OnPlayerTap += OnPlayerTap;

	}
		
	void OnDisable()
	{
		TapDestroyer.OnPlayerTap -= OnPlayerTap;
	}

	void OnPlayerTap(string tappedTag)//TapDestroyer'dan gelen bir nesne yok edildiğinde çalışan olay
	{
		targetTag = GameObject.Find ("TargetObject").tag;//Ortadaki hedef nesnenin etiketi
	
		if (tappedTag == targetTag) //Eğer tıklanan nesneyle hedef nesnenin etiketi aynı ise sağlanan koşul
		{
			//Skor yapıldıysa kayma işlemi için yeni klon oluşturulmasını sağlar ve skor arttırılır.
			OnPlayerScored ();
			score++;
			scoreText.text = score.ToString ();
		} 
		else //Eğer tıklanan nesneyle hedef nesnenin etiketi aynı değil ise sağlanan koşul
		{
			isGameOver = true;//Game Over durumu aktif edilir.
			int savedScore = PlayerPrefs.GetInt ("HighScore");//Önceki yüksek skor alınır.
			if (score > savedScore)
				PlayerPrefs.SetInt ("HighScore", score);//Yeni skor öncekinden yüksekse kaydedilir.
			gameOverPage.SetActive (true);//Game Over sayfası aktif edilir.
		}
	}

	public void ConfirmGameOver()
	{
		// Replay butonuna basıldığında çalışacak kısım
		gameOverPage.SetActive (false);
		isGameOver = false;
		scoreText.text = "0";
		SceneManager.LoadScene ("Game");//İlk sahne tekrar başlatılır.
	}
}
