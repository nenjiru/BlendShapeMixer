# Blend Shape Mixer

複数のブレンドシェイプを組み合わせたプリセットを作成し、合成するスクリプトです。  
例えば「笑顔」と「怒り顔」を合成して「笑顔まじりの怒り顔」を表現することができます。

![blending](https://user-images.githubusercontent.com/437445/63213171-779a8e80-c146-11e9-8bcf-8f16b8fd3d18.png)

[ダイレクト・ブレンディング](https://docs.unity3d.com/ja/current/Manual/BlendTree-DirectBlending.html) でも同じことができますが、なにかしらの事情で Animator Controller を使えないようなケースで有用です。 

## デモシーンについて
リポジトリ内には、Unityちゃん（[© UTJ/UCL](http://unity-chan.com/contents/license_jp/)）の表情を制御するデモが含まれています。キーボードの1、2、3キーでそれぞれの表情へ変化し、同時押しでブレンドすることができます。  

## 使い方

### コンポーネントをアタッチ
適当なオブジェクトに `BlendShapeMixer.cs` をアタッチし、対象のモデルを `MeshRoot` にアサインします  
![attach component](https://user-images.githubusercontent.com/437445/63249108-bb56da80-c2a3-11e9-9ec0-bf0bc2a15f48.png)

### プリセットを作成
作成するプリセットの数と任意のプリセット名を入力  
![preset name](https://user-images.githubusercontent.com/437445/63249134-c873c980-c2a3-11e9-90bf-2a37a5d4b4ce.png)

ヒエラルキー上の BlendShape を調整し、`Capture` ボタンでシェイプを取り込む  
![capture](https://user-images.githubusercontent.com/437445/63218971-d68df100-c1a2-11e9-8886-767175d24c47.png)

この手順を繰り返しいくつかのプリセットを作成します   
![create preset](https://user-images.githubusercontent.com/437445/63218974-e3aae000-c1a2-11e9-887e-c660d1bd20f5.png)


### プリセットを適用する

スライダー、または数値を入力  
![slider](https://user-images.githubusercontent.com/437445/63219678-fc6ec200-c1b1-11e9-82b3-16480f098bf1.png)  
_*組み合わせでシェイプが崩れる場合は、プリセット側のウェイトを調整する_
  
`Clear Weight` ボタンでモデルにセットされた BlendShape のウェイトをすべて初期化  
![clear button](https://user-images.githubusercontent.com/437445/63219687-38098c00-c1b2-11e9-82fc-28fe1600115a.png)  
_*プリセットにない BlendShape はリセットされません_

### UI説明

![editor buttons](https://user-images.githubusercontent.com/437445/63218983-1654d880-c1a3-11e9-833d-4f42958a90af.png)  
- Capture  
  ヒエラルキー上の BlendShape をプリセットとして取り込み
- Apply  
  プリセットをヒエラルキー上の BlendShape へ反映
- Duplicate  
  プリセットの複製を作成
- Remove  
  プリセットを削除

![editor mesh handler](https://user-images.githubusercontent.com/437445/63218990-266cb800-c1a3-11e9-9fe3-510c6f6d8178.png)  
- ドロップダウンリスト  
  操作対象の BlendShape を変更
- 数値フィールド
  BlendShape のウェイト値を調整


## Author
[Minoru Nakanou](https://github.com/nenjiru)  
[@nenjiru](https://twitter.com/nenjiru)

## License
[MIT](https://mit-license.org/)
