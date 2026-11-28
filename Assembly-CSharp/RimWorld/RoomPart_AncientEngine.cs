using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001267 RID: 4711
	public class RoomPart_AncientEngine : RoomPartWorker
	{
		// Token: 0x060074CB RID: 29899 RVA: 0x0028FFBD File Offset: 0x0028E1BD
		public RoomPart_AncientEngine(RoomPartDef def) : base(def)
		{
		}

		// Token: 0x060074CC RID: 29900 RVA: 0x0028FFC8 File Offset: 0x0028E1C8
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			CellRect cellRect = (from r in room.rects
			where r.Width >= 5 && r.Height >= 5
			select r).RandomElementByWeight((CellRect r) => (float)r.Area);
			if (cellRect == default(CellRect))
			{
				Log.Error("Failed to place ancient grav engine.");
				return;
			}
			Thing engine = RoomPart_AncientEngine.SpawnEngine(map, cellRect);
			RoomPart_AncientEngine.SpawnSignal(map, room, engine);
		}

		// Token: 0x060074CD RID: 29901 RVA: 0x00290058 File Offset: 0x0028E258
		private static Thing SpawnEngine(Map map, CellRect largest)
		{
			IntVec3 zero = IntVec3.Zero;
			if (largest.Width % 2 == 0)
			{
				zero.x = 1;
			}
			if (largest.Height % 2 == 0)
			{
				zero.z = 1;
			}
			return GenSpawn.Spawn(ThingDefOf.AncientGravEngine, largest.CenterCell - zero, map, WipeMode.Vanish);
		}

		// Token: 0x060074CE RID: 29902 RVA: 0x002900AC File Offset: 0x0028E2AC
		private static void SpawnSignal(Map map, LayoutRoom room, Thing engine)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(16, 1);
			defaultInterpolatedStringHandler.AppendLiteral("ThingDiscovered_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(Find.UniqueIDsManager.GetNextSignalTagID());
			string signalTag = defaultInterpolatedStringHandler.ToStringAndClear();
			SignalAction_Letter signalAction_Letter = (SignalAction_Letter)ThingMaker.MakeThing(ThingDefOf.SignalAction_Letter, null);
			signalAction_Letter.signalTag = signalTag;
			signalAction_Letter.letterDef = LetterDefOf.NeutralEvent;
			signalAction_Letter.letterLabelKey = "LetterAncientGravEngineDiscoveredLabel";
			signalAction_Letter.letterMessageKey = "LetterAncientGravEngineDiscovered";
			signalAction_Letter.lookTargets = engine;
			signalAction_Letter.requireLookTargetsNotDestroyed = true;
			GenSpawn.Spawn(signalAction_Letter, engine.Position, map, WipeMode.Vanish);
			room.SpawnRectTriggersForAction(signalAction_Letter, map);
		}
	}
}
