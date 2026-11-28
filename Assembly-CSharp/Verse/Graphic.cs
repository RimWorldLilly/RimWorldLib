using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000555 RID: 1365
	public class Graphic
	{
		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x0600288A RID: 10378 RVA: 0x000FA3D0 File Offset: 0x000F85D0
		public Shader Shader
		{
			get
			{
				Material matSingle = this.MatSingle;
				if (matSingle != null)
				{
					return matSingle.shader;
				}
				return ShaderDatabase.Cutout;
			}
		}

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x0600288B RID: 10379 RVA: 0x000FA3F9 File Offset: 0x000F85F9
		// (set) Token: 0x0600288C RID: 10380 RVA: 0x000FA434 File Offset: 0x000F8634
		public Graphic_Shadow ShadowGraphic
		{
			get
			{
				if (this.cachedShadowGraphicInt == null && this.data != null && this.data.shadowData != null)
				{
					this.cachedShadowGraphicInt = new Graphic_Shadow(this.data.shadowData);
				}
				return this.cachedShadowGraphicInt;
			}
			set
			{
				this.cachedShadowGraphicInt = value;
			}
		}

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x0600288D RID: 10381 RVA: 0x000FA43D File Offset: 0x000F863D
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x0600288E RID: 10382 RVA: 0x000FA445 File Offset: 0x000F8645
		public Color ColorTwo
		{
			get
			{
				return this.colorTwo;
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x0600288F RID: 10383 RVA: 0x000FA44D File Offset: 0x000F864D
		public virtual Material MatSingle
		{
			get
			{
				return BaseContent.BadMat;
			}
		}

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x06002890 RID: 10384 RVA: 0x000FA454 File Offset: 0x000F8654
		public virtual Material MatWest
		{
			get
			{
				return this.MatSingle;
			}
		}

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x06002891 RID: 10385 RVA: 0x000FA45C File Offset: 0x000F865C
		public virtual Material MatSouth
		{
			get
			{
				return this.MatSingle;
			}
		}

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x06002892 RID: 10386 RVA: 0x000FA464 File Offset: 0x000F8664
		public virtual Material MatEast
		{
			get
			{
				return this.MatSingle;
			}
		}

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x06002893 RID: 10387 RVA: 0x000FA46C File Offset: 0x000F866C
		public virtual Material MatNorth
		{
			get
			{
				return this.MatSingle;
			}
		}

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x06002894 RID: 10388 RVA: 0x000FA474 File Offset: 0x000F8674
		public virtual bool WestFlipped
		{
			get
			{
				return this.DataAllowsFlip && !this.ShouldDrawRotated;
			}
		}

		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x06002895 RID: 10389 RVA: 0x000FA489 File Offset: 0x000F8689
		public virtual bool EastFlipped
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x06002896 RID: 10390 RVA: 0x000FA48C File Offset: 0x000F868C
		public virtual bool ShouldDrawRotated
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x06002897 RID: 10391 RVA: 0x000FA48F File Offset: 0x000F868F
		public virtual float DrawRotatedExtraAngleOffset
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x06002898 RID: 10392 RVA: 0x000FA496 File Offset: 0x000F8696
		public virtual bool UseSameGraphicForGhost
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x06002899 RID: 10393 RVA: 0x000FA499 File Offset: 0x000F8699
		protected bool DataAllowsFlip
		{
			get
			{
				return this.data == null || this.data.allowFlip;
			}
		}

		// Token: 0x0600289A RID: 10394 RVA: 0x000FA4B0 File Offset: 0x000F86B0
		public static bool TryGetTextureAtlasReplacementInfo(Material mat, TextureAtlasGroup group, bool flipUv, bool vertexColors, out Material material, out Vector2[] uvs, out Color32 vertexColor)
		{
			material = mat;
			uvs = null;
			vertexColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			Graphic.AtlasReplacementInfoCacheKey key = new Graphic.AtlasReplacementInfoCacheKey(mat, group, flipUv, vertexColors);
			Graphic.CachedAtlasReplacementInfo cachedAtlasReplacementInfo;
			if (Graphic.replacementInfoCache.TryGetValue(key, out cachedAtlasReplacementInfo))
			{
				material = cachedAtlasReplacementInfo.material;
				uvs = cachedAtlasReplacementInfo.uvs;
				if (vertexColors)
				{
					vertexColor = cachedAtlasReplacementInfo.vertexColor;
				}
				return true;
			}
			StaticTextureAtlasTile staticTextureAtlasTile;
			if (!GlobalTextureAtlasManager.TryGetStaticTile(group, (Texture2D)mat.mainTexture, out staticTextureAtlasTile, false))
			{
				return false;
			}
			MaterialRequest materialRequest;
			if (!MaterialPool.TryGetRequestForMat(mat, out materialRequest))
			{
				Log.Error("Tried getting texture atlas replacement info for a material that was not created by MaterialPool!");
				return false;
			}
			uvs = new Vector2[4];
			Printer_Plane.GetUVs(staticTextureAtlasTile.uvRect, out uvs[0], out uvs[1], out uvs[2], out uvs[3], flipUv);
			materialRequest.mainTex = staticTextureAtlasTile.atlas.ColorTexture;
			if (vertexColors)
			{
				vertexColor = materialRequest.color;
				materialRequest.color = Color.white;
			}
			if (materialRequest.maskTex != null)
			{
				materialRequest.maskTex = staticTextureAtlasTile.atlas.MaskTexture;
			}
			material = MaterialPool.MatFrom(materialRequest);
			Graphic.replacementInfoCache.Add(key, new Graphic.CachedAtlasReplacementInfo
			{
				material = material,
				uvs = uvs,
				vertexColor = vertexColor
			});
			return true;
		}

		// Token: 0x0600289B RID: 10395 RVA: 0x000FA621 File Offset: 0x000F8821
		public virtual void TryInsertIntoAtlas(TextureAtlasGroup groupKey)
		{
		}

		// Token: 0x0600289C RID: 10396 RVA: 0x000FA624 File Offset: 0x000F8824
		public virtual void Init(GraphicRequest req)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Cannot init Graphic of class ");
			defaultInterpolatedStringHandler.AppendFormatted<Type>(base.GetType());
			Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), 658928);
		}

		// Token: 0x0600289D RID: 10397 RVA: 0x000FA665 File Offset: 0x000F8865
		public virtual Material NodeGetMat(PawnDrawParms parms)
		{
			return this.MatAt(parms.facing, parms.pawn);
		}

		// Token: 0x0600289E RID: 10398 RVA: 0x000FA67C File Offset: 0x000F887C
		public virtual Material MatAt(Rot4 rot, Thing thing = null)
		{
			switch (rot.AsInt)
			{
			case 0:
				return this.MatNorth;
			case 1:
				return this.MatEast;
			case 2:
				return this.MatSouth;
			case 3:
				return this.MatWest;
			default:
				return BaseContent.BadMat;
			}
		}

		// Token: 0x0600289F RID: 10399 RVA: 0x000FA6CC File Offset: 0x000F88CC
		public virtual Mesh MeshAt(Rot4 rot)
		{
			Vector2 vector = this.drawSize;
			if (rot.IsHorizontal && !this.ShouldDrawRotated)
			{
				vector = vector.Rotated();
			}
			if ((rot == Rot4.West && this.WestFlipped) || (rot == Rot4.East && this.EastFlipped))
			{
				return MeshPool.GridPlaneFlip(vector);
			}
			return MeshPool.GridPlane(vector);
		}

		// Token: 0x060028A0 RID: 10400 RVA: 0x000FA72F File Offset: 0x000F892F
		public virtual Material MatSingleFor(Thing thing)
		{
			return this.MatSingle;
		}

		// Token: 0x060028A1 RID: 10401 RVA: 0x000FA737 File Offset: 0x000F8937
		public Vector3 DrawOffset(Rot4 rot)
		{
			if (this.data == null)
			{
				return Vector3.zero;
			}
			return this.data.DrawOffsetForRot(rot);
		}

		// Token: 0x060028A2 RID: 10402 RVA: 0x000FA753 File Offset: 0x000F8953
		public void Draw(Vector3 loc, Rot4 rot, Thing thing, float extraRotation = 0f)
		{
			this.DrawWorker(loc, rot, thing.def, thing, extraRotation);
		}

		// Token: 0x060028A3 RID: 10403 RVA: 0x000FA766 File Offset: 0x000F8966
		public void DrawFromDef(Vector3 loc, Rot4 rot, ThingDef thingDef, float extraRotation = 0f)
		{
			this.DrawWorker(loc, rot, thingDef, null, extraRotation);
		}

		// Token: 0x060028A4 RID: 10404 RVA: 0x000FA774 File Offset: 0x000F8974
		public virtual void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Mesh mesh = this.MeshAt(rot);
			Quaternion quaternion = this.QuatFromRot(rot);
			if (extraRotation != 0f)
			{
				quaternion *= Quaternion.Euler(Vector3.up * extraRotation);
			}
			if (this.data != null && this.data.addTopAltitudeBias)
			{
				quaternion *= Quaternion.Euler(Vector3.left * 2f);
			}
			loc += this.DrawOffset(rot);
			Material mat = this.MatAt(rot, thing);
			this.DrawMeshInt(mesh, loc, quaternion, mat);
			if (this.ShadowGraphic != null)
			{
				this.ShadowGraphic.DrawWorker(loc, rot, thingDef, thing, extraRotation);
			}
		}

		// Token: 0x060028A5 RID: 10405 RVA: 0x000FA81F File Offset: 0x000F8A1F
		protected virtual void DrawMeshInt(Mesh mesh, Vector3 loc, Quaternion quat, Material mat)
		{
			Graphics.DrawMesh(mesh, loc, quat, mat, 0);
		}

		// Token: 0x060028A6 RID: 10406 RVA: 0x000FA82C File Offset: 0x000F8A2C
		public virtual void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			Vector2 vector;
			bool flag;
			if (this.ShouldDrawRotated)
			{
				vector = this.drawSize;
				flag = false;
			}
			else
			{
				if (!thing.Rotation.IsHorizontal)
				{
					vector = this.drawSize;
				}
				else
				{
					vector = this.drawSize.Rotated();
				}
				flag = ((thing.Rotation == Rot4.West && this.WestFlipped) || (thing.Rotation == Rot4.East && this.EastFlipped));
			}
			if (thing.MultipleItemsPerCellDrawn())
			{
				vector *= 0.8f;
			}
			float num = this.AngleFromRot(thing.Rotation) + extraRotation;
			if (flag && this.data != null)
			{
				num += this.data.flipExtraRotation;
			}
			Vector3 center = thing.TrueCenter() + this.DrawOffset(thing.Rotation);
			Material mat = this.MatAt(thing.Rotation, thing);
			Vector2[] uvs;
			Color32 color;
			Graphic.TryGetTextureAtlasReplacementInfo(mat, thing.def.category.ToAtlasGroup(), flag, true, out mat, out uvs, out color);
			Printer_Plane.PrintPlane(layer, center, vector, mat, num, flag, uvs, new Color32[]
			{
				color,
				color,
				color,
				color
			}, 0.01f, 0f);
			Graphic_Shadow shadowGraphic = this.ShadowGraphic;
			if (shadowGraphic == null)
			{
				return;
			}
			shadowGraphic.Print(layer, thing, 0f);
		}

		// Token: 0x060028A7 RID: 10407 RVA: 0x000FA987 File Offset: 0x000F8B87
		public virtual Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			Log.ErrorOnce("CloneColored not implemented on this subclass of Graphic: " + base.GetType().ToString(), 66300);
			return BaseContent.BadGraphic;
		}

		// Token: 0x060028A8 RID: 10408 RVA: 0x000FA9AD File Offset: 0x000F8BAD
		[Obsolete("Will be removed in a future release")]
		public virtual Graphic GetCopy(Vector2 newDrawSize)
		{
			return this.GetCopy(newDrawSize, null);
		}

		// Token: 0x060028A9 RID: 10409 RVA: 0x000FA9B7 File Offset: 0x000F8BB7
		public virtual Graphic GetCopy(Vector2 newDrawSize, Shader overrideShader)
		{
			return GraphicDatabase.Get(base.GetType(), this.path, overrideShader ?? this.Shader, newDrawSize, this.color, this.colorTwo, null);
		}

		// Token: 0x060028AA RID: 10410 RVA: 0x000FA9E4 File Offset: 0x000F8BE4
		public virtual Graphic GetShadowlessGraphic()
		{
			if (this.data == null || this.data.shadowData == null)
			{
				return this;
			}
			if (this.cachedShadowlessGraphicInt == null)
			{
				GraphicData graphicData = new GraphicData();
				graphicData.CopyFrom(this.data);
				graphicData.shadowData = null;
				this.cachedShadowlessGraphicInt = graphicData.Graphic;
			}
			return this.cachedShadowlessGraphicInt;
		}

		// Token: 0x060028AB RID: 10411 RVA: 0x000FAA3C File Offset: 0x000F8C3C
		protected float AngleFromRot(Rot4 rot)
		{
			if (this.ShouldDrawRotated)
			{
				float num = rot.AsAngle;
				num += this.DrawRotatedExtraAngleOffset;
				if ((rot == Rot4.West && this.WestFlipped) || (rot == Rot4.East && this.EastFlipped))
				{
					num += 180f;
				}
				return num;
			}
			return 0f;
		}

		// Token: 0x060028AC RID: 10412 RVA: 0x000FAA9C File Offset: 0x000F8C9C
		public Quaternion QuatFromRot(Rot4 rot)
		{
			float num = this.AngleFromRot(rot);
			if (num == 0f)
			{
				return Quaternion.identity;
			}
			return Quaternion.AngleAxis(num, Vector3.up);
		}

		// Token: 0x04001C9E RID: 7326
		public GraphicData data;

		// Token: 0x04001C9F RID: 7327
		public string path;

		// Token: 0x04001CA0 RID: 7328
		public string maskPath;

		// Token: 0x04001CA1 RID: 7329
		public Color color = Color.white;

		// Token: 0x04001CA2 RID: 7330
		public Color colorTwo = Color.white;

		// Token: 0x04001CA3 RID: 7331
		public Vector2 drawSize = Vector2.one;

		// Token: 0x04001CA4 RID: 7332
		private Graphic_Shadow cachedShadowGraphicInt;

		// Token: 0x04001CA5 RID: 7333
		private Graphic cachedShadowlessGraphicInt;

		// Token: 0x04001CA6 RID: 7334
		private static Dictionary<Graphic.AtlasReplacementInfoCacheKey, Graphic.CachedAtlasReplacementInfo> replacementInfoCache = new Dictionary<Graphic.AtlasReplacementInfoCacheKey, Graphic.CachedAtlasReplacementInfo>();

		// Token: 0x02002781 RID: 10113
		private struct AtlasReplacementInfoCacheKey : IEquatable<Graphic.AtlasReplacementInfoCacheKey>
		{
			// Token: 0x0600F882 RID: 63618 RVA: 0x005A14A0 File Offset: 0x0059F6A0
			public AtlasReplacementInfoCacheKey(Material mat, TextureAtlasGroup group, bool flipUv, bool vertexColors)
			{
				this.mat = mat;
				this.group = group;
				this.flipUv = flipUv;
				this.vertexColors = vertexColors;
				this.hash = Gen.HashCombine<int>(mat.GetHashCode(), group.GetHashCode());
				if (flipUv)
				{
					this.hash = ~this.hash;
				}
				if (vertexColors)
				{
					this.hash ^= 123893723;
				}
			}

			// Token: 0x0600F883 RID: 63619 RVA: 0x005A150E File Offset: 0x0059F70E
			public bool Equals(Graphic.AtlasReplacementInfoCacheKey other)
			{
				return this.mat == other.mat && this.group == other.group && this.flipUv == other.flipUv && this.vertexColors == other.vertexColors;
			}

			// Token: 0x0600F884 RID: 63620 RVA: 0x005A154A File Offset: 0x0059F74A
			public override int GetHashCode()
			{
				return this.hash;
			}

			// Token: 0x0400A183 RID: 41347
			public readonly Material mat;

			// Token: 0x0400A184 RID: 41348
			public readonly TextureAtlasGroup group;

			// Token: 0x0400A185 RID: 41349
			public readonly bool flipUv;

			// Token: 0x0400A186 RID: 41350
			public readonly bool vertexColors;

			// Token: 0x0400A187 RID: 41351
			private readonly int hash;
		}

		// Token: 0x02002782 RID: 10114
		private struct CachedAtlasReplacementInfo
		{
			// Token: 0x0400A188 RID: 41352
			public Material material;

			// Token: 0x0400A189 RID: 41353
			public Vector2[] uvs;

			// Token: 0x0400A18A RID: 41354
			public Color32 vertexColor;
		}
	}
}
