using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CC9 RID: 3273
	public class LordToil_AssaultThings : LordToil
	{
		// Token: 0x17001033 RID: 4147
		// (get) Token: 0x06005CD1 RID: 23761 RVA: 0x002211F1 File Offset: 0x0021F3F1
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001034 RID: 4148
		// (get) Token: 0x06005CD2 RID: 23762 RVA: 0x002211F4 File Offset: 0x0021F3F4
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005CD3 RID: 23763 RVA: 0x002211F7 File Offset: 0x0021F3F7
		public LordToil_AssaultThings(IEnumerable<Thing> things)
		{
			this.things = new List<Thing>(things);
		}

		// Token: 0x06005CD4 RID: 23764 RVA: 0x0022120B File Offset: 0x0021F40B
		public override void Notify_ReachedDutyLocation(Pawn pawn)
		{
			this.UpdateAllDuties();
		}

		// Token: 0x06005CD5 RID: 23765 RVA: 0x00221214 File Offset: 0x0021F414
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty duty = this.lord.ownedPawns[i].mindState.duty;
				if (duty == null || duty.def != DutyDefOf.AssaultThing || duty.focus.ThingDestroyed)
				{
					Thing t2;
					if (!(from t in this.things
					where t != null && t.Spawned
					select t).TryRandomElement(out t2))
					{
						break;
					}
					this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.AssaultThing, t2, -1f);
				}
			}
		}

		// Token: 0x06005CD6 RID: 23766 RVA: 0x002212DE File Offset: 0x0021F4DE
		public override void LordToilTick()
		{
			if (this.lord.ticksInToil % 300 == 0)
			{
				this.UpdateAllDuties();
			}
		}

		// Token: 0x040034E5 RID: 13541
		private List<Thing> things;

		// Token: 0x040034E6 RID: 13542
		public const int UpdateIntervalTicks = 300;
	}
}
