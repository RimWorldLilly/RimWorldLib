using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000853 RID: 2131
	public static class InsultingSpreeMentalStateUtility
	{
		// Token: 0x060042F6 RID: 17142 RVA: 0x001A6F08 File Offset: 0x001A5108
		public static bool CanChaseAndInsult(Pawn bully, Pawn insulted, bool skipReachabilityCheck = false, bool allowPrisoners = true)
		{
			return insulted.RaceProps.Humanlike && (insulted.Faction == bully.Faction || (allowPrisoners && insulted.HostFaction == bully.Faction)) && insulted != bully && !insulted.Dead && !insulted.Downed && insulted.Spawned && insulted.Awake() && insulted.Position.InHorDistOf(bully.Position, 40f) && SocialInteractionUtility.CanReceiveInteraction(insulted, null) && !insulted.HostileTo(bully) && Find.TickManager.TicksGame - insulted.mindState.lastHarmTick >= 833 && (skipReachabilityCheck || bully.CanReach(insulted, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn));
		}

		// Token: 0x060042F7 RID: 17143 RVA: 0x001A6FD4 File Offset: 0x001A51D4
		public static void GetInsultCandidatesFor(Pawn bully, List<Pawn> outCandidates, bool allowPrisoners = true)
		{
			outCandidates.Clear();
			Region region = bully.GetRegion(RegionType.Set_Passable);
			if (region == null)
			{
				return;
			}
			TraverseParms traverseParams = TraverseParms.For(bully, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true);
			RegionTraverser.BreadthFirstTraverse(region, (Region from, Region to) => to.Allows(traverseParams, false), delegate(Region r)
			{
				List<Thing> list = r.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn);
				for (int i = 0; i < list.Count; i++)
				{
					Pawn pawn = (Pawn)list[i];
					if (InsultingSpreeMentalStateUtility.CanChaseAndInsult(bully, pawn, true, allowPrisoners))
					{
						outCandidates.Add(pawn);
					}
				}
				return false;
			}, 40, RegionType.Set_Passable);
		}

		// Token: 0x04002BD9 RID: 11225
		private const int MaxRegionsToSearch = 40;

		// Token: 0x04002BDA RID: 11226
		public const int MaxDistance = 40;

		// Token: 0x04002BDB RID: 11227
		public const int MinTicksBetweenInsults = 1200;
	}
}
