//---------------------------------------------------------------------------
// This code was generated by a tool. Changes to this file may cause 
// incorrect behavior and will be lost if the code is regenerated.
//
// WCF Architect Version:	2.0.2000.0
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

namespace Test1
{
	/**************************************************************************
	*	Data Contracts
	**************************************************************************/

	[KnownType(typeof(Guid[]))]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("WCF Architect Service Compiler", "2.0.2000.0")]
	[DataContract(Name = "TestData1", Namespace = "http://tempuri.org/Test1/")]
	public partial class TestData1
	{
		[DataMember(Name = "sdlfj")] public string sdlfj { get; set; }
		[DataMember(Name = "ID")] public Guid ID { get; set; }
		[DataMember(Name = "linktest")] public Test1.TestData2 linktest { get; set; }
		[DataMember(Name = "collectiontest")] public ObservableCollection<string> collectiontest { get; set; }
		[DataMember(Name = "sdf")] public Dictionary<int, Guid> sdf { get; set; }
		[DataMember(Name = "sdfgg")] public SortedDictionary<int, string> sdfgg { get; set; }
		[DataMember(Name = "askdjsadj")] public List<string> askdjsadj { get; set; }
		[DataMember(Name = "sdkjfsldjf")] public Dictionary<int, string> sdkjfsldjf { get; set; }
		[DataMember(Name = "askdjalsd")] public Guid[] askdjalsd { get; set; }
	}

	[System.CodeDom.Compiler.GeneratedCodeAttribute("WCF Architect Service Compiler", "2.0.2000.0")]
	[DataContract(Name = "TestData2", Namespace = "http://tempuri.org/Test1/")]
	public partial class TestData2
	{
		[DataMember(Name = "sdf")] public string sdf { get; set; }
	}


}

