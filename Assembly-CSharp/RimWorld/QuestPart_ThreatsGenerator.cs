using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200111F RID: 4383
	public class QuestPart_ThreatsGenerator : QuestPartActivable, IIncidentMakerQuestPart
	{
		// Token: 0x06006D9F RID: 28063 RVA: 0x002669B2 File Offset: 0x00264BB2
		public IEnumerable<FiringIncident> MakeIntervalIncidents()
		{
			if (this.mapParent != null && this.mapParent.HasMap)
			{
				return ThreatsGenerator.MakeIntervalIncidents(this.parms, this.mapParent.Map, base.EnableTick + this.threatStartTicks);
			}
			return Enumerable.Empty<FiringIncident>();
		}

		// Token: 0x06006DA0 RID: 28064 RVA: 0x002669F4 File Offset: 0x00264BF4
		public override void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
			if (base.State != QuestPartState.Enabled)
			{
				return;
			}
			Rect rect = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect, "Log future incidents from " + base.GetType().Name, true, true, true, null))
			{
				StorytellerUtility.DebugLogTestFutureIncidents(false, null, this, 50);
			}
			curY += rect.height + 4f;
		}

		// Token: 0x06006DA1 RID: 28065 RVA: 0x00266A6A File Offset: 0x00264C6A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThreatsGeneratorParams>(ref this.parms, "parms", Array.Empty<object>());
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<int>(ref this.threatStartTicks, "threatStartTicks", 0, false);
		}

		// Token: 0x040045E2 RID: 17890
		public ThreatsGeneratorParams parms;

		// Token: 0x040045E3 RID: 17891
		public MapParent mapParent;

		// Token: 0x040045E4 RID: 17892
		public int threatStartTicks;
	}
}
