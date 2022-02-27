# RpTimerDemo

[Question] How should I dispose ReactiveCommand when using WithSubscribe method?

**Œ»Û**

`ReactiveCommand.WithSubscribe()` ‚ðŽg—p‚µ‚½ê‡Aw“Ç‚µ‚½ `Action` ‚Í”jŠü‚Ì‘ÎÛ‚Æ‚È‚è‚Ü‚·‚ªAŒ³ƒCƒ“ƒXƒ^ƒ“ƒX‚Ì `ReactiveCommand` ‚Í”jŠü‚Ì‘ÎÛ‚Æ‚È‚è‚Ü‚¹‚ñB [ReactiveCommand.WithSubscribe()](https://github.com/runceel/ReactiveProperty/blob/8ff5df8f211576d638c92a3326886ee891262e52/Source/ReactiveProperty.NETStandard/ReactiveCommand.cs#L213)

‚»‚Ì‚½‚ßˆÈ‰º‚ÌƒR[ƒh‚Å‚Í `Dispose()` Œã‚à Timer ‚ª”jŠü‚³‚ê‚¸A `ReactiveCommand.CanExecute()` ‚ÌXV‚ªŽ~‚Ü‚è‚Ü‚¹‚ñB

```C#
// Timer ‚Í”jŠü‚³‚ê‚Ü‚¹‚ñB (Dispose Œã‚à CanExecute ‚ÌXV‚ªŽ~‚Ü‚è‚Ü‚¹‚ñ)
TimerCommand = Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
    .Select(x => (x & 1) == 0)
    .ToReactiveCommand()
    .WithSubscribe(() => Message.Value = $"Clicked!", _disposables.Add);
```

**’²¸Œ‹‰Ê**

`WithSubscribe()` Žž‚É `ReactiveCommand` –{‘Ì‚à `Disposer` ‚É“o˜^‚³‚ê‚éiTimer ‚à”jŠü‚³‚ê‚éj‚ÆŽv‚¢ž‚ñ‚Å‚¢‚Ü‚µ‚½‚ªA‚»‚¤‚Å‚Í‚È‚©‚Á‚½‚Å‚·?

**•â‘«**

ŒÂ•Ê‚É”jŠü“o˜^‚·‚é‚æ‚¤‚É‹C‚ð•t‚¯‚ÄŽÀ‘•‚·‚ê‚Î–â‘è‚ ‚è‚Ü‚¹‚ñB

```C#
// ‰ñ”ðô1: ToReactiveCommand().WithSubscribe() ‚ÌŒã‚ÉARC ‚ð Disposer ‚É“o˜^‚·‚éB
TimerCommand = Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
    .Select(x => (x & 1) == 0)
    .ToReactiveCommand()
    .WithSubscribe(() => Message.Value = $"Clicked!", _disposables.Add);
    .AddTo(_disposables);	// Ÿ‚±‚±‚ð’Ç‰Á
```

```C#
// ‰ñ”ðô2: ToReactiveCommand ‚Æ WithSubscribe ‚ð•ª‚¯‚ÄAŒÂ•Ê‚É Disposer ‚É“o˜^‚·‚éB
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
