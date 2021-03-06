# RpTimerDemo

[Question] How should I dispose ReactiveCommand when using WithSubscribe method?

**現象**

`ReactiveCommand.WithSubscribe()` を使用した場合、購読した `Action` は破棄の対象となりますが、元インスタンスの `ReactiveCommand` は破棄の対象となりません。 [ReactiveCommand.WithSubscribe()](https://github.com/runceel/ReactiveProperty/blob/8ff5df8f211576d638c92a3326886ee891262e52/Source/ReactiveProperty.NETStandard/ReactiveCommand.cs#L213)

そのため以下のコードでは `Dispose()` 後も Timer が破棄されず、 `ReactiveCommand.CanExecute()` の更新が止まりません。

```C#
// Timer は破棄されません。 (Dispose 後も CanExecute の更新が止まりません)
TimerCommand = Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
    .Select(x => (x & 1) == 0)
    .ToReactiveCommand()
    .WithSubscribe(() => Message.Value = $"Clicked!", _disposables.Add);
```

**調査結果**

`WithSubscribe()` 時に `ReactiveCommand` 本体も `Disposer` に登録される（Timer も破棄される）と思い込んでいましたが、そうではなかったです?

**補足**

個別に破棄登録するように気を付けて実装すれば問題ありません。

```C#
// 回避策1: ToReactiveCommand().WithSubscribe() の後に、RC を Disposer に登録する。
TimerCommand = Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
    .Select(x => (x & 1) == 0)
    .ToReactiveCommand()
    .WithSubscribe(() => Message.Value = $"Clicked!", _disposables.Add);
    .AddTo(_disposables);	// ◆ここを追加
```

```C#
// 回避策2: ToReactiveCommand と WithSubscribe を分けて、個別に Disposer に登録する。
TimerCommand = Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
    .Select(x => (x & 1) == 0)
    .ToReactiveCommand()
    .AddTo(_disposables);

TimerCommand.WithSubscribe(() => Message.Value = $"Clicked!", _disposables.Add);
```

## Environment

VisualStudio 2022 Version17.2.0 Preview 1.0

.NET 6.0 + C# 10 + WPF

ReactiveProperty 8.0.3
