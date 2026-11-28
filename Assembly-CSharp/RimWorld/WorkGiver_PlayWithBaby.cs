using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C33 RID: 3123
	public class WorkGiver_PlayWithBaby : WorkGiver_Scanner
	{
		// Token: 0x17000F8B RID: 3979
		// (get) Token: 0x060058E6 RID: 22758 RVA: 0x0020D53A File Offset: 0x0020B73A
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		// Token: 0x17000F8C RID: 3980
		// (get) Token: 0x060058E7 RID: 22759 RVA: 0x0020D53D File Offset: 0x0020B73D
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x060058E8 RID: 22760 RVA: 0x0020D546 File Offset: 0x0020B746
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedBabiesInFaction(pawn.Faction);
		}

		// Token: 0x060058E9 RID: 22761 RVA: 0x0020D55E File Offset: 0x0020B75E
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return ModsConfig.BiotechActive && pawn.IsBurning();
		}

		// Token: 0x060058EA RID: 22762 RVA: 0x0020D574 File Offset: 0x0020B774
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null)
			{
				return false;
			}
			if (!pawn2.DevelopmentalStage.Baby())
			{
				return false;
			}
			if (pawn2.needs.play == null)
			{
				return false;
			}
			if (forced)
			{
				if (pawn2.needs.play.CurLevelPercentage >= 0.95f)
				{
					JobFailReason.Is("CannotInteractBabyPlayFull".Translate(), null);
					return false;
				}
			}
			else
			{
				if (!pawn2.Awake())
				{
					return false;
				}
				if (!pawn2.needs.play.IsLow)
				{
					return false;
				}
			}
			using (IEnumerator<BabyPlayDef> enumerator = DefDatabase<BabyPlayDef>.AllDefs.InRandomOrder(null).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Worker.CanDo(pawn, pawn2))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060058EB RID: 22763 RVA: 0x0020D64C File Offset: 0x0020B84C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn baby = (Pawn)t;
			foreach (BabyPlayDef babyPlayDef in DefDatabase<BabyPlayDef>.AllDefs.InRandomOrder(null))
			{
				if (babyPlayDef.Worker.CanDo(pawn, baby))
				{
					return babyPlayDef.Worker.TryGiveJob(pawn, baby);
				}
			}
			return null;
		}

		// Token: 0x04003355 RID: 13141
		private const float BabyPlayNeedFullThreshold = 0.95f;
	}
}
