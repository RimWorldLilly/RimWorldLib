using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000257 RID: 599
	public class DrawStyle_FilledOval : DrawStyle
	{
		// Token: 0x17000356 RID: 854
		// (get) Token: 0x0600118D RID: 4493 RVA: 0x00064A6E File Offset: 0x00062C6E
		public override bool CanHaveDuplicates
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600118E RID: 4494 RVA: 0x00064A74 File Offset: 0x00062C74
		public override void Update(IntVec3 origin, IntVec3 target, List<IntVec3> buffer)
		{
			CellRect cellRect = CellRect.FromLimits(origin, target);
			float radius = (float)cellRect.Width / 2f;
			float ratio = (float)cellRect.Width / (float)cellRect.Height;
			Vector3 b = cellRect.CenterCell.ToVector3();
			if (cellRect.Width % 2 == 0)
			{
				b.x -= 0.5f;
			}
			if (cellRect.Height % 2 == 0)
			{
				b.z -= 0.5f;
			}
			foreach (IntVec3 item in cellRect.Cells)
			{
				Vector3 vector = item.ToVector3() - b;
				if (this.Filled(vector.x, vector.z, radius, ratio))
				{
					buffer.Add(item);
				}
			}
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x00064B60 File Offset: 0x00062D60
		protected float DistanceSqr(float x, float z, float ratio)
		{
			return Mathf.Pow(z * ratio, 2f) + Mathf.Pow(x, 2f);
		}

		// Token: 0x06001190 RID: 4496 RVA: 0x00064B7B File Offset: 0x00062D7B
		protected bool Inside(float x, float z, float radius, float ratio)
		{
			return this.DistanceSqr(x, z, ratio) <= (radius + 0.4f) * (radius + 0.4f);
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x00064B9B File Offset: 0x00062D9B
		protected virtual bool Filled(float x, float z, float radius, float ratio)
		{
			return this.Inside(x, z, radius, ratio);
		}

		// Token: 0x04000FEC RID: 4076
		private const float RadiusOffset = 0.4f;
	}
}
