﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WCFArchitect.Projects;
using WCFArchitect.Compiler.Generators;

namespace WCFArchitect.Compiler
{
	public class Generator
	{
		public Project Project { get; private set; }
		public ProjectGenerationFramework Framework { get; private set; }
		public string OutputDirectory { get; private set; }
		public bool Server { get; private set; }

		public Generator(Project Project, ProjectGenerationFramework Framework, string OutputDirectory, bool Server = true)
		{
			this.Project = Project;
			this.Framework = Framework;
			this.OutputDirectory = OutputDirectory;
			this.Server = Server;
		}

		public void Verify()
		{
			if (string.IsNullOrEmpty(Project.ServerOutputFile))
				Program.AddMessage(new CompileMessage("GS0003", "The '" + Project.Name + "' project does not have a Server Assembly Name. You must specify a Server Assembly Name.", CompileMessageSeverity.ERROR, null, Project, Project.GetType(), Guid.Empty, Project.ID));
			else
				if (Helpers.RegExs.MatchFileName.IsMatch(Project.ServerOutputFile) == false)
					Program.AddMessage(new CompileMessage("GS0004", "The Server Assembly Name in '" + Project.Name + "' project is not set or contains invalid characters. You must specify a valid Windows file name.", CompileMessageSeverity.ERROR, null, Project, Project.GetType(), Guid.Empty, Project.ID));
			if (string.IsNullOrEmpty(Project.ClientOutputFile))
				Program.AddMessage(new CompileMessage("GS0005", "The '" + Project.Name + "' project does not have a Client Assembly Name. You must specify a Client Assembly Name.", CompileMessageSeverity.ERROR, null, Project, Project.GetType(), Guid.Empty, Project.ID));
			else
				if (Helpers.RegExs.MatchFileName.IsMatch(Project.ClientOutputFile) == false)
					Program.AddMessage(new CompileMessage("GS0006", "The Client Assembly Name in '" + Project.Name + "' project is not set or contains invalid characters. You must specify a valid Windows file name.", CompileMessageSeverity.ERROR, null, Project, Project.GetType(), Guid.Empty, Project.ID));
			if ((Project.ServerOutputFile == Project.ClientOutputFile))
				Program.AddMessage(new CompileMessage("GS0007", "The '" + Project.Name + "' project Client and Server Assembly Names are the same. You must specify a different Server or Client Assembly Name.", CompileMessageSeverity.ERROR, null, Project, Project.GetType(), Guid.Empty, Project.ID));
		}

