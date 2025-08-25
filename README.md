# NSequence

[![license](https://img.shields.io/badge/LICENSE-MIT-green.svg)](LICENSE)

## 概要


## 特徴

#### 基本要素
**IProcess** :
```cs
public interface IProcess {
    UniTask RunAsync(PausableToken pausableToken, CancellationToken cancellationToken = default);
}
```

**Swquence** :
```cs
public sealed class Sequence : IProcess {
    private IEnumerable<IProcess> _processes;

    public Sequence(IEnumerable<IProcess> processes) {
        _processes = processes ?? throw new ArgumentNullException(nameof(processes));
    }

    public async UniTask RunAsync(PausableToken pausableToken, CancellationToken cancellationToken){
        foreach(var process in _processes) {
            // ポーズ待機
            await pausableToken.WaitWhilePaused(cancellationToken);
            // 子プロセス
            await process.RunAsync(pausableToken, cancellationToken);
        }
    }
}
```


## セットアップ
#### 要件 / 開発環境
- Unity 6000.0

#### インストール

1. Window > Package ManagerからPackage Managerを開く
2. 「+」ボタン > Add package from git URL
3. 以下のURLを入力する
```
https:/github.com/nitou-kanazawa/lib-unity-NSequence.git?path=<パッケージパス>
```

あるいはPackages/manifest.jsonを開き、dependenciesブロックに以下を追記
```
{
    "dependencies": {
        "com.annulusgames.lit-motion": "https://github.com/nitou-kanazawa/lib-unity-NSequence.git?path=<パッケージパス>"
    }
}
```


## ドキュメント
