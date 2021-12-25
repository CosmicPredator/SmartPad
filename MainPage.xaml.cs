using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Controls;
using System.Text;
using Microsoft.Graphics.Canvas.Text;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SmartPad
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            var view = ApplicationView.GetForCurrentView();
            view.TitleBar.ButtonBackgroundColor = Colors.Transparent;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private async void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".txt");
            picker.FileTypeFilter.Add(".flac");
            picker.FileTypeFilter.Add(".dll");
            StorageFile file = await picker.PickSingleFileAsync();


            try
            {
                StreamReader reader = new StreamReader(await file.OpenStreamForReadAsync());
                this.MasterEditorBox.Text = reader.ReadToEnd();
                this.TitleTextBlock.Text = $"SmartPad - {file.Name}";
            } catch (Exception ex)
            {
                ContentDialog dialog = new ContentDialog();
                dialog.Title = "File Open Error";
                dialog.Content = $@"This file Can't be opened.

{ex}";
                dialog.CloseButtonText = "close";
                await dialog.ShowAsync();
            }

            
        }

        private void SettingsButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            AnimatedIcon.SetState(this.settigsIcon, "PointerOver");
        }

        private void SettingsButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            AnimatedIcon.SetState(this.settigsIcon, "Normal");
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("SmartPad Text", new List<string>() { ".txt" });
            savePicker.SuggestedFileName = "New Pad";
            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                Windows.Storage.CachedFileManager.DeferUpdates(file);
                await Windows.Storage.FileIO.WriteTextAsync(file, MasterEditorBox.Text);
                Windows.Storage.Provider.FileUpdateStatus status =
                    await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
            }
        }

        private async void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            await SettingsContentDialog.ShowAsync();
        }
    }
}