		public void Generate()
		{
			Globals.CurrentGenerationTarget = Framework;

			var code = new StringBuilder();
			code.AppendLine("//---------------------------------------------------------------------------");
			code.AppendLine("// This code was generated by a tool. Changes to this file may cause ");
			code.AppendLine("// incorrect behavior and will be lost if the code is regenerated.");
			code.AppendLine("//");
			code.AppendFormat("// WCF Architect Version:\t{0}{1}", Globals.ApplicationVersion, Environment.NewLine);
			if (Framework == ProjectGenerationFramework.NET30) code.AppendLine("// .NET Framework Version:\t3.0");
			if (Framework == ProjectGenerationFramework.NET35) code.AppendLine("// .NET Framework Version:\t3.5");
			if (Framework == ProjectGenerationFramework.NET35Client) code.AppendLine("// .NET Framework Version:\t3.5 (Client)");
			if (Framework == ProjectGenerationFramework.NET40) code.AppendLine("// .NET Framework Version:\t4.0");
			if (Framework == ProjectGenerationFramework.NET40Client) code.AppendLine("// .NET Framework Version:\t4.0 (Client)");
			if (Framework == ProjectGenerationFramework.NET45) code.AppendLine("// .NET Framework Version:\t4.5");
			if (Framework == ProjectGenerationFramework.SL40) code.AppendLine("// Silverlight Version:\t4.0");
			if (Framework == ProjectGenerationFramework.SL50) code.AppendLine("// Silverlight Version:\t5.0");
			if (Framework == ProjectGenerationFramework.WIN8) code.AppendLine("// .NET Framework Version:\t4.5 (Windows Runtime)");
			code.AppendLine("//---------------------------------------------------------------------------");
			code.AppendLine();

			// Generate using namespaces
			foreach (ProjectUsingNamespace pun in GetUsingNamespaces(Project))
			{
				if (pun.Server && Server)
				{
					if (pun.NET && (Framework == ProjectGenerationFramework.NET30 || Framework == ProjectGenerationFramework.NET35 || Framework == ProjectGenerationFramework.NET40 || Framework == ProjectGenerationFramework.NET45))
						code.AppendFormat("using {0};{1}", pun.Namespace, Environment.NewLine);
					else if (pun.NET && (Framework == ProjectGenerationFramework.NET35Client || Framework == ProjectGenerationFramework.NET40Client))
						if (!pun.IsFullFrameworkOnly) code.AppendFormat("using {0};{1}", pun.Namespace, Environment.NewLine);
				}
				if (pun.Client && !Server)
				{
					if (pun.NET && (Framework == ProjectGenerationFramework.NET30 || Framework == ProjectGenerationFramework.NET35 || Framework == ProjectGenerationFramework.NET40 || Framework == ProjectGenerationFramework.NET45))
						code.AppendFormat("using {0};{1}", pun.Namespace, Environment.NewLine);
					else if (pun.NET && (Framework == ProjectGenerationFramework.NET35Client || Framework == ProjectGenerationFramework.NET40Client))
						if (!pun.IsFullFrameworkOnly) code.AppendFormat("using {0};{1}", pun.Namespace, Environment.NewLine);
					if (pun.SL && (Framework == ProjectGenerationFramework.SL40 || Framework == ProjectGenerationFramework.SL50))
						code.AppendFormat("using {0};{1}", pun.Namespace, Environment.NewLine);
					else if (pun.RT && Framework == ProjectGenerationFramework.WIN8)
						code.AppendFormat("using {0};{1}", pun.Namespace, Environment.NewLine);
				}
			}
			code.AppendLine();

			//Scan and generate references
			var refs = new List<DataType>(ReferenceScan(Project.Namespace));
			foreach (DataType dt in refs)
				code.AppendLine(ReferenceGenerate(dt));

			//Generate project
			if (Server)
			{
				if (Framework == ProjectGenerationFramework.NET30) code.AppendLine(NamespaceCSGenerator.GenerateServerCode30(Project.Namespace));
				if (Framework == ProjectGenerationFramework.NET35) code.AppendLine(NamespaceCSGenerator.GenerateServerCode35(Project.Namespace));
				if (Framework == ProjectGenerationFramework.NET35Client) code.AppendLine(NamespaceCSGenerator.GenerateServerCode35Client(Project.Namespace));
				if (Framework == ProjectGenerationFramework.NET40) code.AppendLine(NamespaceCSGenerator.GenerateServerCode40(Project.Namespace));
				if (Framework == ProjectGenerationFramework.NET40Client) code.AppendLine(NamespaceCSGenerator.GenerateServerCode40Client(Project.Namespace));
				if (Framework == ProjectGenerationFramework.NET45) code.AppendLine(NamespaceCSGenerator.GenerateServerCode45(Project.Namespace));
			}
			else
			{
				if (Framework == ProjectGenerationFramework.NET30) code.AppendLine(NamespaceCSGenerator.GenerateClientCode30(Project.Namespace));
				if (Framework == ProjectGenerationFramework.NET35) code.AppendLine(NamespaceCSGenerator.GenerateClientCode35(Project.Namespace));
				if (Framework == ProjectGenerationFramework.NET35Client) code.AppendLine(NamespaceCSGenerator.GenerateClientCode35Client(Project.Namespace));
				if (Framework == ProjectGenerationFramework.NET40) code.AppendLine(NamespaceCSGenerator.GenerateClientCode40(Project.Namespace));
				if (Framework == ProjectGenerationFramework.NET40Client) code.AppendLine(NamespaceCSGenerator.GenerateClientCode40Client(Project.Namespace));
				if (Framework == ProjectGenerationFramework.NET45 || Framework == ProjectGenerationFramework.WIN8) code.AppendLine(NamespaceCSGenerator.GenerateClientCode45(Project.Namespace));
			}

			System.IO.File.WriteAllText(Server ? new Uri(new Uri(Project.AbsolutePath), System.IO.Path.Combine(OutputDirectory, Project.ServerOutputFile + ".cs")).LocalPath : new Uri(new Uri(Project.AbsolutePath), System.IO.Path.Combine(OutputDirectory, Project.ClientOutputFile + ".cs")).LocalPath, code.ToString());
		}

