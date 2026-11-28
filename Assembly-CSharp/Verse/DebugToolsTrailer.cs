using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LudeonTK;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x020005C4 RID: 1476
	public static class DebugToolsTrailer
	{
		// Token: 0x06002E23 RID: 11811 RVA: 0x0011B878 File Offset: 0x00119A78
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000, requiresOdyssey = true)]
		private static void SwimAt(Pawn pawn)
		{
			DebugTools.curTool = new DebugTool("Select destination...", delegate()
			{
				List<IntVec3> list;
				if (!SwimPathFinder.TryFindSwimPath(pawn, UI.MouseCell(), out list))
				{
					return;
				}
				pawn.needs.joy.CurLevelPercentage = 0.3f;
				Job job = JobMaker.MakeJob(JobDefOf.GoSwimming, list[0]);
				job.locomotionUrgency = LocomotionUrgency.Walk;
				job.targetQueueA = new List<LocalTargetInfo>();
				for (int i = 1; i < list.Count; i++)
				{
					job.targetQueueA.Add(list[i]);
				}
				pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				DebugTools.curTool = null;
			}, null);
		}

		// Token: 0x06002E24 RID: 11812 RVA: 0x0011B8B0 File Offset: 0x00119AB0
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static void MeleeAttackTarget(Pawn p)
		{
			DebugTools.curTool = new DebugTool("Select target...", delegate()
			{
				Pawn t = DebugToolsTrailer.<MeleeAttackTarget>g__PawnAt|1_0(UI.MouseCell());
				Job job = JobMaker.MakeJob(JobDefOf.AttackMelee);
				job.targetA = t;
				p.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, null);
		}

		// Token: 0x06002E25 RID: 11813 RVA: 0x0011B8E6 File Offset: 0x00119AE6
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000, requiresOdyssey = true)]
		private static void VolcanicDebris()
		{
			SkyfallerMaker.SpawnSkyfaller(ThingDefOf.LavaRockIncoming, UI.MouseCell(), Find.CurrentMap);
		}

		// Token: 0x06002E26 RID: 11814 RVA: 0x0011B900 File Offset: 0x00119B00
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000, requiresOdyssey = true)]
		private static List<DebugActionNode> VolcanicDebrisDelayed()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			for (int i = 0; i < 41; i++)
			{
				int delay = i * 30;
				list.Add(new DebugActionNode(((float)delay / 60f).ToString("F1") + " seconds", DebugActionType.ToolMap, delegate()
				{
					SkyfallerMaker.SpawnSkyfaller(ThingDefOf.LavaRockIncoming, UI.MouseCell(), Find.CurrentMap).ticksToImpact += delay;
				}, null));
			}
			return list;
		}

		// Token: 0x06002E27 RID: 11815 RVA: 0x0011B970 File Offset: 0x00119B70
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000, requiresOdyssey = true)]
		private static void NuzzlePawn(Pawn animal)
		{
			if (!animal.IsAnimal)
			{
				Messages.Message("Must select an animal", MessageTypeDefOf.RejectInput, true);
				return;
			}
			DebugTools.curTool = new DebugTool("Select target...", delegate()
			{
				Pawn pawn = animal.Map.thingGrid.ThingsAt(UI.MouseCell()).OfType<Pawn>().FirstOrDefault<Pawn>();
				if (pawn == null || !pawn.RaceProps.Humanlike)
				{
					return;
				}
				pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Wait, 3000, false), new JobTag?(JobTag.Misc), false);
				Job job = JobMaker.MakeJob(JobDefOf.Nuzzle, pawn);
				job.locomotionUrgency = LocomotionUrgency.Walk;
				job.expiryInterval = 3000;
				animal.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				DebugTools.curTool = null;
			}, null);
		}

		// Token: 0x06002E28 RID: 11816 RVA: 0x0011B9C4 File Offset: 0x00119BC4
		[CompilerGenerated]
		internal static Pawn <MeleeAttackTarget>g__PawnAt|1_0(IntVec3 c)
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(c))
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null)
				{
					return pawn;
				}
			}
			return null;
		}
	}
}
