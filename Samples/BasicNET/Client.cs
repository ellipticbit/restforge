//---------------------------------------------------------------------------
// This code was generated by a tool. Changes to this file may cause 
// incorrect behavior and will be lost if the code is regenerated.
//
// NETPath Version:	2.0.0.1364
// .NET Framework Version:	4.5
//---------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Windows;
using System.Collections.Concurrent;

[assembly: System.Runtime.Serialization.ContractNamespaceAttribute("http://tempuri.org/WCFArchitect/SampleServer/BasicNET/", ClrNamespace="WCFArchitect.SampleServer.BasicNET")]

#pragma warning disable 1591
	/**************************************************************************
	*	Dependency Types
	**************************************************************************/

namespace WCFArchitect.SampleServer.BasicWinRT
{
	[System.CodeDom.Compiler.GeneratedCodeAttribute("NETPath .NET CSharp Generator - BETA", "2.0.0.1364")]
	[DataContract(Namespace = "http://tempuri.org/WCFArchitect/SampleServer/BasicWinRT/")]
	public enum Colors : long
	{
		[EnumMember()] Blue,
		[EnumMember()] Green,
		[EnumMember()] Red,
		[EnumMember()] Yellow,
		[EnumMember()] Orange,
		[EnumMember()] Gray,
		[EnumMember()] Teal,
		[EnumMember()] Black,
	}

}

