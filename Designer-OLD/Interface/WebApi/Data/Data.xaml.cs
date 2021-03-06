﻿﻿using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using EllipticBit.Controls.WPF.Dialogs;
using NETPath.Projects.Helpers;
using NETPath.Projects.WebApi;

namespace NETPath.Interface.WebApi.Data
{
	internal partial class Data : ContentControl
	{
		public WebApiData OpenType { get { return (WebApiData)GetValue(OpenTypeProperty); } set { SetValue(OpenTypeProperty, value); } }
		public static readonly DependencyProperty OpenTypeProperty = DependencyProperty.Register("OpenType", typeof(WebApiData), typeof(Data));

		public object ActiveElement { get { return GetValue(ActiveElementProperty); } set { SetValue(ActiveElementProperty, value); } }
		public static readonly DependencyProperty ActiveElementProperty = DependencyProperty.Register("ActiveElement", typeof(object), typeof(Data));

		public Data() { }

		public Data(WebApiData Data)
		{
			OpenType = Data;

			InitializeComponent();

			WebApiDataElement t = OpenType.Elements.FirstOrDefault(a => a.IsSelected);
			if (t != null) ValuesList.SelectedItem = t;
		}

		private void AddMemberType_Selected(object Sender, RoutedEventArgs E)
		{
			AddMember.IsEnabled = (!string.IsNullOrEmpty(AddMemberName.Text) && !AddMemberName.IsInvalid && AddMemberType.IsValid);
			AddMemberName.Focus();
		}

		private void AddMemberName_Validate(object sender, EllipticBit.Controls.WPF.ValidateEventArgs e)
		{
			e.IsValid = true;
			if (string.IsNullOrEmpty(AddMemberName.Text)) return;

			e.IsValid = RegExs.MatchCodeName.IsMatch(AddMemberName.Text);
			AddMember.IsEnabled = (!string.IsNullOrEmpty(AddMemberName.Text) &&  e.IsValid && AddMemberType.IsValid);
		}

		private void AddMemberName_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				if (!string.IsNullOrEmpty(AddMemberName.Text))
					AddMember_Click(sender, null);
		}

