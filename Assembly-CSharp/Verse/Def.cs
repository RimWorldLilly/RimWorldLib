using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RimWorld;

namespace Verse
{
	// Token: 0x0200010D RID: 269
	public class Def : Editable, IEquatable<Def>
	{
		// Token: 0x1700016B RID: 363
		// (get) Token: 0x0600094D RID: 2381 RVA: 0x000356C4 File Offset: 0x000338C4
		public virtual TaggedString LabelCap
		{
			get
			{
				if (this.label.NullOrEmpty())
				{
					return null;
				}
				if (this.cachedLabelCap.NullOrEmpty())
				{
					this.cachedLabelCap = this.label.CapitalizeFirst();
				}
				return this.cachedLabelCap;
			}
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x0003573F File Offset: 0x0003393F
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			Def.<SpecialDisplayStats>d__19 <SpecialDisplayStats>d__ = new Def.<SpecialDisplayStats>d__19(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x00035750 File Offset: 0x00033950
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.modExtensions != null)
			{
				for (int i = 0; i < this.modExtensions.Count; i++)
				{
					this.modExtensions[i].ResolveReferences(this);
				}
			}
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x00035793 File Offset: 0x00033993
		public override IEnumerable<string> ConfigErrors()
		{
			Def.<ConfigErrors>d__23 <ConfigErrors>d__ = new Def.<ConfigErrors>d__23(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x000357A3 File Offset: 0x000339A3
		public virtual void PostSetIndices()
		{
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x000357A5 File Offset: 0x000339A5
		public virtual void ClearCachedData()
		{
			this.cachedLabelCap = null;
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x000357B3 File Offset: 0x000339B3
		public override string ToString()
		{
			return this.defName;
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x000357BB File Offset: 0x000339BB
		public void ResolveDefNameHash()
		{
			this.defNameHash = this.defName.GetHashCode();
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x000357CE File Offset: 0x000339CE
		public override int GetHashCode()
		{
			return this.defNameHash;
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x000357D8 File Offset: 0x000339D8
		public T GetModExtension<T>() where T : DefModExtension
		{
			if (this.modExtensions == null)
			{
				return default(T);
			}
			for (int i = 0; i < this.modExtensions.Count; i++)
			{
				if (this.modExtensions[i] is T)
				{
					return this.modExtensions[i] as T;
				}
			}
			return default(T);
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x00035840 File Offset: 0x00033A40
		public bool HasModExtension<T>() where T : DefModExtension
		{
			return this.GetModExtension<T>() != null;
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x00035850 File Offset: 0x00033A50
		public bool Equals(Def other)
		{
			return other != null && other.defNameHash == this.defNameHash && other.GetType() == base.GetType();
		}

		// Token: 0x0400044E RID: 1102
		[Description("The name of this Def. It is used as an identifier by the game code.")]
		[NoTranslate]
		public string defName = "UnnamedDef";

		// Token: 0x0400044F RID: 1103
		[Description("A human-readable label used to identify this in game.")]
		[DefaultValue(null)]
		[MustTranslate]
		public string label;

		// Token: 0x04000450 RID: 1104
		[Description("A human-readable description given when the Def is inspected by players.")]
		[DefaultValue(null)]
		[MustTranslate]
		public string description;

		// Token: 0x04000451 RID: 1105
		[XmlInheritanceAllowDuplicateNodes]
		public List<DefHyperlink> descriptionHyperlinks;

		// Token: 0x04000452 RID: 1106
		[Description("Disables config error checking. Intended for mod use. (Be careful!)")]
		[DefaultValue(false)]
		[MustTranslate]
		public bool ignoreConfigErrors;

		// Token: 0x04000453 RID: 1107
		public bool ignoreIllegalLabelCharacterConfigError;

		// Token: 0x04000454 RID: 1108
		[Description("Mod-specific data. Not used by core game code.")]
		[DefaultValue(null)]
		public List<DefModExtension> modExtensions;

		// Token: 0x04000455 RID: 1109
		[Unsaved(false)]
		public ushort shortHash;

		// Token: 0x04000456 RID: 1110
		[Unsaved(false)]
		public ushort index = ushort.MaxValue;

		// Token: 0x04000457 RID: 1111
		[Unsaved(false)]
		public ModContentPack modContentPack;

		// Token: 0x04000458 RID: 1112
		[Unsaved(false)]
		public string fileName;

		// Token: 0x04000459 RID: 1113
		[Unsaved(false)]
		public int defNameHash;

		// Token: 0x0400045A RID: 1114
		[Unsaved(false)]
		protected TaggedString cachedLabelCap = null;

		// Token: 0x0400045B RID: 1115
		[Unsaved(false)]
		public bool generated;

		// Token: 0x0400045C RID: 1116
		[Unsaved(false)]
		public ushort debugRandomId = (ushort)Rand.RangeInclusive(0, 65535);

		// Token: 0x0400045D RID: 1117
		public const string DefaultDefName = "UnnamedDef";

		// Token: 0x0400045E RID: 1118
		private static readonly Regex AllowedDefNamesRegex = new Regex("^[a-zA-Z0-9\\-_]*$");

		// Token: 0x0400045F RID: 1119
		private static readonly Regex DisallowedLabelCharsRegex = new Regex("\\[|\\]|\\{|\\}");
	}
}
