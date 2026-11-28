using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld.QuestGen
{
	// Token: 0x02002260 RID: 8800
	public class QuestPart_SpawnPawnsInStructure : QuestPart
	{
		// Token: 0x0600D974 RID: 55668 RVA: 0x00509519 File Offset: 0x00507719
		public QuestPart_SpawnPawnsInStructure()
		{
		}

		// Token: 0x0600D975 RID: 55669 RVA: 0x00509521 File Offset: 0x00507721
		public QuestPart_SpawnPawnsInStructure(List<Pawn> pawns, string inSignal)
		{
			this.pawns = pawns;
			this.inSignal = inSignal;
		}

		// Token: 0x0600D976 RID: 55670 RVA: 0x00509537 File Offset: 0x00507737
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
		}

		// Token: 0x0600D977 RID: 55671 RVA: 0x00509568 File Offset: 0x00507768
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag != this.inSignal)
			{
				return;
			}
			try
			{
				QuestPart_SpawnPawnsInStructure.<>c__DisplayClass5_0 CS$<>8__locals1 = new QuestPart_SpawnPawnsInStructure.<>c__DisplayClass5_0();
				MapParent mapParent;
				signal.args.TryGetArg<MapParent>("SUBJECT", out mapParent);
				CS$<>8__locals1.map = mapParent.Map;
				CellRect var = MapGenerator.GetVar<CellRect>("SpawnRect");
				foreach (Thing newThing in this.pawns)
				{
					IntVec3 loc;
					var.TryFindRandomCell(out loc, new Predicate<IntVec3>(CS$<>8__locals1.<Notify_QuestSignalReceived>g__Validator|0));
					GenSpawn.Spawn(newThing, loc, CS$<>8__locals1.map, WipeMode.Vanish);
				}
				LordMaker.MakeNewLord(Faction.OfAncientsHostile, new LordJob_SitePawns(this.pawns.First<Pawn>().Faction, var.CenterCell, 180000), CS$<>8__locals1.map, this.pawns);
				this.pawns.Clear();
			}
			catch (Exception value)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to generate pawns in structure: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x04008AF9 RID: 35577
		private List<Pawn> pawns;

		// Token: 0x04008AFA RID: 35578
		private string inSignal;
	}
}
