using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001650 RID: 5712
	public class Precept_Xenotype : Precept
	{
		// Token: 0x170018D5 RID: 6357
		// (get) Token: 0x06008F82 RID: 36738 RVA: 0x0034B88F File Offset: 0x00349A8F
		private string XenotypeName
		{
			get
			{
				if (this.xenotype != null)
				{
					return this.xenotype.label;
				}
				CustomXenotype customXenotype = this.customXenotype;
				if (customXenotype == null)
				{
					return null;
				}
				return customXenotype.name;
			}
		}

		// Token: 0x170018D6 RID: 6358
		// (get) Token: 0x06008F83 RID: 36739 RVA: 0x0034B8B6 File Offset: 0x00349AB6
		public override string UIInfoFirstLine
		{
			get
			{
				return this.XenotypeName.CapitalizeFirst();
			}
		}

		// Token: 0x170018D7 RID: 6359
		// (get) Token: 0x06008F84 RID: 36740 RVA: 0x0034B8C3 File Offset: 0x00349AC3
		public override string TipLabel
		{
			get
			{
				return this.def.issue.LabelCap + ": " + this.XenotypeName.CapitalizeFirst();
			}
		}

		// Token: 0x06008F85 RID: 36741 RVA: 0x0034B8F4 File Offset: 0x00349AF4
		public override string GenerateNameRaw()
		{
			return this.name;
		}

		// Token: 0x06008F86 RID: 36742 RVA: 0x0034B8FC File Offset: 0x00349AFC
		public override void DrawIcon(Rect rect)
		{
			if (this.xenotype != null)
			{
				GUI.DrawTexture(rect, this.xenotype.Icon);
				return;
			}
			if (this.customXenotype != null)
			{
				GUI.DrawTexture(rect, this.customXenotype.IconDef.Icon);
			}
		}

		// Token: 0x06008F87 RID: 36743 RVA: 0x0034B936 File Offset: 0x00349B36
		public override void CopyTo(Precept other)
		{
			base.CopyTo(other);
			Precept_Xenotype precept_Xenotype = (Precept_Xenotype)other;
			precept_Xenotype.xenotype = this.xenotype;
			precept_Xenotype.customXenotype = this.customXenotype;
		}

		// Token: 0x06008F88 RID: 36744 RVA: 0x0034B95C File Offset: 0x00349B5C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<XenotypeDef>(ref this.xenotype, "xenotype");
			Scribe_Deep.Look<CustomXenotype>(ref this.customXenotype, "customXenotype", Array.Empty<object>());
		}

		// Token: 0x040057A0 RID: 22432
		public XenotypeDef xenotype;

		// Token: 0x040057A1 RID: 22433
		public CustomXenotype customXenotype;
	}
}