		private static IEnumerable<DataType> ReferenceScan(Namespace Scan)
		{
			var refs = new List<DataType>();

			foreach (DataElement de in Scan.Data.SelectMany(d => d.Elements))
			{
				if(de.DataType.IsTypeReference) refs.Add(de.DataType);
				if(de.ClientType != null && de.ClientType.IsTypeReference) refs.Add(de.ClientType);
				if(de.XAMLType != null && de.XAMLType.IsTypeReference) refs.Add(de.XAMLType);
			}

			foreach (Service s in Scan.Services)
			{
				foreach (Method m in s.Operations)
				{
					if (m.ReturnType.IsTypeReference) refs.Add(m.ReturnType);
					refs.AddRange(from mp in m.Parameters where mp.Type.IsTypeReference select mp.Type);
				}
				refs.AddRange(from Property p in s.Operations where p.ReturnType.IsTypeReference select p.ReturnType);
			}

			foreach(Namespace n in Scan.Children)
				refs.AddRange(ReferenceScan(n));

			return new List<DataType>(refs.Distinct(new ReferenceComparer()));
		}

		private static DataType ReferenceRetrieve(Project Project, Namespace Namesapce, Guid TypeID)
		{
			var d = Namesapce.Data.FirstOrDefault(a => a.ID == TypeID);
			if (d != null) return d;
			var e = Namesapce.Enums.FirstOrDefault(a => a.ID == TypeID);
			if (e != null) return e;

			foreach (Namespace n in Namesapce.Children)
			{
				var t = ReferenceRetrieve(Project, n, TypeID);
				if (t != null) return t;
			}

			if(!Equals(Namesapce, Project.Namespace)) return null;
			foreach(DependencyProject dp in Project.DependencyProjects)
			{
				var t = ReferenceRetrieve(dp.Project, dp.Project.Namespace, TypeID);
				if (t != null) return t;
			}

			return null;
		}

