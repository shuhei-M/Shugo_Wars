# shuhei-M
  

  
## ソースファイル
| ソースファイル | 概要 | 備考 |
| --- | --- | --- |
| ▼[Behaviourフォルダ](https://github.com/shuhei-M/Shugo_Wars/blob/main/VR_Shugo_Wars/Assets/Scripts/Behaviour) |  |  |
| [DeskBehaviour.cs.cs](https://github.com/shuhei-M/Shugo_Wars/blob/main/VR_Shugo_Wars/Assets/Scripts/Behaviour/DeskBehaviour.cs) | 姫が設置しているかどうか判定する。机より下に落ちた場合、PrincessBehaviorのRespawn関数を呼ぶ。 |  |
| [ExpItemBehavior.cs](https://github.com/shuhei-M/Shugo_Wars/blob/main/VR_Shugo_Wars/Assets/Scripts/Behaviour/ExpItemBehavior.cs) | 経験値獲得アイテム。掴み可能オブジェクト。姫に触れると経験値を獲得させ、エフェクトを発生させてから消滅する。姫に吸い寄せられる機能を実装予定。 |  |
| [ExpItemSensorBehaviour.cs](https://github.com/shuhei-M/Shugo_Wars/blob/main/VR_Shugo_Wars/Assets/Scripts/Behaviour/ExpItemSensorBehaviour.cs) | 姫に引き寄せられるかどうかの検知用のクラス。<br>検知範囲に姫がいるかどうかのプロパティを持ち、親オブジェクトのExpItemBehaviorクラスから参照される。 |  |
| [GrabBehaviour.cs](https://github.com/shuhei-M/Shugo_Wars/blob/main/VR_Shugo_Wars/Assets/Scripts/Behaviour/GrabBehaviour.cs) | ものを掴むためのクラス。人差し指の先端についているスフィア型のセンサー。掴まれたものの座標の変更も行う。  摘まめるオブジェクトに必ず継承させるインターフェイスも定義。 |  |
| [HealItemBehaviour.cs](https://github.com/shuhei-M/Shugo_Wars/blob/main/VR_Shugo_Wars/Assets/Scripts/Behaviour/HealItemBehaviour.cs) | 姫回復用アイテム。掴み可能オブジェクト。姫に触れるとライフを回復させ、エフェクトを発生させてから消滅する。 |  |
| [PrincessBehaviour.cs](https://github.com/shuhei-M/Shugo_Wars/blob/main/VR_Shugo_Wars/Assets/Scripts/Behaviour/PrincessBehaviour.cs) | 姫の挙動を管理する。パーシャルクラス。 |  |
| [Princess_State.cs](https://github.com/shuhei-M/Shugo_Wars/blob/main/VR_Shugo_Wars/Assets/Scripts/Behaviour/Princess_State.cs) | 姫の各ステートを設定する。PrincessBehaviourのパーシャルクラス。|  |
| [RideAreaBehaviour.cs](https://github.com/shuhei-M/Shugo_Wars/blob/main/VR_Shugo_Wars/Assets/Scripts/Behaviour/RideAreaBehaviour.cs) | 姫が手に乗ったかどうか判定する。 |  |
| ▼[Controllerフォルダ](https://github.com/shuhei-M/Shugo_Wars/tree/main/VR_Shugo_Wars/Assets/Scripts/Controller) |  |  |
| [GameModeController.cs](https://github.com/shuhei-M/Shugo_Wars/blob/main/VR_Shugo_Wars/Assets/Scripts/Controller/GameModeController.cs) | ゲームの進行状況（状態）を記録・管理するためのシングルトンクラス。  ゲームの経過時間の計測もここで行う。 |  |
| [Quit.cs](https://github.com/shuhei-M/Shugo_Wars/blob/main/VR_Shugo_Wars/Assets/Scripts/Controller/Quit.cs) | ゲーム内でEscapキーを押すと、強制終了させる。 |  |
| ▼[Editorフォルダ](https://github.com/shuhei-M/Shugo_Wars/blob/main/VR_Shugo_Wars/Assets/Scripts/Editor) |  |  |
| [FindReferenceAsset.cs](https://github.com/shuhei-M/Shugo_Wars/blob/main/VR_Shugo_Wars/Assets/Scripts/Editor/FindReferenceAsset.cs) | Editor拡張。オブジェクトの参照を確認する。オブジェクトを右クリックし、そこから「参照を探す」を選択。0個であれば、そのオブジェクトをプロジェクトファイルから削除することを検討する。 |  |
| ▼[Effectフォルダ](https://github.com/shuhei-M/Shugo_Wars/tree/main/VR_Shugo_Wars/Assets/Scripts/Effect) |  |  |
| [HealEffect.cs](https://github.com/shuhei-M/Shugo_Wars/blob/main/VR_Shugo_Wars/Assets/Scripts/Effect/HealEffect.cs) | エフェクトそれぞれにアタッチする。発生させたエフェクトを、0.5秒後に消滅させる。 |  |
| ▼[Managerフォルダ](https://github.com/shuhei-M/Shugo_Wars/tree/main/VR_Shugo_Wars/Assets/Scripts/Manager) |  |  |
| [EffectManager.cs](https://github.com/shuhei-M/Shugo_Wars/tree/main/VR_Shugo_Wars/Assets/Scripts/Manager/EffectManager.cs) | エフェクトを発生させるシングルトンクラス。アイテム消滅時、敵撃破時などに使用。様々なクラスから使用しやすい様に設計。 |  |
| [ItemManager.cs](https://github.com/shuhei-M/Shugo_Wars/tree/main/VR_Shugo_Wars/Assets/Scripts/Manager/ItemManager.cs) | アイテムを生成するシングルトンクラス。プレイヤーのライフや、敵の撃破数に応じて何のアイテムをドロップするか決定する。敵撃破時に、倒された敵クラスでItemManagerクラスの生成関数を呼ぶ。 |  |
| ▼[UIフォルダ](https://github.com/shuhei-M/Shugo_Wars/tree/main/VR_Shugo_Wars/Assets/Scripts/UI) |  |  |
| [UI_Manager.cs](https://github.com/shuhei-M/Shugo_Wars/blob/main/VR_Shugo_Wars/Assets/Scripts/UI/UI_Manager.cs) | UIをコントロールする変数・関数群を収めたクラス。仕様によっては変更・削除される可能性あり。 |  |
| ▼[Utilityフォルダ](https://github.com/shuhei-M/Shugo_Wars/tree/main/VR_Shugo_Wars/Assets/Scripts/Utility) |  |  |
| [Interfaces.cs](https://github.com/shuhei-M/Shugo_Wars/blob/main/VR_Shugo_Wars/Assets/Scripts/Utility/Interfaces.cs) | 摘まめるオブジェクトや、戦闘可能なオブジェクトなどに継承させる、インターフェイスを集めた.csファイル。 |  |
| [SingletonMonobehavior.cs.cs](https://github.com/shuhei-M/Shugo_Wars/blob/main/VR_Shugo_Wars/Assets/Scripts/Utility/SingletonMonobehavior.cs) | ゲームモードなどに使用するシングルトンパターン。ジェネリッククラス。 |  |
| [StateMachine.cs](https://github.com/shuhei-M/Shugo_Wars/blob/main/VR_Shugo_Wars/Assets/Scripts/Utility/StateMachine.cs) | 姫などのAIを作成するためのステートマシン。ジェネリッククラス。 |  |

<!-- 
| [.cs]() |  |
| [ソースファイル名](プロジェクトに保存されているファイル名) | 説明文 |
上の文を4行目以降にコピペしてもらって内容書き換えれば表になります
↓例
| [PrincessBehaviour.cs](https://github.com/shuhei-M/Shugo_Wars/blob/main/VR_Shugo_Wars/Assets/Scripts/Behaviour/PrincessBehaviour.cs) | 姫の挙動を管理する。 |
====================================================================
-->
