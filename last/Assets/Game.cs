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
	int start_time;
	int score;

	int stage_phase; // 0:スタート画面 1:ゲーム画面 2:クリア画面
	int stage1_last_time; // 長押しになってしまって2に入ってもすぐリスタートしてしまうのを防ぐため。
	int stage2_last_time;

	bool start_flag;
	bool restart_flag;

	bool isComplete;

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
		time++;

		if(stage_phase == 0){
			if(time-stage2_last_time >= 120){ // スタート画面に入ってから2秒以上経っていたらスタート可能
				start_flag = true;
				if(gc.GetPointerFrameCount(0)>0){
					stage_phase = 1;
					start_time = time;
				}
			}else{
				start_flag = false;
			}
		}else if(stage_phase == 1){
			if(countBlock()==0){
				stage_phase = 2;
				stage1_last_time = time;
				isComplete = true;
			}
		}else if(stage_phase==2){
			if(time-stage1_last_time >= 120){
				// クリア画面に入ってから2秒以上経ったらリスタート可能
				restart_flag = true;
				if(gc.GetPointerFrameCount(0)>=120){
					resetValue();
					stage_phase = 0;
					stage2_last_time = time;
				}
			}else{
				restart_flag = false;
			}
		}

		if(stage_phase == 1){
			//スコア計算
			score = time - start_time;

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
				// 下に行ったらリセット
				isComplete = false;
				stage_phase = 2; // ゲームオーバー
				stage1_last_time = time;
				score = -999;
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
		gc.DrawImage(GcImage.Universe, 0, 0);

		gc.SetColor(250,250,250);
		gc.SetFontSize(24);//文字の大きさを指定します。
		gc.DrawString("Score: " + score, 10, 446);


		if(stage_phase==0){	// スタート画面
			gc.SetColor(250,250,250);
			gc.SetFontSize(100);//文字の大きさを指定します。
			gc.DrawString("Blocks", 100, 100);
			gc.SetFontSize(24);
			if(start_flag){
				gc.DrawString("Touch to Start", 200, 200);
			}
		}else if(stage_phase==1){// ゲーム画面
			// ボールの描画
			gc.DrawImage(GcImage.BallYellow,ball_x,ball_y);

			// プレイヤーの描画
			gc.SetColor(30,100,150);
			gc.FillRect(player_x,player_y,player_w,player_h);

			// ブロックの描画
			gc.SetColor(250,30,90);
			for(int i=0; i<BLOCK_NUM; i++){
				if(block_alive_flag[i]){
					gc.FillRect(block_x[i],block_y[i],block_w,block_h);
				}
			}
		}else if(stage_phase == 2){ // クリア画面
			gc.SetColor(100,200,240);
			gc.SetFontSize(100);//文字の大きさを指定します。
			if(isComplete){
				gc.DrawString("Game Clear", 200, 100, 32);
			}else{
				gc.DrawString("Game Over", 200, 100, 32);
			}
			if(restart_flag){
				gc.SetFontSize(32);
				gc.DrawString("Hold to Restart", 100, 150, 32);
			}
		}
	}



	void resetValue(){
		ball_x = 308;
		ball_y = 150;
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
		start_time=0;
		score=0;

		isComplete = false;
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
