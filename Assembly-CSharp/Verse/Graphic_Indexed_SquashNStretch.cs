using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200056B RID: 1387
	public class Graphic_Indexed_SquashNStretch : Graphic_Indexed
	{
		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x06002928 RID: 10536 RVA: 0x000FCD68 File Offset: 0x000FAF68
		protected override Type SingleGraphicType
		{
			get
			{
				return typeof(Graphic_Single_SquashNStretch);
			}
		}

		// Token: 0x06002929 RID: 10537 RVA: 0x000FCD74 File Offset: 0x000FAF74
		public float AgeSecs(Thing thing)
		{
			return (float)(Find.TickManager.TicksGame - thing.TickSpawned) / 60f;
		}

		// Token: 0x0600292A RID: 10538 RVA: 0x000FCD90 File Offset: 0x000FAF90
		public override void Init(GraphicRequest req)
		{
			base.Init(req);
			this.snsProps = new Vector4(this.data.maxSnS.x, this.data.maxSnS.y, this.data.offsetSnS.x, this.data.offsetSnS.y);
		}

		// Token: 0x0600292B RID: 10539 RVA: 0x000FCDF0 File Offset: 0x000FAFF0
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Graphic_Single_SquashNStretch graphic_Single_SquashNStretch;
			if (thing != null)
			{
				graphic_Single_SquashNStretch = (base.SubGraphicFor(thing) as Graphic_Single_SquashNStretch);
			}
			else
			{
				graphic_Single_SquashNStretch = (this.subGraphics[0] as Graphic_Single_SquashNStretch);
			}
			graphic_Single_SquashNStretch.propertyBlock.SetVector(ShaderPropertyIDs.SquashNStretch, this.snsProps);
			graphic_Single_SquashNStretch.propertyBlock.SetFloat(ShaderPropertyIDs.RandomPerObject, (float)thing.thingIDNumber.HashOffset());
			graphic_Single_SquashNStretch.MatSingle.SetFloat(ShaderPropertyIDs.AgeSecs, this.AgeSecs(thing));
			graphic_Single_SquashNStretch.DrawWorker(loc, rot, thingDef, thing, extraRotation);
			if (base.ShadowGraphic != null)
			{
				base.ShadowGraphic.DrawWorker(loc, rot, thingDef, thing, extraRotation);
			}
		}

		// Token: 0x04001CC7 RID: 7367
		private Vector4 snsProps;
	}
}
