using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FBC RID: 4028
	public class PawnsArrivalModeDef : Def
	{
		// Token: 0x17001188 RID: 4488
		// (get) Token: 0x0600665C RID: 26204 RVA: 0x002459B4 File Offset: 0x00243BB4
		public PawnsArrivalModeWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (PawnsArrivalModeWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x04003EB6 RID: 16054
		public Type workerClass = typeof(PawnsArrivalModeWorker);

		// Token: 0x04003EB7 RID: 16055
		public SimpleCurve selectionWeightCurve;

		// Token: 0x04003EB8 RID: 16056
		public SimpleCurve pointsFactorCurve;

		// Token: 0x04003EB9 RID: 16057
		public TechLevel minTechLevel;

		// Token: 0x04003EBA RID: 16058
		public bool forQuickMilitaryAid;

		// Token: 0x04003EBB RID: 16059
		public bool walkIn;

		// Token: 0x04003EBC RID: 16060
		public bool canBeBackup = true;

		// Token: 0x04003EBD RID: 16061
		public float minSpaceSelectionWeight = -1f;

		// Token: 0x04003EBE RID: 16062
		public List<BiomeDef> biomeWhitelist;

		// Token: 0x04003EBF RID: 16063
		public List<BiomeDef> biomeBlacklist;

		// Token: 0x04003EC0 RID: 16064
		public List<PlanetLayerDef> layerWhitelist;

		// Token: 0x04003EC1 RID: 16065
		public List<PlanetLayerDef> layerBlacklist;

		// Token: 0x04003EC2 RID: 16066
		[MustTranslate]
		public string textEnemy;

		// Token: 0x04003EC3 RID: 16067
		[MustTranslate]
		public string textFriendly;

		// Token: 0x04003EC4 RID: 16068
		[MustTranslate]
		public string textWillArrive;

		// Token: 0x04003EC5 RID: 16069
		public List<FactionCurve> selectionWeightCurvesPerFaction;

		// Token: 0x04003EC6 RID: 16070
		[Unsaved(false)]
		private PawnsArrivalModeWorker workerInt;
	}
}
