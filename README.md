# RpTimerDemo

[Question] How should I dispose ReactiveCommand when using WithSubscribe method?

**����**

`ReactiveCommand.WithSubscribe()` ���g�p�����ꍇ�A�w�ǂ��� `Action` �͔j���̑ΏۂƂȂ�܂����A���C���X�^���X�� `ReactiveCommand` �͔j���̑ΏۂƂȂ�܂���B [ReactiveCommand.WithSubscribe()](https://github.com/runceel/ReactiveProperty/blob/8ff5df8f211576d638c92a3326886ee891262e52/Source/ReactiveProperty.NETStandard/ReactiveCommand.cs#L213)

���̂��߈ȉ��̃R�[�h�ł� `Dispose()` ��� Timer ���j�����ꂸ�A `ReactiveCommand.CanExecute()` �̍X�V���~�܂�܂���B

```C#
// Timer �͔j������܂���B (Dispose ��� CanExecute �̍X�V���~�܂�܂���)
TimerCommand = Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
    .Select(x => (x & 1) == 0)
    .ToReactiveCommand()
    .WithSubscribe(() => Message.Value = $"Clicked!", _disposables.Add);
```

**��������**

`WithSubscribe()` ���� `ReactiveCommand` �{�̂� `Disposer` �ɓo�^�����iTimer ���j�������j�Ǝv������ł��܂������A�����ł͂Ȃ������ł�?

**�⑫**

�ʂɔj���o�^����悤�ɋC��t���Ď�������Ζ�肠��܂���B

```C#
// �����1: ToReactiveCommand().WithSubscribe() �̌�ɁARC �� Disposer �ɓo�^����B
TimerCommand = Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
    .Select(x => (x & 1) == 0)
    .ToReactiveCommand()
    .WithSubscribe(() => Message.Value = $"Clicked!", _disposables.Add);
    .AddTo(_disposables);	// ��������ǉ�
```

```C#
// �����2: ToReactiveCommand �� WithSubscribe �𕪂��āA�ʂ� Disposer �ɓo�^����B
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
