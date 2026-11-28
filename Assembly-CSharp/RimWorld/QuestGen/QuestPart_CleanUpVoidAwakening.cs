using System;
using System.Collections.Generic;
using RimWorld.Utility;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020022A6 RID: 8870
	public class QuestPart_CleanUpVoidAwakening : QuestPart
	{
		// Token: 0x0600DAFE RID: 56062 RVA: 0x00519635 File Offset: 0x00517835
		public QuestPart_CleanUpVoidAwakening()
		{
		}

		// Token: 0x0600DAFF RID: 56063 RVA: 0x0051963D File Offset: 0x0051783D
		public QuestPart_CleanUpVoidAwakening(string inSignal, Map map)
		{
			this.inSignal = inSignal;
			this.map = map;
		}

		// Token: 0x0600DB00 RID: 56064 RVA: 0x00519654 File Offset: 0x00517854
		private void DestroyThingsOfDef(ThingDef def)
		{
			QuestPart_CleanUpVoidAwakening.tmpToDestroy.AddRange(this.map.listerThings.ThingsOfDef(def));
			for (int i = 0; i < QuestPart_CleanUpVoidAwakening.tmpToDestroy.Count; i++)
			{
				if (!QuestPart_CleanUpVoidAwakening.tmpToDestroy[i].Destroyed)
				{
					QuestPart_CleanUpVoidAwakening.tmpToDestroy[i].Destroy(DestroyMode.QuestLogic);
				}
			}
			QuestPart_CleanUpVoidAwakening.tmpToDestroy.Clear();
		}

		// Token: 0x0600DB01 RID: 56065 RVA: 0x005196C0 File Offset: 0x005178C0
		private void RemoveTerrainOfDef(TerrainDef def)
		{
			foreach (IntVec3 c in this.map.AllCells)
			{
				if (c.GetTerrain(this.map) == def)
				{
					this.map.terrainGrid.RemoveTopLayer(c, false);
				}
			}
		}

		// Token: 0x0600DB02 RID: 56066 RVA: 0x0051972C File Offset: 0x0051792C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag != this.inSignal)
			{
				return;
			}
			Thing.allowDestroyNonDestroyable = true;
			this.DestroyThingsOfDef(ThingDefOf.VoidStructure);
			this.DestroyThingsOfDef(ThingDefOf.PitBurrowSpawner);
			this.DestroyThingsOfDef(ThingDefOf.PitBurrow);
			this.DestroyThingsOfDef(ThingDefOf.Fleshbulb);
			this.DestroyThingsOfDef(ThingDefOf.VoidmetalMassSmall);
			this.DestroyThingsOfDef(ThingDefOf.VoidmetalMassMedium);
			Thing.allowDestroyNonDestroyable = false;
			this.RemoveTerrainOfDef(TerrainDefOf.Voidmetal);
			VoidAwakeningUtility.KillAllFreeEntities(this.map);
			GameCondition activeCondition = this.map.gameConditionManager.GetActiveCondition(GameConditionDefOf.UnnaturalDarkness);
			if (activeCondition == null)
			{
				return;
			}
			activeCondition.End();
		}

		// Token: 0x0600DB03 RID: 56067 RVA: 0x005197D0 File Offset: 0x005179D0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Map>(ref this.map, "map", false);
		}

		// Token: 0x0600DB04 RID: 56068 RVA: 0x005197FB File Offset: 0x005179FB
		public override void Cleanup()
		{
			base.Cleanup();
			this.map = null;
		}

		// Token: 0x04008C35 RID: 35893
		private string inSignal;

		// Token: 0x04008C36 RID: 35894
		private Map map;

		// Token: 0x04008C37 RID: 35895
		private static List<Thing> tmpToDestroy = new List<Thing>();
	}
}
