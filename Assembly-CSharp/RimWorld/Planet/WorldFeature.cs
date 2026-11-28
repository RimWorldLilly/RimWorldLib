using System;
using System.Collections.Generic;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022EA RID: 8938
	public class WorldFeature : IExposable, ILoadReferenceable
	{
		// Token: 0x0600DC84 RID: 56452 RVA: 0x0052261D File Offset: 0x0052081D
		protected static void FeatureSizePoint10_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x0600DC85 RID: 56453 RVA: 0x00522624 File Offset: 0x00520824
		protected static void FeatureSizePoint25_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x0600DC86 RID: 56454 RVA: 0x0052262B File Offset: 0x0052082B
		protected static void FeatureSizePoint50_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x0600DC87 RID: 56455 RVA: 0x00522632 File Offset: 0x00520832
		protected static void FeatureSizePoint100_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x0600DC88 RID: 56456 RVA: 0x00522639 File Offset: 0x00520839
		protected static void FeatureSizePoint200_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x0600DC89 RID: 56457 RVA: 0x00522640 File Offset: 0x00520840
		private static void TweakChanged()
		{
			Find.WorldFeatures.textsCreated = false;
			WorldFeature.EffectiveDrawSizeCurve[0] = new CurvePoint(WorldFeature.EffectiveDrawSizeCurve[0].x, WorldFeature.FeatureSizePoint10);
			WorldFeature.EffectiveDrawSizeCurve[1] = new CurvePoint(WorldFeature.EffectiveDrawSizeCurve[1].x, WorldFeature.FeatureSizePoint25);
			WorldFeature.EffectiveDrawSizeCurve[2] = new CurvePoint(WorldFeature.EffectiveDrawSizeCurve[2].x, WorldFeature.FeatureSizePoint50);
			WorldFeature.EffectiveDrawSizeCurve[3] = new CurvePoint(WorldFeature.EffectiveDrawSizeCurve[3].x, WorldFeature.FeatureSizePoint100);
			WorldFeature.EffectiveDrawSizeCurve[4] = new CurvePoint(WorldFeature.EffectiveDrawSizeCurve[4].x, WorldFeature.FeatureSizePoint200);
		}

		// Token: 0x1700265B RID: 9819
		// (get) Token: 0x0600DC8A RID: 56458 RVA: 0x00522720 File Offset: 0x00520920
		public float EffectiveDrawSize
		{
			get
			{
				return WorldFeature.EffectiveDrawSizeCurve.Evaluate(this.maxDrawSizeInTiles);
			}
		}

		// Token: 0x0600DC8B RID: 56459 RVA: 0x00522732 File Offset: 0x00520932
		public WorldFeature()
		{
		}

		// Token: 0x0600DC8C RID: 56460 RVA: 0x0052273A File Offset: 0x0052093A
		public WorldFeature(FeatureDef def, PlanetLayer layer)
		{
			this.def = def;
			this.layer = layer;
			this.uniqueID = Find.UniqueIDsManager.GetNextWorldFeatureID();
		}

		// Token: 0x0600DC8D RID: 56461 RVA: 0x00522760 File Offset: 0x00520960
		public void ExposeData()
		{
			Scribe_Defs.Look<FeatureDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.uniqueID, "uniqueID", 0, false);
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<Vector3>(ref this.drawCenter, "drawCenter", default(Vector3), false);
			Scribe_Values.Look<float>(ref this.drawAngle, "drawAngle", 0f, false);
			Scribe_Values.Look<float>(ref this.maxDrawSizeInTiles, "maxDrawSizeInTiles", 0f, false);
			Scribe_References.Look<PlanetLayer>(ref this.layer, "layer", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.layer == null)
			{
				this.layer = Find.WorldGrid.Surface;
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600DC8E RID: 56462 RVA: 0x0052281E File Offset: 0x00520A1E
		public string GetUniqueLoadID()
		{
			return "WorldFeature_" + this.uniqueID.ToString();
		}

		// Token: 0x1700265C RID: 9820
		// (get) Token: 0x0600DC8F RID: 56463 RVA: 0x00522835 File Offset: 0x00520A35
		public IEnumerable<int> Tiles
		{
			get
			{
				WorldFeature.<get_Tiles>d__27 <get_Tiles>d__ = new WorldFeature.<get_Tiles>d__27(-2);
				<get_Tiles>d__.<>4__this = this;
				return <get_Tiles>d__;
			}
		}

		// Token: 0x04008D5C RID: 36188
		public int uniqueID;

		// Token: 0x04008D5D RID: 36189
		public FeatureDef def;

		// Token: 0x04008D5E RID: 36190
		public PlanetLayer layer;

		// Token: 0x04008D5F RID: 36191
		public string name;

		// Token: 0x04008D60 RID: 36192
		public Vector3 drawCenter;

		// Token: 0x04008D61 RID: 36193
		public float drawAngle;

		// Token: 0x04008D62 RID: 36194
		public float maxDrawSizeInTiles;

		// Token: 0x04008D63 RID: 36195
		public float alpha;

		// Token: 0x04008D64 RID: 36196
		protected static SimpleCurve EffectiveDrawSizeCurve = new SimpleCurve
		{
			{
				new CurvePoint(10f, 15f),
				true
			},
			{
				new CurvePoint(25f, 40f),
				true
			},
			{
				new CurvePoint(50f, 90f),
				true
			},
			{
				new CurvePoint(100f, 150f),
				true
			},
			{
				new CurvePoint(200f, 200f),
				true
			}
		};

		// Token: 0x04008D65 RID: 36197
		[TweakValue("Interface.World", 0f, 40f)]
		protected static float FeatureSizePoint10 = 15f;

		// Token: 0x04008D66 RID: 36198
		[TweakValue("Interface.World", 0f, 100f)]
		protected static float FeatureSizePoint25 = 40f;

		// Token: 0x04008D67 RID: 36199
		[TweakValue("Interface.World", 0f, 200f)]
		protected static float FeatureSizePoint50 = 90f;

		// Token: 0x04008D68 RID: 36200
		[TweakValue("Interface.World", 0f, 400f)]
		protected static float FeatureSizePoint100 = 150f;

		// Token: 0x04008D69 RID: 36201
		[TweakValue("Interface.World", 0f, 800f)]
		protected static float FeatureSizePoint200 = 200f;
	}
}
