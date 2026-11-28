using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200057E RID: 1406
	public class Graphic_Single_AgeSecs : Graphic_Single
	{
		// Token: 0x060029A4 RID: 10660 RVA: 0x000FF1F5 File Offset: 0x000FD3F5
		public float AgeSecs(Thing thing)
		{
			return (float)(Find.TickManager.TicksGame - thing.TickSpawned) / 60f;
		}

		// Token: 0x060029A5 RID: 10661 RVA: 0x000FF20F File Offset: 0x000FD40F
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			this.mat.SetFloat(ShaderPropertyIDs.AgeSecs, this.AgeSecs(thing));
			base.DrawWorker(loc, rot, thingDef, thing, extraRotation);
		}
	}
}
