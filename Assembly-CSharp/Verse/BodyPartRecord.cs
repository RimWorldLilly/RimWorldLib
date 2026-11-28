using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000118 RID: 280
	public class BodyPartRecord
	{
		// Token: 0x17000171 RID: 369
		// (get) Token: 0x0600096D RID: 2413 RVA: 0x00035A37 File Offset: 0x00033C37
		public bool IsCorePart
		{
			get
			{
				return this.parent == null;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600096E RID: 2414 RVA: 0x00035A42 File Offset: 0x00033C42
		public string Label
		{
			get
			{
				if (!this.customLabel.NullOrEmpty())
				{
					return this.customLabel;
				}
				return this.def.label;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x0600096F RID: 2415 RVA: 0x00035A63 File Offset: 0x00033C63
		public string LabelCap
		{
			get
			{
				if (this.customLabel.NullOrEmpty())
				{
					return this.def.LabelCap;
				}
				if (this.cachedCustomLabelCap == null)
				{
					this.cachedCustomLabelCap = this.customLabel.CapitalizeFirst();
				}
				return this.cachedCustomLabelCap;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000970 RID: 2416 RVA: 0x00035AA2 File Offset: 0x00033CA2
		public string LabelShort
		{
			get
			{
				return this.def.LabelShort;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000971 RID: 2417 RVA: 0x00035AAF File Offset: 0x00033CAF
		public string LabelShortCap
		{
			get
			{
				return this.def.LabelShortCap;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000972 RID: 2418 RVA: 0x00035ABC File Offset: 0x00033CBC
		public int Index
		{
			get
			{
				return this.body.GetIndexOfPart(this);
			}
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x00035ACC File Offset: 0x00033CCC
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"BodyPartRecord(",
				(this.def != null) ? this.def.defName : "NULL_DEF",
				" parts.Count=",
				this.parts.Count.ToString(),
				")"
			});
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x00035B2F File Offset: 0x00033D2F
		public void PostLoad()
		{
			this.untranslatedCustomLabel = this.customLabel;
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x00035B40 File Offset: 0x00033D40
		public bool IsInGroup(BodyPartGroupDef group)
		{
			for (int i = 0; i < this.groups.Count; i++)
			{
				if (this.groups[i] == group)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x00035B75 File Offset: 0x00033D75
		public IEnumerable<BodyPartRecord> GetChildParts(BodyPartTagDef tag)
		{
			BodyPartRecord.<GetChildParts>d__31 <GetChildParts>d__ = new BodyPartRecord.<GetChildParts>d__31(-2);
			<GetChildParts>d__.<>4__this = this;
			<GetChildParts>d__.<>3__tag = tag;
			return <GetChildParts>d__;
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x00035B8C File Offset: 0x00033D8C
		public IEnumerable<BodyPartRecord> GetPartAndAllChildParts()
		{
			BodyPartRecord.<GetPartAndAllChildParts>d__32 <GetPartAndAllChildParts>d__ = new BodyPartRecord.<GetPartAndAllChildParts>d__32(-2);
			<GetPartAndAllChildParts>d__.<>4__this = this;
			return <GetPartAndAllChildParts>d__;
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x00035B9C File Offset: 0x00033D9C
		public IEnumerable<BodyPartRecord> GetDirectChildParts()
		{
			BodyPartRecord.<GetDirectChildParts>d__33 <GetDirectChildParts>d__ = new BodyPartRecord.<GetDirectChildParts>d__33(-2);
			<GetDirectChildParts>d__.<>4__this = this;
			return <GetDirectChildParts>d__;
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x00035BAC File Offset: 0x00033DAC
		public bool HasChildParts(BodyPartTagDef tag)
		{
			return this.GetChildParts(tag).Any<BodyPartRecord>();
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x00035BBA File Offset: 0x00033DBA
		public IEnumerable<BodyPartRecord> GetConnectedParts(BodyPartTagDef tag)
		{
			BodyPartRecord.<GetConnectedParts>d__35 <GetConnectedParts>d__ = new BodyPartRecord.<GetConnectedParts>d__35(-2);
			<GetConnectedParts>d__.<>4__this = this;
			<GetConnectedParts>d__.<>3__tag = tag;
			return <GetConnectedParts>d__;
		}

		// Token: 0x04000484 RID: 1156
		public BodyDef body;

		// Token: 0x04000485 RID: 1157
		[TranslationHandle]
		public BodyPartDef def;

		// Token: 0x04000486 RID: 1158
		[MustTranslate]
		public string customLabel;

		// Token: 0x04000487 RID: 1159
		[Unsaved(false)]
		[TranslationHandle(Priority = 100)]
		public string untranslatedCustomLabel;

		// Token: 0x04000488 RID: 1160
		public List<BodyPartRecord> parts = new List<BodyPartRecord>();

		// Token: 0x04000489 RID: 1161
		public BodyPartHeight height;

		// Token: 0x0400048A RID: 1162
		public BodyPartDepth depth;

		// Token: 0x0400048B RID: 1163
		public float coverage = 1f;

		// Token: 0x0400048C RID: 1164
		public List<BodyPartGroupDef> groups = new List<BodyPartGroupDef>();

		// Token: 0x0400048D RID: 1165
		[NoTranslate]
		public string woundAnchorTag;

		// Token: 0x0400048E RID: 1166
		public bool flipGraphic;

		// Token: 0x0400048F RID: 1167
		public List<Rot4> visibleHediffRots;

		// Token: 0x04000490 RID: 1168
		[Unsaved(false)]
		public BodyPartRecord parent;

		// Token: 0x04000491 RID: 1169
		[Unsaved(false)]
		public float coverageAbsWithChildren;

		// Token: 0x04000492 RID: 1170
		[Unsaved(false)]
		public float coverageAbs;

		// Token: 0x04000493 RID: 1171
		[Unsaved(false)]
		private string cachedCustomLabelCap;
	}
}
