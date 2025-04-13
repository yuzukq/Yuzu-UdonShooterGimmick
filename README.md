# VRChat Yuzu-UdonShooter-DemoYuzu-UdonShooter-Demo

このリポジトリは、VRChat向けに作成した自作ゲームワールドの一部ソースコードを公開しています。ワールド内のギミックやスクリプトの実装について、アドバイスや改善点のフィードバックをいただけると幸いです。また、VRChatのワールド制作に携わる方々の参考になれば嬉しく思います。

## プロジェクト概要

このプロジェクトは、VRChatのユーザーが自分のワールドに独自のギミックを追加するための参考資料として設計されています。参考までに以下の機能を含んでいます：

- mathfを用いた物理ハンドルによるオブジェクトのスケーリング
- パーティクルガンの衝突検知
- ローカルプレイヤーのボーンからの相対位置依存のUI,オブジェクトの固定
- スコア変数の同期，衝突判定関数の同期処理(グローバル化)

## 使用技術

- **UdonSharp**: VRChat world APIを使用するためのC#ライブラリ
- **Unity**: VRChatワールドの開発に使用されるゲームエンジン



## 参考リンク

- [ワールドリンク](https://vrchat.com/home/launch?worldId=wrld_18bd5d2c-7dc3-40c2-8a70-0e3227b88575)
- [UdonSharp ドキュメント](https://udonsharp.docs.vrchat.com/udonsharp)


