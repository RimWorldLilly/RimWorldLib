using System;
using System.Collections.Generic;
using System.Xml;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200038A RID: 906
	public class DrawData
	{
		// Token: 0x06001B8D RID: 7053 RVA: 0x000A9958 File Offset: 0x000A7B58
		public float ScaleFor(Pawn pawn)
		{
			float num = this.scale;
			if (pawn.RaceProps.Humanlike)
			{
				if (pawn.DevelopmentalStage.Child())
				{
					num *= this.childScale;
				}
				if (this.bodyTypeScales != null)
				{
					foreach (DrawData.BodyTypeDefWithScale bodyTypeDefWithScale in this.bodyTypeScales)
					{
						if (pawn.story.bodyType == bodyTypeDefWithScale.bodyType)
						{
							num *= bodyTypeDefWithScale.scale;
							break;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06001B8E RID: 7054 RVA: 0x000A99F8 File Offset: 0x000A7BF8
		public Vector2 PivotForRot(Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
			{
				Vector2? vector = (this.dataNorth != null) ? this.dataNorth.GetValueOrDefault().pivot : null;
				if (vector != null)
				{
					return vector.GetValueOrDefault();
				}
				Vector2? pivot = this.defaultData.pivot;
				if (pivot == null)
				{
					return DrawData.PivotCenter;
				}
				return pivot.GetValueOrDefault();
			}
			case 1:
			{
				Vector2? vector = (this.dataEast != null) ? this.dataEast.GetValueOrDefault().pivot : null;
				if (vector != null)
				{
					return vector.GetValueOrDefault();
				}
				Vector2? pivot = this.defaultData.pivot;
				if (pivot == null)
				{
					return DrawData.PivotCenter;
				}
				return pivot.GetValueOrDefault();
			}
			case 2:
			{
				Vector2? vector = (this.dataSouth != null) ? this.dataSouth.GetValueOrDefault().pivot : null;
				if (vector != null)
				{
					return vector.GetValueOrDefault();
				}
				Vector2? pivot = this.defaultData.pivot;
				if (pivot == null)
				{
					return DrawData.PivotCenter;
				}
				return pivot.GetValueOrDefault();
			}
			case 3:
			{
				Vector2? vector = (this.dataWest != null) ? this.dataWest.GetValueOrDefault().pivot : null;
				if (vector != null)
				{
					return vector.GetValueOrDefault();
				}
				Vector2? pivot = this.defaultData.pivot;
				if (pivot == null)
				{
					return DrawData.PivotCenter;
				}
				return pivot.GetValueOrDefault();
			}
			default:
			{
				Vector2? vector = this.defaultData.pivot;
				if (vector == null)
				{
					return DrawData.PivotCenter;
				}
				return vector.GetValueOrDefault();
			}
			}
		}

		// Token: 0x06001B8F RID: 7055 RVA: 0x000A9BB0 File Offset: 0x000A7DB0
		public Vector3 OffsetForRot(Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
			{
				Vector3? vector = (this.dataNorth != null) ? this.dataNorth.GetValueOrDefault().offset : null;
				if (vector != null)
				{
					return vector.GetValueOrDefault();
				}
				Vector3? offset = this.defaultData.offset;
				if (offset == null)
				{
					return Vector3.zero;
				}
				return offset.GetValueOrDefault();
			}
			case 1:
			{
				Vector3? vector2 = (this.dataEast != null) ? this.dataEast.GetValueOrDefault().offset : null;
				if (vector2 != null)
				{
					return vector2.Value;
				}
				Vector3? vector3 = (this.dataWest != null) ? this.dataWest.GetValueOrDefault().offset : null;
				if (vector3 != null)
				{
					Vector3 value = vector3.Value;
					value.x *= -1f;
					return value;
				}
				Vector3? offset2 = this.defaultData.offset;
				if (offset2 != null)
				{
					return offset2.Value;
				}
				return Vector3.zero;
			}
			case 2:
			{
				Vector3? vector = (this.dataSouth != null) ? this.dataSouth.GetValueOrDefault().offset : null;
				if (vector != null)
				{
					return vector.GetValueOrDefault();
				}
				Vector3? offset = this.defaultData.offset;
				if (offset == null)
				{
					return Vector3.zero;
				}
				return offset.GetValueOrDefault();
			}
			case 3:
			{
				Vector3? vector4 = (this.dataWest != null) ? this.dataWest.GetValueOrDefault().offset : null;
				if (vector4 != null)
				{
					return vector4.Value;
				}
				Vector3? vector5 = (this.dataEast != null) ? this.dataEast.GetValueOrDefault().offset : null;
				if (vector5 != null)
				{
					Vector3 value2 = vector5.Value;
					value2.x *= -1f;
					return value2;
				}
				Vector3? offset3 = this.defaultData.offset;
				if (offset3 != null)
				{
					return offset3.Value;
				}
				Vector3? vector = (this.dataWest != null) ? this.dataWest.GetValueOrDefault().offset : null;
				if (vector != null)
				{
					return vector.GetValueOrDefault();
				}
				Vector3? offset = this.defaultData.offset;
				if (offset == null)
				{
					return Vector3.zero;
				}
				return offset.GetValueOrDefault();
			}
			default:
			{
				Vector3? vector = this.defaultData.offset;
				if (vector == null)
				{
					return Vector3.zero;
				}
				return vector.GetValueOrDefault();
			}
			}
		}

		// Token: 0x06001B90 RID: 7056 RVA: 0x000A9E60 File Offset: 0x000A8060
		public float RotationOffsetForRot(Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
			{
				float? num = (this.dataNorth != null) ? this.dataNorth.GetValueOrDefault().rotationOffset : null;
				if (num == null)
				{
					return this.defaultData.rotationOffset.GetValueOrDefault();
				}
				return num.GetValueOrDefault();
			}
			case 1:
			{
				float? num = (this.dataEast != null) ? this.dataEast.GetValueOrDefault().rotationOffset : null;
				if (num == null)
				{
					return this.defaultData.rotationOffset.GetValueOrDefault();
				}
				return num.GetValueOrDefault();
			}
			case 2:
			{
				float? num = (this.dataSouth != null) ? this.dataSouth.GetValueOrDefault().rotationOffset : null;
				if (num == null)
				{
					return this.defaultData.rotationOffset.GetValueOrDefault();
				}
				return num.GetValueOrDefault();
			}
			case 3:
			{
				float? num = (this.dataWest != null) ? this.dataWest.GetValueOrDefault().rotationOffset : null;
				if (num == null)
				{
					return this.defaultData.rotationOffset.GetValueOrDefault();
				}
				return num.GetValueOrDefault();
			}
			default:
				return this.defaultData.rotationOffset.GetValueOrDefault();
			}
		}

		// Token: 0x06001B91 RID: 7057 RVA: 0x000A9FBC File Offset: 0x000A81BC
		public bool FlipForRot(Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
			{
				bool? flag = (this.dataNorth != null) ? this.dataNorth.GetValueOrDefault().flip : null;
				if (flag == null)
				{
					return this.defaultData.flip.GetValueOrDefault();
				}
				return flag.GetValueOrDefault();
			}
			case 1:
			{
				bool? flag = (this.dataEast != null) ? this.dataEast.GetValueOrDefault().flip : null;
				if (flag == null)
				{
					return this.defaultData.flip.GetValueOrDefault();
				}
				return flag.GetValueOrDefault();
			}
			case 2:
			{
				bool? flag = (this.dataSouth != null) ? this.dataSouth.GetValueOrDefault().flip : null;
				if (flag == null)
				{
					return this.defaultData.flip.GetValueOrDefault();
				}
				return flag.GetValueOrDefault();
			}
			case 3:
			{
				bool? flag = (this.dataWest != null) ? this.dataWest.GetValueOrDefault().flip : null;
				if (flag == null)
				{
					return this.defaultData.flip.GetValueOrDefault();
				}
				return flag.GetValueOrDefault();
			}
			default:
				return this.defaultData.flip.GetValueOrDefault();
			}
		}

		// Token: 0x06001B92 RID: 7058 RVA: 0x000AA118 File Offset: 0x000A8318
		public float LayerForRot(Rot4 rot, float defaultLayer)
		{
			switch (rot.AsInt)
			{
			case 0:
			{
				float? num = (this.dataNorth != null) ? this.dataNorth.GetValueOrDefault().layer : null;
				if (num == null)
				{
					return this.defaultData.layer.GetValueOrDefault(defaultLayer);
				}
				return num.GetValueOrDefault();
			}
			case 1:
			{
				float? num = (this.dataEast != null) ? this.dataEast.GetValueOrDefault().layer : null;
				if (num == null)
				{
					return this.defaultData.layer.GetValueOrDefault(defaultLayer);
				}
				return num.GetValueOrDefault();
			}
			case 2:
			{
				float? num = (this.dataSouth != null) ? this.dataSouth.GetValueOrDefault().layer : null;
				if (num == null)
				{
					return this.defaultData.layer.GetValueOrDefault(defaultLayer);
				}
				return num.GetValueOrDefault();
			}
			case 3:
			{
				float? num = (this.dataWest != null) ? this.dataWest.GetValueOrDefault().layer : null;
				if (num == null)
				{
					return this.defaultData.layer.GetValueOrDefault(defaultLayer);
				}
				return num.GetValueOrDefault();
			}
			default:
				return this.defaultData.layer.GetValueOrDefault(defaultLayer);
			}
		}

		// Token: 0x06001B93 RID: 7059 RVA: 0x000AA27C File Offset: 0x000A847C
		public static DrawData NewWithData(params DrawData.RotationalData[] data)
		{
			DrawData drawData = new DrawData();
			foreach (DrawData.RotationalData value in data)
			{
				if (value.rotation == null)
				{
					drawData.defaultData = value;
				}
				else
				{
					switch (value.rotation.Value.AsInt)
					{
					case 0:
						drawData.dataNorth = new DrawData.RotationalData?(value);
						break;
					case 1:
						drawData.dataEast = new DrawData.RotationalData?(value);
						break;
					case 2:
						drawData.dataSouth = new DrawData.RotationalData?(value);
						break;
					case 3:
						drawData.dataWest = new DrawData.RotationalData?(value);
						break;
					}
				}
			}
			return drawData;
		}

		// Token: 0x04001694 RID: 5780
		public static readonly Vector2 PivotCenter = new Vector2(0.5f, 0.5f);

		// Token: 0x04001695 RID: 5781
		private DrawData.RotationalData defaultData;

		// Token: 0x04001696 RID: 5782
		private DrawData.RotationalData? dataNorth;

		// Token: 0x04001697 RID: 5783
		private DrawData.RotationalData? dataEast;

		// Token: 0x04001698 RID: 5784
		private DrawData.RotationalData? dataSouth;

		// Token: 0x04001699 RID: 5785
		private DrawData.RotationalData? dataWest;

		// Token: 0x0400169A RID: 5786
		public bool scaleOffsetByBodySize;

		// Token: 0x0400169B RID: 5787
		public bool useBodyPartAnchor;

		// Token: 0x0400169C RID: 5788
		public float scale = 1f;

		// Token: 0x0400169D RID: 5789
		public float childScale = 1f;

		// Token: 0x0400169E RID: 5790
		private List<DrawData.BodyTypeDefWithScale> bodyTypeScales;

		// Token: 0x020026A0 RID: 9888
		public struct RotationalData
		{
			// Token: 0x0600F43B RID: 62523 RVA: 0x005933B4 File Offset: 0x005915B4
			public RotationalData(Rot4? rotation, float layer)
			{
				this = default(DrawData.RotationalData);
				this.rotation = rotation;
				this.layer = new float?(layer);
				this.pivot = null;
				this.offset = null;
				this.rotationOffset = null;
				this.flip = null;
			}

			// Token: 0x04009E20 RID: 40480
			public Rot4? rotation;

			// Token: 0x04009E21 RID: 40481
			public Vector2? pivot;

			// Token: 0x04009E22 RID: 40482
			public Vector3? offset;

			// Token: 0x04009E23 RID: 40483
			public float? rotationOffset;

			// Token: 0x04009E24 RID: 40484
			public bool? flip;

			// Token: 0x04009E25 RID: 40485
			public float? layer;
		}

		// Token: 0x020026A1 RID: 9889
		private class BodyTypeDefWithScale
		{
			// Token: 0x0600F43C RID: 62524 RVA: 0x0059340B File Offset: 0x0059160B
			public void LoadDataFromXmlCustom(XmlNode xmlRoot)
			{
				DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "bodyType", xmlRoot.Name, null, null, null);
				this.scale = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
			}

			// Token: 0x04009E26 RID: 40486
			public BodyTypeDef bodyType;

			// Token: 0x04009E27 RID: 40487
			public float scale = 1f;
		}
	}
}