namespace WCFArchitect.SampleServer.BasicWinRT
{
	[System.Diagnostics.DebuggerStepThroughAttribute]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("NETPath .NET CSharp Generator - BETA", "2.0.0.1364")]
	[ProtoBuf.ProtoContract(SkipConstructor = false, UseProtoMembersOnly = true)]
	public partial class Customer : System.Runtime.Serialization.IExtensibleDataObject
	{
		public CustomerXAML XAMLObject { get; private set; }

		//Data Change Messaging Support
		private static readonly System.Collections.Concurrent.ConcurrentDictionary<Guid, Customer> __dcm;
		static Customer()
		{
			__dcm = new System.Collections.Concurrent.ConcurrentDictionary<Guid, Customer>();
		}
		public static bool HasData(Customer data)
		{
			return __dcm.ContainsKey(data._DCMID);
		}
		public static Customer RegisterData(Customer data)
		{
			return __dcm.GetOrAdd(data._DCMID, data);
		}
		public static bool UnregisterData(Customer data)
		{
			Customer t;
			return __dcm.TryRemove(data._DCMID, out t);
		}
		private readonly System.Threading.ReaderWriterLockSlim __dcmlock = new System.Threading.ReaderWriterLockSlim();

		public System.Runtime.Serialization.ExtensionDataObject ExtensionData { get; set; }

		//Constuctors
		public Customer()
		{
			XAMLObject = this;
			ListTest = new List<WCFArchitect.SampleServer.BasicWinRT.Customer>();
			DictionaryTest = new System.Collections.Concurrent.ConcurrentDictionary<Guid, WCFArchitect.SampleServer.BasicWinRT.Customer>();
		}
		public Customer(CustomerXAML Data)
		{
			XAMLObject = Data;
			ID = Data.ID;
			Name = Data.Name;
			AddressLine1 = Data.AddressLine1;
			AddressLine2 = Data.AddressLine2;
			City = Data.City;
			State = Data.State;
			ZipCode = Data.ZipCode;
			Color = Data.Color;
			_DCMID = Data._DCMID;
			List<WCFArchitect.SampleServer.BasicWinRT.Customer> vListTest = new List<WCFArchitect.SampleServer.BasicWinRT.Customer>();
			foreach(WCFArchitect.SampleServer.BasicWinRT.CustomerXAML a in Data.ListTest) { vListTest.Add(a); }
			ListTest = vListTest;
			WCFArchitect.SampleServer.BasicWinRT.Customer[] vArrayTest = new WCFArchitect.SampleServer.BasicWinRT.Customer[Data.ArrayTest.GetLength(0)];
			for(int i = 0; i < Data.ArrayTest.GetLength(0); i++) { vArrayTest[i] = Data.ArrayTest[i]; }
			ArrayTest = vArrayTest;
			System.Collections.Concurrent.ConcurrentDictionary<Guid, WCFArchitect.SampleServer.BasicWinRT.Customer> vDictionaryTest = new System.Collections.Concurrent.ConcurrentDictionary<Guid, WCFArchitect.SampleServer.BasicWinRT.Customer>();
			foreach(KeyValuePair<Guid, WCFArchitect.SampleServer.BasicWinRT.CustomerXAML> a in Data.DictionaryTest) { vDictionaryTest.TryAdd(a.Key, a.Value); }
			DictionaryTest = vDictionaryTest;
		}

		[DataMember] private bool IDChanged;
		private Guid IDField;
		[DataMember(Name = "ID")] public Guid ID { get { __dcmlock.EnterReadLock(); try { return IDField; } finally { __dcmlock.ExitReadLock(); } } set { __dcmlock.EnterWriteLock(); try { IDField = value; IDChanged = true; } finally { __dcmlock.ExitWriteLock(); } } }
		[DataMember] private bool NameChanged;
		private string NameField;
		[DataMember(Name = "Name")] public string Name { get { __dcmlock.EnterReadLock(); try { return NameField; } finally { __dcmlock.ExitReadLock(); } } set { __dcmlock.EnterWriteLock(); try { NameField = value; NameChanged = true; } finally { __dcmlock.ExitWriteLock(); } } }
		private string AddressLine1Field;
		[DataMember(Name = "AddressLine1")] public string AddressLine1 { get { return AddressLine1Field; } set { AddressLine1Field = value; } }
		private string AddressLine2Field;
		[DataMember(Name = "AddressLine2")] public string AddressLine2 { get { return AddressLine2Field; } set { AddressLine2Field = value; } }
		[DataMember] private bool CityChanged;
		private string CityField;
		[DataMember(Name = "City")] public string City { get { __dcmlock.EnterReadLock(); try { return CityField; } finally { __dcmlock.ExitReadLock(); } } set { __dcmlock.EnterWriteLock(); try { CityField = value; CityChanged = true; } finally { __dcmlock.ExitWriteLock(); } } }
		private string StateField;
		[ProtoBuf.ProtoMember(1, IsPacked = true, OverwriteList = true, AsReference = true, DynamicType = true)] public string State { get { return StateField; } set { StateField = value; } }
		private int ZipCodeField;
		[ProtoBuf.ProtoMember(2, OverwriteList = true)] public int ZipCode { get { return ZipCodeField; } set { ZipCodeField = value; } }
		private WCFArchitect.SampleServer.BasicWinRT.Colors ColorField;
		[ProtoBuf.ProtoMember(3, OverwriteList = true)] public WCFArchitect.SampleServer.BasicWinRT.Colors Color { get { return ColorField; } set { ColorField = value; } }
		private Guid _DCMIDField;
		[ProtoBuf.ProtoMember(4, OverwriteList = true)] public Guid _DCMID { get { return _DCMIDField; } protected set { _DCMIDField = value; } }
		[DataMember] private bool ListTestChanged;
		private List<WCFArchitect.SampleServer.BasicWinRT.Customer> ListTestField;
		[DataMember(Name = "ListTest")] public List<WCFArchitect.SampleServer.BasicWinRT.Customer> ListTest { get { __dcmlock.EnterReadLock(); try { return ListTestField; } finally { __dcmlock.ExitReadLock(); } } set { __dcmlock.EnterWriteLock(); try { ListTestField = value; ListTestChanged = true; } finally { __dcmlock.ExitWriteLock(); } } }
		private WCFArchitect.SampleServer.BasicWinRT.Customer[] ArrayTestField;
		[DataMember(Name = "ArrayTest")] public WCFArchitect.SampleServer.BasicWinRT.Customer[] ArrayTest { get { return ArrayTestField; } set { ArrayTestField = value; } }
		[DataMember] private bool DictionaryTestChanged;
		private System.Collections.Concurrent.ConcurrentDictionary<Guid, WCFArchitect.SampleServer.BasicWinRT.Customer> DictionaryTestField;
		[DataMember(Name = "DictionaryTest")] public System.Collections.Concurrent.ConcurrentDictionary<Guid, WCFArchitect.SampleServer.BasicWinRT.Customer> DictionaryTest { get { __dcmlock.EnterReadLock(); try { return DictionaryTestField; } finally { __dcmlock.ExitReadLock(); } } set { __dcmlock.EnterWriteLock(); try { DictionaryTestField = value; DictionaryTestChanged = true; } finally { __dcmlock.ExitWriteLock(); } } }
	}


	//XAML Integration Object for the Customer DTO
	[System.CodeDom.Compiler.GeneratedCodeAttribute("NETPath .NET CSharp Generator - BETA", "2.0.0.1364")]
	public partial class CustomerXAML : DependencyObjectEx
	{
		public Customer DataObject { get; private set; }
		public bool IsUpdating { get; set; }

		//Properties
		public Guid ID { get { return (Guid)GetValueThreaded(IDProperty); } set { SetValueThreaded(IDProperty, value); } }
		public static readonly DependencyProperty IDProperty = DependencyProperty.Register("ID", typeof(Guid), typeof(CustomerXAML), IDPropertyMetadata);
		private static PropertyMetadata IDPropertyMetadata = new PropertyMetadata((o, e) => { var t = o as CustomerXAML; if (t == null) return; t.DataObject.ID = (Guid)e.NewValue; t.IDPropertyChanged((Guid)e.NewValue, (Guid)e.OldValue); });
		partial void IDPropertyChanged(Guid OldValue, Guid NewValue);

		public string Name { get { return (string)GetValueThreaded(NameProperty); } set { SetValueThreaded(NameProperty, value); } }
		public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(string), typeof(CustomerXAML), NamePropertyMetadata);
		private static PropertyMetadata NamePropertyMetadata = new PropertyMetadata((o, e) => { var t = o as CustomerXAML; if (t == null) return; t.DataObject.Name = (string)e.NewValue; t.NamePropertyChanged((string)e.NewValue, (string)e.OldValue); });
		partial void NamePropertyChanged(string OldValue, string NewValue);

		public string AddressLine1 { get { return (string)GetValue(AddressLine1Property); } set { SetValue(AddressLine1Property, value); } }
		public static readonly DependencyProperty AddressLine1Property = DependencyProperty.Register("AddressLine1", typeof(string), typeof(CustomerXAML), null);

		public string AddressLine2 { get { return (string)GetValue(AddressLine2Property); } set { SetValue(AddressLine2Property, value); } }
		public static readonly DependencyProperty AddressLine2Property = DependencyProperty.Register("AddressLine2", typeof(string), typeof(CustomerXAML), null);

		public string City { get { return (string)GetValueThreaded(CityProperty); } set { SetValueThreaded(CityProperty, value); } }
		public static readonly DependencyProperty CityProperty = DependencyProperty.Register("City", typeof(string), typeof(CustomerXAML), CityPropertyMetadata);
		private static PropertyMetadata CityPropertyMetadata = new PropertyMetadata((o, e) => { var t = o as CustomerXAML; if (t == null) return; t.DataObject.City = (string)e.NewValue; t.CityPropertyChanged((string)e.NewValue, (string)e.OldValue); });
		partial void CityPropertyChanged(string OldValue, string NewValue);

		public string State { get { return (string)GetValue(StateProperty); } set { SetValue(StateProperty, value); } }
		public static readonly DependencyProperty StateProperty = DependencyProperty.Register("State", typeof(string), typeof(CustomerXAML), null);

		public int ZipCode { get { return (int)GetValue(ZipCodeProperty); } set { SetValue(ZipCodeProperty, value); } }
		public static readonly DependencyProperty ZipCodeProperty = DependencyProperty.Register("ZipCode", typeof(int), typeof(CustomerXAML), null);

		public WCFArchitect.SampleServer.BasicWinRT.Colors Color { get { return (WCFArchitect.SampleServer.BasicWinRT.Colors)GetValue(ColorProperty); } set { SetValue(ColorProperty, value); } }
		public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(WCFArchitect.SampleServer.BasicWinRT.Colors), typeof(CustomerXAML), null);

		public Guid _DCMID { get { return (Guid)GetValue(_DCMIDProperty); } protected set { SetValue(_DCMIDPropertyKey, value); } }
		public static readonly DependencyPropertyKey _DCMIDPropertyKey = DependencyProperty.RegisterReadOnly("_DCMID", typeof(Guid), typeof(CustomerXAML), null);
		public static readonly DependencyProperty _DCMIDProperty = _DCMIDPropertyKey.DependencyProperty;

		public DependencyList<WCFArchitect.SampleServer.BasicWinRT.CustomerXAML> ListTest { get { return (DependencyList<WCFArchitect.SampleServer.BasicWinRT.CustomerXAML>)GetValueThreaded(ListTestProperty); } set { SetValueThreaded(ListTestProperty, value); } }
		public static readonly DependencyProperty ListTestProperty = DependencyProperty.Register("ListTest", typeof(DependencyList<WCFArchitect.SampleServer.BasicWinRT.CustomerXAML>), typeof(CustomerXAML), ListTestPropertyMetadata);
		private static readonly PropertyMetadata ListTestPropertyMetadata = new PropertyMetadata((o, e) => { var t = o as CustomerXAML; var nl = e.NewValue as DependencyList<WCFArchitect.SampleServer.BasicWinRT.CustomerXAML>; var ol = e.OldValue as DependencyList<WCFArchitect.SampleServer.BasicWinRT.CustomerXAML>; if (t == null || nl == null || ol == null) return; 
			nl.Added += (x) => { foreach (var z in x) t.DataObject.ListTest.AddNoUpdate(z); t.ListTestAdded(x); };
			nl.Removed += (x) => { t.DataObject.ListTest.Lock(); try { foreach (var z in x) t.DataObject.ListTest.RemoveNoUpdate(z); } finally { t.DataObject.ListTest.Unlock(); } t.ListTestRemoved(x); };
			nl.Cleared += (x) => { t.DataObject.ListTest.ClearNoUpdate(); t.ListTestCleared(x); };
			nl.Inserted += (idx, x) => { t.DataObject.ListTest.Lock(); try { int c = idx; foreach (var z in x) t.DataObject.ListTest.InsertNoUpdate(c++, z); } finally { t.DataObject.ListTest.Unlock(); } t.ListTestInserted(idx, x); };
			nl.RemovedAt += (idx, x) => { t.DataObject.ListTest.Lock(); try { foreach (var z in x) t.DataObject.ListTest.RemoveNoUpdate(z); } finally { t.DataObject.ListTest.Unlock() ;} t.ListTestRemovedAt(idx, x); };
			nl.Moved += (x, nidx) => { t.DataObject.ListTest.Lock(); try { t.DataObject.ListTest.MoveNoUpdate(x, nidx); } finally { t.DataObject.ListTest.Unlock(); } t.ListTestMoved(x, nidx); };
			nl.Replaced += (ox, nx) => { t.DataObject.ListTest.Lock(); try { t.DataObject.ListTest[t.DataObject.ListTest.IndexOf(ox)] = nx; } finally { t.DataObject.ListTest.Unlock(); } t.ListTestReplaced(ox, nx); };
			ol.ClearEventHandlers();
		});
		partial void ListTestAdded(IEnumerable<WCFArchitect.SampleServer.BasicWinRT.CustomerXAML> Values);
		partial void ListTestRemoved(IEnumerable<WCFArchitect.SampleServer.BasicWinRT.CustomerXAML> Values);
		partial void ListTestCleared(IEnumerable<WCFArchitect.SampleServer.BasicWinRT.CustomerXAML> Values);
		partial void ListTestInserted(int Index, IEnumerable<WCFArchitect.SampleServer.BasicWinRT.CustomerXAML> Values);
		partial void ListTestRemovedAt(int Index, IEnumerable<WCFArchitect.SampleServer.BasicWinRT.CustomerXAML> Values);
		partial void ListTestMoved(WCFArchitect.SampleServer.BasicWinRT.CustomerXAML Value, int Index);
		partial void ListTestReplaced(WCFArchitect.SampleServer.BasicWinRT.CustomerXAML OldValue, WCFArchitect.SampleServer.BasicWinRT.CustomerXAML NewValue);

		public WCFArchitect.SampleServer.BasicWinRT.CustomerXAML[] ArrayTest { get { return (WCFArchitect.SampleServer.BasicWinRT.CustomerXAML[])GetValue(ArrayTestProperty); } set { SetValue(ArrayTestProperty, value); } }
		public static readonly DependencyProperty ArrayTestProperty = DependencyProperty.Register("ArrayTest", typeof(WCFArchitect.SampleServer.BasicWinRT.CustomerXAML[]), typeof(CustomerXAML), null);

		public DependencyDictionary<Guid, WCFArchitect.SampleServer.BasicWinRT.CustomerXAML> DictionaryTest { get { return (DependencyDictionary<Guid, WCFArchitect.SampleServer.BasicWinRT.CustomerXAML>)GetValueThreaded(DictionaryTestProperty); } set { SetValueThreaded(DictionaryTestProperty, value); } }
		public static readonly DependencyProperty DictionaryTestProperty = DependencyProperty.Register("DictionaryTest", typeof(DependencyDictionary<Guid, WCFArchitect.SampleServer.BasicWinRT.CustomerXAML>), typeof(CustomerXAML), DictionaryTestPropertyMetadata);
		private static readonly PropertyMetadata DictionaryTestPropertyMetadata = new PropertyMetadata((o, e) => { var t = o as CustomerXAML; var nl = e.NewValue as DependencyDictionary<Guid, WCFArchitect.SampleServer.BasicWinRT.CustomerXAML>; var ol = e.OldValue as DependencyDictionary<Guid, WCFArchitect.SampleServer.BasicWinRT.CustomerXAML>; if (t == null || nl == null || ol == null) return; 
			nl.Added += (xk, xv) => { t.DataObject.DictionaryTest.AddOrUpdateNoUpdate(xk, xv, (k,v) => xv);  t.DictionaryTestAdded(xk, xv); };
			nl.Removed += (xk, xv) => { WCFArchitect.SampleServer.BasicWinRT.Customer result; t.DataObject.DictionaryTest.TryRemoveNoUpdate(xk, out result); t.DictionaryTestRemoved(xk, xv); };
			nl.Cleared += (x) => { t.DataObject.DictionaryTest.ClearNoUpdate(); t.DictionaryTestCleared(x); };
			nl.Updated += (xk, ox, nx) => { t.DataObject.DictionaryTest.AddOrUpdateNoUpdate(xk, nx, (k,v) => nx); t.DictionaryTestUpdated(xk, ox, nx); };
			ol.ClearEventHandlers();
		});
		partial void DictionaryTestAdded(Guid Key, WCFArchitect.SampleServer.BasicWinRT.CustomerXAML Value);
		partial void DictionaryTestRemoved(Guid Key, WCFArchitect.SampleServer.BasicWinRT.CustomerXAML Value);
		partial void DictionaryTestCleared(IEnumerable<KeyValuePair<Guid, WCFArchitect.SampleServer.BasicWinRT.CustomerXAML>> Values);
		partial void DictionaryTestUpdated(Guid Key, WCFArchitect.SampleServer.BasicWinRT.CustomerXAML OldValue, WCFArchitect.SampleServer.BasicWinRT.CustomerXAML NewValue);


		//Implicit Conversion
		public static implicit operator Customer(CustomerXAML Data)
		{
			if (Data == null) return null;
			Customer v = null;
			if (Application.Current.Dispatcher.CheckAccess()) v = ConvertFromXAMLObject(Data);
			else Application.Current.Dispatcher.Invoke(() => { v = ConvertFromXAMLObject(Data); }, System.Windows.Threading.DispatcherPriority.Normal);
			return v;
		}
		public static implicit operator CustomerXAML(Customer Data)
		{
			if (Data == null) return null;
			CustomerXAML v = null;
			if (Application.Current.Dispatcher.CheckAccess()) v = ConvertToXAMLObject(Data);
			else Application.Current.Dispatcher.Invoke(() => { v = ConvertToXAMLObject(Data); }, System.Windows.Threading.DispatcherPriority.Normal);
			return v;
		}

		//Constructors
		public CustomerXAML()
		{
			DataObject = this;
			ListTest = new DependencyList<WCFArchitect.SampleServer.BasicWinRT.CustomerXAML>();
			DictionaryTest = new DependencyDictionary<Guid, WCFArchitect.SampleServer.BasicWinRT.CustomerXAML>();
		}

		public CustomerXAML(Customer Data)
		{
			DataObject = Data;
			ID = Data.ID;
			Name = Data.Name;
			AddressLine1 = Data.AddressLine1;
			AddressLine2 = Data.AddressLine2;
			City = Data.City;
			State = Data.State;
			ZipCode = Data.ZipCode;
			Color = Data.Color;
			_DCMID = Data._DCMID;
			DependencyList<WCFArchitect.SampleServer.BasicWinRT.CustomerXAML> vListTest = new DependencyList<WCFArchitect.SampleServer.BasicWinRT.CustomerXAML>();
			foreach(WCFArchitect.SampleServer.BasicWinRT.Customer a in Data.ListTest) { vListTest.Add(a); }
			ListTest = vListTest;
			WCFArchitect.SampleServer.BasicWinRT.CustomerXAML[] vArrayTest = new WCFArchitect.SampleServer.BasicWinRT.CustomerXAML[Data.ArrayTest.GetLength(0)];
			for(int i = 0; i < Data.ArrayTest.GetLength(0); i++) { vArrayTest[i] = Data.ArrayTest[i]; }
			ArrayTest = vArrayTest;
			DependencyDictionary<Guid, WCFArchitect.SampleServer.BasicWinRT.CustomerXAML> vDictionaryTest = new DependencyDictionary<Guid, WCFArchitect.SampleServer.BasicWinRT.CustomerXAML>();
			foreach(KeyValuePair<Guid, WCFArchitect.SampleServer.BasicWinRT.Customer> a in Data.DictionaryTest) { vDictionaryTest.TryAdd(a.Key, a.Value); }
			DictionaryTest = vDictionaryTest;
		}

		//XAML/DTO Conversion Functions
		public static Customer ConvertFromXAMLObject(CustomerXAML Data)
		{
			if (Data.DataObject != null) return Data.DataObject;
			return new Customer(Data);
		}

		public static CustomerXAML ConvertToXAMLObject(Customer Data)
		{
			if (Data.XAMLObject != null) return Data.XAMLObject;
			return new CustomerXAML(Data);
		}
	}

}

