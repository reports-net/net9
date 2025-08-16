Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Globalization
Imports System.Resources
Imports System.Windows
<Assembly: AssemblyTrademark("")> 
<Assembly: ComVisible(false)>

'ローカライズ可能なアプリケーションのビルドを開始するには、
'.vbproj ファイルの <UICulture>CultureYouAreCodingWith</UICulture> を
'<PropertyGroup> 内に設定します。たとえば、
'ソース ファイルで英語 (米国) を使用している場合、<UICulture> を "en-US" に設定します。次に、
'下の NeutralResourceLanguage 属性のコメントを解除し、下の行の "en-US" を
'プロジェクト ファイルの UICulture 設定と一致するように更新します。

'<Assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)> 


'ThemeInfo 属性は、テーマ固有および汎用のリソース ディクショナリがある場所を表します。
'第 1 パラメータ: テーマ固有のリソース ディクショナリが置かれている場所
'(リソースがページ、
' またはアプリケーション リソース ディクショナリに見つからない場合に使用されます)

'第 2 パラメータ: 汎用リソース ディクショナリが置かれている場所
'(リソースがページ、
'アプリケーション、テーマ固有のリソース ディクショナリに見つからない場合に使用されます)
<Assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly)>



'このプロジェクトが COM に公開される場合、次の GUID がタイプ ライブラリの ID になります。
<Assembly: Guid("cd751cbc-a860-4ed3-885a-759d1a345e91")> 
