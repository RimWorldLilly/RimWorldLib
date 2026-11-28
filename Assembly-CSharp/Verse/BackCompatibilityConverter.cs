using System;
using System.Collections.Generic;
using System.Xml;

namespace Verse
{
	// Token: 0x02000682 RID: 1666
	public abstract class BackCompatibilityConverter
	{
		// Token: 0x060034B5 RID: 13493
		public abstract bool AppliesToVersion(int majorVer, int minorVer);

		// Token: 0x060034B6 RID: 13494
		public abstract string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null);

		// Token: 0x060034B7 RID: 13495
		public abstract Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node);

		// Token: 0x060034B8 RID: 13496 RVA: 0x0015360F File Offset: 0x0015180F
		public virtual int GetBackCompatibleBodyPartIndex(BodyDef body, int index)
		{
			return index;
		}

		// Token: 0x060034B9 RID: 13497
		public abstract void PostExposeData(object obj);

		// Token: 0x060034BA RID: 13498 RVA: 0x00153612 File Offset: 0x00151812
		public virtual void PostCouldntLoadDef(string defName)
		{
		}

		// Token: 0x060034BB RID: 13499 RVA: 0x00153614 File Offset: 0x00151814
		public virtual void PreLoadSavegame(string loadingVersion)
		{
		}

		// Token: 0x060034BC RID: 13500 RVA: 0x00153616 File Offset: 0x00151816
		public virtual void PostLoadSavegame(string loadingVersion)
		{
		}

		// Token: 0x060034BD RID: 13501 RVA: 0x00153618 File Offset: 0x00151818
		public bool AppliesToLoadedGameVersion(bool allowInactiveScribe = false)
		{
			return !ScribeMetaHeaderUtility.loadedGameVersion.NullOrEmpty() && (allowInactiveScribe || Scribe.mode != LoadSaveMode.Inactive) && this.AppliesToVersion(ScribeMetaHeaderUtility.loadedGameVersionMajor, ScribeMetaHeaderUtility.loadedGameVersionMinor);
		}

		// Token: 0x060034BE RID: 13502 RVA: 0x00153644 File Offset: 0x00151844
		protected void Scribe_TypeToType<T1, T2>(object obj, ref T2 v, string label, Func<T1, T2> converter) where T2 : new()
		{
			if (!this.typeToTypeLookup.ContainsKey(obj))
			{
				this.typeToTypeLookup[obj] = new Dictionary<string, object>();
			}
			object value = this.typeToTypeLookup[obj][label];
			Scribe_Values.Look<object>(ref value, label, null, false);
			this.typeToTypeLookup[obj][label] = value;
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				v = converter((T1)((object)this.typeToTypeLookup[obj][label]));
			}
		}

		// Token: 0x0400226D RID: 8813
		private readonly Dictionary<object, Dictionary<string, object>> typeToTypeLookup = new Dictionary<object, Dictionary<string, object>>();
	}
}