namespace WCFArchitect.SampleServer.BasicNET
{
	/**************************************************************************
	*	Service Contracts
	**************************************************************************/

	[System.CodeDom.Compiler.GeneratedCodeAttribute("NETPath .NET CSharp Generator - BETA", "2.0.0.1364")]
	[ServiceContract(CallbackContract = typeof(ITestNETCallback), SessionMode = System.ServiceModel.SessionMode.Allowed, Namespace = "http://tempuri.org/WCFArchitect/SampleServer/BasicNET/")]
	public interface ITestNET
	{
		[OperationContract(Action = "http://tempuri.org/WCFArchitect/SampleServer/BasicNET/ITestNET/RefTestAsync", ReplyAction = "http://tempuri.org/WCFArchitect/SampleServer/BasicNET/ITestNET/RefTestAsyncResponse")]
		System.Threading.Tasks.Task<WCFArchitect.SampleServer.BasicWinRT.Customer> RefTestAsync();

	}

	[System.CodeDom.Compiler.GeneratedCodeAttribute("NETPath .NET CSharp Generator - BETA", "2.0.0.1364")]
	public interface ITestNETCallback
	{
		[OperationContract(Action = "http://tempuri.org/WCFArchitect/SampleServer/BasicNET/ITestNET/RetTestCallbackAsync", ReplyAction = "http://tempuri.org/WCFArchitect/SampleServer/BasicNET/ITestNET/RetTestCallbackAsyncResponse")]
		System.Threading.Tasks.Task<WCFArchitect.SampleServer.BasicWinRT.Customer> RetTestCallbackAsync();

	}

