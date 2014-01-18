﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NETPath.Projects;
using NETPath.Projects.Helpers;

namespace NETPath.Generators.CS
{
	internal static class SecurityGenerator
	{
		public static string GenerateCode45(BindingSecurity o)
		{
			Type t = o.GetType();
			if (t == typeof(BindingSecurityBasicHTTP)) return SecurityBasicHTTPCSGenerator.GenerateCode45(o as BindingSecurityBasicHTTP);
			if (t == typeof(BindingSecurityBasicHTTPS)) return SecurityBasicHTTPSCSGenerator.GenerateCode45(o as BindingSecurityBasicHTTPS);
			if (t == typeof(BindingSecurityWSHTTP)) return SecurityWSHTTPCSGenerator.GenerateCode45(o as BindingSecurityWSHTTP);
			if (t == typeof(BindingSecurityWSDualHTTP)) return SecurityWSDualHTTPCSGenerator.GenerateCode45(o as BindingSecurityWSDualHTTP);
			if (t == typeof(BindingSecurityWSFederationHTTP)) return SecurityWSFederationHTTPCSGenerator.GenerateCode45(o as BindingSecurityWSFederationHTTP);
			if (t == typeof(BindingSecurityTCP)) return SecurityTCPCSGenerator.GenerateCode45(o as BindingSecurityTCP);
			if (t == typeof(BindingSecurityNamedPipe)) return SecurityNamedPipeCSGenerator.GenerateCode45(o as BindingSecurityNamedPipe);
			if (t == typeof(BindingSecurityMSMQ)) return SecurityMSMQCSGenerator.GenerateCode45(o as BindingSecurityMSMQ);
			if (t == typeof(BindingSecurityPeerTCP)) return SecurityPeerTCPCSGenerator.GenerateCode45(o as BindingSecurityPeerTCP);
			if (t == typeof(BindingSecurityWebHTTP)) return SecurityWebHTTPCSGenerator.GenerateCode45(o as BindingSecurityWebHTTP);
			if (t == typeof(BindingSecurityMSMQIntegration)) return SecurityMSMQIntegrationCSGenerator.GenerateCode45(o as BindingSecurityMSMQIntegration);
			return "";
		}
	}

	#region - BindingSecurityBasicHTTP Class -

