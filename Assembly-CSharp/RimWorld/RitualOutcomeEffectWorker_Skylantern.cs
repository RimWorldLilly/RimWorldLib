using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020016EA RID: 5866
	public class RitualOutcomeEffectWorker_Skylantern : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x060092B8 RID: 37560 RVA: 0x00359D7E File Offset: 0x00357F7E
		public RitualOutcomeEffectWorker_Skylantern()
		{
		}

		// Token: 0x060092B9 RID: 37561 RVA: 0x00359D86 File Offset: 0x00357F86
		public RitualOutcomeEffectWorker_Skylantern(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x060092BA RID: 37562 RVA: 0x00359D90 File Offset: 0x00357F90
		protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			extraOutcomeDesc = null;
			if (outcome.Positive && Rand.Chance(0.2f))
			{
				List<Thing> list = new List<Thing>();
				Ideo ideo = jobRitual.Ritual.ideo;
				foreach (Precept precept in ideo.PreceptsListForReading)
				{
					Precept_Building precept_Building = precept as Precept_Building;
					if (precept_Building != null && precept_Building.ThingDef.ritualFocus != null && precept_Building.ThingDef.ritualFocus.consumable)
					{
						list.Add(ThingMaker.MakeThing(precept_Building.ThingDef, null).MakeMinified(DestroyMode.Vanish));
					}
				}
				if (list.Count == 0)
				{
					list.AddRange(ThingSetMakerDefOf.VisitorGift.root.Generate(new ThingSetMakerParams
					{
						totalMarketValueRange = new FloatRange?(RitualOutcomeEffectWorker_Skylantern.RewardMarketValueRange)
					}));
				}
				IncidentParms incidentParms = new IncidentParms
				{
					target = jobRitual.Map,
					points = 160f,
					gifts = list,
					pawnIdeo = ideo,
					pawnCount = 3,
					storeGeneratedNeutralPawns = new List<Pawn>()
				};
				if (IncidentDefOf.WanderersSkylantern.Worker.TryExecute(incidentParms))
				{
					List<GlobalTargetInfo> list2 = new List<GlobalTargetInfo>();
					list2.AddRange(letterLookTargets.targets);
					list2.AddRange(from p in incidentParms.storeGeneratedNeutralPawns
					select new GlobalTargetInfo(p));
					letterLookTargets = new LookTargets(list2);
					extraOutcomeDesc = "RitualOutcomeExtraDesc_SkylanternWanderers".Translate();
				}
			}
		}

		// Token: 0x040058CD RID: 22733
		public const float WanderersChance = 0.2f;

		// Token: 0x040058CE RID: 22734
		public const float WanderersPoints = 160f;

		// Token: 0x040058CF RID: 22735
		public static readonly FloatRange RewardMarketValueRange = new FloatRange(200f, 500f);
	}
}
