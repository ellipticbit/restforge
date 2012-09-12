﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using WCFArchitect.Interface;
using Prospective.Controls.Dialogs;

namespace WCFArchitect.Interface
{
	public partial class Main : Window
	{
		//Project Properties
		public object SelectedProject { get { return (object)GetValue(SelectedProjectProperty); } set { SetValue(SelectedProjectProperty, value); } }
		public static readonly DependencyProperty SelectedProjectProperty = DependencyProperty.Register("SelectedProject", typeof(object), typeof(Main));
		
		public WCFArchitect.Options.UserProfile UserProfile { get { return (WCFArchitect.Options.UserProfile)GetValue(UserProfileProperty); } set { SetValue(UserProfileProperty, value); } }
		public static readonly DependencyProperty UserProfileProperty = DependencyProperty.Register("UserProfile", typeof(WCFArchitect.Options.UserProfile), typeof(Main));

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211")] public static RoutedCommand SelectProjectCommand = new RoutedCommand();

		static Main()
		{
			CommandManager.RegisterClassCommandBinding(typeof(Main), new CommandBinding(Main.SelectProjectCommand, OnSelectProjectCommandExecuted));
		}

		public Main()
		{
			if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 0) Globals.WindowsLevel = Globals.WindowsVersion.WinVista;
			if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 1) Globals.WindowsLevel = Globals.WindowsVersion.WinSeven;
			if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 2) Globals.WindowsLevel = Globals.WindowsVersion.WinEight;

			InitializeComponent();

			DialogService.Initialize("WCF ARCHITECT", DialogViewer);

			Globals.MainScreen = this;

			//Initialize the Home screen.
			RefreshRecentList();

			//Initialize the Options screen.
			UserProfile = Globals.UserProfile;
			if (Globals.UserProfile.AutomaticBackupsEnabled == true) AutomaticBackupsEnabled.Content = "Yes";
			AboutVersion.Content = "Version " + System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileVersion;
#if TRIAL
			ProductTitle.Content = "Prospective Software WCF Architect Trial Edition";
#elif WINRT
			ProductTitle.Content = "Prospective Software WCF Architect for Windows Runtime";
#elif PROFESSIONAL
			ProductTitle.Content = "Prospective Software WCF Architect Professional";
#else
			ProductTitle.Content = "Prospective Software WCF Architect Developer";
#endif

			//Set the logo in the options screen.
			BitmapImage logo = new BitmapImage();
			logo.BeginInit();
#if TRIAL
			logo.UriSource = new Uri("pack://application:,,,/WCFArchitect;component/Icons/Odd/FullLogoTrial.png");
#elif WINRT
			logo.UriSource = new Uri("pack://application:,,,/WCFArchitect;component/Icons/Odd/FullLogoWinRT.png");
#elif PROFESSIONAL
			logo.UriSource = new Uri("pack://application:,,,/WCFArchitect;component/Icons/Odd/FullLogoProfessional.png");
#else
			logo.UriSource = new Uri("pack://application:,,,/WCFArchitect;component/Icons/Odd/FullLogoDeveloper.png");
