using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B5B RID: 2907
	public class JobGiver_PrisonerGetDressed : ThinkNode_JobGiver
	{
		// Token: 0x060054E2 RID: 21730 RVA: 0x001F9558 File Offset: 0x001F7758
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.guest.PrisonerIsSecure || pawn.apparel == null)
			{
				return null;
			}
			if (ThoughtUtility.CanGetThought(pawn, ThoughtDefOf.ClothedNudist, true) && pawn.AmbientTemperature >= pawn.SafeTemperatureRange().min)
			{
				return null;
			}
			if (pawn.royalty != null && pawn.royalty.AllTitlesInEffectForReading.Count > 0)
			{
				RoyalTitleDef def = pawn.royalty.MostSeniorTitle.def;
				if (def != null && def.requiredApparel != null)
				{
					for (int i = 0; i < def.requiredApparel.Count; i++)
					{
						if (def.requiredApparel[i].IsActive(pawn) && !def.requiredApparel[i].IsMet(pawn))
						{
							Apparel apparel = this.FindGarmentSatisfyingTitleRequirement(pawn, def.requiredApparel[i]);
							if (apparel != null)
							{
								Job job = JobMaker.MakeJob(JobDefOf.Wear, apparel);
								job.ignoreForbidden = true;
								return job;
							}
						}
					}
				}
			}
			if (!pawn.apparel.BodyPartGroupIsCovered(BodyPartGroupDefOf.Legs, null))
			{
				Apparel apparel2 = this.FindGarmentCoveringPart(pawn, BodyPartGroupDefOf.Legs);
				if (apparel2 != null)
				{
					Job job2 = JobMaker.MakeJob(JobDefOf.Wear, apparel2);
					job2.ignoreForbidden = true;
					return job2;
				}
			}
			if (!pawn.apparel.BodyPartGroupIsCovered(BodyPartGroupDefOf.Torso, null))
			{
				Apparel apparel3 = this.FindGarmentCoveringPart(pawn, BodyPartGroupDefOf.Torso);
				if (apparel3 != null)
				{
					Job job3 = JobMaker.MakeJob(JobDefOf.Wear, apparel3);
					job3.ignoreForbidden = true;
					return job3;
				}
			}
			return null;
		}

		// Token: 0x060054E3 RID: 21731 RVA: 0x001F96C4 File Offset: 0x001F78C4
		private Apparel FindGarmentCoveringPart(Pawn pawn, BodyPartGroupDef bodyPartGroupDef)
		{
			Room room = pawn.GetRoom(RegionType.Set_All);
			if (room.IsPrisonCell)
			{
				foreach (IntVec3 c in room.Cells)
				{
					List<Thing> thingList = c.GetThingList(pawn.Map);
					for (int i = 0; i < thingList.Count; i++)
					{
						Apparel apparel = thingList[i] as Apparel;
						if (apparel != null && apparel.def.apparel.bodyPartGroups.Contains(bodyPartGroupDef) && pawn.CanReserveAndReach(apparel, PathEndMode.OnCell, pawn.NormalMaxDanger(), 1, -1, null, false) && !apparel.IsBurning() && (!CompBiocodable.IsBiocoded(apparel) || CompBiocodable.IsBiocodedFor(apparel, pawn)) && ApparelUtility.HasPartsToWear(pawn, apparel.def) && apparel.PawnCanWear(pawn, true))
						{
							return apparel;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060054E4 RID: 21732 RVA: 0x001F97CC File Offset: 0x001F79CC
		private Apparel FindGarmentSatisfyingTitleRequirement(Pawn pawn, ApparelRequirement req)
		{
			Room room = pawn.GetRoom(RegionType.Set_All);
			if (room.IsPrisonCell)
			{
				foreach (IntVec3 c in room.Cells)
				{
					List<Thing> thingList = c.GetThingList(pawn.Map);
					for (int i = 0; i < thingList.Count; i++)
					{
						Apparel apparel = thingList[i] as Apparel;
						if (apparel != null && req.ApparelMeetsRequirement(thingList[i].def, false) && pawn.CanReserveAndReach(apparel, PathEndMode.OnCell, pawn.NormalMaxDanger(), 1, -1, null, false) && !apparel.IsBurning() && (!CompBiocodable.IsBiocoded(apparel) || CompBiocodable.IsBiocodedFor(apparel, pawn)) && ApparelUtility.HasPartsToWear(pawn, apparel.def) && apparel.PawnCanWear(pawn, true))
						{
							return apparel;
						}
					}
				}
			}
			return null;
		}
	}
}