	[System.CodeDom.Compiler.GeneratedCodeAttribute("NETPath .NET CSharp Generator - BETA", "2.0.0.1364")]
	public interface ITestNETChannel : ITestNET, System.ServiceModel.IClientChannel
	{
	}

	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("NETPath .NET CSharp Generator - BETA", "2.0.0.1364")]
	public partial class TestNETProxy : System.ServiceModel.DuplexClientBase<ITestNET>, ITestNET
	{
		public TestNETProxy(string endpointConfigurationName, string remoteAddress) : base(endpointConfigurationName, remoteAddress)
		{
		}

		public TestNETProxy(System.ServiceModel.InstanceContext callbackInstance) : base(callbackInstance)
		{
		}

		public TestNETProxy(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : base(callbackInstance, endpointConfigurationName)
		{
		}

		public TestNETProxy(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : base(callbackInstance, endpointConfigurationName, remoteAddress)
		{
		}

		public TestNETProxy(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : base(callbackInstance, endpointConfigurationName, remoteAddress)
		{
		}

		public TestNETProxy(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : base(callbackInstance, binding, remoteAddress)
		{
		}

		public TestNETProxy(System.ServiceModel.Description.ServiceEndpoint endpoint) : base(endpoint)
		{
		}

		public TestNETProxy(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Description.ServiceEndpoint endpoint) : base(callbackInstance, endpoint)
		{
		}

		public System.Threading.Tasks.Task<WCFArchitect.SampleServer.BasicWinRT.Customer> RefTestAsync()
		{
			return base.Channel.RefTestAsync();
		}

	}


}

#pragma warning restore 1591
