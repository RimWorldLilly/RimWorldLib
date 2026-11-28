using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002310 RID: 8976
	public class DebugWorldLine
	{
		// Token: 0x170026C2 RID: 9922
		// (get) Token: 0x0600DDAB RID: 56747 RVA: 0x005288CC File Offset: 0x00526ACC
		// (set) Token: 0x0600DDAC RID: 56748 RVA: 0x005288D4 File Offset: 0x00526AD4
		public int TicksLeft
		{
			get
			{
				return this.ticksLeft;
			}
			set
			{
				this.ticksLeft = value;
			}
		}

		// Token: 0x0600DDAD RID: 56749 RVA: 0x005288DD File Offset: 0x00526ADD
		public DebugWorldLine(Vector3 a, Vector3 b, bool onPlanetSurface)
		{
			this.a = a;
			this.b = b;
			this.onPlanetSurface = onPlanetSurface;
			this.ticksLeft = 100;
		}

		// Token: 0x0600DDAE RID: 56750 RVA: 0x00528902 File Offset: 0x00526B02
		public DebugWorldLine(Vector3 a, Vector3 b, bool onPlanetSurface, int ticksLeft)
		{
			this.a = a;
			this.b = b;
			this.onPlanetSurface = onPlanetSurface;
			this.ticksLeft = ticksLeft;
		}

		// Token: 0x0600DDAF RID: 56751 RVA: 0x00528928 File Offset: 0x00526B28
		public void Draw()
		{
			float num = Vector3.Distance(this.a, this.b);
			if (num < 0.001f)
			{
				return;
			}
			if (this.onPlanetSurface)
			{
				float averageTileSize = Find.WorldGrid.AverageTileSize;
				int num2 = Mathf.Max(Mathf.RoundToInt(num / averageTileSize), 0);
				float num3 = 0.08f;
				for (int i = 0; i < num2; i++)
				{
					Vector3 vector = Vector3.Lerp(this.a, this.b, (float)i / (float)num2);
					Vector3 vector2 = Vector3.Lerp(this.a, this.b, (float)(i + 1) / (float)num2);
					vector = vector.normalized * (100f + num3);
					vector2 = vector2.normalized * (100f + num3);
					GenDraw.DrawWorldLineBetween(vector, vector2, 1f);
				}
				return;
			}
			GenDraw.DrawWorldLineBetween(this.a, this.b, 1f);
		}

		// Token: 0x04008E5F RID: 36447
		public Vector3 a;

		// Token: 0x04008E60 RID: 36448
		public Vector3 b;

		// Token: 0x04008E61 RID: 36449
		public int ticksLeft;

		// Token: 0x04008E62 RID: 36450
		private bool onPlanetSurface;
	}
}
