using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F5F RID: 3935
	public class FleshTypeDef : Def
	{
		// Token: 0x06006533 RID: 25907 RVA: 0x002413FC File Offset: 0x0023F5FC
		public FleshTypeDef.ResolvedWound ChooseBandagedOverlay()
		{
			if (this.bandagedWounds == null)
			{
				return null;
			}
			if (this.bandagedWoundsResolved == null)
			{
				this.bandagedWoundsResolved = (from wound in this.bandagedWounds
				select wound.Resolve()).ToList<FleshTypeDef.ResolvedWound>();
			}
			return this.bandagedWoundsResolved.RandomElement<FleshTypeDef.ResolvedWound>();
		}

		// Token: 0x06006534 RID: 25908 RVA: 0x0024145C File Offset: 0x0023F65C
		public FleshTypeDef.ResolvedWound ChooseWoundOverlay(Hediff hediff)
		{
			if (this.genericWounds == null)
			{
				return null;
			}
			if (this.hediffWounds != null)
			{
				foreach (FleshTypeDef.HediffWound hediffWound in this.hediffWounds)
				{
					if (hediffWound.hediff == hediff.def)
					{
						FleshTypeDef.ResolvedWound resolvedWound = hediffWound.ChooseWoundOverlay(hediff);
						if (resolvedWound != null)
						{
							if (resolvedWound.wound.missingBodyPartFresh != null && !resolvedWound.wound.missingBodyPartFresh.Value)
							{
								return resolvedWound;
							}
							if (hediff.IsTended())
							{
								return this.ChooseBandagedOverlay();
							}
							return resolvedWound;
						}
					}
				}
			}
			if (!(hediff is Hediff_Injury))
			{
				Hediff_MissingPart hediff_MissingPart = hediff as Hediff_MissingPart;
				if (hediff_MissingPart == null || !hediff_MissingPart.IsFresh)
				{
					return null;
				}
			}
			if (hediff.IsTended())
			{
				return this.ChooseBandagedOverlay();
			}
			if (this.woundsResolved == null)
			{
				this.woundsResolved = (from wound in this.genericWounds
				select wound.Resolve()).ToList<FleshTypeDef.ResolvedWound>();
			}
			return this.woundsResolved.RandomElement<FleshTypeDef.ResolvedWound>();
		}

		// Token: 0x04003B86 RID: 15238
		public ThoughtDef ateDirect;

		// Token: 0x04003B87 RID: 15239
		public ThoughtDef ateAsIngredient;

		// Token: 0x04003B88 RID: 15240
		public ThingCategoryDef corpseCategory;

		// Token: 0x04003B89 RID: 15241
		public EffecterDef damageEffecter;

		// Token: 0x04003B8A RID: 15242
		public bool isOrganic = true;

		// Token: 0x04003B8B RID: 15243
		public List<FleshTypeDef.Wound> genericWounds;

		// Token: 0x04003B8C RID: 15244
		public List<FleshTypeDef.Wound> bandagedWounds;

		// Token: 0x04003B8D RID: 15245
		public List<FleshTypeDef.HediffWound> hediffWounds;

		// Token: 0x04003B8E RID: 15246
		private List<FleshTypeDef.ResolvedWound> bandagedWoundsResolved;

		// Token: 0x04003B8F RID: 15247
		private List<FleshTypeDef.ResolvedWound> woundsResolved;

		// Token: 0x02002F60 RID: 12128
		public class HediffWound
		{
			// Token: 0x06011E81 RID: 73345 RVA: 0x0061CBE4 File Offset: 0x0061ADE4
			public FleshTypeDef.ResolvedWound ChooseWoundOverlay(Hediff hediff)
			{
				if (this.wounds == null || (this.onlyHumanlikes && !hediff.pawn.RaceProps.Humanlike))
				{
					return null;
				}
				if (this.woundsResolved == null)
				{
					this.woundsResolved = (from wound in this.wounds
					select wound.Resolve()).ToList<FleshTypeDef.ResolvedWound>();
				}
				return (from w in this.woundsResolved
				where w.wound.Fits(hediff)
				select w).RandomElementWithFallback(null);
			}

			// Token: 0x0400C080 RID: 49280
			public bool onlyHumanlikes = true;

			// Token: 0x0400C081 RID: 49281
			public HediffDef hediff;

			// Token: 0x0400C082 RID: 49282
			public List<FleshTypeDef.Wound> wounds;

			// Token: 0x0400C083 RID: 49283
			private List<FleshTypeDef.ResolvedWound> woundsResolved;
		}

		// Token: 0x02002F61 RID: 12129
		public class Wound
		{
			// Token: 0x06011E83 RID: 73347 RVA: 0x0061CC90 File Offset: 0x0061AE90
			public bool Fits(Hediff hediff)
			{
				if (this.onlyOnPart != null && (hediff.Part == null || hediff.Part.def != this.onlyOnPart))
				{
					return false;
				}
				if (this.missingBodyPartFresh != null)
				{
					Hediff_MissingPart hediff_MissingPart = hediff as Hediff_MissingPart;
					if (hediff_MissingPart != null && this.missingBodyPartFresh.Value != (hediff_MissingPart.IsFresh || hediff_MissingPart.Bleeding))
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x06011E84 RID: 73348 RVA: 0x0061CCFC File Offset: 0x0061AEFC
			public FleshTypeDef.ResolvedWound Resolve()
			{
				Shader shader = this.tintWithSkinColor ? ShaderDatabase.WoundSkin : ShaderDatabase.Wound;
				if (this.texture != null)
				{
					return new FleshTypeDef.ResolvedWound(this, MaterialPool.MatFrom(this.texture, shader, this.color));
				}
				return new FleshTypeDef.ResolvedWound(this, MaterialPool.MatFrom(this.textureSouth, shader, this.color), MaterialPool.MatFrom(this.textureEast, shader, this.color), MaterialPool.MatFrom(this.textureNorth, shader, this.color), MaterialPool.MatFrom(this.textureWest, shader, this.color), this.flipSouth, this.flipEast, this.flipNorth, this.flipWest);
			}

			// Token: 0x0400C084 RID: 49284
			[NoTranslate]
			public string texture;

			// Token: 0x0400C085 RID: 49285
			[NoTranslate]
			public string textureSouth;

			// Token: 0x0400C086 RID: 49286
			[NoTranslate]
			public string textureEast;

			// Token: 0x0400C087 RID: 49287
			[NoTranslate]
			public string textureNorth;

			// Token: 0x0400C088 RID: 49288
			[NoTranslate]
			public string textureWest;

			// Token: 0x0400C089 RID: 49289
			public bool flipSouth;

			// Token: 0x0400C08A RID: 49290
			public bool flipEast;

			// Token: 0x0400C08B RID: 49291
			public bool flipNorth;

			// Token: 0x0400C08C RID: 49292
			public bool flipWest;

			// Token: 0x0400C08D RID: 49293
			public float scale = 1f;

			// Token: 0x0400C08E RID: 49294
			public Vector3 drawOffsetSouth;

			// Token: 0x0400C08F RID: 49295
			public Vector3 drawOffsetEastWest;

			// Token: 0x0400C090 RID: 49296
			public Color color = Color.white;

			// Token: 0x0400C091 RID: 49297
			public bool tintWithSkinColor;

			// Token: 0x0400C092 RID: 49298
			[NoTranslate]
			public string flipOnWoundAnchorTag;

			// Token: 0x0400C093 RID: 49299
			public Rot4 flipOnRotation;

			// Token: 0x0400C094 RID: 49300
			public BodyPartDef onlyOnPart;

			// Token: 0x0400C095 RID: 49301
			public bool displayOverApparel = true;

			// Token: 0x0400C096 RID: 49302
			public bool displayPermanent;

			// Token: 0x0400C097 RID: 49303
			public bool? missingBodyPartFresh;
		}

		// Token: 0x02002F62 RID: 12130
		public class ResolvedWound
		{
			// Token: 0x06011E86 RID: 73350 RVA: 0x0061CDCC File Offset: 0x0061AFCC
			public ResolvedWound(FleshTypeDef.Wound wound, Material material)
			{
				this.wound = wound;
				this.matWest = material;
				this.matNorth = material;
				this.matEast = material;
				this.matSouth = material;
			}

			// Token: 0x06011E87 RID: 73351 RVA: 0x0061CE08 File Offset: 0x0061B008
			public ResolvedWound(FleshTypeDef.Wound wound, Material matSouth, Material matEast, Material matNorth, Material matWest, bool flipSouth, bool flipEast, bool flipNorth, bool flipWest)
			{
				this.wound = wound;
				this.matSouth = matSouth;
				this.matEast = matEast;
				this.matNorth = matNorth;
				this.matWest = matWest;
				this.flipSouth = flipSouth;
				this.flipEast = flipEast;
				this.flipNorth = flipNorth;
				this.flipWest = flipWest;
			}

			// Token: 0x06011E88 RID: 73352 RVA: 0x0061CE60 File Offset: 0x0061B060
			public Material GetMaterial(Rot4 rotation, out bool flip)
			{
				flip = false;
				if (rotation == Rot4.South)
				{
					flip = this.flipSouth;
					return this.matSouth;
				}
				if (rotation == Rot4.East)
				{
					flip = this.flipEast;
					return this.matEast;
				}
				if (rotation == Rot4.West)
				{
					flip = this.flipWest;
					return this.matWest;
				}
				if (rotation == Rot4.North)
				{
					flip = this.flipNorth;
					return this.matNorth;
				}
				return this.matSouth;
			}

			// Token: 0x0400C098 RID: 49304
			public Material material;

			// Token: 0x0400C099 RID: 49305
			public Material matSouth;

			// Token: 0x0400C09A RID: 49306
			public Material matEast;

			// Token: 0x0400C09B RID: 49307
			public Material matNorth;

			// Token: 0x0400C09C RID: 49308
			public Material matWest;

			// Token: 0x0400C09D RID: 49309
			public bool flipSouth;

			// Token: 0x0400C09E RID: 49310
			public bool flipEast;

			// Token: 0x0400C09F RID: 49311
			public bool flipNorth;

			// Token: 0x0400C0A0 RID: 49312
			public bool flipWest;

			// Token: 0x0400C0A1 RID: 49313
			public FleshTypeDef.Wound wound;
		}
	}
}
