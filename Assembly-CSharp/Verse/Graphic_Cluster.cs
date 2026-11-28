using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200055F RID: 1375
	public class Graphic_Cluster : Graphic_Collection
	{
		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x060028EC RID: 10476 RVA: 0x000FBB89 File Offset: 0x000F9D89
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[Rand.Range(0, this.subGraphics.Length)].MatSingle;
			}
		}

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x060028ED RID: 10477 RVA: 0x000FBBA5 File Offset: 0x000F9DA5
		protected virtual float PositionVariance
		{
			get
			{
				return 0.45f;
			}
		}

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x060028EE RID: 10478 RVA: 0x000FBBAC File Offset: 0x000F9DAC
		protected virtual float SizeVariance
		{
			get
			{
				return 0.2f;
			}
		}

		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x060028EF RID: 10479 RVA: 0x000FBBB3 File Offset: 0x000F9DB3
		private float SizeFactorMin
		{
			get
			{
				return 1f - this.SizeVariance;
			}
		}

		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x060028F0 RID: 10480 RVA: 0x000FBBC1 File Offset: 0x000F9DC1
		private float SizeFactorMax
		{
			get
			{
				return 1f + this.SizeVariance;
			}
		}

		// Token: 0x060028F1 RID: 10481 RVA: 0x000FBBCF File Offset: 0x000F9DCF
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Log.ErrorOnce("Graphic_Cluster cannot draw realtime.", 9432243);
		}

		// Token: 0x060028F2 RID: 10482 RVA: 0x000FBBE0 File Offset: 0x000F9DE0
		protected virtual Vector3 GetCenter(Thing thing, int index)
		{
			return thing.TrueCenter();
		}

		// Token: 0x060028F3 RID: 10483 RVA: 0x000FBBE8 File Offset: 0x000F9DE8
		protected virtual int ScatterCount(Thing thing)
		{
			Filth filth = thing as Filth;
			int result;
			if (filth == null)
			{
				result = 3;
			}
			else
			{
				result = filth.thickness;
			}
			return result;
		}

		// Token: 0x060028F4 RID: 10484 RVA: 0x000FBC0C File Offset: 0x000F9E0C
		public Graphic SubGraphicFor(Thing thing)
		{
			if (thing == null)
			{
				return this.subGraphics[0];
			}
			int num = thing.OverrideGraphicIndex ?? thing.thingIDNumber;
			return this.subGraphics[num % this.subGraphics.Length];
		}

		// Token: 0x060028F5 RID: 10485 RVA: 0x000FBC58 File Offset: 0x000F9E58
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			Rand.PushState(thing.Position.GetHashCode());
			int num = this.ScatterCount(thing);
			Filth filth = thing as Filth;
			for (int i = 0; i < num; i++)
			{
				Material matSingle = this.MatSingle;
				Vector3 center = this.GetCenter(thing, i) + new Vector3(Rand.Range(-this.PositionVariance, this.PositionVariance), 0f, Rand.Range(-this.PositionVariance, this.PositionVariance));
				Vector2 size = new Vector2(Rand.Range(this.data.drawSize.x * this.SizeFactorMin, this.data.drawSize.x * this.SizeFactorMax), Rand.Range(this.data.drawSize.y * this.SizeFactorMin, this.data.drawSize.y * this.SizeFactorMax));
				float rot = (float)Rand.RangeInclusive(0, 360) + extraRotation;
				if (((filth != null) ? filth.drawInstances : null) != null && filth.drawInstances.Count == num)
				{
					rot = filth.drawInstances[i].rotation;
				}
				bool flipUv = Rand.Value < 0.5f;
				Vector2[] uvs;
				Color32 color;
				Graphic.TryGetTextureAtlasReplacementInfo(matSingle, thing.def.category.ToAtlasGroup(), flipUv, true, out matSingle, out uvs, out color);
				Printer_Plane.PrintPlane(layer, center, size, matSingle, rot, flipUv, uvs, new Color32[]
				{
					color,
					color,
					color,
					color
				}, 0.01f, 0f);
			}
			Rand.PopState();
		}

		// Token: 0x060028F6 RID: 10486 RVA: 0x000FBE0C File Offset: 0x000FA00C
		public override string ToString()
		{
			string[] array = new string[5];
			array[0] = "Scatter(subGraphic[0]=";
			int num = 1;
			Graphic graphic = this.subGraphics[0];
			array[num] = ((graphic != null) ? graphic.ToString() : null);
			array[2] = ", count=";
			array[3] = this.subGraphics.Length.ToString();
			array[4] = ")";
			return string.Concat(array);
		}
	}
}
