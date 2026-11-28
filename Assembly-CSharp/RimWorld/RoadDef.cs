using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FD9 RID: 4057
	public class RoadDef : Def
	{
		// Token: 0x060066C9 RID: 26313 RVA: 0x002477AC File Offset: 0x002459AC
		public float GetLayerWidth(RoadWorldLayerDef def)
		{
			if (this.cachedLayerWidth == null)
			{
				this.cachedLayerWidth = new float[DefDatabase<RoadWorldLayerDef>.DefCount];
				for (int i = 0; i < DefDatabase<RoadWorldLayerDef>.DefCount; i++)
				{
					RoadWorldLayerDef roadWorldLayerDef = DefDatabase<RoadWorldLayerDef>.AllDefsListForReading[i];
					if (this.worldRenderSteps != null)
					{
						foreach (RoadDef.WorldRenderStep worldRenderStep in this.worldRenderSteps)
						{
							if (worldRenderStep.layer == roadWorldLayerDef)
							{
								this.cachedLayerWidth[(int)roadWorldLayerDef.index] = worldRenderStep.width;
							}
						}
					}
				}
			}
			return this.cachedLayerWidth[(int)def.index];
		}

		// Token: 0x060066CA RID: 26314 RVA: 0x00247860 File Offset: 0x00245A60
		public override void ClearCachedData()
		{
			base.ClearCachedData();
			this.cachedLayerWidth = null;
		}

		// Token: 0x04003FF3 RID: 16371
		public int priority;

		// Token: 0x04003FF4 RID: 16372
		public bool ancientOnly;

		// Token: 0x04003FF5 RID: 16373
		public float movementCostMultiplier = 1f;

		// Token: 0x04003FF6 RID: 16374
		public int tilesPerSegment = 15;

		// Token: 0x04003FF7 RID: 16375
		public RoadPathingDef pathingMode;

		// Token: 0x04003FF8 RID: 16376
		public List<RoadDefGenStep> roadGenSteps;

		// Token: 0x04003FF9 RID: 16377
		public List<RoadDef.WorldRenderStep> worldRenderSteps;

		// Token: 0x04003FFA RID: 16378
		[NoTranslate]
		public string worldTransitionGroup = "";

		// Token: 0x04003FFB RID: 16379
		public float distortionFrequency = 1f;

		// Token: 0x04003FFC RID: 16380
		public float distortionIntensity;

		// Token: 0x04003FFD RID: 16381
		[Unsaved(false)]
		private float[] cachedLayerWidth;

		// Token: 0x02002F86 RID: 12166
		public class WorldRenderStep
		{
			// Token: 0x0400C138 RID: 49464
			public RoadWorldLayerDef layer;

			// Token: 0x0400C139 RID: 49465
			public float width;
		}
	}
}
