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
	int ball_x;
	int ball_y;
	int ball_speed_x;
	int ball_speed_y;

	int player_x;
	int player_y;
	int player_w;
	int player_h;

	const int BLOCK_NUM = 50;
	int[] block_x = new int [BLOCK_NUM];
	int[] block_y = new int [BLOCK_NUM];
	bool[] block_alive_flag = new bool [BLOCK_NUM];
	int block_w = 58;
	int block_h = 20;
	int block_interval = 4;

	int time;
	int stage_phase;
	int stage1_pointer_count; // 長押しになってしまって2に入ってもすぐリスタートしてしまうのを防ぐため。

	/// <summary>
	/// 初期化処理
	/// </summary>
	public override void InitGame(){
		gc.SetResolution(640, 480);
		resetValue();
		stage_phase = 0;
	}

	/// <summary>
	/// 動きなどの更新処理
	/// </summary>
	public override void UpdateGame(){
		if(gc.GetPointerFrameCount(0)>0 && stage_phase == 0){ // タッチされたらスタート
			stage_phase = 1;
		}
		if(countBlock()==0){
			stage_phase = 2;
		}
		if(stage_phase == 2){
			if(gc.GetPointerFrameCount(0)>=120 && gc.GetPointerFrameCount(0)<=stage1_pointer_count){
				// 2秒以上タッチされていたらリスタート
				resetValue();
				stage_phase = 0;
			}
			resetValue();
			stage_phase = 0;
		}

		if(stage_phase == 1){
			stage1_pointer_count = gc.GetPointerFrameCount(0);
			if(stage1_pointer_count<120)stage1_pointer_count = 120; //どうやって処理するのが正しいのかわからん
			// 時間測定
			time++;

			// ボールを動かす
			ball_x = ball_x + ball_speed_x;
			ball_y = ball_y + ball_speed_y;

			// 壁との跳ね返り判定
			if(ball_x<0){
				ball_x = 0;
				ball_speed_x = -ball_speed_x;
			}
			if(ball_y<0){
				ball_y = 0;
				ball_speed_y = -ball_speed_y;
			}
			if(ball_x>616){
				ball_x = 616;
				ball_speed_x = -ball_speed_x;
			}
			if(ball_y>456){
				// 下に行った
				// ball_y = 456;
				// ball_speed_y = -ball_speed_y;
			}

			// プレイヤーの操作
			if(gc.GetPointerFrameCount(0)>0){ //タッチされていたら
				player_x = (int)gc.GetPointerX(0) - player_w/2; //タッチ位置の真ん中をプレイヤーの位置にする
				player_y = (int)gc.GetPointerY(0) - player_h/2;
			}

			// プレイヤーとボールの跳ね返り判定
			if(gc.CheckHitRect(ball_x,ball_y,24,24,player_x,player_y,player_w,player_h)){
				if(ball_speed_y > 0){
					ball_speed_y = -ball_speed_y;
				}
			}

			// ブロックとボールの跳ね返り判定
			for(int i=0; i<BLOCK_NUM; i++){
				if(block_alive_flag[i]){ // ブロックが生きているかどうか
					if(gc.CheckHitRect(ball_x,ball_y,24,24,block_x[i],block_y[i],block_w,block_h)){
						block_alive_flag[i] = false;
						if(ball_speed_y < 0){
							ball_speed_y = -ball_speed_y;
						}
					}
				}
			}
		}
	}

	/// <summary>
	/// 描画の処理
	/// </summary>
	public override void DrawGame(){
		// 画面を白で塗りつぶします
		gc.ClearScreen();
		// 0番の画像を描画します
		gc.DrawImage(GcImage.BlueSky, 0, 0);

		gc.SetColor(0,0,0);
		gc.SetFontSize(24);//文字の大きさを指定します。
		gc.DrawString("Time: " + time, 10, 446);

		if(stage_phase == 2){ // クリア画面
			gc.SetColor(200,255,200);
			gc.SetFontSize(100);//文字の大きさを指定します。
			gc.DrawString("Game Clear", 100, 100, 32);
		}else if(stage_phase==0 || stage_phase==1){// ゲーム画面
			// ボールの描画
			gc.DrawImage(GcImage.BallYellow,ball_x,ball_y);

			// プレイヤーの描画
			gc.SetColor(0,0,255);
			gc.FillRect(player_x,player_y,player_w,player_h);

			// ブロックの描画
			for(int i=0; i<BLOCK_NUM; i++){
				if(block_alive_flag[i]){
					gc.SetColor(255,0,0);
					gc.FillRect(block_x[i],block_y[i],block_w,block_h);
				}
			}
		}
	}

	void resetValue(){
		ball_x = 308;
		ball_y = 350;
		ball_speed_x = 3;
		ball_speed_y = 3;

		player_x = 270;
		player_y = 460;
		player_w = 100;
		player_h = 20;

		for(int i=0; i<BLOCK_NUM ; i++){
			block_x[i] = 7 + (i%10) * (block_w + block_interval);
			block_y[i] = 7 + (i/10) * (block_h + block_interval);
			block_alive_flag[i] = true;
		}
		time = 0;
	}

	int countBlock(){
		int num = 0;
		for(int i=0; i<BLOCK_NUM; i++){
			if(block_alive_flag[i]){
				num++;
			}
		}
		return num;
	}
}