	public static class SecurityBasicHTTPCSGenerator
	{
		public static string GenerateCode45(BindingSecurityBasicHTTP o)
		{
			var code = new StringBuilder();
			code.AppendLine(string.Format("\t\t\tthis.Security.Mode = BasicHttpSecurityMode.{0};", System.Enum.GetName(typeof(System.ServiceModel.BasicHttpSecurityMode), o.Mode)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Transport.ClientCredentialType = HttpClientCredentialType.{0};", System.Enum.GetName(typeof(System.ServiceModel.HttpClientCredentialType), o.TransportClientCredentialType)));
			//TODO: Verify WinRT can connect with these set.
			{
				code.AppendLine(string.Format("\t\t\tthis.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.{0};", System.Enum.GetName(typeof(System.ServiceModel.HttpProxyCredentialType), o.TransportProxyCredentialType)));
				code.AppendLine(string.Format("\t\t\tthis.Security.Transport.Realm = \"{0}\";", o.TransportRealm));
				code.AppendLine(string.Format("\t\t\tthis.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.{0};", System.Enum.GetName(typeof (BindingSecurityAlgorithmSuite), o.MessageAlgorithmSuite)));
				code.AppendLine(string.Format("\t\t\tthis.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.{0};", System.Enum.GetName(typeof (System.ServiceModel.BasicHttpMessageCredentialType), o.MessageClientCredentialType)));
			}
			return code.ToString();
		}
	}

	#endregion

	#region - BindingSecurityBasicHTTPS Class -

	public static class SecurityBasicHTTPSCSGenerator
	{
		public static string GenerateCode45(BindingSecurityBasicHTTPS o)
		{
			var code = new StringBuilder();
			code.AppendLine(string.Format("\t\t\tthis.Security.Mode = BasicHttpsSecurityMode.{0};", System.Enum.GetName(typeof(System.ServiceModel.BasicHttpsSecurityMode), o.Mode)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.{0};", System.Enum.GetName(typeof(BindingSecurityAlgorithmSuite), o.MessageAlgorithmSuite)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.{0};", System.Enum.GetName(typeof(System.ServiceModel.BasicHttpMessageCredentialType), o.MessageClientCredentialType)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Transport.ClientCredentialType = HttpClientCredentialType.{0};", System.Enum.GetName(typeof(System.ServiceModel.HttpClientCredentialType), o.TransportClientCredentialType)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.{0};", System.Enum.GetName(typeof(System.ServiceModel.HttpProxyCredentialType), o.TransportProxyCredentialType)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Transport.Realm = \"{0}\";", o.TransportRealm));
			return code.ToString();
		}
	}

	#endregion

	#region - BindingSecurityWSHTTP Class -

	public static class SecurityWSHTTPCSGenerator
	{
		public static string GenerateCode45(BindingSecurityWSHTTP o)
		{
			var code = new StringBuilder();
			code.AppendLine(string.Format("\t\t\tthis.Security.Mode = SecurityMode.{0};", System.Enum.GetName(typeof(System.ServiceModel.SecurityMode), o.Mode)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.{0};", System.Enum.GetName(typeof(BindingSecurityAlgorithmSuite), o.MessageAlgorithmSuite)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Message.ClientCredentialType = MessageCredentialType.{0};", System.Enum.GetName(typeof(System.ServiceModel.MessageCredentialType), o.MessageClientCredentialType)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Message.EstablishSecurityContext = {0};", o.MessageEstablishSecurityContext ? Boolean.TrueString.ToLower() : Boolean.FalseString.ToLower()));
			code.AppendLine(string.Format("\t\t\tthis.Security.Message.NegotiateServiceCredential = {0};", o.MessageNegotiateServiceCredential ? Boolean.TrueString.ToLower() : Boolean.FalseString.ToLower()));
			code.AppendLine(string.Format("\t\t\tthis.Security.Transport.ClientCredentialType = HttpClientCredentialType.{0};", System.Enum.GetName(typeof(System.ServiceModel.HttpClientCredentialType), o.TransportClientCredentialType)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.{0};", System.Enum.GetName(typeof(System.ServiceModel.HttpProxyCredentialType), o.TransportProxyCredentialType)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Transport.Realm = \"{0}\";", o.TransportRealm));
			return code.ToString();
		}
	}

	#endregion

	#region - BindingSecurityWSDualHTTP Class -

	public static class SecurityWSDualHTTPCSGenerator
	{
		public static string GenerateCode45(BindingSecurityWSDualHTTP o)
		{
			var code = new StringBuilder();
			code.AppendLine(string.Format("\t\t\tthis.Security.Mode = WSDualHttpSecurityMode.{0};", System.Enum.GetName(typeof(System.ServiceModel.WSDualHttpSecurityMode), o.Mode)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.{0};", System.Enum.GetName(typeof(BindingSecurityAlgorithmSuite), o.MessageAlgorithmSuite)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Message.ClientCredentialType = MessageCredentialType.{0};", System.Enum.GetName(typeof(System.ServiceModel.MessageCredentialType), o.MessageClientCredentialType)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Message.NegotiateServiceCredential = {0};", o.MessageNegotiateServiceCredential ? Boolean.TrueString.ToLower() : Boolean.FalseString.ToLower()));
			return code.ToString();
		}
	}

	#endregion

	#region - BindingSecurityWSFederationHTTP Class -

	public static class SecurityWSFederationHTTPCSGenerator
	{
		public static string GenerateCode45(BindingSecurityWSFederationHTTP o)
		{
			var code = new StringBuilder();
			code.AppendLine(string.Format("\t\t\tthis.Security.Mode = WSFederationHttpSecurityMode.{0};", System.Enum.GetName(typeof(System.ServiceModel.WSFederationHttpSecurityMode), o.Mode)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.{0};", System.Enum.GetName(typeof(BindingSecurityAlgorithmSuite), o.MessageAlgorithmSuite)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Message.EstablishSecurityContext = {0};", o.MessageEstablishSecurityContext ? Boolean.TrueString.ToLower() : Boolean.FalseString.ToLower()));
			code.AppendLine(string.Format("\t\t\tthis.Security.Message.IssuedKeyType = System.IdentityModel.Tokens.SecurityKeyType.{0};", System.Enum.GetName(typeof(System.IdentityModel.Tokens.SecurityKeyType), o.MessageIssuedKeyType)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Message.IssuedTokenType = \"{0}\";", o.MessageIssuedTokenType));
			code.AppendLine(string.Format("\t\t\tthis.Security.Message.IssuerAddress = new EndpointAddress(\"{0}\");", o.MessageIssuerAddress));
			code.AppendLine(string.Format("\t\t\tthis.Security.Message.IssuerMetadataAddress = new EndpointAddress(\"{0}\");", o.MessageIssuerMetadataAddress));
			code.AppendLine(string.Format("\t\t\tthis.Security.Message.NegotiateServiceCredential = {0};", o.MessageNegotiateServiceCredential ? Boolean.TrueString.ToLower() : Boolean.FalseString.ToLower()));
			return code.ToString();
		}
	}

	#endregion

	#region - BindingSecurityTCP Class -

	public static class SecurityTCPCSGenerator
	{
		public static string GenerateCode45(BindingSecurityTCP o)
		{
			var code = new StringBuilder();
			code.AppendLine(string.Format("\t\t\tthis.Security.Mode = SecurityMode.{0};", System.Enum.GetName(typeof(System.ServiceModel.SecurityMode), o.Mode)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Transport.ClientCredentialType = TcpClientCredentialType.{0};", System.Enum.GetName(typeof (System.ServiceModel.TcpClientCredentialType), o.TransportClientCredentialType)));
			//TODO: Verify WinRT can connect with these set.
			{
				code.AppendLine(string.Format("\t\t\tthis.Security.Transport.ProtectionLevel = ProtectionLevel.{0};", System.Enum.GetName(typeof (System.Net.Security.ProtectionLevel), o.TransportProtectionLevel)));
				code.AppendLine(string.Format("\t\t\tthis.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.{0};", System.Enum.GetName(typeof (BindingSecurityAlgorithmSuite), o.MessageAlgorithmSuite)));
			}
			code.AppendLine(string.Format("\t\t\tthis.Security.Message.ClientCredentialType = MessageCredentialType.{0};", System.Enum.GetName(typeof(System.ServiceModel.MessageCredentialType), o.MessageClientCredentialType)));
			return code.ToString();
		}
	}

	#endregion

	#region - BindingSecurityNamedPipe Class -

	public static class SecurityNamedPipeCSGenerator
	{
		public static string GenerateCode45(BindingSecurityNamedPipe o)
		{
			var code = new StringBuilder();
			code.AppendLine(string.Format("\t\t\tthis.Security.Mode = NetNamedPipeSecurityMode.{0};", System.Enum.GetName(typeof(System.ServiceModel.NetNamedPipeSecurityMode), o.Mode)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Transport.ProtectionLevel = ProtectionLevel.{0};", System.Enum.GetName(typeof(System.Net.Security.ProtectionLevel), o.TransportProtectionLevel)));
			return code.ToString();
		}
	}

	#endregion

	#region - BindingSecurityMSMQ Class -

	public static class SecurityMSMQCSGenerator
	{
		public static string GenerateCode45(BindingSecurityMSMQ o)
		{
			var code = new StringBuilder();
			code.AppendLine(string.Format("\t\t\tthis.Security.Mode = NetMsmqSecurityMode.{0};", System.Enum.GetName(typeof(System.ServiceModel.NetMsmqSecurityMode), o.Mode)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.{0};", System.Enum.GetName(typeof(BindingSecurityAlgorithmSuite), o.MessageAlgorithmSuite)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Message.ClientCredentialType = MessageCredentialType.{0};", System.Enum.GetName(typeof(System.ServiceModel.MessageCredentialType), o.MessageClientCredentialType)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Transport.MsmqAuthenticationMode = MsmqAuthenticationMode.{0};", System.Enum.GetName(typeof(System.ServiceModel.MsmqAuthenticationMode), o.TransportAuthenticationMode)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Transport.MsmqEncryptionAlgorithm = MsmqEncryptionAlgorithm.{0};", System.Enum.GetName(typeof(System.ServiceModel.MsmqEncryptionAlgorithm), o.TransportEncryptionAlgorithm)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Transport.MsmqProtectionLevel = MsmqProtectionLevel.{0};", System.Enum.GetName(typeof(System.Net.Security.ProtectionLevel), o.TransportProtectionLevel)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Transport.MsmqSecureHashAlgorithm = MsmqSecureHashAlgorithm.{0};", System.Enum.GetName(typeof(System.ServiceModel.MsmqSecureHashAlgorithm), o.TransportSecureHashAlgorithm)));
			return code.ToString();
		}
	}

	#endregion

	#region - BindingSecurityPeerTCP Class -

	public static class SecurityPeerTCPCSGenerator
	{
		public static string GenerateCode45(BindingSecurityPeerTCP o)
		{
			var code = new StringBuilder();
			code.AppendLine(string.Format("\t\t\tthis.Security.Mode = PeerTransportCredentialType.{0};", System.Enum.GetName(typeof(System.ServiceModel.PeerTransportCredentialType), o.Mode)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Transport.CredentialType = PeerTransportCredentialType.{0};", System.Enum.GetName(typeof(System.ServiceModel.PeerTransportCredentialType), o.TransportClientCredentialType)));
			return code.ToString();
		}
	}

	#endregion

	#region - BindingSecurityWebHTTP Class -

	public static class SecurityWebHTTPCSGenerator
	{
		public static string GenerateCode45(BindingSecurityWebHTTP o)
		{
			var code = new StringBuilder();
			code.AppendLine(string.Format("\t\t\tthis.Security.Mode = WebHttpSecurityMode.{0};", System.Enum.GetName(typeof(System.ServiceModel.WebHttpSecurityMode), o.Mode)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Transport.ClientCredentialType = HttpClientCredentialType.{0};", System.Enum.GetName(typeof(System.ServiceModel.HttpClientCredentialType), o.TransportClientCredentialType)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.{0};", System.Enum.GetName(typeof(System.ServiceModel.HttpProxyCredentialType), o.TransportProxyCredentialType)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Transport.Realm = \"{0}\";", o.TransportRealm));
			return code.ToString();
		}
	}

	#endregion

	#region - BindingSecurityMSMQIntegration Class -

	public static class SecurityMSMQIntegrationCSGenerator
	{
		public static string GenerateCode45(BindingSecurityMSMQIntegration o)
		{
			var code = new StringBuilder();
			code.AppendLine(string.Format("\t\t\tthis.Security.Mode = NetMsmqSecurityMode.{0};", System.Enum.GetName(typeof(System.ServiceModel.MsmqIntegration.MsmqIntegrationSecurityMode), o.Mode)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Transport.MsmqAuthenticationMode = MsmqAuthenticationMode.{0};", System.Enum.GetName(typeof(System.ServiceModel.MsmqAuthenticationMode), o.TransportAuthenticationMode)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Transport.MsmqEncryptionAlgorithm = MsmqEncryptionAlgorithm.{0};", System.Enum.GetName(typeof(System.ServiceModel.MsmqEncryptionAlgorithm), o.TransportEncryptionAlgorithm)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Transport.MsmqProtectionLevel = MsmqProtectionLevel.{0};", System.Enum.GetName(typeof(System.Net.Security.ProtectionLevel), o.TransportProtectionLevel)));
			code.AppendLine(string.Format("\t\t\tthis.Security.Transport.MsmqSecureHashAlgorithm = MsmqSecureHashAlgorithm.{0};", System.Enum.GetName(typeof(System.ServiceModel.MsmqSecureHashAlgorithm), o.TransportSecureHashAlgorithm)));
			return code.ToString();
		}
	}
	#endregion

}