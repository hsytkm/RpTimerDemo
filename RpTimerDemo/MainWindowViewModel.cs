using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace RpTimerDemo;

internal sealed class MainWindowViewModel : INotifyPropertyChanged, IDisposable
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private readonly CompositeDisposable _disposables = new();

    public ReactiveCommand TimerCommand { get; }
    public IReactiveProperty<long> Counter { get; }
    public IReactiveProperty<string> Message { get; }

    public MainWindowViewModel()
    {
        Counter = new ReactivePropertySlim<long>().AddTo(_disposables);
        Message = new ReactivePropertySlim<string>("").AddTo(_disposables);

        // 現象：ToReactiveCommand().WithSubscribe() 使用時の ReactiveCommand 本体の破棄について教えてください。
        //      以下の実装だと Timer は破棄されません。(本VM の Dispose 後も ReactiveCommand.CanExecute 更新が止まりません)
        //      破棄を意識して WithSubscribe を使用していますが、Dispose の対象は ReactiveCommand.Subscribe のみです。
        // 調査結果：WithSubscribe() 時に ReactiveCommand 本体も Dispose に登録すれば破棄できけど、そのようになってない。
        TimerCommand = Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
            .Do(x => Counter.Value = x)     // ホントは Do() に意味を持たせちゃダメ
            .Select(x => (x & 1) == 0)
            .ToReactiveCommand()
            .WithSubscribe(() => Message.Value = $"Clicked {Counter.Value}.", _disposables.Add);
            //.AddTo(_disposables);     ◆ReactiveCommand自体(Timer)の破棄にはこれが必要！
    }

    public void Dispose() => _disposables.Dispose();
}
