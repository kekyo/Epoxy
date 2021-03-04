# Epoxy - An independent flexible XAML MVVM library for .NET

![Epoxy bin](Images/Epoxy.160.png)

[English language is here](https://github.com/kekyo/Epoxy)

[![Project Status: WIP – Initial development is in progress, usable release suitable for the public.](https://www.repostatus.org/badges/latest/wip.svg)](https://www.repostatus.org/#wip)

|Package|All (C#)|Description|
|:--|:--|:--|
|Epoxy.Wpf|[![NuGet Epoxy.Wpf](https://img.shields.io/nuget/v/Epoxy.Wpf.svg?style=flat)](https://www.nuget.org/packages/Epoxy.Wpf)|WPF version|
|Epoxy.Xamarin.Forms|[![NuGet Epoxy.Xamarin.Forms](https://img.shields.io/nuget/v/Epoxy.Xamarin.Forms.svg?style=flat)](https://www.nuget.org/packages/Epoxy.Xamarin.Forms)|Xamarin Forms version|
|Epoxy.Avalonia|[![NuGet Epoxy.Avalonia](https://img.shields.io/nuget/v/Epoxy.Avalonia.svg?style=flat)](https://www.nuget.org/packages/Epoxy.Avalonia)|Avalonia version|
|Epoxy.Uwp|[![NuGet Epoxy.Uwp](https://img.shields.io/nuget/v/Epoxy.Uwp.svg?style=flat)](https://www.nuget.org/packages/Epoxy.Uwp)|Universal Windows version|
|Epoxy.WinUI|[![NuGet Epoxy.WinUI](https://img.shields.io/nuget/v/Epoxy.WinUI.svg?style=flat)](https://www.nuget.org/packages/Epoxy.WinUI)|WinUI 3 version (Broken?)|
|Epoxy.Uno|[![NuGet Epoxy.Uno](https://img.shields.io/nuget/v/Epoxy.Uno.svg?style=flat)](https://www.nuget.org/packages/Epoxy.Uno)|Uno platform version (**BUGGY**)|

|Package|F# specialized|Description|
|:--|:--|:--|
|FSharp.Epoxy.Wpf|[![NuGet FSharp.Epoxy.Wpf](https://img.shields.io/nuget/v/FSharp.Epoxy.Wpf.svg?style=flat)](https://www.nuget.org/packages/FSharp.Epoxy.Wpf)|WPF version|
|FSharp.Epoxy.Avalonia|[![NuGet FSharp.Epoxy.Avalonia](https://img.shields.io/nuget/v/FSharp.Epoxy.Avalonia.svg?style=flat)](https://www.nuget.org/packages/FSharp.Epoxy.Avalonia)|Avalonia version|

## これは何?

* Epoxyは、.NET XAML環境で使える、Model-View-ViewModel (MVVM) アーキテクチャ向けの、独立した柔軟性のあるライブラリです。
  * C#を含む.NETの全処理系向け、及びF#用のNuGetパッケージがあります。
* 以下の環境をサポートしています:
  * WPF: .NET 5/.NET Core 3.0/3.1, .NET Framework 4.5/4.8
  * Xamarin Forms: [Xamarin Forms](https://github.com/xamarin/Xamarin.Forms) (4.8.0.1821)
  * Avalonia: [Avalonia](https://avaloniaui.net/) (0.10.0)
  * Universal Windows: Universal Windows 10 (Fall creators update 10.0.16299 or higher)
  * WinUI: [WinUI 3 preview 4](https://docs.microsoft.com/ja-jp/windows/apps/winui/winui3/) (windows3.0.0-preview4.210210.4, 10.0.17134.0 or upper, [但し、このissueと同じ問題で実行時エラーが発生する可能性があります](https://github.com/microsoft/microsoft-ui-xaml/issues/4226))
  * Uno: [Uno platform](https://platform.uno/) (uap10.0.17763, netstandard2.0[wpf, wasm, tizen], xamarinios10, xamarinmac20 and monoandroid10.0) / **Unoは安定していないため、検証したのはUWPホストのみです**
* 非同期処理 (async-await) を安全に書くことが出来るように配慮しています。
* C# 8.0でサポートされた、null許容参照型を使えます。
* F#は5.0に対応しています。F#向けのシグネチャ (camel case functions・関数型・Async型前提) が定義されています。
* 小さなライブラリで、理解しやすいAPIです。
  * プラットフォーム標準以外のフレームワークやライブラリに依存していません。
* 大げさにならない、最小の手間とコストで Model-View-ViewModel 設計を実現します。
  * Viewにコードビハインドを書かずに済むことが着地点ですが、そのために煩雑な処理を記述しなければならなくなる事を避ける方針です。
  * MVVMビギナーが躓きそうな部分に焦点を当てています。
  * 完全な共通化は行いません。Epoxyについてだけ同じように記述可能にし、その他の部分はそれぞれの環境に依存させることで、最大公約数的にならないようにしています。
  * それぞれの機能が、相互に関係「しません」。独立しているので、自由に組み合わせることが出来ます。
* ほかのフレームワークライブラリ(例: ReactiveProperty)と組み合わせて使えるように、余計な操作や暗黙の前提を排除しています。

### 解説動画があります (YouTube, 日本語のみ):

[![Epoxyで C# MVVMアーキテクチャを簡単に実装する話 - 作ってみた 第一回](https://img.youtube.com/vi/LkyrgJbuiQs/0.jpg)](https://www.youtube.com/watch?v=LkyrgJbuiQs)

[(再生出来ない場合はこちら)](https://www.youtube.com/watch?v=LkyrgJbuiQs)


## サンプルコード

様々な環境の実働サンプルがあります。
このサンプルは、Reddit掲示板のr/awwから、最新の投稿記事と画像を非同期でダウンロードしながら、
リスト形式で表示するものです。

### サンプルコードの入手とビルド方法

.NET CLIテンプレートに対応しています。以下のようなコマンドで、簡単にサンプルコードをクリーンな状態で試すことができます:

```bash
# テンプレートパッケージをインストール（1度だけでOK）
dotnet new -i Epoxy.Templates

# 現在のディレクトリにWPFサンプルコードを展開
dotnet new epoxy-wpf

# ビルド
dotnet build
```

### 現在サポートしているテンプレート一覧

|`dotnet new`引数|言語|対象|
|:--|:--|:--|
|`epoxy-wpf`|C#, F#|WPFのサンプルコード|
|`epoxy-uwp`|C#|UWPのサンプルコード|
|`epoxy-xamarin-forms`|C#|Xamarin Formsのサンプルコード|
|`epoxy-avalonia`|C#, F#|Avaloniaのサンプルコード|
|`epoxy-winui`|C#|WinUIのサンプルコード|

* デフォルトではC#のサンプルコードが展開されます。F#にする場合は、`dotnet new epoxy-wpf -lang F#`のように、オプションをコマンドラインに加えます。
* 現在、WinUIはpreview版制限のために、正しく動作しない可能性があります。
* Uno platformのテンプレートはまだ用意していません。

### サンプルコードの解説

起動後にボタンをクリックすると、完全に非同期でダウンロードしながら、リストに結果を追加していきます。

![EpoxyHello.Wpf](https://github.com/kekyo/Epoxy/raw/main/Images/sample.Wpf.png)

![EpoxyHello.Xamarin.Forms](https://github.com/kekyo/Epoxy/raw/main/Images/sample.Xamarin.Forms.png)


---

## MVVMアプリケーションの実装を、最小限の手間で始める

Model-View-ViewModelの役割についてのおさらい:

* `View`: XAMLでユーザーインターフェイスを記述し、`ViewModel`とバインディングする（コードビハインドを書かない）。
* `ViewModel`: `Model`から情報を取得して、`View`にマッピングするプロパティを定義する。
* `Model`: ユーザーインターフェイスに直接関係の無い処理を実装。ここではRedditから投稿をダウンロードする処理。

以下にこれらのMVVM要素の関係を図示します:

![MVVMアーキテクチャ](Images/diagram.png)

注意: MVVMの役割については諸説あります。
はじめから完全な設計を目指さずに、ブラッシュアップすると良いでしょう。
Epoxyは段階的に改善する事を想定して開発しています。

XAMLビューの定義とその実装を、MVVMに従って完全に分離しつつ、最小限の手間で実装する例です
(このコードはWPFの例で、ポイントとなる点に絞っているため、完全な例はサンプルコードを参照して下さい):

### View (WPF XAML)の実装例

```xml
<Window
    x:Class="EpoxyHello.Wpf.Views.MainWindow"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    viewmodels="clr-namespace:EpoxyHello.Wpf.ViewModels"
    Title="EpoxyHello.Wpf" Height="450" Width="800">

    <!-- ここに、ViewModelクラスのインスタンスを配置します。この定義によって、IDEがViewModelの型を認識できます。 -->
    <Window.DataContext>
        <viewmodels:MainWindowViewModel />
    </Window.DataContext>
    
    <DockPanel>
        <!-- ボタンのクリックイベントを、ViewModel側にバインディングで通知します。 -->
        <Button DockPanel.Dock="Top" Height="30"
                Command="{Binding Fetch}">Asynchronous fetch r/aww from Reddit!</Button>
        <Grid>
            <!-- ListBoxに、ダウンロードした記事を保持するViewModelのコレクションをバインディングします。 -->
            <ListBox ItemsSource="{Binding Items}"
                ScrollViewer.HorizontalScrollBarVisibility="Disabled"
                ScrollViewer.VerticalScrollBarVisibility="Auto"
                ScrollViewer.CanContentScroll="False">
                <ListBox.ItemTemplate>
                    <DataTemplate>
                        <!-- ダウンロードした記事のイメージを表示します。 -->
                        <Image
                            Source="{Binding Image}"
                            Stretch="UniformToFill" />
                    </DataTemplate>
                </ListBox.ItemTemplate>
            </ListBox>
        </Grid>
    </DockPanel>
</Window>
```

### ViewModel (WPF)の実装例

完全に分離された、ViewModelクラスの実装です。
完全に、とは、つまりViewクラス[(MainWindow.xaml.cs)](samples/EpoxyHello.Wpf/Views/MainWindow.xaml.cs)に、コードビハインドを一切記述しないことを指します。

```csharp
// ステップ 1: ViewModelクラスを、 Epoxy.ViewModel から継承して作ります。
public sealed class MainWindowViewModel : ViewModel
{
    // ステップ 2: XAMLから参照したいプロパティを定義します。
    //    Epoxyは、 C# 8.0 で追加された、null許容参照型定義に対応しています。
    public Command? Fetch
    {
        // ステップ 2-1: 一般的な型であれば、GetValue(), SetValue()の引数に、
        //    プロパティ名を書いたり型キャストを書く必要はありません。
        get => this.GetValue();
        private set => this.SetValue(value);
    }

    public ObservableCollection<ImageSource>? Items
    {
        // ステップ 2-2: このように、複雑な型やジェネリック型の場合にのみ、
        //    GetValue()にジェネリック型引数を与える必要があります。
        get => this.GetValue<ObservableCollection<ImageSource>?>();
        private set => this.SetValue(value);
    }

    // ViewModelのコンストラクタ
    public MainWindowViewModel()
    {
        // ステップ 3: プロパティに定義したSetValue()が呼び出されると、
        //    PropertyChangedイベントが発生して、XAML側に変更が通知されます。
        this.Items = new ObservableCollection<ItemViewModel>();

        // ステップ 4: XAMLで定義したボタンがクリックされた時に、このラムダ式が呼び出されます。
        //   この式はもちろん async-await を使用した非同期処理で書くことが出来て、
        //   未処理の例外も正しく処理されます。
        this.Fetch = CommandFactory.Create(async () =>
        {
            var reddits = await Reddit.FetchNewPostsAsync("r/aww");

            this.Items.Clear();

            foreach (var reddit in reddits)
            {
                var bitmap = new WriteableBitmap(
                    BitmapFrame.Create(new MemoryStream(await Reddit.FetchImageAsync(url))));
                bitmap.Freeze();
                this.Items.Add(bitmap);
            }
        });
    }
}
```

### Modelの実装例

Redditにアクセスする共通コードは、`EpoxyHello.Core` プロジェクトで実装しています。
このプロジェクトは、WPF・Xamarin Forms・Uno・UWPのいずれにも依存せず、完全に独立しています。

このように、依存性を排除することで、マルチプラットフォーム対応の共通化を行うことが出来ますが、
小規模な開発であれば、`Model`の実装を`ViewModel`と同じプロジェクトに配置してもかまいません
(分離しておけば、意図せず依存してしまったという失敗を排除出来ます)。

[投稿画像をダウンロードする部分 (EpoxyHello.Core)](https://github.com/kekyo/Epoxy/blob/1b16a9e447876a5e109166c7c5f5902a1dc52947/samples/EpoxyHello.Core/Models/Reddit.cs#L63)を抜粋します:

```csharp
// Modelの実装: netstandard2.0の純粋なライブラリ
// Redditから画像をダウンロードする
public static async ValueTask<byte[]> FetchImageAsync(Uri url)
{
    using (var response =
        await httpClient.GetAsync(url).ConfigureAwait(false))
    {
        using (var stream =
            await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
        {
            var ms = new MemoryStream();
            await stream.CopyToAsync(ms).ConfigureAwait(false);
            return ms.ToArray();
        }
    }
}
```

Modelの実装は、直接ユーザーインターフェイスを操作する事がないため、
非同期操作でタスクコンテキストを分離 `task.ConfigureAwait(false)` することで、
パフォーマンスを向上させることが出来ます。

### ViewModel基底クラスについて

`ViewModel`基底クラスは、`GetValue`/`SetValue`メソッドの実装を提供します。
これらのメソッドは、XAML側にプロパティの変更通知 `NotifyPropertyChanging`/`NotifyPropertyChanged` を自動的に行います。
たとえば、ボタンクリックの契機で`ViewModel`からプロパティを変更すると、変更がXAMLのコントロールに通知され、ユーザーインターフェイスに反映されます。

上記サンプルコードのコメントにあるように、`GetValue`については型引数を省略できる場合があります。
省略可能な型は、[implicit operatorの定義](https://github.com/kekyo/Epoxy/blob/1b16a9e447876a5e109166c7c5f5902a1dc52947/Epoxy/ValueHolder.cs#L61)を参照してください。

なお、`GetValue`には、デフォルト値の定義が、
`SetValue`には、値変更時に追加操作を行うことが出来るオーバーロードが定義されています。

---

## その他の有用な機能

それぞれの機能は独立しているため、自由に組み合わせて使用出来ます
（例えば、`ViewModel`を継承していないと使えない、と言うような事はありません）。

### EventBinder

`EventBinder`は、バインディング出来ないイベントが公開されている場合に、`Command`としてバインディング可能にします。
この機能により、イベントハンドラを記述するために、やむを得ずコードビハインドを書くと言う手法を回避できます。

例えば、以下のように、WPFの`Window.Loaded`イベントをバインディング出来ます:

```xml
<!-- EpoxyのXML名前空間を定義します -->
<Window xmlns:epoxy="https://github.com/kekyo/Epoxy">

    <!-- ... -->

    <epoxy:EventBinder.Events>
        <!-- Window.Loadedイベントを、ViewModelのReadyプロパティにバインディングする -->
        <epoxy:Event Name="Loaded" Command="{Binding Ready}" />
    </epoxy:EventBinder.Events>
</Window>
```

`ViewModel`側はButtonと同じように、Commandでハンドラを書くことが出来ます:

```csharp
// Loadedイベントを受信するためのCommandプロパティの定義
public Command? Ready
{
    get => this.GetValue();
    private set => this.SetValue(value);
}

// ...

// Loadedイベントが発生した場合の処理を記述
this.Ready = CommandFactory.Create<EventArgs>(async _ =>
{
    // リストに表示する情報をModelから非同期で取得
    foreach (var item in await Model.FetchInitialItemsAsync())
    {
        this.Items.Add(item);
    }
});
```

`CommandFactory.Create<T>`のジェネリック引数には、イベントの第二引数(通常EventArgsを継承したクラス)を指定します。
現在のところ、チェックを厳しくしているため、この型は必ず指定する必要があります。
但し、引数を使用しない場合や、重要でないと分かっている場合は、
上の例のように、一律`EventArgs`としておくことが可能です。

補足1: WPF,UWPやXamarin Formsでは、`Behavior`や`Trigger`で同じことを実現できますが、
WPFやUWPの場合は追加のパッケージが必要になることと、汎用的に設計されているため、やや複雑です。
`EventBinder`を使うことで、同じ記法でシンプルに記述できる利点があります。

補足2: UWP環境(UnoのUWPビルドを含む)では、対象のイベントは以下のようなシグネチャである必要があります:

```csharp
// EventBinderでバインディング可能なイベント
public event RoutedEventHandler Loaded;
```

つまり、RoutedEventHandler型で公開されているイベントだけが対象です。
UWPの実行環境はセキュリティチェックが厳しいため、
動的にイベントをフックする場合に制約が存在するためです。

* [For example (In WPF XAML)](https://github.com/kekyo/Epoxy/blob/21d16d00311f9379f0e0d431bcd856594b446cf0/samples/EpoxyHello.Wpf/Views/MainWindow.xaml#L36)
* [For example (In WPF view model)](https://github.com/kekyo/Epoxy/blob/21d16d00311f9379f0e0d431bcd856594b446cf0/samples/EpoxyHello.Wpf/ViewModels/MainWindowViewModel.cs#L45)
* [For example (In Xamarin Forms XAML)](https://github.com/kekyo/Epoxy/blob/21d16d00311f9379f0e0d431bcd856594b446cf0/samples/EpoxyHello.Xamarin.Forms/EpoxyHello.Xamarin.Forms/Views/MainPage.xaml#L33)
* [For example (In Xamarin Forms view model)](https://github.com/kekyo/Epoxy/blob/21d16d00311f9379f0e0d431bcd856594b446cf0/samples/EpoxyHello.Xamarin.Forms/EpoxyHello.Xamarin.Forms/ViewModels/MainContentPageViewModel.cs#L40)

### Anchor/Pile

`Anchor`/`Pile`は、XAMLと`ViewModel`をゆるく結合して、XAML側のコントロールの完全な操作を、一時的に可能にします。

MVVMアーキテクチャのレアケースにおいて、コントロールを直接操作したくなることがままあります。
しかし、厳密に分離された`View`と`ViewModel`では、コードビハインドを書かないことが前提となるため、
このような連携が難しくなります。
また、オブジェクト参照の管理を誤るとメモリリークにつながり、かつ、その箇所を特定するのが難しくなります。

`Anchor`/`Pile`は、コントロールへの参照を一時的にレンタルすることによって、`View`と`ViewModel`を分離しながら、
この問題を解決します。もちろん、レンタル中の処理は非同期処理対応です。

```xml
<!-- EpoxyのXML名前空間を定義します -->
<Window xmlns:epoxy="https://github.com/kekyo/Epoxy">

    <!-- ... -->

    <!-- AnchorをTextBoxに配置してバインディングします -->
    <TextBox epoxy:Anchor.Pile="{Binding LogPile}" />
</Window>
```

```csharp
// PileをViewModelに配置します。
// (操作したいTextBoxのXAMLにAnchorを配置して、バインディングします)
this.LogPile = Pile.Create<TextBox>();

// ...

// TextBoxを操作したくなったら、Pileを通じて参照をレンタルします:
await this.LogPile.ExecuteAsync(async textBox =>
{
    // モデルから情報を非同期で取得します
    var result = await ServerAccessor.GetResultTextAsync();
    // TextBoxを直接操作できます
    textBox.AppendText(result);
});
```

* [For example (In WPF XAML)](https://github.com/kekyo/Epoxy/blob/09a274bd2852cf8120347411d898aca414a16baa/samples/EpoxyHello.Wpf/Views/MainWindow.xaml#L39)
* [For example (In WPF view model)](https://github.com/kekyo/Epoxy/blob/09a274bd2852cf8120347411d898aca414a16baa/samples/EpoxyHello.Wpf/ViewModels/MainWindowViewModel.cs#L74)

### ValueConverter

`ValueConverter`クラスは、いわゆるXAMLのコンバーターを安全に実装するための基底クラスです。
型を明示的に指定することで、煩雑な型キャストを回避する事が出来、
互換性のない型については、自動的に変換を失敗させる事が出来ます。

コンバーターには`ConverterParameter`で引数を与える事が出来ますが、
このパラメータを受け取る場合と受け取らない場合で、使用する基底クラスを変えて下さい。

```csharp
// intの値を受け取り、Brush型に変換するコンバーターの実装です。
// ジェネリック引数に、想定される型を指定します。
public sealed class ScoreToBrushConverter : ValueConverter<int, Brush>
{
    // 変換の必要が生じると、TryConvertが呼び出されます。
    public override bool TryConvert(int from, out Brush result)
    {
        // 変換した結果は、out引数で返します。
        result = from >= 5 ? Brush.Red : Brush.White;
        // 変換に失敗する場合はfalseを返します。
        return true;
    }

    // ここでは例示しませんが、TryConvertBackを実装する事も出来ます。
}
```

コンバーターパラメータを受け取る例です:

```csharp
// この例では、ConverterParameterで指定された値を受け取ります。
// その型は、ジェネリック第3引数で指定します。ここでは文字列を受け取る例を示します:
public sealed class ScoreToBrushConverter : ValueConverter<int, string, Brush>
{
    // 第2引数にパラメータの値が渡されます。
    public override bool TryConvert(int from, string parameter, out Brush result)
    {
        // ...
    }
}
```

注意: XAMLコンバーターは、XAMLの構造上、非同期化出来ません。つまり、`TryConvert`メソッドを`TryConvertAsync`のように振舞わせることは出来ません。

XAMLコンバーター内で非同期処理を行わないようにしましょう
（そうしたくなった場合は、ModelやViewModel側で実装すれば、デッドロックなどのトラブルを回避できます）。

* [For example](https://github.com/kekyo/Epoxy/blob/09a274bd2852cf8120347411d898aca414a16baa/samples/EpoxyHello.Wpf/Views/Converters/ScoreToBrushConverter.cs#L25)

### UIThread

UIスレッドの取り扱いは、異なるプラットフォームにおいても重要な点です。
Epoxyでは[UIThreadクラス](https://github.com/kekyo/Epoxy/blob/09a274bd2852cf8120347411d898aca414a16baa/Epoxy/UIThread.cs#L29)で同じ操作が行えるようにしています。
また、このクラスを使うことで、UIの操作と非同期処理を簡単に組み合わせる事が出来ます。

```csharp
// 現在のスレッドがUIスレッドかどうか
Debug.Assert(UIThread.IsBound);

// ワーカースレッドで継続させる
var read = await httpStream.ReadAsync(...).ConfigureAwait(false);

// ここではワーカースレッドで処理
Console.WriteLine($"Read={read}");

// UIスレッドに切り替える
await UIThread.Bind();

// バインディングされたTextBlockに反映する
this.Log = $"Read={read}";
```

#### UWP環境で実行する場合の注意

現在の実装では、UWPネイティブや、Xamarin Forms/UnoでのUWP環境においての実行、WinUIなどのUWP由来のランタイムで
`UIThread`クラスを使う場合、`View`構築中の`ViewModel`のコンストラクタなどで使用すると、正しい結果が得られない場合があります。

UWPは、ビューを保持するウインドウ毎に異なるUIスレッドが割り当てられていて、
インスタンスを構築中に使用すると、ビューを判別できない事から、正しく判定できないためです。

### ChildrenBinder

TODO:

* [For example (In WPF XAML)](https://github.com/kekyo/Epoxy/blob/09a274bd2852cf8120347411d898aca414a16baa/samples/EpoxyHello.Wpf/Views/MainWindow.xaml#L71)
* [For example (In WPF view model)](https://github.com/kekyo/Epoxy/blob/09a274bd2852cf8120347411d898aca414a16baa/samples/EpoxyHello.Wpf/ViewModels/MainWindowViewModel.cs#L119)

### GlobalService (高度なトピック)

`GlobalService`クラスは、依存注入や依存分離といったテクニックを、Epoxy上で実現するものです。
他の機能と同様に、安全に非同期処理を実装出来ます。

依存分離を行うポイントは、共通のインターフェイス型を定義しておくことです:

```csharp
// 共通プロジェクトの、Sample.Xamarin.Formsプロジェクトで定義する

// プラットフォームに依存しないBluetooth操作の定義。GlobalService属性を適用します:
[GlobalService]
public interface IBluetoothAccessor
{
    // Bluetooth探索を開始する
    ValueTask BeginDiscoverAsync();
}
```

そして、それぞれのプラットフォームのプロジェクトで、このインターフェイスを実装したものを登録します。
以下はAndroidの例です:

```csharp
// Android向けの、Sample.Xamarin.Forms.Androidプロジェクトで定義する

// Android向けの実装
public sealed class AndroidBluetoothAccessor : IBluetoothAccessor
{
    public async ValueTask BeginDiscoverAsync()
    {
        // Androidに固有の実装...
    }
}

// Applicationコンストラクタ
public Application()
{
    // Android依存の処理を行うクラスを登録する
    GlobalService.Register(new AndroidBluetoothAccessor());
}
```

これで、共通プロジェクト内で、インターフェイスを通じて分離された実装を使えるようになりました:

```csharp
// 共通プロジェクトの、Sample.Xamarin.Formsプロジェクトで使う

// Bluetoothを使いたくなった:
await GlobalService.ExecuteAsync<IBluetoothAccessor>(async accessor =>
{
    // Bluetoothの探索を開始する
    await accessor.BeginDiscoverAsync();

    // ...
});

```

既存の依存注入や依存分離を行うライブラリ(例:`DependencyService`クラスやUnity、MEFなど)には、以下のような問題があります:

* 複雑な機能を持っている: 多くのシチュエーションでは、単に共通のインターフェイスを実装したインスタンスが欲しいだけであるので、
`GlobalService`クラスでは、そのような操作を高速に実行できるようにしました。
* 取得したインスタンスを保持されると、生存期間の管理が出来ない: 高速なので、毎回`ExecuteAsync`を呼び出しても問題ありません。
むしろ、必要な場合にのみ、その都度使用することが望ましいです。

注意: "Global"の名の通り、`GlobalService`は、一種のグローバル変数のように振る舞います。
本来必要のない場所で`GlobalService`を使わないようにして下さい。
少しでも区別できるように、`GlobalService`は`Epoxy.Advanced`名前空間に配置されています（using宣言が必要です）。

---

## License

Apache-v2

## History

* 0.14.0:
  * XamlDesignerクラスを追加。
  * Avaloniaランタイムプラットフォームを追加: net48, netcoreapp2.1, netcoreapp3.1, net5.0.
  * F#のcamel-case APIを追加。
  * FSharp.Coreのバージョンを5.0.1から5.0.0にダウングレードした。
  * Avalonia XAMLデザイン時に、実際にViewModelが駆動されてしまう不具合を修正。
  * Avalonia C# XAMLデザイナが動作しなかった問題を修正。
* 0.13.0:
  * dotnet CLIテンプレートを追加。
  * WinUIでのUIThread検出を強化。
  * F# WPF NuGetパッケージで、XAMLを自動的にアセンブリリソースに配置するようにした。
* 0.11.0:
  * F#のサポートを追加しました。
  * ValueConverterの一般的な引数をスワップしました。(Breaking)
  * いくつかのファクトリーメソッドを "Factory" と名のついた型に移動しました。(Breaking)
* 0.10.0:
  * WinUIに対応しました。
* 0.9.0:
  * Uno platformとAvaloniaに対応しました。
* 0.8.0:
  * GlobalServiceとEventBinder機能を追加しました。
* 0.7.0:
  * Xamarin Formsのサンプルコードを追加しました。
* 0.6.0:
  * 同期コマンドハンドラを分割しました。
* 0.5.0:
  * UIThreadとAnchor/Pile機能を追加しました。