#endif
			logo.EndInit();
			SKULevel.Source = logo;
		}

		#region - Window Events -

		private void Main_SourceInitialized(object sender, EventArgs e)
		{
		}

		private void Main_StateChanged(object sender, EventArgs e)
		{
			if (this.WindowState == System.Windows.WindowState.Maximized)
			{
				if (Globals.WindowsLevel == Globals.WindowsVersion.WinVista || Globals.WindowsLevel == Globals.WindowsVersion.WinSeven)
				{
					WindowBorder.Margin = new Thickness(5, 4, 5, 5);
				}
				else if (Globals.WindowsLevel == Globals.WindowsVersion.WinEight)
				{
					WindowBorder.Margin = new Thickness(8);
				}
				WindowBorder.BorderThickness = new Thickness(0);
				MaximizeWindow.Visibility = System.Windows.Visibility.Collapsed;
				RestoreWindow.Visibility = System.Windows.Visibility.Visible;
			}
			if (this.WindowState == System.Windows.WindowState.Normal)
			{
				WindowBorder.Margin = new Thickness(0);
				WindowBorder.BorderThickness = new Thickness(1);
				MaximizeWindow.Visibility = System.Windows.Visibility.Visible;
				RestoreWindow.Visibility = System.Windows.Visibility.Collapsed;
			}
		}

		private void Main_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
		}

		private void Main_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				e.Handled = true;
			}
		}

		private void CloseWindow_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void MinimizeWindow_Click(object sender, RoutedEventArgs e)
		{
			this.WindowState = System.Windows.WindowState.Minimized;
		}

		private void MaximizeWindow_Click(object sender, RoutedEventArgs e)
		{
			this.WindowState = System.Windows.WindowState.Maximized;
		}

		private void RestoreWindow_Click(object sender, RoutedEventArgs e)
		{
			this.WindowState = System.Windows.WindowState.Normal;
		}

		#endregion

		#region - System Menu -

		public void ShowHomeScreen()
		{
			SystemMenuHome_Click(null, null);
		}

		private void SystemMenuHome_Click(object sender, RoutedEventArgs e)
		{
			ActiveProjectScreen.Visibility = System.Windows.Visibility.Collapsed;
			HomeScreen.Visibility = System.Windows.Visibility.Visible;
			OptionsScreen.Visibility = System.Windows.Visibility.Collapsed;
		}

		private void SystemMenu_Click(object sender, RoutedEventArgs e)
		{
			SystemMenu.ContextMenu.PlacementTarget = SystemMenu;
			SystemMenu.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
			SystemMenu.ContextMenu.IsOpen = true;
		}

		private void SystemMenuOpen_Click(object sender, RoutedEventArgs e)
		{
		}

		private void SystemMenuSave_Click(object sender, RoutedEventArgs e)
		{
			Globals.SaveSolution();
		}

		private void SystemMenuSaveAs_Click(object sender, RoutedEventArgs e)
		{
		}

		private void SystemMenuClose_Click(object sender, RoutedEventArgs e)
		{
			CloseSolution(true);
		}

		private void SystemMenuOptions_Click(object sender, RoutedEventArgs e)
		{
			ActiveProjectScreen.Visibility = System.Windows.Visibility.Collapsed;
			HomeScreen.Visibility = System.Windows.Visibility.Collapsed;
			OptionsScreen.Visibility = System.Windows.Visibility.Visible;
		}

		private void SystemMenuHelp_Click(object sender, RoutedEventArgs e)
		{
		}

		private void SystemMenuExit_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		#endregion

		#region - Projects -

		internal void Projects_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
			{
				ScreenButtons.Items.Add(new SolutionItem(e.NewItems[0] as Projects.Project));
			}
			if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
			{
				foreach(SolutionItem pi in ScreenButtons.Items)
					if (pi.Project == e.OldItems[0])
					{
						ScreenButtons.Items.Remove(pi);
						break;
					}
			}
		}

		private static void OnSelectProjectCommandExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			Navigator t = e.Parameter as Navigator;
			if (t == null) return;
			Main s = sender as Main;
			if (s == null) return;

			s.SelectProjectScreen(t);
		}

		internal void SelectProjectScreen(Navigator NewScreen)
		{
			SelectedProject = NewScreen;
			ActiveProjectScreen.Visibility = System.Windows.Visibility.Visible;
			HomeScreen.Visibility = Visibility.Collapsed;
			OptionsScreen.Visibility = Visibility.Collapsed;

			foreach (SolutionItem pi in ScreenButtons.Items)
			{
				pi.IsSelected = false;
				if (pi.Project == NewScreen.Project)
					pi.IsSelected = true;
			}
		}

		#endregion

		#region - Home -

		private void NewSolution_Click(object sender, RoutedEventArgs e)
		{
			Dialogs.NewSolution np = new Dialogs.NewSolution();
			DialogService.ShowContentDialog(null, "New Solution", np, new DialogAction("Create", new Action(() => np.Create()), true), new DialogAction("Cancel", false, true));
		}

		private void AddProject_Click(object sender, RoutedEventArgs e)
		{
			Dialogs.NewProject np = new Dialogs.NewProject(typeof(Projects.Project));
			DialogService.ShowContentDialog(null, "New Project", np, new DialogAction("Create", new Action(() => np.Create()), true), new DialogAction("Cancel", false, true));
		}

		private void UpdateYes_Click(object sender, RoutedEventArgs e)
		{
			System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(Globals.NewVersionPath));
			Application.Current.Shutdown(0);
		}

		private void UpdateNo_Click(object sender, RoutedEventArgs e)
		{
			UpdateAvailable.Visibility = System.Windows.Visibility.Collapsed;
		}

		public void RefreshRecentList()
		{
			if (Globals.UserProfile.ImportantProjects.Count > 0) Globals.UserProfile.ImportantProjects.Sort(delegate(WCFArchitect.Options.RecentSolution p1, WCFArchitect.Options.RecentSolution p2) { return p2.LastAccessed.CompareTo(p1.LastAccessed); });
			if (Globals.UserProfile.RecentProjects.Count > 0) Globals.UserProfile.RecentProjects.Sort(delegate(WCFArchitect.Options.RecentSolution p1, WCFArchitect.Options.RecentSolution p2) { return p2.LastAccessed.CompareTo(p1.LastAccessed); });

			ImportantProjectsBlock.Visibility = System.Windows.Visibility.Collapsed;
			ImportantProjectsList.Children.Clear();
			RecentProjectsList.Children.Clear();

			foreach (WCFArchitect.Options.RecentSolution RP in Globals.UserProfile.ImportantProjects)
			{
				ImportantProjectsBlock.Visibility = System.Windows.Visibility.Visible;
				RecentProjectItem NRPI = new RecentProjectItem(RP, true);
				ImportantProjectsList.Children.Add(NRPI);
			}
			foreach (WCFArchitect.Options.RecentSolution RP in Globals.UserProfile.RecentProjects)
			{
				RecentProjectItem NRPI = new RecentProjectItem(RP, false);
				RecentProjectsList.Children.Add(NRPI);
			}
		}

		#endregion

		#region - Options -

		private void DefaultProjectFolderBrowse_Click(object sender, RoutedEventArgs e)
		{
			string openpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			if (!(Globals.UserProfile.DefaultProjectFolder == "" || Globals.UserProfile.DefaultProjectFolder == null)) openpath = Globals.UserProfile.DefaultProjectFolder;

			Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog ofd = new Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog("Select Default Project Directory");
			ofd.AllowNonFileSystemItems = false;
			ofd.EnsurePathExists = true;
			ofd.IsFolderPicker = true;
			ofd.InitialDirectory = openpath;
			ofd.Multiselect = false;
			ofd.ShowPlacesList = true;
			if (ofd.ShowDialog() == Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Cancel) return;

			Globals.UserProfile.DefaultProjectFolder = ofd.FileName;
			DefaultProjectFolder.Text = Globals.UserProfile.DefaultProjectFolder;
		}

		private void MILink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
		{
			string navigateUri = MILink.NavigateUri.ToString();
			Process.Start(new ProcessStartInfo(navigateUri));
			e.Handled = true;
		}

		private void OILLink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
		{
			string navigateUri = OILLink.NavigateUri.ToString();
			Process.Start(new ProcessStartInfo(navigateUri));
			e.Handled = true;
		}

		private void SLink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
		{
			string navigateUri = SLink.NavigateUri.ToString();
			Process.Start(new ProcessStartInfo(navigateUri));
			e.Handled = true;
		}

		private void AutomaticBackupsEnabled_Checked(object sender, RoutedEventArgs e)
		{
			if (IsLoaded == false) return;
			AutomaticBackupsEnabled.Content = "Yes";
			AutomaticBackupsInterval.IsEnabled = true;
		}

		private void AutomaticBackupsEnabled_Unchecked(object sender, RoutedEventArgs e)
		{
			if (IsLoaded == false) return;
			AutomaticBackupsEnabled.Content = "No";
			AutomaticBackupsInterval.IsEnabled = false;
		}

		private void AutomaticBackupsInterval_ValueChanged(object sender, C1.WPF.DateTimeEditors.NullablePropertyChangedEventArgs<TimeSpan> e)
		{
			if (IsLoaded == false) return;
			if (e.NewValue.HasValue == false) { UserProfile.AutomaticBackupsInterval = new TimeSpan(0, 5, 0); return; }
			if (e.NewValue.Value.TotalMinutes < 1) { UserProfile.AutomaticBackupsInterval = new TimeSpan(0, 1, 0); return; }
			UserProfile.AutomaticBackupsInterval = e.NewValue.Value;
			Globals.BackupTimer.Dispose();
			Globals.BackupTimer = new System.Threading.Timer(new System.Threading.TimerCallback(Globals.BackupSolution), null, (long)UserProfile.AutomaticBackupsInterval.TotalMilliseconds, (long)UserProfile.AutomaticBackupsInterval.TotalMilliseconds);
		}

		#endregion

		#region - Solutions - 
		
		public void NewSolution(string Name, string Path)
		{
			if (Globals.Solution != null)
				CloseSolution(true, false, new Action(() => { Projects.Solution.Save(new Projects.Solution(Name), Path); Globals.OpenSolution(Path, OpenSolutionFinished); }), new Action(() => { Projects.Solution.Save(new Projects.Solution(Name), Path); Globals.OpenSolution(Path, OpenSolutionFinished); }));
			else
			{
				Projects.Solution.Save(new Projects.Solution(Name), Path); 
				Globals.OpenSolution(Path, OpenSolutionFinished);
			}
		}

		public void NewProject(string Name, string Path)
		{
			Projects.Project NP = new Projects.Project(Name);
			Globals.Solution.Projects.Add(Globals.GetRelativePath(Globals.SolutionPath, Path));
			Projects.Project.Save(NP, Path);
			Globals.Projects.Add(Projects.Project.Open(Globals.SolutionPath, Path));
			Globals.SaveSolution();
		}

		public void OpenSolution(string Path)
		{
			if (Globals.Solution != null)
				CloseSolution(true, false, new Action(() => { Globals.OpenSolution(Path, OpenSolutionFinished); }), new Action(() => { Globals.OpenSolution(Path, OpenSolutionFinished); }));
			else
				Globals.OpenSolution(Path, OpenSolutionFinished);
		}

		public void OpenSolutionFinished(bool Success)
		{
			if (Success == false) return;

			//Determine if there is already a recent entry. Create one if false. Update the current one if true.
			bool ProjectHasRecentEntry = false;
			foreach (WCFArchitect.Options.RecentSolution RP in Globals.UserProfile.ImportantProjects)
				if (RP.Path == Globals.SolutionPath)
				{
					ProjectHasRecentEntry = true;
					RP.LastAccessed = DateTime.Now;
					Globals.SolutionInfo = RP;
				}
			foreach (WCFArchitect.Options.RecentSolution RP in Globals.UserProfile.RecentProjects)
				if (RP.Path == Globals.SolutionPath)
				{
					ProjectHasRecentEntry = true;
					RP.LastAccessed = DateTime.Now;
					Globals.SolutionInfo = RP;
				}
			if (ProjectHasRecentEntry == false)
			{
				Options.RecentSolution NRP = new Options.RecentSolution(Globals.Solution.Name, Globals.SolutionPath);
				Globals.UserProfile.RecentProjects.Add(NRP);
				Globals.SolutionInfo = NRP;
			}

			RefreshRecentList();

			//Select the first screen if any project were loaded.
			if (Globals.Projects.Count > 0)
			{
				SolutionItem t = ScreenButtons.Items[0] as SolutionItem;
				if (t != null)
					SelectProjectScreen(t.Content as Navigator);
			}

			AddProject.IsEnabled = true;
			SystemMenuSave.IsEnabled = true;
			SystemMenuSaveAs.IsEnabled = true;
			SystemMenuClose.IsEnabled = true;
			this.Title = Globals.Solution.Name + " - Prospective Software WCF Architect";
		}

		public void CloseSolution(bool AskBeforeClose = false, bool Closing = false, Action ContinueYes = null, Action ContinueNo = null)
		{
			Globals.IsClosing = true;

			if (Globals.Solution != null)
			{
				if (AskBeforeClose == true)
				{
					DialogService.ShowMessageDialog(null, "Continue?", "In order to perform the requested action, the current project will be saved and closed. Would you like to continue?", new DialogAction("Yes", new Action(() => { Globals.CloseSolution(true); CloseSolutionFinished(); if (ContinueYes != null) ContinueYes(); }), true), new DialogAction("No", new Action(() => { Globals.CloseSolution(false); CloseSolutionFinished(); if (ContinueYes != null) ContinueNo(); })), new DialogAction("Cancel", false, true));
				}
				else
				{
					if (Closing == true)
					{
						DialogService.ShowMessageDialog(null, "Save Solution?", "Would you like to save your work?", new DialogAction("Yes", new Action(() => { Globals.CloseSolution(true); CloseSolutionFinished(); if (ContinueYes != null) ContinueYes(); }), true), new DialogAction("No", new Action(() => { Globals.CloseSolution(false); CloseSolutionFinished(); if (ContinueYes != null) ContinueNo(); })), new DialogAction("Cancel", false, true));
					}
					else
					{
						Globals.CloseSolution(true);
						CloseSolutionFinished();
					}
				}
			}
		}

		public void CloseSolutionFinished()
		{
			Globals.SolutionInfo = null;
			AddProject.IsEnabled = false;
			SystemMenuSave.IsEnabled = false;
			SystemMenuSaveAs.IsEnabled = false;
			SystemMenuClose.IsEnabled = false;
			this.Title = "Prospective Software WCF Architect";

			if (!(Globals.SolutionPath == null || Globals.SolutionPath == ""))
				System.IO.File.Delete(System.IO.Path.ChangeExtension(Globals.SolutionPath, ".bak"));

			Globals.Solution = null;

			Globals.IsClosing = false;
		}

		#endregion
	}

	internal partial class SolutionItem : Button
	{
		public bool IsSelected { get { return (bool)GetValue(IsSelectedProperty); } set { SetValue(IsSelectedProperty, value); } }
		public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(SolutionItem), new PropertyMetadata(false));

		public object Header { get { return (object)GetValue(HeaderProperty); } set { SetValue(HeaderProperty, value); } }
		public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(SolutionItem), new PropertyMetadata(new object()));

		internal Projects.Project Project { get; private set; }

		public SolutionItem(Projects.Project Project)
		{
			this.Project = Project;
			this.Header = Project.Name.ToUpper();
			this.Content = new Navigator(Project);
		}
	}

	internal partial class RecentProjectItem : Control
	{
		public WCFArchitect.Options.RecentSolution Data;
		public bool IsImportant = false;

		public string ItemTitle { get { return (string)GetValue(ItemTitleProperty); } set { SetValue(ItemTitleProperty, value); } }
		public static readonly DependencyProperty ItemTitleProperty = DependencyProperty.Register("ItemTitle", typeof(string), typeof(RecentProjectItem), new PropertyMetadata(""));

		public string ItemPath { get { return (string)GetValue(ItemPathProperty); } set { SetValue(ItemPathProperty, value); } }
		public static readonly DependencyProperty ItemPathProperty = DependencyProperty.Register("ItemPath", typeof(string), typeof(RecentProjectItem), new PropertyMetadata(""));

		public string ItemFlag { get { return (string)GetValue(ItemFlagProperty); } set { SetValue(ItemFlagProperty, value); } }
		public static readonly DependencyProperty ItemFlagProperty = DependencyProperty.Register("ItemFlag", typeof(string), typeof(RecentProjectItem), new PropertyMetadata("pack://application:,,,/WCFArchitect;component/Icons/X32/FlagRecent.png"));

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211")] public static RoutedCommand OpenCommand = new RoutedCommand();
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211")] public static RoutedCommand FlagCommand = new RoutedCommand();
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211")] public static RoutedCommand FolderCommand = new RoutedCommand();
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211")] public static RoutedCommand RemoveCommand = new RoutedCommand();

		static RecentProjectItem()
		{
			CommandManager.RegisterClassCommandBinding(typeof(RecentProjectItem), new CommandBinding(RecentProjectItem.OpenCommand, OnOpenCommandExecuted));
			CommandManager.RegisterClassCommandBinding(typeof(RecentProjectItem), new CommandBinding(RecentProjectItem.FlagCommand, OnFlagCommandExecuted));
			CommandManager.RegisterClassCommandBinding(typeof(RecentProjectItem), new CommandBinding(RecentProjectItem.FolderCommand, OnFolderCommandExecuted));
			CommandManager.RegisterClassCommandBinding(typeof(RecentProjectItem), new CommandBinding(RecentProjectItem.RemoveCommand, OnRemoveCommandExecuted));
		}

		private static void OnRemoveCommandExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			RecentProjectItem t = e.Parameter as RecentProjectItem;
			if (t == null) return;

			t.CMRemove();
		}

		private static void OnFolderCommandExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			RecentProjectItem t = e.Parameter as RecentProjectItem;
			if (t == null) return;

			t.CMOpenFolder();
		}

		private static void OnFlagCommandExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			RecentProjectItem t = e.Parameter as RecentProjectItem;
			if (t == null) return;

			t.FlagImportant();
		}

		private static void OnOpenCommandExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			RecentProjectItem t = e.Parameter as RecentProjectItem;
			if (t == null) return;

			t.OpenProject();
		}

		public RecentProjectItem(WCFArchitect.Options.RecentSolution Data, bool IsImportant)
		{
			this.Data = Data;
			this.IsImportant = IsImportant;

			ItemTitle = Data.Name;
			ItemPath = Data.Path;

			if (IsImportant == true) ItemFlag = "pack://application:,,,/WCFArchitect;component/Icons/X32/FlagImportant.png";
		}

		private void FlagImportant()
		{
			if (IsImportant == true)
			{
				Globals.UserProfile.ImportantProjects.Remove(Data);
				Globals.UserProfile.RecentProjects.Add(Data);

				ItemFlag = "pack://application:,,,/WCFArchitect;component/Icons/X32/FlagImportant.png";
			}
			else
			{
				Globals.UserProfile.RecentProjects.Remove(Data);
				Globals.UserProfile.ImportantProjects.Add(Data);

				ItemFlag = "pack://application:,,,/WCFArchitect;component/Icons/X32/FlagRecent.png";
			}

			Globals.MainScreen.RefreshRecentList();
		}

		private void OpenProject()
		{
			if (!System.IO.File.Exists(Data.Path))
			{
				DialogService.ShowMessageDialog(null, "Unable to Locate Project File.", "Unable to located the requested file, would you like to remove this project from the list?",
					new DialogAction("Yes", new Action(() => { if (IsImportant == true) { Globals.UserProfile.ImportantProjects.Remove(Data); } else { Globals.UserProfile.RecentProjects.Remove(Data); } Globals.MainScreen.RefreshRecentList(); }), true, false), new DialogAction("No", false, true));
				return;
			}

			Data.LastAccessed = DateTime.Now;
			Globals.SolutionInfo = Data;
			Globals.MainScreen.OpenSolution(Data.Path);
		}

		private void CMOpenFolder()
		{
			System.Diagnostics.ProcessStartInfo PSI = new System.Diagnostics.ProcessStartInfo(System.IO.Path.GetDirectoryName(Data.Path));
			PSI.UseShellExecute = true;
			System.Diagnostics.Process.Start(PSI);
		}

		private void CMRemove()
		{
			if (IsImportant == true)
			{
				Globals.UserProfile.ImportantProjects.Remove(Data);
			}
			else
			{
				Globals.UserProfile.RecentProjects.Remove(Data);
			}
			Globals.MainScreen.RefreshRecentList();
		}
	}
}