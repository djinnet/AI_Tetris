using AITetris.Pages;
using AITetris.Stores;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITetris.Services;
public class TxtIOService
{
    public static void Write(int metaCurrency, int Points)
    {
        File.WriteAllText(FileStore.MetaCurrencyFileLocation.AbsolutePath, CalculatePoints(metaCurrency, Points));

    }

    private static string CalculatePoints(int metaCurrency, int Points)
    {
        return ((Points / 100) + metaCurrency).ToString();
    }

    public static int Read => Convert.ToInt32(File.ReadAllText(FileStore.MetaCurrencyFileLocation.AbsolutePath));
}
