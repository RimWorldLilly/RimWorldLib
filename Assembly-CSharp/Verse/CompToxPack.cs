using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000539 RID: 1337
	public class CompToxPack : CompAIUsablePack
	{
		// Token: 0x06002759 RID: 10073 RVA: 0x000F61BC File Offset: 0x000F43BC
		protected override float ChanceToUse(Pawn wearer)
		{
			if (!ModsConfig.BiotechActive)
			{
				return 0f;
			}
			int num = GenRadial.NumCellsInRadius(1.9f);
			float num2 = 0f;
			for (int i = 0; i < num; i++)
			{
				IntVec3 c = wearer.Position + GenRadial.RadialPattern[i];
				if (c.InBounds(wearer.Map))
				{
					List<Thing> thingList = c.GetThingList(wearer.Map);
					for (int j = 0; j < thingList.Count; j++)
					{
						Pawn pawn = thingList[j] as Pawn;
						if (pawn != null && pawn != wearer && pawn.HostileTo(wearer) && GasUtility.IsAffectedByExposure(pawn) && !pawn.IsPsychologicallyInvisible())
						{
							num2 += pawn.BodySize;
							if (num2 >= 1f)
							{
								break;
							}
						}
					}
				}
			}
			return num2;
		}

		// Token: 0x0600275A RID: 10074 RVA: 0x000F628A File Offset: 0x000F448A
		protected override void UsePack(Pawn wearer)
		{
			Verb_DeployToxPack.TryDeploy(this.parent.TryGetComp<CompApparelReloadable>(), this.parent.TryGetComp<CompReleaseGas>());
		}
	}
}
