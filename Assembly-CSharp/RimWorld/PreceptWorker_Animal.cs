using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001647 RID: 5703
	public class PreceptWorker_Animal : PreceptWorker
	{
		// Token: 0x06008F15 RID: 36629 RVA: 0x00349A84 File Offset: 0x00347C84
		public override AcceptanceReport CanUse(ThingDef def, Ideo ideo, FactionDef generatingFor)
		{
			foreach (Precept precept in ideo.PreceptsListForReading)
			{
				Precept_Animal precept_Animal = precept as Precept_Animal;
				if (precept_Animal != null && precept_Animal.ThingDef == def)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x170018B5 RID: 6325
		// (get) Token: 0x06008F16 RID: 36630 RVA: 0x00349AF4 File Offset: 0x00347CF4
		public override IEnumerable<PreceptThingChance> ThingDefs
		{
			get
			{
				return new PreceptWorker_Animal.<get_ThingDefs>d__2(-2);
			}
		}
	}
}
