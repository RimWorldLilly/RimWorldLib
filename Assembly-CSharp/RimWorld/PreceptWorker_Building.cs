using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001641 RID: 5697
	public class PreceptWorker_Building : PreceptWorker
	{
		// Token: 0x06008EB7 RID: 36535 RVA: 0x00347358 File Offset: 0x00345558
		public override AcceptanceReport CanUse(ThingDef def, Ideo ideo, FactionDef generatingFor)
		{
			bool flag = false;
			foreach (Precept precept in ideo.PreceptsListForReading)
			{
				Precept_Building precept_Building = precept as Precept_Building;
				if (precept_Building != null && precept_Building.ThingDef != null && precept_Building.ThingDef.isAltar)
				{
					flag = true;
					break;
				}
			}
			if (flag && def.isAltar)
			{
				return new AcceptanceReport("IdeoAlreadyHasAltar".Translate());
			}
			if (!flag)
			{
				return def.isAltar;
			}
			if (!def.isAltar)
			{
				return true;
			}
			if (flag && def.isAltar)
			{
				return false;
			}
			return true;
		}
	}
}
