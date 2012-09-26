﻿using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Runtime.Serialization;

namespace WCFArchitect.Projects
{
	public enum DataScope
	{
		Public,
		Protected,
		Private,
		Internal,
		ProtectedInternal,
	}

	public class Data : DataType
	{
		public bool HasXAMLType { get { return (bool)GetValue(HasXAMLTypeProperty); } set { SetValue(HasXAMLTypeProperty, value); } }
		public static readonly DependencyProperty HasXAMLTypeProperty = DependencyProperty.Register("HasXAMLType", typeof(bool), typeof(Data), new PropertyMetadata(true, HasXAMLTypeChangedCallback));

		private static void HasXAMLTypeChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			var t = o as Data;
			if(t==null) return;

			if (Convert.ToBoolean(e.NewValue) == false)
				t.XAMLType = null; 
			else
			{
				if (t.XAMLType == null)
				{
					t.XAMLType = Convert.ToBoolean(e.NewValue) == false ? null : new DataType(t.TypeMode) {ID = t.ID, Name = t.Name, Scope = t.Scope, Partial = t.Partial, Abstract = t.Abstract, Sealed = t.Sealed};
					if (t.XAMLType != null) t.XAMLType.InheritedTypes.Add(new DataType("DependencyObject", DataTypeMode.Class));
				}
			}
		}

		public DataType XAMLType { get { return (DataType)GetValue(XAMLTypeProperty); } set { SetValue(XAMLTypeProperty, value); } }
		public static readonly DependencyProperty XAMLTypeProperty = DependencyProperty.Register("XAMLType", typeof(DataType), typeof(Data));

		public bool IsReference { get { return (bool)GetValue(IsReferenceProperty); } set { SetValue(IsReferenceProperty, value); } }
		public static readonly DependencyProperty IsReferenceProperty = DependencyProperty.Register("IsReference", typeof(bool), typeof(Data));

		public ObservableCollection<DataElement> Elements { get { return (ObservableCollection<DataElement>)GetValue(ElementsProperty); } set { SetValue(ElementsProperty, value); } }
		public static readonly DependencyProperty ElementsProperty = DependencyProperty.Register("Elements", typeof(ObservableCollection<DataElement>), typeof(Data));

		//Internal Use - Searching / Filtering
		[IgnoreDataMember] public bool IsSearching { get { return (bool)GetValue(IsSearchingProperty); } set { SetValue(IsSearchingProperty, value); } }
		public static readonly DependencyProperty IsSearchingProperty = DependencyProperty.Register("IsSearching", typeof(bool), typeof(Data));

		[IgnoreDataMember] public bool IsSearchMatch { get { return (bool)GetValue(IsSearchMatchProperty); } set { SetValue(IsSearchMatchProperty, value); } }
		public static readonly DependencyProperty IsSearchMatchProperty = DependencyProperty.Register("IsSearchMatch", typeof(bool), typeof(Data));

		[IgnoreDataMember] public bool IsFiltering { get { return (bool)GetValue(IsFilteringProperty); } set { SetValue(IsFilteringProperty, value); } }
		public static readonly DependencyProperty IsFilteringProperty = DependencyProperty.Register("IsFiltering", typeof(bool), typeof(Data));

		[IgnoreDataMember] public bool IsFilterMatch { get { return (bool)GetValue(IsFilterMatchProperty); } set { SetValue(IsFilterMatchProperty, value); } }
		public static readonly DependencyProperty IsFilterMatchProperty = DependencyProperty.Register("IsFilterMatch", typeof(bool), typeof(Data));

		public bool IsTreeExpanded { get { return (bool)GetValue(IsTreeExpandedProperty); } set { SetValue(IsTreeExpandedProperty, value); } }
		public static readonly DependencyProperty IsTreeExpandedProperty = DependencyProperty.Register("IsTreeExpanded", typeof(bool), typeof(Data));

		[IgnoreDataMember] public bool HasWinFormsBindings { get { return Elements.Any(a => a.GenerateWinFormsSupport); } }

		public Data() : base(DataTypeMode.Class)
		{
		}

