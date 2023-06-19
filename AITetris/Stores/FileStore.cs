using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;

namespace AITetris.Stores;

/// <summary>
/// Stores all the filenames and filelocation that are hardcoded in the entire project
/// </summary>
public static class FileStore
{
	public static string? ExeDir
	{
		get
		{
			try
			{
				return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
			}
			catch (Exception)
			{
				//log the IO exception crash
				return string.Empty;
			}
		}
	}

	

	public static string? SettingJsonFileLocation
	{
		get
		{
			return ExeDir + @"/Assets/JSON/Settings.json";
		}
	}

	public static Uri MainMenu => SetPageUri("MainPage");
	public static Uri GameMenu => SetPageUri("StartGameMenu");

    public static Uri LeaderBoardMenu => SetPageUri("Leaderboard");

    public static Uri PointShopMenu => SetPageUri("PointShop");

    public static Uri SettingsMenu => SetPageUri("SettingsMenu");

    private static Uri SetPageUri(string value)
	{
		return new Uri(@$"Pages/{value}.xaml", UriKind.Relative);
    }

    private static Uri SetAssetJSONUri(string value)
    {
        return new Uri(@$"Assets/JSON/{value}.json", UriKind.Relative);
    }

    private static Uri SetAssetTxtUri(string value)
    {
        return new Uri(@$"Assets/JSON/{value}.txt", UriKind.Relative);
    }

    private static Uri SetAssetSoundUri(string value)
    {
        return new Uri(@$"Assets/Sound/{value}", UriKind.Relative);
    }

    private static Uri SetAssetSpritesUri(string value)
    {
        return new Uri(@$"Assets/Sprites/{value}", UriKind.Relative);
    }

    public static Uri BackgroundImage => SetAssetSpritesUri(@"Background\SirBackground2.png");

    public static Uri ColorImage(string value) => SetAssetSpritesUri(@$"{value}.png");


    // Set an image source
    public static ImageSource BackgroundImageSource => new BitmapImage(BackgroundImage);

	public static Image GeneratedSpriteImage(string value)
	{
        var image = new Image
        {
            Source = new BitmapImage(ColorImage(value))
        };
        return image;
	}

	public static Uri MetaCurrencyFileLocation => SetAssetTxtUri("MetaCurrency");

	
}
