using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000582 RID: 1410
	public class Graphic_Tiling : Graphic_WithPropertyBlock
	{
		// Token: 0x060029B6 RID: 10678 RVA: 0x000FF57D File Offset: 0x000FD77D
		public Graphic_Tiling WithTiling(Vector2 tiling)
		{
			this.Tiling = tiling;
			return this;
		}

		// Token: 0x060029B7 RID: 10679 RVA: 0x000FF587 File Offset: 0x000FD787
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			this.propertyBlock.SetVector(ShaderPropertyIDs.Tiling, this.Tiling);
			base.DrawWorker(loc, rot, thingDef, thing, extraRotation);
		}

		// Token: 0x04001CE9 RID: 7401
		public Vector2 Tiling;
	}
}
