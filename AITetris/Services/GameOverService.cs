using AITetris.Pages;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using AITetris.Enums;
using AITetris.Classes;

namespace AITetris.Services;
public static class GameOverService
{
    public static (bool SaveAi, bool Revive) CheckPlayerStateOfSaveAiAndRevive(this GameBoard gameBoard)
    {
        bool copysaveai = true;
        bool copyrevive = true;
        // Check if the game contains a player or AI
        if (gameBoard.game.IsPlayer == true)
        {
            // Disable save AI button for player
            copysaveai = false;

            // Enable or disable the upgrades bought
            if (gameBoard.game?.Upgrades?.Revive == 0)
            {
                // Disable Revive button for Player if you have no revives
                copyrevive = false;
            }
        }
        else
        {
            // Disable Revive button for AI
            copyrevive = false;
        }

        return (copysaveai, copyrevive);
    }

    public static Border GenerateHeaders(EColumnsHeaderNames column, int row)
    {
        // Border for each label
        Border border = new Border();

        // Border styling
        border.BorderBrush = Brushes.Silver;
        border.BorderThickness = new Thickness(2);

        // Creating a new label
        Label label = new Label();

        // Add content and styling to the label
        switch (column)
        {
            case EColumnsHeaderNames.Rank:
                label.Content = "Rank";
                break;
            case EColumnsHeaderNames.Name:
                label.Content = "Name";
                break;
            case EColumnsHeaderNames.Point:
                label.Content = "Point";
                break;
            case EColumnsHeaderNames.Lines:
                label.Content = "Lines";
                break;
            case EColumnsHeaderNames.Time:
                label.Content = "Time";
                break;
            case EColumnsHeaderNames.Character:
                label.Content = "Character";
                break;

            default:
                break;
        }

        label.HorizontalAlignment = HorizontalAlignment.Center;
        label.VerticalAlignment = VerticalAlignment.Center;
        label.FontFamily = new FontFamily("Tahomaa");
        label.FontSize = 20;
        label.FontWeight = FontWeights.Bold;

        // Add label to border
        border.Child = label;

        // Add the border to the leaderboardgrid
        Grid.SetColumn(border, (int)column);
        Grid.SetRow(border, row);
        return border;
    }

    public static Border GeneratedNormalBorder(this List<Game> scores, int column, int row)
    {
        // Border for each label
        Border border = new Border();

        // Border styling
        border.BorderBrush = Brushes.Silver;
        border.BorderThickness = new Thickness(2);

        // Creating a new label
        Label label = new Label();

        // Add content and styling to the label

        label.Content = column switch
        {
            0 => scores[row - 1].Rank,
            1 => scores[row - 1].Character.Name,
            2 => scores[row - 1].Points,
            3 => scores[row - 1].LinesCleared,
            4 => scores[row - 1].Time,
            5 => scores[row - 1].IsPlayer ? "Player" : "AI",
            _ => ""
        };

        label.HorizontalAlignment = HorizontalAlignment.Center;
        label.VerticalAlignment = VerticalAlignment.Center;
        label.FontFamily = new FontFamily("Tahomaa");
        label.FontSize = 15;
        label.FontWeight = FontWeights.Bold;

        // Add label to border
        border.Child = label;

        // Add the border to the leaderboardgrid
        Grid.SetColumn(border, column);
        Grid.SetRow(border, row);
        return border;
    }
}
