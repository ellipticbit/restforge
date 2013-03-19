﻿/******************************************************************************
*	Copyright 2013 Prospective Software Inc.
*	Licensed under the Apache License, Version 2.0 (the "License");
*	you may not use this file except in compliance with the License.
*	You may obtain a copy of the License at
*
*		http://www.apache.org/licenses/LICENSE-2.0
*
*	Unless required by applicable law or agreed to in writing, software
*	distributed under the License is distributed on an "AS IS" BASIS,
*	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
*	See the License for the specific language governing permissions and
*	limitations under the License.
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace System
{
	public enum PropertyMode
	{
		Value,
		List,
		Dictionary,
		Stack,
		Queue
	}

	public abstract class DeltaPropertyBase
	{
		protected static readonly ConcurrentDictionary<HashID, DeltaPropertyBase> registered;

		static DeltaPropertyBase()
		{
			registered = new ConcurrentDictionary<HashID, DeltaPropertyBase>();
		}

		public HashID ID { get; protected set; }
		public HashID OwnerID { get; protected set; }
		public Type OwnerType { get; protected set; }
		public Type PropertyType { get; protected set; }
		internal object defaultValue { get; set; }

		internal static DeltaPropertyBase FromID(HashID ID)
		{
			DeltaPropertyBase ret;
			registered.TryGetValue(ID, out ret);
			return ret;
		}
	}

	public sealed class DeltaProperty<T> : DeltaPropertyBase
	{
		public T DefaultValue { get { return (T)defaultValue; } private set { defaultValue = value; } }
		public PropertyMode Mode { get; private set; }

		internal Action<DeltaObject, T, T> DeltaPropertyChangedCallback { get; private set; }
		internal Action<DeltaObject, T, T> DeltaPropertyUpdatedCallback { get; private set; }
		internal Func<DeltaObject, T, bool> DeltaValidateValueCallback { get; private set; }

		private DeltaProperty(HashID ID, HashID OwnerID, Type OwnerType)
		{
			this.ID = ID;
			this.OwnerID = OwnerID;
			this.OwnerType = OwnerType;
			PropertyType = typeof(T);
			DefaultValue = default(T);
			DeltaPropertyChangedCallback = null;
			DeltaValidateValueCallback = null;
			Mode = PropertyMode.Value;
		}

		private DeltaProperty(HashID ID, HashID OwnerID, Type OwnerType, T DefaultValue)
		{
			this.ID = ID;
			this.OwnerID = OwnerID;
			this.OwnerType = OwnerType;
			PropertyType = typeof(T);
			this.DefaultValue = DefaultValue;
			DeltaPropertyChangedCallback = null;
			DeltaValidateValueCallback = null;
			Mode = PropertyMode.Value;
		}

		private DeltaProperty(HashID ID, HashID OwnerID, Type OwnerType, T DefaultValue, Action<DeltaObject, T, T> DeltaPropertyChangedCallback, Action<DeltaObject, T, T> DeltaPropertyUpdatedCallback = null, Func<DeltaObject, T, bool> DeltaValidateValueCallback = null)
		{
			this.ID = ID;
			this.OwnerID = OwnerID;
			this.OwnerType = OwnerType;
			PropertyType = typeof(T);
			this.DefaultValue = DefaultValue;
			this.DeltaPropertyChangedCallback = DeltaPropertyChangedCallback;
			this.DeltaPropertyUpdatedCallback = DeltaPropertyUpdatedCallback;
			this.DeltaValidateValueCallback = DeltaValidateValueCallback;
			Mode = PropertyMode.Value;
		}

		private DeltaProperty(HashID ID, HashID OwnerID, Type OwnerType, Action<DeltaObject, T, T> DeltaPropertyChangedCallback)
		{
			this.ID = ID;
			this.OwnerID = OwnerID;
			this.OwnerType = OwnerType;
			PropertyType = typeof(T);
			DefaultValue = default(T);
			this.DeltaPropertyChangedCallback = DeltaPropertyChangedCallback;
			DeltaValidateValueCallback = null;
			Mode = PropertyMode.Value;
		}

		public static DeltaProperty<TType> Register<TType>(string Name, Type OwnerType)
		{
			var np = new DeltaProperty<TType>(HashID.GenerateHashID(OwnerType.FullName + "." + Name), HashID.GenerateHashID(OwnerType.FullName), OwnerType);
			if (!registered.TryAdd(np.ID, np))
				throw new ArgumentException(string.Format("Unable to register the DeltaProperty '{0}' on type '{1}'. A DeltaProperty with the same Name and OwnerType has already been registered.", Name, np.OwnerType));
			return np;
		}

		public static DeltaProperty<TType> Register<TType>(string Name, Type OwnerType, TType defaultValue)
		{
			var np = new DeltaProperty<TType>(HashID.GenerateHashID(OwnerType.FullName + "." + Name), HashID.GenerateHashID(OwnerType.FullName), OwnerType, defaultValue);
			if (!registered.TryAdd(np.ID, np))
				throw new ArgumentException(string.Format("Unable to register the DeltaProperty '{0}' on type '{1}'. A DeltaProperty with the same Name and OwnerType has already been registered.", Name, np.OwnerType));
			return np;
		}

		public static DeltaProperty<TType> Register<TType>(string Name, Type OwnerType, TType defaultValue, Action<DeltaObject, TType, TType> DeltaPropertyChangedCallback, Action<DeltaObject, TType, TType> DeltaPropertyUpdatedCallback = null, Func<DeltaObject, TType, bool> DeltaValidateValueCallback = null)
		{
			var np = new DeltaProperty<TType>(HashID.GenerateHashID(OwnerType.FullName + "." + Name), HashID.GenerateHashID(OwnerType.FullName), OwnerType, defaultValue, DeltaPropertyChangedCallback, DeltaPropertyUpdatedCallback, DeltaValidateValueCallback);
			if (!registered.TryAdd(np.ID, np))
				throw new ArgumentException(string.Format("Unable to register the DeltaProperty '{0}' on type '{1}'. A DeltaProperty with the same Name and OwnerType has already been registered.", Name, np.OwnerType));
			return np;
		}

		public static DeltaProperty<DeltaDictionary<TKey, TType>> RegisterDictionary<TKey, TType>(string Name, Type OwnerType, Action<DeltaObject, DeltaDictionary<TKey, TType>, DeltaDictionary<TKey, TType>> DeltaPropertyChangedCallback)
		{
			var np = new DeltaProperty<DeltaDictionary<TKey, TType>>(HashID.GenerateHashID(OwnerType.FullName + "." + Name), HashID.GenerateHashID(OwnerType.FullName), OwnerType, DeltaPropertyChangedCallback) { Mode = PropertyMode.Dictionary };
			if (!registered.TryAdd(np.ID, np))
				throw new ArgumentException(string.Format("Unable to register the DeltaProperty '{0}' on type '{1}'. A DeltaProperty with the same Name and OwnerType has already been registered.", Name, np.OwnerType));
			return np;
		}

		public static DeltaProperty<DeltaList<TType>> RegisterList<TType>(string Name, Type OwnerType, Action<DeltaObject, DeltaList<TType>, DeltaList<TType>> DeltaPropertyChangedCallback)
		{
			var np = new DeltaProperty<DeltaList<TType>>(HashID.GenerateHashID(OwnerType.FullName + "." + Name), HashID.GenerateHashID(OwnerType.FullName), OwnerType, DeltaPropertyChangedCallback) { Mode = PropertyMode.List };
			if (!registered.TryAdd(np.ID, np))
				throw new ArgumentException(string.Format("Unable to register the DeltaProperty '{0}' on type '{1}'. A DeltaProperty with the same Name and OwnerType has already been registered.", Name, np.OwnerType));
			return np;
		}

		public override int GetHashCode()
		{
			return ID.GetHashCode() ^ OwnerType.GetHashCode();
		}
	}
}