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

	/// <summary>
	/// 初期化処理
	/// </summary>
	public override void InitGame(){
		gc.SetResolution(720,1280);
		gc.IsAccelerometerEnabled = true;
	}

	/// <summary>
	/// 動きなどの更新処理
	/// </summary>
	public override void UpdateGame(){
		// 起動からの経過時間を取得します
		sec = (int)gc.TimeSinceStartup;

	}

	/// <summary>
	/// 描画の処理
	/// </summary>
	public override void DrawGame(){
		gc.ClearScreen();
		gc.SetColor(0, 0, 0);
		gc.SetFontSize(36);
		gc.DrawString("AcceX:"+gc.AccelerationLastX,0,0);
		gc.DrawString("AcceY:"+gc.AccelerationLastY,0,40);
		gc.DrawString("AcceZ:"+gc.AccelerationLastZ,0,80);
	}
}