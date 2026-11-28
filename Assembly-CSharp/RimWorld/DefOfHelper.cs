using System;
using System.Reflection;
using System.Threading.Tasks;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E6A RID: 7786
	public static class DefOfHelper
	{
		// Token: 0x0600CA23 RID: 51747 RVA: 0x004A58F0 File Offset: 0x004A3AF0
		public static void RebindAllDefOfs(bool earlyTryMode)
		{
			DefOfHelper.earlyTry = earlyTryMode;
			DefOfHelper.bindingNow = true;
			try
			{
				Parallel.ForEach<Type>(GenTypes.AllTypesWithAttribute<DefOf>(), new Action<Type>(DefOfHelper.BindDefsFor));
			}
			finally
			{
				DefOfHelper.bindingNow = false;
			}
		}

		// Token: 0x0600CA24 RID: 51748 RVA: 0x004A593C File Offset: 0x004A3B3C
		private static void BindDefsFor(Type type)
		{
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Static | BindingFlags.Public))
			{
				Type fieldType = fieldInfo.FieldType;
				if (!GenTypes.IsDefThreaded(fieldType))
				{
					Type type2 = fieldType;
					Log.Error(((type2 != null) ? type2.ToString() : null) + " is not a Def.");
				}
				else
				{
					bool flag = !DefOfHelper.earlyTry;
					MayRequireAttribute mayRequireAttribute = fieldInfo.TryGetAttribute<MayRequireAttribute>();
					if (mayRequireAttribute != null && !ModsConfig.IsActive(mayRequireAttribute.modId))
					{
						flag = false;
					}
					MayRequireAnyOfAttribute mayRequireAnyOfAttribute = fieldInfo.TryGetAttribute<MayRequireAnyOfAttribute>();
					if (mayRequireAnyOfAttribute != null)
					{
						bool flag2 = false;
						string[] modIds = mayRequireAnyOfAttribute.modIds;
						for (int j = 0; j < modIds.Length; j++)
						{
							if (ModsConfig.IsActive(modIds[j]))
							{
								flag2 = true;
								break;
							}
						}
						if (!flag2)
						{
							flag = false;
						}
					}
					string text = fieldInfo.Name;
					DefAliasAttribute defAliasAttribute = fieldInfo.TryGetAttribute<DefAliasAttribute>();
					if (defAliasAttribute != null)
					{
						text = defAliasAttribute.defName;
					}
					if (fieldType == typeof(SoundDef))
					{
						SoundDef soundDef = SoundDef.Named(text);
						if (soundDef.isUndefined && flag)
						{
							Log.Error("Could not find SoundDef named " + text);
						}
						fieldInfo.SetValue(null, soundDef);
					}
					else
					{
						Def def = GenDefDatabase.GetDef(fieldType, text, flag);
						fieldInfo.SetValue(null, def);
					}
				}
			}
		}

		// Token: 0x0600CA25 RID: 51749 RVA: 0x004A5A7C File Offset: 0x004A3C7C
		public static void EnsureInitializedInCtor(Type defOf)
		{
			if (!DefOfHelper.bindingNow)
			{
				string text;
				if (DirectXmlToObject.currentlyInstantiatingObjectOfType.Any<Type>())
				{
					string str = "DirectXmlToObject is currently instantiating an object of type ";
					Type type = DirectXmlToObject.currentlyInstantiatingObjectOfType.Peek();
					text = str + ((type != null) ? type.ToString() : null);
				}
				else if (Scribe.mode == LoadSaveMode.LoadingVars)
				{
					text = "curParent=" + Scribe.loader.curParent.ToStringSafe<IExposable>() + " curPathRelToParent=" + Scribe.loader.curPathRelToParent;
				}
				else
				{
					text = "";
				}
				Log.Warning("Tried to use an uninitialized DefOf of type " + defOf.Name + ". DefOfs are initialized right after all defs all loaded. Uninitialized DefOfs will return only nulls. (hint: don't use DefOfs as default field values in Defs, try to resolve them in ResolveReferences() instead)" + (text.NullOrEmpty() ? "" : (" Debug info: " + text)));
			}
			if (GenTypes.IsDefThreaded(defOf))
			{
				Log.Warning("Possible typo: " + defOf.Name + ". Using def type name not preceded by \"Of\"");
			}
		}

		// Token: 0x040072F4 RID: 29428
		private static bool bindingNow;

		// Token: 0x040072F5 RID: 29429
		private static bool earlyTry = true;
	}
}
