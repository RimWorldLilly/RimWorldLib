using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F65 RID: 3941
	public class GauranlenTreeModeDef : Def
	{
		// Token: 0x17001151 RID: 4433
		// (get) Token: 0x0600654F RID: 25935 RVA: 0x002419E0 File Offset: 0x0023FBE0
		public string Description
		{
			get
			{
				if (this.cachedDescription == null)
				{
					this.cachedDescription = this.description;
					PawnKindDef pawnKindDef = this.pawnKindDef;
					CompProperties_Spawner compProperties_Spawner = (pawnKindDef != null) ? pawnKindDef.race.GetCompProperties<CompProperties_Spawner>() : null;
					if (compProperties_Spawner != null)
					{
						this.cachedDescription = this.cachedDescription + "\n\n" + "DryadProducesResourcesDesc".Translate(this.pawnKindDef.Named("DRYAD"), GenLabel.ThingLabel(compProperties_Spawner.thingToSpawn, null, compProperties_Spawner.spawnCount).Named("RESOURCES"), compProperties_Spawner.spawnIntervalRange.max.ToStringTicksToPeriod(true, false, true, true, false).Named("DURATION")).Resolve().CapitalizeFirst();
					}
				}
				return this.cachedDescription;
			}
		}

		// Token: 0x04003B9D RID: 15261
		public GauranlenTreeModeDef previousStage;

		// Token: 0x04003B9E RID: 15262
		public PawnKindDef pawnKindDef;

		// Token: 0x04003B9F RID: 15263
		public List<MemeDef> requiredMemes;

		// Token: 0x04003BA0 RID: 15264
		public Vector2 drawPosition;

		// Token: 0x04003BA1 RID: 15265
		public List<StatDef> displayedStats;

		// Token: 0x04003BA2 RID: 15266
		public List<DefHyperlink> hyperlinks;

		// Token: 0x04003BA3 RID: 15267
		private string cachedDescription;
	}
}
