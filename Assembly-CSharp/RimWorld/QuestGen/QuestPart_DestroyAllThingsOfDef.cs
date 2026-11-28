using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200229E RID: 8862
	public class QuestPart_DestroyAllThingsOfDef : QuestPart
	{
		// Token: 0x0600DACF RID: 56015 RVA: 0x00517ACD File Offset: 0x00515CCD
		public QuestPart_DestroyAllThingsOfDef()
		{
		}

		// Token: 0x0600DAD0 RID: 56016 RVA: 0x00517AE0 File Offset: 0x00515CE0
		public QuestPart_DestroyAllThingsOfDef(string inSignal, MapParent mapParent, List<ThingDef> defsToDestroy)
		{
			this.inSignal = inSignal;
			this.mapParent = mapParent;
			this.defsToDestroy = defsToDestroy;
		}

		// Token: 0x0600DAD1 RID: 56017 RVA: 0x00517B08 File Offset: 0x00515D08
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal)
			{
				this.DestroyThings();
			}
		}

		// Token: 0x0600DAD2 RID: 56018 RVA: 0x00517B24 File Offset: 0x00515D24
		private void DestroyThingsOfDef(ThingDef def)
		{
			QuestPart_DestroyAllThingsOfDef.tmpToDestroy.AddRange(this.mapParent.Map.listerThings.ThingsOfDef(def));
			for (int i = 0; i < QuestPart_DestroyAllThingsOfDef.tmpToDestroy.Count; i++)
			{
				if (!QuestPart_DestroyAllThingsOfDef.tmpToDestroy[i].Destroyed)
				{
					QuestPart_DestroyAllThingsOfDef.tmpToDestroy[i].Destroy(DestroyMode.QuestLogic);
				}
			}
			QuestPart_DestroyAllThingsOfDef.tmpToDestroy.Clear();
		}

		// Token: 0x0600DAD3 RID: 56019 RVA: 0x00517B94 File Offset: 0x00515D94
		private void DestroyThings()
		{
			MapParent mapParent = this.mapParent;
			if (((mapParent != null) ? mapParent.Map : null) == null)
			{
				return;
			}
			foreach (ThingDef def in this.defsToDestroy)
			{
				this.DestroyThingsOfDef(def);
			}
		}

		// Token: 0x0600DAD4 RID: 56020 RVA: 0x00517BFC File Offset: 0x00515DFC
		public override void Cleanup()
		{
			base.Cleanup();
			this.DestroyThings();
			this.mapParent = null;
		}

		// Token: 0x0600DAD5 RID: 56021 RVA: 0x00517C14 File Offset: 0x00515E14
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Collections.Look<ThingDef>(ref this.defsToDestroy, "defsToDestroy", LookMode.Def, Array.Empty<object>());
		}

		// Token: 0x04008C07 RID: 35847
		public string inSignal;

		// Token: 0x04008C08 RID: 35848
		public MapParent mapParent;

		// Token: 0x04008C09 RID: 35849
		public List<ThingDef> defsToDestroy = new List<ThingDef>();

		// Token: 0x04008C0A RID: 35850
		private static List<Thing> tmpToDestroy = new List<Thing>();
	}
}
