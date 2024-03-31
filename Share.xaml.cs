using System;
using System.ComponentModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;
using menu.Models;
using menu.Data;
using menu.ViewModels;

namespace menu;

public partial class Share : ContentPage
{
    public Share(MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private int _shareCount = 1;
    

    private void DecreaseCount(object sender, EventArgs e)
    {
        if(_shareCount > 1) {
            _shareCount--;
            ShareCountEntry.Text = _shareCount.ToString();
        }     
    }

    private void IncreaseCount(object sender, EventArgs e)
    {
        _shareCount++;
        ShareCountEntry.Text = _shareCount.ToString();
    }

    private void CanViewClicked(object sender, EventArgs e)
    {
        
    }

    private void CanEditClicked(object sender, EventArgs e)
    {
        
    }

    
    private string GenerateClicked(object sender, EventArgs e)
    {
        var random = new Random();
        string code;                     
        bool isUnique;

        do
        {
            code = new string(Enumerable.Repeat("abcdefghijklmnopqrstuvwxyz0123456789", 10)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            isUnique = !db.GetUserLists().Any(l => l.ShareCode == code);
        } while (!isUnique);

        selectedlist.ShareCode = code;
        db.SaveUserList(selectedlist);
         
        return code;
    }

    private async void CopyClicked(object sender, EventArgs e)
    {
        string textToCopy = GeneratedCodeEntry.Text;

        if (!string.IsNullOrEmpty(textToCopy))
        {
            await Clipboard.SetTextAsync(textToCopy);

            await DisplayAlert("Copied", "The code has been copied to clipboard.", "OK");
        }
    }

}