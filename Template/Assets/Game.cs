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

	}
}