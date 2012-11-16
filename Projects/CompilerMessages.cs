﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WCFArchitect.Projects
{
	public enum CompileMessageSeverity
	{
		INFO,
		WARN,
		ERROR,
	}

	public class CompileMessage
	{
		public string Code { get; private set; }
		public string Description { get; private set; }
		public CompileMessageSeverity Severity { get; private set; }
		[IgnoreDataMember] public object Owner { get; private set; }
		[IgnoreDataMember] public object ErrorObject { get; private set; }
		[IgnoreDataMember] public Type ErrorObjectType { get; private set; }
		public Guid ProjectID { get; private set; }

		public CompileMessage() { }

		public CompileMessage(string Code, string Description, CompileMessageSeverity Severity, object Owner, object ErrorObject, Type ErrorObjectType, Guid ProjectID)
		{
			this.Code = Code;
			this.Description = Description;
			this.Severity = Severity;
			this.Owner = Owner;
			this.ErrorObject = ErrorObject;
			this.ErrorObjectType = ErrorObjectType;
			this.ProjectID = ProjectID;
		}
	}
}
