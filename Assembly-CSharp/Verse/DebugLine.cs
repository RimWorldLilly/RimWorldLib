using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200024C RID: 588
	internal struct DebugLine
	{
		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06001149 RID: 4425 RVA: 0x00063290 File Offset: 0x00061490
		public bool Done
		{
			get
			{
				return this.deathTick <= Find.TickManager.TicksGame;
			}
		}

		// Token: 0x0600114A RID: 4426 RVA: 0x000632A7 File Offset: 0x000614A7
		public DebugLine(Vector3 a, Vector3 b, int ticksLeft = 100, SimpleColor color = SimpleColor.White)
		{
			this.a = a;
			this.b = b;
			this.deathTick = Find.TickManager.TicksGame + ticksLeft;
			this.color = color;
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x000632D1 File Offset: 0x000614D1
		public void Draw()
		{
			GenDraw.DrawLineBetween(this.a, this.b, this.color, 0.2f);
		}

		// Token: 0x04000FCD RID: 4045
		public Vector3 a;

		// Token: 0x04000FCE RID: 4046
		public Vector3 b;

		// Token: 0x04000FCF RID: 4047
		private int deathTick;

		// Token: 0x04000FD0 RID: 4048
		private SimpleColor color;
	}
}
