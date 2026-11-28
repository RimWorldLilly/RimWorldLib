using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Verse
{
	// Token: 0x02000108 RID: 264
	public static class GenDefDatabase
	{
		// Token: 0x06000934 RID: 2356 RVA: 0x000348F4 File Offset: 0x00032AF4
		public static Def GetDef(Type defType, string defName, bool errorOnFail = true)
		{
			object obj = GenDefDatabase.cachedGetNamedLock;
			Func<string, bool, Def> func;
			lock (obj)
			{
				if (!GenDefDatabase.cachedGetNamed.TryGetValue(defType, out func))
				{
					MethodInfo method = GenGeneric.MethodOnGenericType(typeof(DefDatabase<>), defType, "GetNamed");
					func = (Func<string, bool, Def>)Delegate.CreateDelegate(typeof(Func<string, bool, Def>), method);
					GenDefDatabase.cachedGetNamed.Add(defType, func);
				}
			}
			return func(defName, errorOnFail);
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x0003497C File Offset: 0x00032B7C
		public static Def GetDefSilentFail(Type type, string targetDefName, bool specialCaseForSoundDefs = true)
		{
			if (specialCaseForSoundDefs && type == typeof(SoundDef))
			{
				return SoundDef.Named(targetDefName);
			}
			object obj = GenDefDatabase.cachedGetNamedSilentFailLock;
			Func<string, Def> func;
			lock (obj)
			{
				if (!GenDefDatabase.cachedGetNamedSilentFail.TryGetValue(type, out func))
				{
					MethodInfo method = GenGeneric.MethodOnGenericType(typeof(DefDatabase<>), type, "GetNamedSilentFail");
					func = (Func<string, Def>)Delegate.CreateDelegate(typeof(Func<string, Def>), method);
					GenDefDatabase.cachedGetNamedSilentFail.Add(type, func);
				}
			}
			return func(targetDefName);
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x00034A20 File Offset: 0x00032C20
		public static IEnumerable<Def> GetAllDefsInDatabaseForDef(Type defType)
		{
			return ((IEnumerable)GenGeneric.GetStaticPropertyOnGenericType(typeof(DefDatabase<>), defType, "AllDefs")).Cast<Def>();
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x00034A41 File Offset: 0x00032C41
		public static IEnumerable<Type> AllDefTypesWithDatabases()
		{
			return new GenDefDatabase.<AllDefTypesWithDatabases>d__7(-2);
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x00034A4A File Offset: 0x00032C4A
		public static IEnumerable<T> DefsToGoInDatabase<T>(ModContentPack mod)
		{
			return mod.AllDefs.OfType<T>();
		}

		// Token: 0x04000446 RID: 1094
		private static readonly Dictionary<Type, Func<string, bool, Def>> cachedGetNamed = new Dictionary<Type, Func<string, bool, Def>>();

		// Token: 0x04000447 RID: 1095
		private static readonly object cachedGetNamedLock = new object();

		// Token: 0x04000448 RID: 1096
		private static readonly Dictionary<Type, Func<string, Def>> cachedGetNamedSilentFail = new Dictionary<Type, Func<string, Def>>();

		// Token: 0x04000449 RID: 1097
		private static readonly object cachedGetNamedSilentFailLock = new object();
	}
}
