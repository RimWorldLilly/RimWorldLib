using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000486 RID: 1158
	public class HediffCompProperties_VerbGiver : HediffCompProperties
	{
		// Token: 0x0600203B RID: 8251 RVA: 0x000C22C5 File Offset: 0x000C04C5
		public HediffCompProperties_VerbGiver()
		{
			this.compClass = typeof(HediffComp_VerbGiver);
		}

		// Token: 0x0600203C RID: 8252 RVA: 0x000C22E0 File Offset: 0x000C04E0
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.tools != null)
			{
				for (int i = 0; i < this.tools.Count; i++)
				{
					this.tools[i].id = i.ToString();
				}
			}
		}

		// Token: 0x0600203D RID: 8253 RVA: 0x000C2329 File Offset: 0x000C0529
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			HediffCompProperties_VerbGiver.<ConfigErrors>d__5 <ConfigErrors>d__ = new HediffCompProperties_VerbGiver.<ConfigErrors>d__5(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x04001909 RID: 6409
		public List<VerbProperties> verbs;

		// Token: 0x0400190A RID: 6410
		public List<Tool> tools;

		// Token: 0x0400190B RID: 6411
		public ImplementOwnerTypeDef ownerTypeOverride;
	}
}