		private string ReferenceGenerate(DataType Reference)
		{
			//Get the referenced type
			DataType typeref = ReferenceRetrieve(Project, Project.Namespace, Reference.ID);
			if(typeref == null)
			{
				Program.AddMessage(new CompileMessage("GS0008", string.Format("Unable to locate type '{0}'. Please ensure that you have added the project containing this type to the Dependency Projects list and that it has not been renamed or removed from the project.", Reference.Name), CompileMessageSeverity.ERROR, null, Project, Project.GetType(), Reference.ID, Project.ID));
				return "";
			}
			Type rt = typeref.GetType();

			//Generate a namespace wrapper for the type then generate the type inside the namespace
			var code = new StringBuilder();
			code.AppendFormat("namespace {0}{1}", typeref.Parent.FullName, Environment.NewLine);
			code.AppendLine("{");
			if (Server && rt == typeof(Data))
			{
				if (Framework == ProjectGenerationFramework.NET30) code.AppendLine(DataCSGenerator.GenerateServerCode30(typeref as Data));
				if (Framework == ProjectGenerationFramework.NET35 || Framework == ProjectGenerationFramework.NET35Client) code.AppendLine(DataCSGenerator.GenerateServerCode35(typeref as Data));
				if (Framework == ProjectGenerationFramework.NET40 || Framework == ProjectGenerationFramework.NET40Client) code.AppendLine(DataCSGenerator.GenerateServerCode40(typeref as Data));
				if (Framework == ProjectGenerationFramework.NET45) code.AppendLine(DataCSGenerator.GenerateServerCode45(typeref as Data));
			}
			else if (!Server && rt == typeof(Data))
			{
				if (Framework == ProjectGenerationFramework.NET30) code.AppendLine(DataCSGenerator.GenerateProxyCode30(typeref as Data));
				if (Framework == ProjectGenerationFramework.NET35 || Framework == ProjectGenerationFramework.NET35Client) code.AppendLine(DataCSGenerator.GenerateProxyCode35(typeref as Data));
				if (Framework == ProjectGenerationFramework.NET40 || Framework == ProjectGenerationFramework.NET40Client) code.AppendLine(DataCSGenerator.GenerateProxyCode40(typeref as Data));
				if (Framework == ProjectGenerationFramework.NET45 || Framework == ProjectGenerationFramework.WIN8) code.AppendLine(DataCSGenerator.GenerateProxyCode45(typeref as Data));
				code.AppendLine();
				if (Framework == ProjectGenerationFramework.NET30) code.AppendLine(DataCSGenerator.GenerateXAMLCode30(typeref as Data));
				if (Framework == ProjectGenerationFramework.NET35 || Framework == ProjectGenerationFramework.NET35Client) code.AppendLine(DataCSGenerator.GenerateXAMLCode35(typeref as Data));
				if (Framework == ProjectGenerationFramework.NET40 || Framework == ProjectGenerationFramework.NET40Client) code.AppendLine(DataCSGenerator.GenerateXAMLCode40(typeref as Data));
				if (Framework == ProjectGenerationFramework.NET45 || Framework == ProjectGenerationFramework.WIN8) code.AppendLine(DataCSGenerator.GenerateXAMLCode45(typeref as Data));
			}
			else if (Server && rt == typeof(Projects.Enum))
			{
				if (Framework == ProjectGenerationFramework.NET30) code.AppendLine(EnumCSGenerator.GenerateServerCode30(typeref as Projects.Enum));
				if (Framework == ProjectGenerationFramework.NET35 || Framework == ProjectGenerationFramework.NET35Client) code.AppendLine(EnumCSGenerator.GenerateServerCode35(typeref as Projects.Enum));
				if (Framework == ProjectGenerationFramework.NET40 || Framework == ProjectGenerationFramework.NET40Client) code.AppendLine(EnumCSGenerator.GenerateServerCode40(typeref as Projects.Enum));
				if (Framework == ProjectGenerationFramework.NET45) code.AppendLine(EnumCSGenerator.GenerateServerCode45(typeref as Projects.Enum));
			}
			else if (!Server && rt == typeof(Projects.Enum))
			{
				if (Framework == ProjectGenerationFramework.NET30) code.AppendLine(EnumCSGenerator.GenerateProxyCode30(typeref as Projects.Enum));
				if (Framework == ProjectGenerationFramework.NET35 || Framework == ProjectGenerationFramework.NET35Client) code.AppendLine(EnumCSGenerator.GenerateProxyCode35(typeref as Projects.Enum));
				if (Framework == ProjectGenerationFramework.NET40 || Framework == ProjectGenerationFramework.NET40Client) code.AppendLine(EnumCSGenerator.GenerateProxyCode40(typeref as Projects.Enum));
				if (Framework == ProjectGenerationFramework.NET45 || Framework == ProjectGenerationFramework.WIN8) code.AppendLine(EnumCSGenerator.GenerateProxyCode45(typeref as Projects.Enum));
			}
			code.AppendLine("}");

			return code.ToString();
		}

		private static IEnumerable<ProjectUsingNamespace> GetUsingNamespaces(Project CurProject)
		{
			var puns = new List<ProjectUsingNamespace>(CurProject.UsingNamespaces);

			foreach (DependencyProject dp in CurProject.DependencyProjects)
				puns.AddRange(GetUsingNamespaces(dp.Project));

			return new List<ProjectUsingNamespace>(puns.Distinct(new UsingNamespaceComparer()));
		}
	}

	public class UsingNamespaceComparer : IEqualityComparer<ProjectUsingNamespace>
	{
		public bool Equals(ProjectUsingNamespace x, ProjectUsingNamespace y)
		{
			return x.Namespace == y.Namespace;
		}

		public int GetHashCode(ProjectUsingNamespace obj)
		{
			return obj.GetHashCode();
		}
	}

	public class ReferenceComparer : IEqualityComparer<DataType>
	{
		public bool Equals(DataType x, DataType y)
		{
			return x.ID.CompareTo(y.ID) == 0;
		}

		public int GetHashCode(DataType obj)
		{
			return obj.GetHashCode();
		}
	}
}