		private void AddMemberName_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (AddMemberName.IsInvalid == false)
			{
				AddMember.IsEnabled = false;
				return;
			}
			if (AddMemberType.OpenType != null && AddMemberName.Text != "") AddMember.IsEnabled = true;
			else AddMember.IsEnabled = false;
		}

		private void AddMember_Click(object sender, RoutedEventArgs e)
		{
			if (AddMember.IsEnabled == false) return;

			var t = new WebApiDataElement(AddMemberType.OpenType, AddMemberName.Text, OpenType);
			OpenType.Elements.Add(t);

			AddMemberType.Focus();
			AddMemberType.OpenType = null;
			AddMemberName.Text = "";

			ValuesList.SelectedItem = t;
		}

		private void ValuesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var t = ValuesList.SelectedItem as WebApiDataElement;
			if (t == null) return;

			//Set the new item as selected.
			foreach (WebApiDataElement de in OpenType.Elements)
				de.IsSelected = false;
			t.IsSelected = true;

			ActiveElement = new DataElement(t);
		}

		private void DeleteElement_Click(object sender, RoutedEventArgs e)
		{
			var lbi = Globals.GetVisualParent<ListBoxItem>(sender);
			var op = lbi.Content as WebApiDataElement;
			if (op == null) return;

			DialogService.ShowMessageDialog("NETPath", "Delete Data Member?", "Are you sure you wish to delete the '" + op.DataType + " " + op.DataName + "' data member?", new DialogAction("Yes", () => { ActiveElement = null; OpenType.Elements.Remove(op); }, true), new DialogAction("No", false, true));
		}

		#region - Drag/Drop Support -

		private int dragItemStartIndex;
		private int dragItemNewIndex;
		private WebApiDataElement dragElement;
		private Point dragStartPos;
		private Themes.DragAdorner dragAdorner;
		private AdornerLayer dragLayer;
		public bool IsDragging { get; set; }
		public bool DragHasLeftScope { get; set; }

		private void ValuesList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			dragStartPos = e.GetPosition(null);
		}

		private void ValuesList_PreviewMouseMove(object sender, MouseEventArgs e)
		{
			if (e.LeftButton != MouseButtonState.Pressed || IsDragging) return;
			Point position = e.GetPosition(null);

			if (Math.Abs(position.X - dragStartPos.X) > SystemParameters.MinimumHorizontalDragDistance || Math.Abs(position.Y - dragStartPos.Y) > SystemParameters.MinimumVerticalDragDistance)
				StartValueDrag(e);
		}

		private void StartValueDrag(MouseEventArgs e)
		{
			//Here we create our adorner.. 
			dragElement = ValuesList.SelectedItem as WebApiDataElement;
			dragItemStartIndex = ValuesList.SelectedIndex;
			var tuie = (UIElement)ValuesList.ItemContainerGenerator.ContainerFromItem(ValuesList.SelectedItem);
			if (tuie == null) return;
			try { dragAdorner = new Themes.DragAdorner(ValuesList, tuie, true, 0.5); }
			catch { return; }
			dragLayer = AdornerLayer.GetAdornerLayer(ValuesList as Visual);
			dragLayer.Add(dragAdorner);

			IsDragging = true;
			DragHasLeftScope = false;

			var data = new DataObject(DataFormats.Text.ToString(), "");
			DragDropEffects de = DragDrop.DoDragDrop(ValuesList, data, DragDropEffects.Move);

			// Clean up our mess
			AdornerLayer.GetAdornerLayer(ValuesList).Remove(dragAdorner);
			dragAdorner = null;

			IsDragging = false;
		}

		private void ValuesList_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
		{
			if (!DragHasLeftScope) return;
			e.Action = DragAction.Cancel;
			e.Handled = true;
		}

		private void ValuesList_DragLeave(object sender, DragEventArgs e)
		{
			if (e.OriginalSource != ValuesList) return;
			Point p = e.GetPosition(ValuesList);
			Rect r = VisualTreeHelper.GetContentBounds(ValuesList);

			if (r.Contains(p)) return;
			DragHasLeftScope = true;
			e.Handled = true;

			foreach (var lbi in OpenType.Elements.Select(o => (ListBoxItem)ValuesList.ItemContainerGenerator.ContainerFromItem(o)).Where(lbi => lbi != null))
				lbi.Margin = new Thickness(0);
		}

		private void ValuesList_PreviewDragOver(object sender, DragEventArgs e)
		{
			if (dragAdorner != null)
			{
				dragAdorner.LeftOffset = e.GetPosition(ValuesList).X;
				dragAdorner.TopOffset = e.GetPosition(ValuesList).Y;
			}

			Point mp = e.GetPosition(ValuesList);
			HitTestResult htr = VisualTreeHelper.HitTest(ValuesList, mp);
			if (htr == null) return;

			foreach (WebApiDataElement o in OpenType.Elements)
			{
				var lbi = (ListBoxItem)ValuesList.ItemContainerGenerator.ContainerFromItem(o);
				var lbiht = Globals.GetVisualParent<ListBoxItem>(htr.VisualHit);
				if (lbi == null) continue;
				if (lbiht == null) continue;
				if (lbi != lbiht)
					lbi.Margin = new Thickness(0);
				else
				{
					if (dragAdorner != null) lbi.Margin = new Thickness(0, dragAdorner.ActualHeight, 0, 0);
					dragItemNewIndex = ValuesList.ItemContainerGenerator.IndexFromContainer(lbi);
				}
			}
		}

		private void ValuesList_Drop(object sender, DragEventArgs e)
		{
			OpenType.Elements.Move(dragItemStartIndex, dragItemNewIndex);

			foreach (var lbi in OpenType.Elements.Select(o => (ListBoxItem)ValuesList.ItemContainerGenerator.ContainerFromItem(o)).Where(lbi => lbi != null))
				lbi.Margin = new Thickness(0);
		}

		private ContentPresenter GetListBoxItemPresenter()
		{
			if (ValuesList.SelectedItem == null) return null;
			var lbi = ValuesList.ItemContainerGenerator.ContainerFromIndex(ValuesList.SelectedIndex) as ListBoxItem;
			return Globals.GetVisualChild<ContentPresenter>(lbi);
		}

		#endregion
	}

	[ValueConversion(typeof(int), typeof(string))]
	public class ElementOrderConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var v = (int)value;
			return v < 0 ? "" : v.ToString(CultureInfo.InvariantCulture);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var v = (string)value;
			if (string.IsNullOrEmpty(v)) return -1;
			try { return System.Convert.ToInt32(v); }
			catch { return -1; }
		}
	}
}