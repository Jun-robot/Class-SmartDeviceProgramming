#nullable enable
using GameCanvas;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ゲームクラス。
/// 学生が編集すべきソースコードです。
/// </summary>
public sealed class Game : GameBase{
	// 変数の宣言
	int sec = 0;
	int score=0;
	string pname = "t24354js";
	string url = "";
	string str = "";
	int gameState = 0;

	const int BOX_NUM = 10;
	int[] box_x = new int [BOX_NUM];
	int[] box_y = new int [BOX_NUM];
	int[] box_speed = new int [BOX_NUM];
	int box_w = 24;
	int box_h = 24;

	int player_x = 304;
	int player_y = 400;
	int player_dir = 1;
	int player_speed = 5;

	int count =0;

	/// <summary>
	/// 初期化処理
	/// </summary>
	public override void InitGame(){
		gc.SetResolution(640, 480);
		score = gc.Random(0,100);

		for(int i =0 ; i < BOX_NUM ; i ++ ){
			box_x[i] = gc.Random(0,616);
			box_y[i] = -gc.Random(100,480);
			box_speed[i] = gc.Random(3,6);
		}
	}

	/// <summary>
	/// 動きなどの更新処理
	/// </summary>
	public override void UpdateGame(){
		// 起動からの経過時間を取得します
		sec = (int)gc.TimeSinceStartup;

		// gameStateの処理
		if(gameState == 0){
			//タイトル画面の処理
			if(gc.GetPointerFrameCount(0) ==1 ){
				gameState = 1;
			}
		}else if(gameState == 1){
			//ゲーム中の処理
			count++;
			score = count/60;
			box_w = 24+count/300;
			box_h = 24+count/300;

			if(gc.GetPointerFrameCount(0) ==1 ){
				player_dir = -player_dir;
			}

			if(score<10){
				player_speed = 3;
			}else{
				player_speed = 8;
			}

			player_x += player_dir * player_speed;

			for(int i =0 ; i < BOX_NUM ; i ++ ){
				//箱を動かす処理
				box_y[i] = box_y[i] + box_speed[i];

				if(box_y[i]> 480){
					box_x[i] = gc.Random(0,616);
					box_y[i] = -gc.Random(100,480);
					box_speed[i] = gc.Random(1,3);
				}

				//playerと箱の当り判定
				if (gc.CheckHitRect (
					player_x,player_y,32,32,
					box_x[i],box_y[i],box_w,box_h)) {
					//当たった時の処理
					gameState = 2;
				}
			}
			//playerが画面左右に着いてもgameOverになるように
			// if(player_x < 0 || player_x > 608){
			// 	gameState=2;
			// }
			if(player_x < 0){
				player_x=608;
			}else if(player_x > 608){
				player_x=0;
			}

		
		}else if(gameState == 2){
			//ゲームオーバー時の処理
			if(gc.GetPointerFrameCount(0) ==1 ){
				url = "https://web.sfc.keio.ac.jp/~wadari/sdp/k07_web/score.cgi?score=" + score + "&name=" + pname;
				gc.GetOnlineTextAsync(url,out str);
			}
		}
	}

	/// <summary>
	/// 描画の処理
	/// </summary>
	public override void DrawGame(){
		gc.ClearScreen();
		gc.SetColor(0, 0, 0);
		gc.SetFontSize(36);

		// gameStateの処理
		if(gameState == 0){
			//タイトル画面の処理
			gc.DrawString("TITLE",320,240);
		}else if(gameState == 1){
			//ゲーム中の処理
			gc.DrawOnlineImage("https://jun-robot.github.io/img/favicon/favicon-32x32.png",player_x,player_y);
			for(int i =0 ; i < BOX_NUM ; i ++ ){
				gc.FillRect(box_x[i],box_y[i],box_w,box_h);  
			}
			gc.DrawString("SCORE:"+score,0,0);
		}else if(gameState == 2){
			//ゲームオーバー時の処理
			gc.DrawString("GAME OVER",320,240);
			gc.DrawString("SCORE:"+score,0,0);
			gc.DrawString(str,0,300);
		}
	}
}