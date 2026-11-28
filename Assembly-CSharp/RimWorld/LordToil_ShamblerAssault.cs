using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CF2 RID: 3314
	public class LordToil_ShamblerAssault : LordToil
	{
		// Token: 0x1700105D RID: 4189
		// (get) Token: 0x06005D87 RID: 23943 RVA: 0x00224128 File Offset: 0x00222328
		public override bool AssignsDuties
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700105E RID: 4190
		// (get) Token: 0x06005D88 RID: 23944 RVA: 0x0022412B File Offset: 0x0022232B
		public override bool AllowAggressiveTargetingOfRoamers
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005D89 RID: 23945 RVA: 0x00224130 File Offset: 0x00222330
		public override void UpdateAllDuties()
		{
			IEnumerable<Pawn> source = from pt in this.lord.Map.mapPawns.AllHumanlikeSpawned.Concat(this.lord.Map.mapPawns.SpawnedColonyAnimals).Concat(this.lord.Map.mapPawns.SpawnedColonyMechs)
			where pt.Faction != Faction.OfEntities && !pt.ThreatDisabled(null)
			select pt;
			if (!source.Any<Pawn>())
			{
				base.Map.lordManager.RemoveLord(this.lord);
				return;
			}
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (pawn.mindState.enemyTarget == null)
				{
					Hediff_Shambler hediff_Shambler = pawn.mutant.Hediff as Hediff_Shambler;
					if (hediff_Shambler != null)
					{
						hediff_Shambler.Notify_DelayedAlert(source.RandomElement<Pawn>());
					}
					pawn.mindState.Notify_EngagedTarget();
				}
			}
		}

		// Token: 0x06005D8A RID: 23946 RVA: 0x00224230 File Offset: 0x00222430
		public override void LordToilTick()
		{
			if (this.lord.ticksInToil % 300 == 0)
			{
				this.UpdateAllDuties();
			}
		}

		// Token: 0x06005D8B RID: 23947 RVA: 0x0022424B File Offset: 0x0022244B
		public override bool CanAddPawn(Pawn p)
		{
			return p.IsShambler;
		}

		// Token: 0x04003518 RID: 13592
		private const int UpdateIntervalTicks = 300;
	}
}
