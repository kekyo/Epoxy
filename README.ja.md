# Epoxy - An independent flexible XAML MVVM library

![Epoxy bin](Images/Epoxy.160.png)

[English language is here](https://github.com/kekyo/Epoxy)

[![Project Status: Active](https://www.repostatus.org/badges/latest/active.svg)](https://www.repostatus.org/#wip)

## NuGetパッケージ (C#)

|Package|main|Description|
|:--|:--|:--|
|Epoxy.Wpf|[![NuGet Epoxy.Wpf](https://img.shields.io/nuget/v/Epoxy.Wpf.svg?style=flat)](https://www.nuget.org/packages/Epoxy.Wpf)|WPF version|
|Epoxy.Avalonia|[![NuGet Epoxy.Avalonia](https://img.shields.io/nuget/v/Epoxy.Avalonia.svg?style=flat)](https://www.nuget.org/packages/Epoxy.Avalonia)|Avalonia version|
|Epoxy.OpenSilver|[![NuGet Epoxy.OpenSilver](https://img.shields.io/nuget/v/Epoxy.OpenSilver.svg?style=flat)](https://www.nuget.org/packages/Epoxy.OpenSilver)|OpenSilver version|
|Epoxy.Xamarin.Forms|[![NuGet Epoxy.Xamarin.Forms](https://img.shields.io/nuget/v/Epoxy.Xamarin.Forms.svg?style=flat)](https://www.nuget.org/packages/Epoxy.Xamarin.Forms)|Xamarin Forms version|
|Epoxy.Uwp|[![NuGet Epoxy.Uwp](https://img.shields.io/nuget/v/Epoxy.Uwp.svg?style=flat)](https://www.nuget.org/packages/Epoxy.Uwp)|Universal Windows version|
|Epoxy.WinUI|[![NuGet Epoxy.WinUI](https://img.shields.io/nuget/v/Epoxy.WinUI.svg?style=flat)](https://www.nuget.org/packages/Epoxy.WinUI)|WinUI 3 version|
|Epoxy.Maui|[![NuGet Epoxy.Maui](https://img.shields.io/nuget/v/Epoxy.Maui.svg?style=flat)](https://www.nuget.org/packages/Epoxy.Maui)|.NET MAUI|

## NuGetパッケージ F#専用

|Package|main|Description|
|:--|:--|:--|
|FSharp.Epoxy.Wpf|[![NuGet FSharp.Epoxy.Wpf](https://img.shields.io/nuget/v/FSharp.Epoxy.Wpf.svg?style=flat)](https://www.nuget.org/packages/FSharp.Epoxy.Wpf)|WPF version|
|FSharp.Epoxy.Avalonia|[![NuGet FSharp.Epoxy.Avalonia](https://img.shields.io/nuget/v/FSharp.Epoxy.Avalonia.svg?style=flat)](https://www.nuget.org/packages/FSharp.Epoxy.Avalonia)|Avalonia version|

## dotnet CLIテンプレート

|Package|main|Description|
|:--|:--|:--|
|Epoxy.Templates|[![NuGet Epoxy.Templates](https://img.shields.io/nuget/v/Epoxy.Templates.svg?style=flat)](https://www.nuget.org/packages/Epoxy.Templates)|dotnet CLI template package|

## これは何?

* Epoxyは、.NET XAML環境で使える、Model-View-ViewModel (MVVM) アーキテクチャ向けの、独立した柔軟性のあるライブラリです。
  * C#を含む.NETの全処理系向け、及びF#用のNuGetパッケージがあります。
* 以下の環境をサポートしています:
  * WPF: .NET 7.0/6.0/5.0, .NET Core 3.0/3.1, .NET Framework 4.5/4.8
  * Avalonia: [Avalonia](https://avaloniaui.net/) (0.10.0 or higher)
  * OpenSilver: [OpenSilver](https://opensilver.net/) (1.0.0 or higher)
  * Xamarin Forms: [Xamarin Forms](https://github.com/xamarin/Xamarin.Forms)  5.0.0.1874 or higher)
  * Universal Windows: Universal Windows 10 (uap10.0.16299 or higher)
  * WinUI 3: [Windows App SDK](https://github.com/microsoft/WindowsAppSDK) (net5.0-windows10.0.17763.0 or higher)
  * .NET MAUI: [.NET Multi-platform App UI](https://dotnet.microsoft.com/en-us/apps/maui) (.net6.0 or higher)
* 非同期処理 (async-await) を安全に書くことが出来るように配慮しています。
* C# 8.0でサポートされた、null許容参照型を使えます。
* F#は6.0に対応しています。F#向けのシグネチャ (camel case functions・関数型・Async型前提) が定義されています。
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

.NET 6 SDKのCLIテンプレートに対応しています。以下のようなコマンドで、簡単にサンプルコードをクリーンな状態で試すことができます:

```bash
# テンプレートパッケージをインストール（初回又はバージョンアップ時のみ）
dotnet new -i Epoxy.Templates

# 現在のディレクトリにWPFサンプルコードを展開
dotnet new epoxy-wpf

# ビルド
dotnet build
```

* 注意: テンプレートは .NET 6 を想定しているため、.NET 6 SDKをあらかじめインストールして下さい。
  他のバージョンのみの環境（例えば.NET 7/5 SDK）では、`TargetFramework`を修正しないと、ビルドに失敗します。

### 現在サポートしているテンプレート一覧

|`dotnet new`引数|言語|対象|
|:--|:--|:--|
|`epoxy-wpf`|C#, F#|WPFのサンプルコード|
|`epoxy-avalonia`|C#, F#|Avaloniaのサンプルコード|
|`epoxy-opensilver`|C#|OpenSilverのサンプルコード|
|`epoxy-xamarin-forms`|C#|Xamarin Formsのサンプルコード|
|`epoxy-uwp`|C#|UWPのサンプルコード|
|`epoxy-winui`|C#|WinUI 3のサンプルコード|
|`epoxy-maui`|C#|.NET MAUIのサンプルコード|

* デフォルトではC#のサンプルコードが展開されます。F#にする場合は、`dotnet new epoxy-wpf -lang F#`のように、オプションをコマンドラインに加えます。
* Xamarin Forms, UWP, WinUI 3は、古い形式のMSBuildプロジェクトを使用しています。
  * ビルド・実行する場合は、`dotnet build` ではなく、Visual Studioでソリューションを開く必要があります。
* OpenSilverのサンプルコードは、.NET Frameworkベースのシミュレータプロジェクトが含まれています。
  * ビルド・実行する場合は、`dotnet build` ではなく、Visual Studioでソリューションを開く必要があります。
  * WebAssemblyとしてChromeやFirefoxなどでホストする場合は、別途プロジェクトが必要です。
* 上記以外のサンプルコードは、リポジトリ内の `samples` ディレクトリを参照して下さい。
* MyGetに配置されたdevelブランチパッケージを使用できます。dotnet CLI公式には説明されていませんが、`--nuget-source`オプションを使用します: `dotnet new -i Epoxy.Templates::<version> --nuget-source http://nuget.kekyo.online:59103/repository/nuget/index.json`

### Visual Studioのウィザードから選択

上記テンプレートのインストールを行っておけば、Visual Studioの新規プロジェクト生成でも選択する事が出来ます。

![Template selection dialog](Images/vswizard_ja.png)

----

### サンプルコードの解説

起動後にボタンをクリックすると、完全に非同期でダウンロードしながら、リストに結果を追加していきます。

![EpoxyHello.Wpf](https://github.com/kekyo/Epoxy/raw/main/Images/sample.Wpf.png)

![EpoxyHello.Xamarin.Forms](https://github.com/kekyo/Epoxy/raw/main/Images/sample.Xamarin.Forms.png)


----

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
完全に、とは、つまりViewクラスに、コードビハインドを一切記述しないことを指します。

```csharp
// ステップ 1: ViewModelクラスを作ります。そしてViewModel属性を付与します。
//    この属性は、PropertyChangedを自動的に実装して、XAML側に伝搬できるようにします。
[ViewModel]
public sealed class MainWindowViewModel
{
    // ステップ 2: XAMLから参照したいプロパティを自動実装プロパティで定義します。
    //    Epoxyは、 C# 8.0 で追加された、null許容参照型定義に対応しています。
    public Command? Fetch { get; }
    public ObservableCollection<ImageSource>? Items { get; }

    // ViewModelのコンストラクタ
    public MainWindowViewModel()
    {
        // ステップ 3: プロパティのsetterが呼び出されると、
        //    PropertyChangedイベントが発生して、XAML側に変更が通知されます。
        this.Items = new ObservableCollection<ItemViewModel>();

        // ステップ 4: XAMLで定義したボタンがクリックされた時に、このラムダ式が呼び出されます。
        //   この式はもちろん async-await を使用した非同期処理で書くことが出来て、
        //   未処理の例外も正しく処理されます。
        this.Fetch = Command.Factory.Create(async () =>
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

[投稿画像をダウンロードする部分 (EpoxyHello.Core)](https://github.com/kekyo/Epoxy/blob/main/samples/EpoxyHello.Core/Models/Reddit.cs#L63)を抜粋します:

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

----

## 機能一覧

それぞれの機能は独立しているため、自由に組み合わせて使用出来ます
（例えば、`ViewModel`を継承していないと使えない、と言うような事はありません）。

|機能名|概要|
|:----|:----|
|ViewModelインジェクタ|ViewModelに必要なPropertyChangedイベントなどを、ビルド時に自動的に実装出来る機能です。対象のクラスに属性を適用するだけで、煩雑なコードの実装を省略出来ます。|
|ViewModel基底クラス|ViewModelに必要なPropertyChangedイベントなどを、オーソドックスな基底クラスとして提供します。ViewModelインジェクタが適さないシナリオで、使用することが出来ます。|
|Command factory|任意の非同期デリゲートを、ICommandとして利用できるようにします。非同期処理を安全にICommandとして実装出来ます。|
|EventBinder|任意のXAMLコントロールのCLRイベントを、ICommandとしてバインディング可能にする添付プロパティです。Commandプロパティが提供されていない任意のイベントを、安全にバインディング出来ます。|
|Anchor/Pile|任意のXAMLコントロールを、一時的かつ安全にViewModelから参照出来るようにします。Anchor/Pileを使用すると、全てのコードビハインドを排除出来るため、MVVMを使用する場合の実装の見通しが良くなります。Messengerパターンとして知られたテクニックも、Anchor/PileでViewModelに集約することが出来ます。|
|ValueConverter|XAMLの値コンバーターの基底クラスを提供します。事前に型判定が行われ、型制約がある状態で実装することが出来ます。|
|UIThread|UIスレッドの扱いを、プラットフォーム間で統一出来ます。また、非同期操作の継続として扱えるため、async-awaitやF#の非同期ワークフローで、シームレスにスレッドを扱うことが出来ます。|
|GlobalService|コンパクトな、依存注入のためのインフラです。非常に高速で単純なため、多くのシナリオに適し、プラットフォーム間で実装方法を統一出来ます。|
|Designer|デザイン時編集をサポートします。|

以下の解説には細かく記載していませんが、Epoxyの機能全体に渡って、非同期処理を考慮した設計となっています。

* メソッドのシグネチャは、原則として非同期(`ValueTask`の返却)を使用します。
* 誤用の可能性のあるオーバーロードは存在しないか、又は`Epoxy.Synchronized`名前空間内にのみ、配置されています。
* `Task`を使用するオーバーロードは、`Epoxy.Supplemental`名前空間に分離されています。これは、誤って`ValueTask`の代わりに`Task`を使用してしまう可能性を減らすためです。

----

### ViewModelインジェクタとViewModel基底クラス

`ViewModel`属性が適用されると、コンパイル時に自動的に`PropertyChanging`、`PropertyChanged`が実装されます。また、自動実装プロパティのsetterで、これらのイベントが自動的に発生するように処理されます。この機能を、`ViewModelインジェクタ`と呼びます。

以前のEpoxy(<0.15)の実装では、`ViewModel`基底クラスから継承する事を強制していましたが、この属性を使用することで、任意のクラスを負担なしでViewModelにすることが出来ます。

また、プロパティに`IgnoreInject`属性を適用すると、そのプロパティは`PropertyChanging`、`PropertyChanged`の処理対象から除外出来ます。

次のようなシグネチャのメソッドを併設することで、プロパティ変更時の処理を簡単に追加出来ます:

```csharp
// 定義したプロパティ
public string Title { get; set; }

// プロパティが変更された場合に呼び出される。
// シグネチャは強制されないので、以下の条件を守る必要がある:
// * 引数は、プロパティと同じ型 (引数名は任意)
// * 戻り値はValueTaskでなければならない
// * PropertyChanged属性を適用する。引数にプロパティ名を指定する（メソッド名は自由）
//   * PropertyChanged属性を使わない場合は、メソッド名を、"On<プロパティ名>ChangedAsync"とする
[PropertyChanged(nameof(Title))]
private ValueTask TitleChangedAsync(string value)
{
  // 値が変更された場合の処理...
}
```

`ViewModelインジェクタ`を使わず、従来通り`ViewModel`基底クラスを派生して実装することも出来ます。

`ViewModel`基底クラスは、`GetValue`/`SetValue`メソッドの実装を提供します。
これらのメソッドは、XAML側にプロパティの変更通知 `PropertyChanging`/`PropertyChanged` を自動的に行います。
たとえば、ボタンクリックの契機で`ViewModel`からプロパティを変更すると、変更がXAMLのコントロールに通知され、ユーザーインターフェイスに反映されます。

なお、`GetValue`には、デフォルト値の定義が、
`SetValue`には、値変更時に追加操作を行うことが出来るオーバーロードが定義されています。

プロジェクト内で全くViewModelインジェクタを使用しない場合は、
ViewModelインジェクタを無効化する事で、自動的なコードを解析を停止させ、ビルドを高速化出来ます。
csprojの`PropertyGroup`の`EpoxyBuildEnable`に`False`を指定して下さい。

----

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
        <epoxy:Event EventName="Loaded" Command="{Binding Ready}" />
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
this.Ready = Command.Factory.Create<EventArgs>(async _ =>
{
    // リストに表示する情報をModelから非同期で取得
    foreach (var item in await Model.FetchInitialItemsAsync())
    {
        this.Items.Add(item);
    }
});
```

`Command.Factory.Create<T>`のジェネリック引数には、イベントの第二引数(通常EventArgsを継承したクラス)を指定します。
イベントの引数が必要でない場合は、非ジェネリックメソッドを使う事も出来ます。

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

* [For example (In WPF XAML)](https://github.com/kekyo/Epoxy/blob/main/samples/EpoxyHello.Wpf/Views/MainWindow.xaml#L36)
* [For example (In WPF view model)](https://github.com/kekyo/Epoxy/blob/main/samples/EpoxyHello.Wpf/ViewModels/MainWindowViewModel.cs#L45)
* [For example (In Xamarin Forms XAML)](https://github.com/kekyo/Epoxy/blob/main/samples/EpoxyHello.Xamarin.Forms/EpoxyHello.Xamarin.Forms/Views/MainPage.xaml#L33)
* [For example (In Xamarin Forms view model)](https://github.com/kekyo/Epoxy/blob/main/samples/EpoxyHello.Xamarin.Forms/EpoxyHello.Xamarin.Forms/ViewModels/MainContentPageViewModel.cs#L40)

----

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
this.LogPile = Pile.Factory.Create<TextBox>();

// ...

// TextBoxを操作したくなったら、Pileを通じて参照をレンタルします:
await this.LogPile.RentAsync(async textBox =>
{
    // モデルから情報を非同期で取得します
    var result = await ServerAccessor.GetResultTextAsync();
    // TextBoxを直接操作できます
    textBox.AppendText(result);
});
```

* [For example (In WPF XAML)](https://github.com/kekyo/Epoxy/blob/main/samples/EpoxyHello.Wpf/Views/MainWindow.xaml#L39)
* [For example (In WPF view model)](https://github.com/kekyo/Epoxy/blob/main/samples/EpoxyHello.Wpf/ViewModels/MainWindowViewModel.cs#L74)

----

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
// その型は、ジェネリック第2引数で指定します。ここでは文字列を受け取る例を示します:
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

* [For example](https://github.com/kekyo/Epoxy/blob/main/samples/EpoxyHello.Wpf/Views/Converters/ScoreToBrushConverter.cs#L25)

----

### UIThread

UIスレッドの取り扱いは、異なるプラットフォームにおいても重要な点です。
Epoxyでは[UIThreadクラス](https://github.com/kekyo/Epoxy/blob/main/Epoxy/UIThread.cs#L29)で同じ操作が行えるようにしています。
また、このクラスを使うことで、UIの操作と非同期処理を簡単に組み合わせる事が出来ます。

```csharp
// 現在のスレッドがUIスレッドかどうか
Debug.Assert(await UIThread.IsBoundAsync());

// ワーカースレッドで継続させる
var read = await httpStream.ReadAsync(...).ConfigureAwait(false);

// ここではワーカースレッドで処理
Console.WriteLine($"Read={read}");

// UIスレッドに切り替える
await UIThread.Bind();

// バインディングされたTextBlockに反映する
this.Log = $"Read={read}";

// 明示的にワーカースレッドに切り替える
await UIThread.Unbind();

// (ワーカースレッドで継続)

// 一時的な操作をUIスレッドで行う
await UIThread.InvokeAsync(async () =>
    this.FetchedText = await httpStream.ReadStringAsync(...));
```

他にも、`UIThread.TryBind()`を使用すると、UIスレッドへの切り替えが成功したかどうかを確認する事が出来ます。
これは、ホストとなるUIフレームワーク(WPFなど)が終了する間際に、UIスレッドへのアクセスが成功したかどうかを確認して、
継続処理を行う事が出来ます。

#### UWP環境で実行する場合の注意

現在の実装では、UWPネイティブや、Xamarin Forms/UnoでのUWP環境においての実行、WinUIなどのUWP由来のランタイムで
`UIThread`クラスを使う場合、`View`構築中の`ViewModel`のコンストラクタなどで使用すると、正しい結果が得られない場合があります。

UWPは、ビューを保持するウインドウ毎に異なるUIスレッドが割り当てられていて、
インスタンスを構築中に使用すると、ビューを判別できない事から、正しく判定できないためです。

----

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
    GlobalService.Accessor.Register(new AndroidBluetoothAccessor());
}
```

これで、共通プロジェクト内で、インターフェイスを通じて分離された実装を使えるようになりました:

```csharp
// 共通プロジェクトの、Sample.Xamarin.Formsプロジェクトで使う

// Bluetoothを使いたくなった:
await GlobalService.Accessor.ExecuteAsync<IBluetoothAccessor>(async accessor =>
{
    // Bluetoothの探索を開始する
    await accessor.BeginDiscoverAsync();

    // ...
});
```

`Register()`の代わりに、`RegisterExplicit<TService>()`を使用すると、
`GlobalService`属性が適用されていないインターフェイスでも、管理する事が出来ます。
例えば、既存の（変更できない）インターフェイス型を使用したい場合に有用です。

#### 補足

既存の依存注入や依存分離を行うライブラリ(例:`DependencyService`クラスやUnity、MEFなど)には、以下のような問題があります:

* 複雑な機能を持っている: 多くのシチュエーションでは、単に共通のインターフェイスを実装したインスタンスが欲しいだけであるので、
`GlobalService`クラスでは、そのような操作を高速に実行できるようにしました。
* 取得したインスタンスを保持されると、生存期間の管理が出来ない: 高速なので、毎回`ExecuteAsync`を呼び出しても問題ありません。
むしろ、必要な場合にのみ、その都度使用することが望ましいです。

注意: "Global"の名の通り、`GlobalService`は、一種のグローバル変数のように振る舞います。
本来必要のない場所で`GlobalService`を使わないようにして下さい。
（これは `GlobalService` 固有の問題ではなく、任意のDIコンテナでシングルトンインスタンスを保持した場合に起きる、一般的な問題です。）

少しでも区別できるように、`GlobalService`は`Epoxy.Advanced`名前空間に配置されています（using宣言が必要です）。

----

### Designer (高度なトピック)

`Designer`クラスは、デザイン編集に関係のある処理を記述するために使用出来ます。

カスタムコントロールやユーザーコントロールを実装した場合、
IDE(Visual StudioやRiderなど)が、コントロールのビジュアル編集を行っている時に、
実際にコントロールのインスタンスを、IDE内で生成する可能性があります。

そのような場合は、本当のコントロールの動作を行うのではなく、デザイン編集にふさわしい見た目や挙動に変えたい場合があります。

`IsDesignTime`プロパティを参照する事で、デザイン時編集を行っているかどうかを、プラットフォームに依存しない方法で取得できます。

----

## F#バージョンについて

F#バージョンのパッケージを使う事で、以下のようなF#の流儀に沿うコードを記述できます。
使用するインスタンスは共有されます。C#/F#混在プロジェクトにおいても、保持するインスタンスは同一でありながら、
C#/F#それぞれで好ましいAPIを使い分けることができます。

F#でEpoxyを使う解説については、以前にconnpassで発表したスライドがあるので、参考にどうぞ:

[F# Epoxy - Fun Fan F# (見えない場合はこちら)](https://speakerdeck.com/kekyo/f-number-epoxy)

### camel-caseの関数名

FSharp.Epoxyのすべての関数は、camel-case化されています。例えば、`ViewModel`基底クラスの、`GetValue`/`SetValue`メソッドの代わりに、`getValue`/`setValue`関数を使います。

```fsharp
open Epoxy

type ItemViewModel() =
    inherit ViewModel()

    // プロパティの転送は、getValue, setValue関数を使う。
    // 型推論が利く場所に型を書けるので、get()やset()に型注釈を寄せて書ける。
    member __.Title
        with get(): string = __.getValue()
        and set (value: string) = __.setValue value
```

### F#型の直接サポート

デリゲート型ではなく関数型、outパラメータではなく`Option`型、のように、F#で扱いやすいように配慮しています。

```fsharp
// デリゲートを受ける引数は、代わりにF#の関数型を直接受け取ることが出来る。
self.Ready <- Command.Factory.createSync(fun (e:RoutedEventArgs) ->
    self.IsEnabled <- true)
```

```fsharp
type public ScoreToBrushConverter() =
    inherit ValueConverter<int, Brush>()

    // convert関数はoutパラメータを持たず、'T optionを返すように記述できる。
    override __.convert from =
        if from >= 5 then Some yellow else Some gray
```

### 既定の非同期型として`ValueTask`型ではなく`Async`型を使う

基本的に、全ての非同期処理は`Async`型でスムーズに記述できるように配慮しています。

```fsharp
// デフォルトの関数定義は、全てF#の`Async`型を受け取るように定義されているため、
// 以下のように非同期ワークフロー `async { ... }` で書くことが出来る。
self.Fetch <- Command.Factory.create(fun () -> async {
    let! reddits = Reddit.fetchNewPostsAsync "r/aww"
    // ...
})
```

私の別のプロジェクト、[FusionTasks](https://github.com/kekyo/FSharp.Control.FusionTasks)を併用すると、既存の`Task`/`ValueTask`を使うライブラリ（例えば`HttpClient.GetAsync`のような）を、更に簡単に扱うことが出来るようになります。dotnet CLIテンプレートは、既定で有効になっています。

`Task`型や`ValueTask`型を返すメソッドを直接渡す場合や、これらの型を構成するコンピュテーション式を与える場合は、`Epoxy.Supplements`名前空間を明示的に参照してください。

注意: `Async`型を優先する事については、[将来のF#で、`resumable`構造がリリース](https://github.com/dotnet/fsharp/pull/6811)された際に、変更される可能性があります。

### ViewModelインジェクタ

F#でもViewModelインジェクタは使用できます。但し、自動実装プロパティの構文上の制約があります:

```fsharp
open Epoxy

// ViewModelインジェクタを使う
[<ViewModel>]
type ItemViewModel() as self =
    do
        // 通常、この式は例外を起こすが、ViewModelインジェクタを使用した場合は合法となる。
        self.Title <- "CCC"
        // この挙動を使用して、doブロック内でCommandを割り当てることが出来る。
        self.Click <- Command.Factory.create(fun () -> async {
            // ...
        })

    // F#の自動実装プロパティには初期化式が必要だが、doブロックでインスタンスが
    // 割り当てられた場合は無視される。
    member val Title = "AAA" with get, set
    member val Body = "BBB" with get, set
    member val Click: Command = null with get, set

// 結果:
let vm = new ItemViewModel()
Debug.Assert(vm.Title = "CCC")
Debug.Assert(vm.Body = "BBB")
```

`IgnoreInject`属性も、F#で同様に使用出来ます。プロパティ変更時の処理は、`Async<unit>`を返却します:

```fsharp
// 定義したプロパティ
member val Title = "Unknown"
    with get, set

// プロパティが変更された場合に呼び出される。
// シグネチャは強制されないので、以下の条件を守る必要がある:
// * 引数は、プロパティと同じ型 (引数名は任意)
// * 戻り値はAsync<unit>でなければならない
// * PropertyChanged属性を適用する。引数にプロパティ名を指定する（メソッド名は自由）
//   * PropertyChanged属性を使わない場合は、メソッド名を、"on<プロパティ名>ChangedAsync"とする
[<PropertyChanged("Title")>]
member self.titleChangedAsync (value: string) = async {
    // 値が変更された場合の処理...
}
```

### WPF XAMLページの自動リソース化

F#でWPFを扱う場合、XAMLをC#のpartial classに変換されるとビルド出来ないため、リソースとしてそのままプロジェクトに追加されるように、XAMLのビルドアクションが自動的に変更されます。

プロジェクトにXAMLファイルを追加する際に、特に何もしなくても、正しく設定されます。

この機能の制約として、XAMLは常にソースコード(XMLテキスト)のままリソースに保存され、バイナリ(BAML)には変換されません。また、実行時に型を参照できるようにするため、XAML名前空間には、常にアセンブリ名を指定する必要があります:

```xml
<!-- clr-namespaceの指定には、常にassembly指定を加える -->
<Window
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:viewmodels="clr-namespace:EpoxyHello.ViewModels;assembly=EpoxyHello"
    Title="EpoxyHello" Height="450" Width="800">

    <Window.DataContext>
        <viewmodels:MainWindowViewModel />
    </Window.DataContext>

    <!-- ... -->
</Window>
```

----

## License

Apache-v2

## History

* 1.10.0:
  * GlobalServiceに、`RegisterExplicit<TService>()`と`UnregisterExplicit<TService>()`を追加しました。
    これらは、対象のインターフェイスに`GlobalService`属性が適用されていなくても、管理を可能にします。
  * `CommandFactory`と`PileFactory`をobsoleteにしました。代わりに`Command.Factory` `Pile.Factory`を使用して下さい。
    * F#言語では、スタティックメンバに対する拡張が可能ですが、C#と同様に`Factory`プロパティから参照するように合わせてあります。
  * パッケージバイナリは、破壊的変更を含んでいます。アップグレード後は再ビルドを行ってください。
* 1.9.0:
  * .NET 7 SDKに対応しました。
  * パッケージ依存関係をより柔軟にするため、以下のサポートバージョンをそれぞれダウングレードしました。但し動作確認は最新バージョンのみで行っています。
    * Avalonia: 0.10.0
    * Xamarin Forms: 5.0.0.1874
  * サンプルテンプレートの各パッケージを最新版を使用するように変更しました。
    * 現在の所、サンプルテンプレートのターゲットは、 .NET 6 SDKです。
      .NET 7 SDKを使用する場合は、`TargetFramework`の値を手動で調整する必要があります。（例えば`net7.0`のようにする）
      .NET 7 SDKと関連するパッケージの安定性が確認できた時点で、サンプルテンプレートのデフォルトバージョンも変更する予定です。
* 1.8.0:
  * .NET MAUIをサポートしました。
  * Unoのパッケージが壊れてCI構築に失敗するようになったので削除しました。
    (このバージョンではUnoプロジェクトを保存していますが、次のバージョンで完全に削除する予定です。必要であれば検討しますので、issueを追加して下さい)。
  * 関連するパッケージのアップグレードを行いました。
* 1.7.0:
  * MacOS上でEpoxyを含んだプロジェクトをビルドする際に、カスタムタスクでエラーが発生する問題を修正。
* 1.6.0:
  * `UIThread.TryBind()`, `UIThread.InvokeAsync()`, `UIThread.TryInvokeAsync()` を追加。
* 1.5.0:
  * `UIThread.Unbind()` を追加。
  * WinUI 3の正式版を使用するように修正。 (`Microsoft.WindowsAppSDK` 1.0.0)
  * WinUI 3のテンプレートを追加。
  * `EventBinder`にバインドするコマンドの引数が、オプション扱いとなりました。必要ない場合は、非ジェネリックの`CommandFactory.Create(() => ...)`が使えます。
  * テンプレートプロジェクトで`ViewModel`属性を使用していないものを修正しました。
* 1.4.0:
  * Xamarin Formsを最新版に更新。
  * Xamarin FormsでAnchor.Pileが見つからない問題を修正。
* 1.3.0:
  * Avalonia, Xamarin Formsを最新版に更新。
  * 暗黙に.NET Core 2.0の動作環境が必要とされていた問題を修正。
* 1.2.0:
  * .NET 6.0に対応。
  * OpenSilverに対応。
  * 現時点の最新パッケージ群に対応（UWP/WinUI/Unoを除く）。
  * WinUIのサンプルテンプレートを削除（必要であれば `samples` ディレクトリを参照して下さい）。
  * Visual Studioのプロジェクトウィザードに対応。
* 1.1.1:
  * WPF NuGetパッケージ生成時に、NU1201が発生する問題を修正。
* 1.1.0:
  * `PropertyChanged`属性を追加し、PropertyChanged発生時のハンドラ対象を属性で指定可能にしました。 [See #8](https://github.com/kekyo/Epoxy/issues/8)
  * Anchor/Pileの`ExecuteAsync`を非推奨とし、代わりに別名の`RentAsync`を追加しました。 [See #9](https://github.com/kekyo/Epoxy/issues/9)
  * プロジェクトに`EpoxyBuildEnable`を指定することで、ViewModelインジェクタを完全に停止させる事が出来るようにしました。 [See #6](https://github.com/kekyo/Epoxy/issues/6)
  * 依存するパッケージを更新しました (Uno.UI: 3.7.6, 但しUWPホスト以外は未検証)
* 1.0 正式リリース 🎉
  * ChildrenAnchor/ChildrenPile/ChildrenBinderは廃止しました。 [See #5](https://github.com/kekyo/Epoxy/issues/5)
* 0.17.0:
  * ChildrenAnchor/ChildrenPileの追加。ChildrenBinderは廃止予定。
  * XMLコメントを拡充。
  * 細かなインターフェイスの整理と修正。
  * 正式リリース候補 :)
* 0.16.0:
  * ViewModelインジェクタに、IgnoreInject属性の追加と、カスタムSetValueハンドラのサポートを追加。
  * .NET SDK3.1又は5.0のみをインストールした環境でViewModelインジェクタ実行時にエラーが発生する問題を修正。
* 0.15.0:
  * ViewModelの自動実装を可能にする、ViewModelインジェクタ機能を追加しました。
  * F#のcamel-casing UIThread関数を追加。
  * Commandのハンドラで発生する例外の処理を改善した。
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
