using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FD1 RID: 4049
	public class RaidStrategyDef : Def
	{
		// Token: 0x1700119D RID: 4509
		// (get) Token: 0x060066B3 RID: 26291 RVA: 0x002471A4 File Offset: 0x002453A4
		public RaidStrategyWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (RaidStrategyWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x04003FCA RID: 16330
		public Type workerClass;

		// Token: 0x04003FCB RID: 16331
		public SimpleCurve selectionWeightPerPointsCurve;

		// Token: 0x04003FCC RID: 16332
		public float minPawns = 1f;

		// Token: 0x04003FCD RID: 16333
		public List<FactionCurve> selectionWeightCurvesPerFaction;

		// Token: 0x04003FCE RID: 16334
		public List<PlanetLayerDef> layerWhitelist;

		// Token: 0x04003FCF RID: 16335
		public List<PlanetLayerDef> layerBlacklist;

		// Token: 0x04003FD0 RID: 16336
		[MustTranslate]
		public string arrivalTextFriendly;

		// Token: 0x04003FD1 RID: 16337
		[MustTranslate]
		public string arrivalTextEnemy;

		// Token: 0x04003FD2 RID: 16338
		[MustTranslate]
		public string letterLabelEnemy;

		// Token: 0x04003FD3 RID: 16339
		[MustTranslate]
		public string letterLabelFriendly;

		// Token: 0x04003FD4 RID: 16340
		public SimpleCurve pointsFactorCurve;

		// Token: 0x04003FD5 RID: 16341
		public bool pawnsCanBringFood;

		// Token: 0x04003FD6 RID: 16342
		public List<PawnsArrivalModeDef> arriveModes;

		// Token: 0x04003FD7 RID: 16343
		public float raidLootValueFactor = 1f;

		// Token: 0x04003FD8 RID: 16344
		private RaidStrategyWorker workerInt;
	}
}