		public Data(string Name, Namespace Parent) : base(DataTypeMode.Class)
		{
			this.Elements = new ObservableCollection<DataElement>();
			this.ID = Guid.NewGuid();
			this.Name = Name;
			this.Name = Helpers.RegExs.ReplaceSpaces.Replace(Name, @"");
			this.HasClientType = false;
			this.HasXAMLType = true;
			this.XAMLType = new DataType(DataTypeMode.Class) { Name = this.Name + "XAML", Scope = this.Scope };
			this.XAMLType.InheritedTypes.Add(new DataType("DependencyObject", DataTypeMode.Class));
			this.Parent = Parent;
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);

			if (e.Property == IsDirtyProperty) return;
			if (e.Property == IsSearchingProperty) return;
			if (e.Property == IsSearchMatchProperty) return;
			if (e.Property == IsFilteringProperty) return;
			if (e.Property == IsFilterMatchProperty) return;
			if (e.Property == IsTreeExpandedProperty) return;

			IsDirty = true;
		}

		public void Search(string Value)
		{
			if (Value != "")
			{
				foreach (DataElement T in Elements)
				{
					T.IsSearching = true;
					T.IsSearchMatch = true;
					if (T.DataType.Name != "" && T.DataType.Name.IndexOf(Value, StringComparison.InvariantCultureIgnoreCase) < 0) T.IsSearchMatch = false;
					if (IsSearchMatch == false) IsSearchMatch = T.IsSearchMatch;
				}
			}
			else
			{
				foreach (DataElement T in Elements)
				{
					T.IsSearching = false;
					T.IsSearchMatch = false;
				}
			}
		}

		public List<FindReplaceResult> FindReplace(FindReplaceInfo Args)
		{
			List<FindReplaceResult> results = new List<FindReplaceResult>();

			if (Args.Items == FindItems.Data || Args.Items == FindItems.Any)
			{
				if (Args.UseRegex == false)
				{
					if (Args.MatchCase == false)
					{
						if (Args.IsDataType == false)
						{
							if (Name != null && Name != "") if (Name.IndexOf(Args.Search, StringComparison.InvariantCultureIgnoreCase) >= 0) results.Add(new FindReplaceResult("Name", Name, Parent.Owner, this));
							if (Name != null && Name != "") if (Name.IndexOf(Args.Search, StringComparison.InvariantCultureIgnoreCase) >= 0) results.Add(new FindReplaceResult("Code Name", Name, Parent.Owner, this));
							if (ClientType.Name != null && ClientType.Name != "") if (ClientType.Name.IndexOf(Args.Search, StringComparison.InvariantCultureIgnoreCase) >= 0) results.Add(new FindReplaceResult("Contract Name", ClientType.Name, Parent.Owner, this));
							if (XAMLType.Name != null && XAMLType.Name != "") if (XAMLType.Name.IndexOf(Args.Search, StringComparison.InvariantCultureIgnoreCase) >= 0) results.Add(new FindReplaceResult("WPF Name", XAMLType.Name, Parent.Owner, this));
						}
					}
					else
					{
						if (Args.IsDataType == false)
						{
							if (Name != null && Name != "") if (Name.IndexOf(Args.Search) >= 0) results.Add(new FindReplaceResult("Name", Name, Parent.Owner, this));
							if (Name != null && Name != "") if (Name.IndexOf(Args.Search) >= 0) results.Add(new FindReplaceResult("Code Name", Name, Parent.Owner, this));
							if (ClientType.Name != null && ClientType.Name != "") if (ClientType.Name.IndexOf(Args.Search) >= 0) results.Add(new FindReplaceResult("Contract Name", ClientType.Name, Parent.Owner, this));
							if (XAMLType.Name != null && XAMLType.Name != "") if (XAMLType.Name.IndexOf(Args.Search) >= 0) results.Add(new FindReplaceResult("WPF Name", XAMLType.Name, Parent.Owner, this));
						}
					}
				}
				else
				{
					if (Args.IsDataType == false)
					{
						if (Name != null && Name != "") if (Args.RegexSearch.IsMatch(Name)) results.Add(new FindReplaceResult("Name", Name, Parent.Owner, this));
						if (Name != null && Name != "") if (Args.RegexSearch.IsMatch(Name)) results.Add(new FindReplaceResult("Code Name", Name, Parent.Owner, this));
						if (ClientType.Name != null && ClientType.Name != "") if (Args.RegexSearch.IsMatch(ClientType.Name)) results.Add(new FindReplaceResult("Contract Name", ClientType.Name, Parent.Owner, this));
						if (XAMLType.Name != null && XAMLType.Name != "") if (Args.RegexSearch.IsMatch(XAMLType.Name)) results.Add(new FindReplaceResult("WPF Name", XAMLType.Name, Parent.Owner, this));
					}
				}

				if (Args.ReplaceAll == true)
				{
					bool ia = IsActive;
					IsActive = true;
					if (Args.UseRegex == false)
					{
						if (Args.MatchCase == false)
						{
							if (Args.IsDataType == false)
							{
								if (Name != null && Name != "") Name = Microsoft.VisualBasic.Strings.Replace(Name, Args.Search, Args.Replace, 1, -1, Microsoft.VisualBasic.CompareMethod.Text);
								if (Name != null && Name != "") Name = Microsoft.VisualBasic.Strings.Replace(Name, Args.Search, Args.Replace, 1, -1, Microsoft.VisualBasic.CompareMethod.Text);
								if (ClientType.Name != null && ClientType.Name != "") ClientType.Name = Microsoft.VisualBasic.Strings.Replace(ClientType.Name, Args.Search, Args.Replace, 1, -1, Microsoft.VisualBasic.CompareMethod.Text);
								if (XAMLType.Name != null && XAMLType.Name != "") XAMLType.Name = Microsoft.VisualBasic.Strings.Replace(XAMLType.Name, Args.Search, Args.Replace, 1, -1, Microsoft.VisualBasic.CompareMethod.Text);
							}
						}
						else
						{
							if (Args.IsDataType == false)
							{
								if (Name != null && Name != "") Name = Microsoft.VisualBasic.Strings.Replace(Name, Args.Search, Args.Replace, 1, -1, Microsoft.VisualBasic.CompareMethod.Binary);
								if (Name != null && Name != "") Name = Microsoft.VisualBasic.Strings.Replace(Name, Args.Search, Args.Replace, 1, -1, Microsoft.VisualBasic.CompareMethod.Binary);
								if (ClientType.Name != null && ClientType.Name != "") ClientType.Name = Microsoft.VisualBasic.Strings.Replace(ClientType.Name, Args.Search, Args.Replace, 1, -1, Microsoft.VisualBasic.CompareMethod.Binary);
								if (XAMLType.Name != null && XAMLType.Name != "") XAMLType.Name = Microsoft.VisualBasic.Strings.Replace(XAMLType.Name, Args.Search, Args.Replace, 1, -1, Microsoft.VisualBasic.CompareMethod.Binary);
							}
						}
					}
					else
					{
						if (Args.IsDataType == false)
						{
							if (Name != null && Name != "") Name = Args.RegexSearch.Replace(Name, Args.Replace);
							if (Name != null && Name != "") Name = Args.RegexSearch.Replace(Name, Args.Replace);
							if (ClientType.Name != null && ClientType.Name != "") ClientType.Name = Args.RegexSearch.Replace(ClientType.Name, Args.Replace);
							if (XAMLType.Name != null && XAMLType.Name != "") XAMLType.Name = Args.RegexSearch.Replace(XAMLType.Name, Args.Replace);
						}
					}
					IsActive = ia;
				}
			}

			foreach (DataElement DE in Elements)
				results.AddRange(DE.FindReplace(Args));

			return results;
		}

		public void Replace(FindReplaceInfo Args, string Field)
		{
			if (Args.ReplaceAll == true)
			{
				bool ia = IsActive;
				IsActive = true;
				if (Args.UseRegex == false)
				{
					if (Args.MatchCase == false)
					{
						if (Args.IsDataType == false)
						{
							if (Field == "Name") Name = Microsoft.VisualBasic.Strings.Replace(Name, Args.Search, Args.Replace, 1, -1, Microsoft.VisualBasic.CompareMethod.Text);
							if (Field == "Code Name") Name = Microsoft.VisualBasic.Strings.Replace(Name, Args.Search, Args.Replace, 1, -1, Microsoft.VisualBasic.CompareMethod.Text);
							if (Field == "Contract Name") ClientType.Name = Microsoft.VisualBasic.Strings.Replace(ClientType.Name, Args.Search, Args.Replace, 1, -1, Microsoft.VisualBasic.CompareMethod.Text);
							if (Field == "WPF Name") XAMLType.Name = Microsoft.VisualBasic.Strings.Replace(XAMLType.Name, Args.Search, Args.Replace, 1, -1, Microsoft.VisualBasic.CompareMethod.Text);
						}
					}
					else
					{
						if (Args.IsDataType == false)
						{
							if (Field == "Name") Name = Microsoft.VisualBasic.Strings.Replace(Name, Args.Search, Args.Replace, 1, -1, Microsoft.VisualBasic.CompareMethod.Binary);
							if (Field == "Code Name") Name = Microsoft.VisualBasic.Strings.Replace(Name, Args.Search, Args.Replace, 1, -1, Microsoft.VisualBasic.CompareMethod.Binary);
							if (Field == "Contract Name") ClientType.Name = Microsoft.VisualBasic.Strings.Replace(ClientType.Name, Args.Search, Args.Replace, 1, -1, Microsoft.VisualBasic.CompareMethod.Binary);
							if (Field == "WPF Name") XAMLType.Name = Microsoft.VisualBasic.Strings.Replace(XAMLType.Name, Args.Search, Args.Replace, 1, -1, Microsoft.VisualBasic.CompareMethod.Binary);
						}
					}
				}
				else
				{
					if (Args.IsDataType == false)
					{
						if (Field == "Name") Name = Args.RegexSearch.Replace(Name, Args.Replace);
						if (Field == "Code Name") Name = Args.RegexSearch.Replace(Name, Args.Replace);
						if (Field == "Contract Name") ClientType.Name = Args.RegexSearch.Replace(ClientType.Name, Args.Replace);
						if (Field == "WPF Name") XAMLType.Name = Args.RegexSearch.Replace(XAMLType.Name, Args.Replace);
					}
				}
				IsActive = ia;
			}
		}
	}

	public class DataElement : DependencyObject
	{
		public Guid ID { get; set; }

		//Basic Data-Type Settings
		public DataScope DataScope { get { return (DataScope)GetValue(DataScopeProperty); } set { SetValue(DataScopeProperty, value); } }
		public static readonly DependencyProperty DataScopeProperty = DependencyProperty.Register("DataScope", typeof(DataScope), typeof(DataElement), new PropertyMetadata(DataScope.Public));

		public DataType DataType { get { return (DataType)GetValue(DataTypeProperty); } set { SetValue(DataTypeProperty, value); } }
		public static readonly DependencyProperty DataTypeProperty = DependencyProperty.Register("DataType", typeof(DataType), typeof(DataElement));

		public string DataName { get { return (string)GetValue(DataNameProperty); } set { SetValue(DataNameProperty, value); } }
		public static readonly DependencyProperty DataNameProperty = DependencyProperty.Register("DataName", typeof(string), typeof(DataElement), new PropertyMetadata(""));

		public bool HasClientType { get { return (bool)GetValue(HasClientTypeProperty); } set { SetValue(HasClientTypeProperty, value); } }
		public static readonly DependencyProperty HasClientTypeProperty = DependencyProperty.Register("HasClientType", typeof(bool), typeof(DataElement), new PropertyMetadata(false, HasClientTypeChangedCallback));

		private static void HasClientTypeChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			var t = o as DataElement;
			if (t == null) return;

			if (Convert.ToBoolean(e.NewValue))
			{
				t.ClientScope = t.DataScope;
				t.ClientType = t.DataType.Copy();
				t.ClientName = t.DataName;
			}
			else
			{
				t.ClientScope = DataScope.Public;
				t.ClientType = null;
				t.ClientName = "";
				t.IsAttached = false;
			}
		}

		public DataScope ClientScope { get { return (DataScope)GetValue(ClientScopeProperty); } set { SetValue(ClientScopeProperty, value); } }
		public static readonly DependencyProperty ClientScopeProperty = DependencyProperty.Register("ClientScope", typeof(DataScope), typeof(DataElement), new PropertyMetadata(DataScope.Public));

		public DataType ClientType { get { return (DataType)GetValue(ClientTypeProperty); } set { SetValue(ClientTypeProperty, value); } }
		public static readonly DependencyProperty ClientTypeProperty = DependencyProperty.Register("ClientType", typeof(DataType), typeof(DataElement));

		public string ClientName { get { return (string)GetValue(ClientNameProperty); } set { SetValue(ClientNameProperty, value); } }
		public static readonly DependencyProperty ClientNameProperty = DependencyProperty.Register("ClientName", typeof(string), typeof(DataElement), new PropertyMetadata(""));

		public bool HasXAMLType { get { return (bool)GetValue(HasXAMLTypeProperty); } set { SetValue(HasXAMLTypeProperty, value); } }
		public static readonly DependencyProperty HasXAMLTypeProperty = DependencyProperty.Register("HasXAMLType", typeof(bool), typeof(DataElement), new PropertyMetadata(false, HasXAMLTypeChangedCallback));

		private static void HasXAMLTypeChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			var t = o as DataElement;
			if (t == null) return;

			if (Convert.ToBoolean(e.NewValue))
			{
				t.XAMLScope = t.DataScope;
				t.XAMLType = t.DataType.Copy();
				t.XAMLName = t.DataName;
			}
			else
			{
				t.XAMLScope = DataScope.Public;
				t.XAMLType = null;
				t.XAMLName = "";
				t.IsAttached = false;
			}
		}

		public DataScope XAMLScope { get { return (DataScope)GetValue(XAMLScopeProperty); } set { SetValue(XAMLScopeProperty, value); } }
		public static readonly DependencyProperty XAMLScopeProperty = DependencyProperty.Register("XAMLScope", typeof(DataScope), typeof(DataElement), new PropertyMetadata(DataScope.Public));

		public DataType XAMLType { get { return (DataType)GetValue(XAMLTypeProperty); } set { SetValue(XAMLTypeProperty, value); } }
		public static readonly DependencyProperty XAMLTypeProperty = DependencyProperty.Register("XAMLType", typeof(DataType), typeof(DataElement));

		public string XAMLName { get { return (string)GetValue(XAMLNameProperty); } set { SetValue(XAMLNameProperty, value); } }
		public static readonly DependencyProperty XAMLNameProperty = DependencyProperty.Register("XAMLName", typeof(string), typeof(DataElement), new PropertyMetadata(""));

		public bool IsHidden { get { return (bool)GetValue(IsHiddenProperty); } set { SetValue(IsHiddenProperty, value); } }
		public static readonly DependencyProperty IsHiddenProperty = DependencyProperty.Register("IsHidden", typeof(bool), typeof(DataElement), new PropertyMetadata(false));

		public bool IsReadOnly { get { return (bool)GetValue(IsReadOnlyProperty); } set { SetValue(IsReadOnlyProperty, value); } }
		public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(DataElement), new PropertyMetadata(false));

		public bool GenerateWinFormsSupport { get { return (bool)GetValue(GenerateWinFormsSupportProperty); } set { SetValue(GenerateWinFormsSupportProperty, value); } }
		public static readonly DependencyProperty GenerateWinFormsSupportProperty = DependencyProperty.Register("GenerateWinFormsSupport", typeof(bool), typeof(DataElement), new PropertyMetadata(false));

		//DataMember Settings
		public bool IsDataMember { get { return (bool)GetValue(IsDataMemberProperty); } set { SetValue(IsDataMemberProperty, value); } }
		public static readonly DependencyProperty IsDataMemberProperty = DependencyProperty.Register("IsDataMember", typeof(bool), typeof(DataElement), new UIPropertyMetadata(true, IsDataMemberChangedCallback));

		private static void IsDataMemberChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			var t = o as DataElement;
			if (t == null) return;
			if (Convert.ToBoolean(e.NewValue)) return;

			t.IsRequired = false;
			t.EmitDefaultValue = false;
			t.HasClientType = false;
			t.GenerateWinFormsSupport = false;
			t.HasXAMLType = false;
			t.IsAttached = false;
		}

		public bool EmitDefaultValue { get { return (bool)GetValue(EmitDefaultValueProperty); } set { SetValue(EmitDefaultValueProperty, value); } }
		public static readonly DependencyProperty EmitDefaultValueProperty = DependencyProperty.Register("EmitDefaultValue", typeof(bool), typeof(DataElement), new PropertyMetadata(false));

		public bool IsRequired { get { return (bool)GetValue(IsRequiredProperty); } set { SetValue(IsRequiredProperty, value); } }
		public static readonly DependencyProperty IsRequiredProperty = DependencyProperty.Register("IsRequired", typeof(bool), typeof(DataElement), new PropertyMetadata(false));

		public int Order { get { return (int)GetValue(OrderProperty); } set { SetValue(OrderProperty, value); } }
		public static readonly DependencyProperty OrderProperty = DependencyProperty.Register("Order", typeof(int), typeof(DataElement), new PropertyMetadata(-1));

		//WPF Class Settings
		public bool IsAttached { get { return (bool)GetValue(IsAttachedProperty); } set { SetValue(IsAttachedProperty, value); } }
		public static readonly DependencyProperty IsAttachedProperty = DependencyProperty.Register("IsAttached", typeof(bool), typeof(DataElement), new PropertyMetadata(false));

		public bool AttachedBrowsable { get { return (bool)GetValue(AttachedBrowsableProperty); } set { SetValue(AttachedBrowsableProperty, value); } }
		public static readonly DependencyProperty AttachedBrowsableProperty = DependencyProperty.Register("AttachedBrowsable", typeof(bool), typeof(DataElement), new PropertyMetadata(false));

		public bool AttachedBrowsableIncludeDescendants { get { return (bool)GetValue(AttachedBrowsableIncludeDescendantsProperty); } set { SetValue(AttachedBrowsableIncludeDescendantsProperty, value); } }
		public static readonly DependencyProperty AttachedBrowsableIncludeDescendantsProperty = DependencyProperty.Register("AttachedBrowsableIncludeDescendants", typeof(bool), typeof(DataElement), new PropertyMetadata(false));

		public string AttachedTargetTypes { get { return (string)GetValue(AttachedTargetTypesProperty); } set { SetValue(AttachedTargetTypesProperty, value); } }
		public static readonly DependencyProperty AttachedTargetTypesProperty = DependencyProperty.Register("AttachedTargetTypes", typeof(string), typeof(DataElement), new PropertyMetadata(""));

		public string AttachedAttributeTypes { get { return (string)GetValue(AttachedAttributeTypesProperty); } set { SetValue(AttachedAttributeTypesProperty, value); } }
		public static readonly DependencyProperty AttachedAttributeTypesProperty = DependencyProperty.Register("AttachedAttributeTypes", typeof(string), typeof(DataElement), new PropertyMetadata(""));

		//Internal Use
		[IgnoreDataMember] public bool IsSearching { get { return (bool)GetValue(IsSearchingProperty); } set { SetValue(IsSearchingProperty, value); } }
		public static readonly DependencyProperty IsSearchingProperty = DependencyProperty.Register("IsSearching", typeof(bool), typeof(DataElement), new PropertyMetadata(false));

		[IgnoreDataMember] public bool IsSearchMatch { get { return (bool)GetValue(IsSearchMatchProperty); } set { SetValue(IsSearchMatchProperty, value); } }
		public static readonly DependencyProperty IsSearchMatchProperty = DependencyProperty.Register("IsSearchMatch", typeof(bool), typeof(DataElement), new PropertyMetadata(false));

		[IgnoreDataMember] public bool IsFiltering { get { return false; } set { } }
		[IgnoreDataMember] public bool IsFilterMatch { get { return false; } set { } }
		public bool IsTreeExpanded { get { return false; } set { } }
		public Data Owner { get; set; }

		public DataElement()
		{
			ID = Guid.NewGuid();
			DataType = new DataType(PrimitiveTypes.String);
			DataScope = DataScope.Public;
			HasXAMLType = true;
			AttachedTargetTypes = "";
			AttachedAttributeTypes = "";
			EmitDefaultValue = false;
			Order = -1;
		}

		public DataElement(DataScope Scope, DataType DataType, string Name, Data Owner)
		{
			ID = Guid.NewGuid();
			this.DataType = DataType;
			this.Owner = Owner;
			DataName = Name;
			DataScope = Scope;
			HasXAMLType = true;
			AttachedTargetTypes = "";
			AttachedAttributeTypes = "";
			EmitDefaultValue = false;
			Order = -1;
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);

			if (e.Property == OpenableDocument.IsDirtyProperty) return;
			if (e.Property == IsSearchingProperty) return;
			if (e.Property == IsSearchMatchProperty) return;

			if (Owner != null)
				Owner.IsDirty = true;
		}

		public override string ToString()
		{
			return DataName;
		}

		public List<FindReplaceResult> FindReplace(FindReplaceInfo Args)
		{
			var results = new List<FindReplaceResult>();

			if (Args.Items == FindItems.Data || Args.Items == FindItems.Any)
			{
				if (Args.UseRegex == false)
				{
					if (Args.MatchCase == false)
					{
						if (Args.IsDataType == false)
						{
							if (!string.IsNullOrEmpty(DataType.Name)) if (DataType.Name.IndexOf(Args.Search, StringComparison.InvariantCultureIgnoreCase) >= 0) results.Add(new FindReplaceResult("Name", DataType.Name, Owner.Parent.Owner, this));
							if (!string.IsNullOrEmpty(ClientType.Name)) if (ClientType.Name.IndexOf(Args.Search, StringComparison.InvariantCultureIgnoreCase) >= 0) results.Add(new FindReplaceResult("Contract Name", ClientType.Name, Owner.Parent.Owner, this));
							if (!string.IsNullOrEmpty(XAMLType.Name)) if (XAMLType.Name.IndexOf(Args.Search, StringComparison.InvariantCultureIgnoreCase) >= 0) results.Add(new FindReplaceResult("WPF Name", XAMLType.Name, Owner.Parent.Owner, this));
						}
					}
					else
					{
						if (Args.IsDataType == false)
						{
							if (!string.IsNullOrEmpty(DataType.Name)) if (DataType.Name.IndexOf(Args.Search, StringComparison.CurrentCultureIgnoreCase) >= 0) results.Add(new FindReplaceResult("Name", DataType.Name, Owner.Parent.Owner, this));
							if (!string.IsNullOrEmpty(ClientType.Name)) if (ClientType.Name.IndexOf(Args.Search, StringComparison.CurrentCultureIgnoreCase) >= 0) results.Add(new FindReplaceResult("Contract Name", ClientType.Name, Owner.Parent.Owner, this));
							if (!string.IsNullOrEmpty(XAMLType.Name)) if (XAMLType.Name.IndexOf(Args.Search, StringComparison.CurrentCultureIgnoreCase) >= 0) results.Add(new FindReplaceResult("WPF Name", XAMLType.Name, Owner.Parent.Owner, this));
						}
					}
				}
				else
				{
					if (Args.IsDataType == false)
					{
						if (!string.IsNullOrEmpty(DataType.Name)) if (Args.RegexSearch.IsMatch(DataType.Name)) results.Add(new FindReplaceResult("Name", DataType.Name, Owner.Parent.Owner, this));
						if (!string.IsNullOrEmpty(ClientType.Name)) if (ClientType.Name.IndexOf(Args.Search, StringComparison.CurrentCultureIgnoreCase) >= 0) results.Add(new FindReplaceResult("Contract Name", ClientType.Name, Owner.Parent.Owner, this));
						if (!string.IsNullOrEmpty(XAMLType.Name)) if (Args.RegexSearch.IsMatch(XAMLType.Name)) results.Add(new FindReplaceResult("WPF Name", XAMLType.Name, Owner.Parent.Owner, this));
					}
				}

				if (Args.ReplaceAll)
				{
					bool ia = Owner.IsActive;
					Owner.IsActive = true;
					if (Args.UseRegex == false)
					{
						if (Args.MatchCase == false)
						{
							if (Args.IsDataType == false)
							{
								if (!string.IsNullOrEmpty(DataType.Name)) DataType.Name = Microsoft.VisualBasic.Strings.Replace(DataType.Name, Args.Search, Args.Replace, 1, -1, Microsoft.VisualBasic.CompareMethod.Text);
								if (!string.IsNullOrEmpty(ClientType.Name)) ClientType.Name = Microsoft.VisualBasic.Strings.Replace(ClientType.Name, Args.Search, Args.Replace, 1, -1, Microsoft.VisualBasic.CompareMethod.Text);
								if (!string.IsNullOrEmpty(XAMLType.Name)) XAMLType.Name = Microsoft.VisualBasic.Strings.Replace(XAMLType.Name, Args.Search, Args.Replace, 1, -1, Microsoft.VisualBasic.CompareMethod.Text);
							}
						}
						else
						{
							if (Args.IsDataType == false)
							{
								if (!string.IsNullOrEmpty(DataType.Name)) DataType.Name = Microsoft.VisualBasic.Strings.Replace(DataType.Name, Args.Search, Args.Replace);
								if (!string.IsNullOrEmpty(ClientType.Name)) ClientType.Name = Microsoft.VisualBasic.Strings.Replace(ClientType.Name, Args.Search, Args.Replace);
								if (!string.IsNullOrEmpty(XAMLType.Name)) XAMLType.Name = Microsoft.VisualBasic.Strings.Replace(XAMLType.Name, Args.Search, Args.Replace);
							}
						}
					}
					else
					{
						if (Args.IsDataType == false)
						{
							if (!string.IsNullOrEmpty(DataType.Name)) DataType.Name = Args.RegexSearch.Replace(DataType.Name, Args.Replace);
							if (!string.IsNullOrEmpty(ClientType.Name)) ClientType.Name = Args.RegexSearch.Replace(ClientType.Name, Args.Replace);
							if (!string.IsNullOrEmpty(XAMLType.Name)) XAMLType.Name = Args.RegexSearch.Replace(XAMLType.Name, Args.Replace);
						}
					}
					Owner.IsActive = ia;
				}
			}

			return results;
		}

		public void Replace(FindReplaceInfo Args, string Field)
		{
			if (!Args.ReplaceAll) return;
			bool ia = Owner.IsActive;
			Owner.IsActive = true;
			if (Args.UseRegex == false)
			{
				if (Args.MatchCase == false)
				{
					if (Args.IsDataType == false)
					{
						if (Field == "Name") DataType.Name = Microsoft.VisualBasic.Strings.Replace(DataType.Name, Args.Search, Args.Replace, 1, -1, Microsoft.VisualBasic.CompareMethod.Text);
						if (Field == "Contract Name") ClientType.Name = Microsoft.VisualBasic.Strings.Replace(ClientType.Name, Args.Search, Args.Replace, 1, -1, Microsoft.VisualBasic.CompareMethod.Text);
						if (Field == "WPF Name") XAMLType.Name = Microsoft.VisualBasic.Strings.Replace(XAMLType.Name, Args.Search, Args.Replace, 1, -1, Microsoft.VisualBasic.CompareMethod.Text);
					}
				}
				else
				{
					if (Args.IsDataType == false)
					{
						if (Field == "Name") DataType.Name = Microsoft.VisualBasic.Strings.Replace(DataType.Name, Args.Search, Args.Replace);
						if (Field == "Contract Name") ClientType.Name = Microsoft.VisualBasic.Strings.Replace(ClientType.Name, Args.Search, Args.Replace);
						if (Field == "WPF Name") XAMLType.Name = Microsoft.VisualBasic.Strings.Replace(XAMLType.Name, Args.Search, Args.Replace);
					}
				}
			}
			else
			{
				if (Args.IsDataType == false)
				{
					if (Field == "Name") DataType.Name = Args.RegexSearch.Replace(DataType.Name, Args.Replace);
					if (Field == "Contract Name") ClientType.Name = Args.RegexSearch.Replace(ClientType.Name, Args.Replace);
					if (Field == "WPF Name") XAMLType.Name = Args.RegexSearch.Replace(XAMLType.Name, Args.Replace);
				}
			}
			Owner.IsActive = ia;
		}
	}
}