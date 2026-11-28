using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200057F RID: 1407
	public class Graphic_Single_SquashNStretch : Graphic_WithPropertyBlock
	{
		// Token: 0x060029A7 RID: 10663 RVA: 0x000FF23E File Offset: 0x000FD43E
		public float AgeSecs(Thing thing)
		{
			return (float)(Find.TickManager.TicksGame - thing.TickSpawned) / 60f;
		}

		// Token: 0x060029A8 RID: 10664 RVA: 0x000FF258 File Offset: 0x000FD458
		public override void Init(GraphicRequest req)
		{
			base.Init(req);
			this.snsProps = new Vector4(this.data.maxSnS.x, this.data.maxSnS.y, this.data.offsetSnS.x, this.data.offsetSnS.y);
		}

		// Token: 0x060029A9 RID: 10665 RVA: 0x000FF2B8 File Offset: 0x000FD4B8
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			this.mat.SetFloat(ShaderPropertyIDs.AgeSecs, this.AgeSecs(thing));
			this.propertyBlock.SetVector(ShaderPropertyIDs.SquashNStretch, this.snsProps);
			this.propertyBlock.SetFloat(ShaderPropertyIDs.RandomPerObject, (float)thing.thingIDNumber.HashOffset());
			base.DrawWorker(loc, rot, thingDef, thing, extraRotation);
		}

		// Token: 0x04001CE8 RID: 7400
		private Vector4 snsProps;
	}
}
