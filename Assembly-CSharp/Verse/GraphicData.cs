using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using UnityStubs;

namespace Verse
{
	// Token: 0x02000141 RID: 321
	public class GraphicData
	{
		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000A07 RID: 2567 RVA: 0x0003807D File Offset: 0x0003627D
		public bool Linked
		{
			get
			{
				return this.linkType > LinkDrawerType.None;
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000A08 RID: 2568 RVA: 0x00038088 File Offset: 0x00036288
		public Graphic Graphic
		{
			get
			{
				if (this.cachedGraphic == null)
				{
					this.Init();
				}
				return this.cachedGraphic;
			}
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x0003809E File Offset: 0x0003629E
		public void ExplicitlyInitCachedGraphic()
		{
			this.cachedGraphic = this.Graphic;
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x000380AC File Offset: 0x000362AC
		public void CopyFrom(GraphicData other)
		{
			this.texPath = other.texPath;
			this.maskPath = other.maskPath;
			this.graphicClass = other.graphicClass;
			this.shaderType = other.shaderType;
			this.textureRepeats = other.textureRepeats;
			this.borderMipMaps = other.borderMipMaps;
			this.uncompressed = other.uncompressed;
			this.color = other.color;
			this.colorTwo = other.colorTwo;
			this.drawSize = other.drawSize;
			this.drawOffset = other.drawOffset;
			this.drawOffsetNorth = other.drawOffsetNorth;
			this.drawOffsetEast = other.drawOffsetEast;
			this.drawOffsetSouth = other.drawOffsetSouth;
			this.drawOffsetWest = other.drawOffsetWest;
			this.attachments = other.attachments;
			this.onGroundRandomRotateAngle = other.onGroundRandomRotateAngle;
			this.drawRotated = other.drawRotated;
			this.allowFlip = other.allowFlip;
			this.flipExtraRotation = other.flipExtraRotation;
			this.shadowData = other.shadowData;
			this.damageData = other.damageData;
			this.linkType = other.linkType;
			this.linkFlags = other.linkFlags;
			this.asymmetricLink = other.asymmetricLink;
			this.allowAtlasing = other.allowAtlasing;
			this.renderInstanced = other.renderInstanced;
			this.renderQueue = other.renderQueue;
			this.ignoreThingDrawColor = other.ignoreThingDrawColor;
			this.maxSnS = other.maxSnS;
			this.offsetSnS = other.offsetSnS;
			this.cornerOverlayPath = other.cornerOverlayPath;
			this.cachedGraphic = null;
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x00038240 File Offset: 0x00036440
		private void Init()
		{
			if (this.graphicClass == null)
			{
				this.cachedGraphic = null;
				return;
			}
			ShaderTypeDef cutout = this.shaderType;
			if (cutout == null)
			{
				cutout = ShaderTypeDefOf.Cutout;
			}
			Shader shader = cutout.Shader;
			if (this.borderMipMaps)
			{
				ImportDefaults.BorderMipMaps(this.texPath);
			}
			if (this.uncompressed)
			{
				ImportDefaults.Uncompressed(this.texPath);
			}
			this.cachedGraphic = GraphicDatabase.Get(this.graphicClass, this.texPath, shader, this.drawSize, this.color, this.colorTwo, this, this.shaderParameters, this.maskPath);
			if (this.textureRepeats)
			{
				Material matSingle = this.cachedGraphic.MatSingle;
				if (((matSingle != null) ? matSingle.mainTexture : null) != null)
				{
					this.cachedGraphic.MatSingle.mainTexture.wrapMode = TextureWrapMode.Repeat;
				}
			}
			if (this.onGroundRandomRotateAngle > 0.01f)
			{
				this.cachedGraphic = new Graphic_RandomRotated(this.cachedGraphic, this.onGroundRandomRotateAngle);
			}
			if (this.Linked)
			{
				this.cachedGraphic = GraphicUtility.WrapLinked(this.cachedGraphic, this.linkType);
			}
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x00038357 File Offset: 0x00036557
		public void ResolveReferencesSpecial()
		{
			if (this.damageData != null)
			{
				this.damageData.ResolveReferencesSpecial();
			}
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x0003836C File Offset: 0x0003656C
		public Vector3 DrawOffsetForRot(Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
			{
				Vector3? vector = this.drawOffsetNorth;
				if (vector == null)
				{
					return this.drawOffset;
				}
				return vector.GetValueOrDefault();
			}
			case 1:
			{
				Vector3? vector = this.drawOffsetEast;
				if (vector == null)
				{
					return this.drawOffset;
				}
				return vector.GetValueOrDefault();
			}
			case 2:
			{
				Vector3? vector = this.drawOffsetSouth;
				if (vector == null)
				{
					return this.drawOffset;
				}
				return vector.GetValueOrDefault();
			}
			case 3:
			{
				Vector3? vector = this.drawOffsetWest;
				if (vector == null)
				{
					return this.drawOffset;
				}
				return vector.GetValueOrDefault();
			}
			default:
				return this.drawOffset;
			}
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x0003841C File Offset: 0x0003661C
		public Graphic GraphicColoredFor(Thing t)
		{
			if (this.ignoreThingDrawColor || (t.DrawColor.IndistinguishableFrom(this.Graphic.Color) && t.DrawColorTwo.IndistinguishableFrom(this.Graphic.ColorTwo)))
			{
				return this.Graphic;
			}
			return this.Graphic.GetColoredVersion(this.Graphic.Shader, t.DrawColor, t.DrawColorTwo);
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x0003848A File Offset: 0x0003668A
		internal IEnumerable<string> ConfigErrors(ThingDef thingDef)
		{
			GraphicData.<ConfigErrors>d__48 <ConfigErrors>d__ = new GraphicData.<ConfigErrors>d__48(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__thingDef = thingDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x040005E8 RID: 1512
		[NoTranslate]
		public string name;

		// Token: 0x040005E9 RID: 1513
		[NoTranslate]
		public string texPath;

		// Token: 0x040005EA RID: 1514
		[NoTranslate]
		public string maskPath;

		// Token: 0x040005EB RID: 1515
		public Type graphicClass;

		// Token: 0x040005EC RID: 1516
		public ShaderTypeDef shaderType;

		// Token: 0x040005ED RID: 1517
		public List<ShaderParameter> shaderParameters;

		// Token: 0x040005EE RID: 1518
		public bool textureRepeats;

		// Token: 0x040005EF RID: 1519
		public bool borderMipMaps;

		// Token: 0x040005F0 RID: 1520
		public bool uncompressed;

		// Token: 0x040005F1 RID: 1521
		public Color color = Color.white;

		// Token: 0x040005F2 RID: 1522
		public Color colorTwo = Color.white;

		// Token: 0x040005F3 RID: 1523
		public Vector2 drawSize = Vector2.one;

		// Token: 0x040005F4 RID: 1524
		public Vector3 drawOffset = Vector3.zero;

		// Token: 0x040005F5 RID: 1525
		public Vector3? drawOffsetNorth;

		// Token: 0x040005F6 RID: 1526
		public Vector3? drawOffsetEast;

		// Token: 0x040005F7 RID: 1527
		public Vector3? drawOffsetSouth;

		// Token: 0x040005F8 RID: 1528
		public Vector3? drawOffsetWest;

		// Token: 0x040005F9 RID: 1529
		public float onGroundRandomRotateAngle;

		// Token: 0x040005FA RID: 1530
		public bool drawRotated = true;

		// Token: 0x040005FB RID: 1531
		public bool allowFlip = true;

		// Token: 0x040005FC RID: 1532
		public float flipExtraRotation;

		// Token: 0x040005FD RID: 1533
		public bool renderInstanced;

		// Token: 0x040005FE RID: 1534
		public bool allowAtlasing = true;

		// Token: 0x040005FF RID: 1535
		public int renderQueue;

		// Token: 0x04000600 RID: 1536
		public float overlayOpacity;

		// Token: 0x04000601 RID: 1537
		public List<GraphicData> attachments;

		// Token: 0x04000602 RID: 1538
		public List<AttachPoint> attachPoints;

		// Token: 0x04000603 RID: 1539
		public bool addTopAltitudeBias;

		// Token: 0x04000604 RID: 1540
		public bool ignoreThingDrawColor;

		// Token: 0x04000605 RID: 1541
		public Vector2 maxSnS;

		// Token: 0x04000606 RID: 1542
		public Vector2 offsetSnS;

		// Token: 0x04000607 RID: 1543
		public ShadowData shadowData;

		// Token: 0x04000608 RID: 1544
		public DamageGraphicData damageData;

		// Token: 0x04000609 RID: 1545
		public LinkDrawerType linkType;

		// Token: 0x0400060A RID: 1546
		public LinkFlags linkFlags;

		// Token: 0x0400060B RID: 1547
		public AsymmetricLinkData asymmetricLink;

		// Token: 0x0400060C RID: 1548
		[NoTranslate]
		public string cornerOverlayPath;

		// Token: 0x0400060D RID: 1549
		[Unsaved(false)]
		private Graphic cachedGraphic;
	}
}
