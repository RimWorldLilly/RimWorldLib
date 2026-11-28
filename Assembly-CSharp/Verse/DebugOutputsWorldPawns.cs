using System;
using System.Collections.Generic;
using System.Linq;
using LudeonTK;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020005D4 RID: 1492
	public class DebugOutputsWorldPawns
	{
		// Token: 0x06002F65 RID: 12133 RVA: 0x0012F8F8 File Offset: 0x0012DAF8
		[DebugOutput("World pawns", true)]
		public static void ColonistRelativeChance()
		{
			HashSet<Pawn> hashSet = new HashSet<Pawn>(Find.WorldPawns.AllPawnsAliveOrDead);
			List<Pawn> list = new List<Pawn>();
			for (int i = 0; i < 500; i++)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.SpaceRefugee, Faction.OfAncients, PawnGenerationContext.NonPlayer, null, true, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
				list.Add(pawn);
				if (!pawn.IsWorldPawn())
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.KeepForever);
				}
			}
			int num = list.Count((Pawn x) => PawnRelationUtility.GetMostImportantColonyRelative(x) != null);
			Log.Message(string.Concat(new string[]
			{
				"Colony relatives: ",
				((float)num / 500f).ToStringPercent(),
				" (",
				num.ToString(),
				" of ",
				500.ToString(),
				")"
			}));
			foreach (Pawn pawn2 in Find.WorldPawns.AllPawnsAliveOrDead.ToList<Pawn>())
			{
				if (!hashSet.Contains(pawn2))
				{
					Find.WorldPawns.RemovePawn(pawn2);
					Find.WorldPawns.PassToWorld(pawn2, PawnDiscardDecideMode.Discard);
				}
			}
		}

		// Token: 0x06002F66 RID: 12134 RVA: 0x0012FADC File Offset: 0x0012DCDC
		[DebugOutput("World pawns", true)]
		public static void KidnappedPawns()
		{
			Find.FactionManager.LogKidnappedPawns();
		}

		// Token: 0x06002F67 RID: 12135 RVA: 0x0012FAE8 File Offset: 0x0012DCE8
		[DebugOutput("World pawns", true)]
		public static void WorldPawnList()
		{
			Find.WorldPawns.LogWorldPawns();
		}

		// Token: 0x06002F68 RID: 12136 RVA: 0x0012FAF4 File Offset: 0x0012DCF4
		[DebugOutput("World pawns", true)]
		public static void WorldPawnMothballInfo()
		{
			Find.WorldPawns.LogWorldPawnMothballPrevention();
		}

		// Token: 0x06002F69 RID: 12137 RVA: 0x0012FB00 File Offset: 0x0012DD00
		[DebugOutput("World pawns", true)]
		public static void WorldPawnGcBreakdown()
		{
			Find.WorldPawns.gc.LogGC();
		}

		// Token: 0x06002F6A RID: 12138 RVA: 0x0012FB11 File Offset: 0x0012DD11
		[DebugOutput("World pawns", true)]
		public static void WorldPawnDotgraph()
		{
			Find.WorldPawns.gc.LogDotgraph();
		}

		// Token: 0x06002F6B RID: 12139 RVA: 0x0012FB22 File Offset: 0x0012DD22
		[DebugOutput("World pawns", true)]
		public static void RunWorldPawnGc()
		{
			Find.WorldPawns.gc.RunGC();
		}

		// Token: 0x06002F6C RID: 12140 RVA: 0x0012FB33 File Offset: 0x0012DD33
		[DebugOutput("World pawns", true)]
		public static void RunWorldPawnMothball()
		{
			Find.WorldPawns.DebugRunMothballProcessing();
		}
	}
}
