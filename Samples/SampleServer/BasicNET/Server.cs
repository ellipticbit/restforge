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

#pragma warning disable 1591
namespace WCFArchitect.SampleServer.BasicNET
{
	/**************************************************************************
	*	Service Contracts
	**************************************************************************/

	[System.CodeDom.Compiler.GeneratedCodeAttribute("WCF Architect .NET CSharp Generator - BETA", "2.0.2000.0")]
	[ServiceContract(SessionMode = System.ServiceModel.SessionMode.Allowed, Namespace = "http://tempuri.org/WCFArchitect/SampleServer/BasicNET/")]
	public interface ITestNET
	{
		[OperationContract(IsInitiating = false)]
		WCFArchitect.SampleServer.BasicWinRT.Customer RefTestAsync();

	}


}

#pragma warning restore 1591
