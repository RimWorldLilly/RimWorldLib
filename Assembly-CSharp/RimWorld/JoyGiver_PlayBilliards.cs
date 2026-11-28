using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B71 RID: 2929
	public class JoyGiver_PlayBilliards : JoyGiver_InteractBuilding
	{
		// Token: 0x17000F0F RID: 3855
		// (get) Token: 0x06005536 RID: 21814 RVA: 0x001FADED File Offset: 0x001F8FED
		protected override bool CanDoDuringGathering
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005537 RID: 21815 RVA: 0x001FADF0 File Offset: 0x001F8FF0
		protected override Job TryGivePlayJob(Pawn pawn, Thing t)
		{
			if (!JoyGiver_PlayBilliards.ThingHasStandableSpaceOnAllSides(t))
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, t);
		}

		// Token: 0x06005538 RID: 21816 RVA: 0x001FAE14 File Offset: 0x001F9014
		public static bool ThingHasStandableSpaceOnAllSides(Thing t)
		{
			CellRect cellRect = t.OccupiedRect();
			foreach (IntVec3 c in cellRect.ExpandedBy(1))
			{
				if (!cellRect.Contains(c) && !c.Standable(t.Map))
				{
					return false;
				}
			}
			return true;
		}
	}
}
