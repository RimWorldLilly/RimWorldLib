using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A71 RID: 6769
	public class Building_TurretFoam : Building_TurretGun
	{
		// Token: 0x17001DF9 RID: 7673
		// (get) Token: 0x0600AA2F RID: 43567 RVA: 0x003C463A File Offset: 0x003C283A
		public override bool IsEverThreat
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600AA30 RID: 43568 RVA: 0x003C4640 File Offset: 0x003C2840
		public override LocalTargetInfo TryFindNewTarget()
		{
			int num = GenRadial.NumCellsInRadius(this.AttackVerb.EffectiveRange);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = base.Position + GenRadial.RadialPattern[i];
				if (GenSight.LineOfSight(base.Position, intVec, base.Map, true, null, 0, 0))
				{
					List<Thing> thingList = intVec.GetThingList(base.Map);
					for (int j = 0; j < thingList.Count; j++)
					{
						if (thingList[j] is Fire || thingList[j].HasAttachment(ThingDefOf.Fire))
						{
							return thingList[j].Position;
						}
					}
				}
			}
			return LocalTargetInfo.Invalid;
		}
	}
}
