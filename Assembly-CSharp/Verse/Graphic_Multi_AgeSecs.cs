using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000576 RID: 1398
	public class Graphic_Multi_AgeSecs : Graphic_Multi
	{
		// Token: 0x0600296E RID: 10606 RVA: 0x000FE405 File Offset: 0x000FC605
		public float AgeSecs(Thing thing)
		{
			return (float)(Find.TickManager.TicksGame - thing.TickSpawned) / 60f;
		}

		// Token: 0x0600296F RID: 10607 RVA: 0x000FE420 File Offset: 0x000FC620
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Material material = this.MatAt(rot, thing);
			if (thing != null)
			{
				material.SetFloat(ShaderPropertyIDs.AgeSecs, this.AgeSecs(thing));
			}
			base.DrawWorker(loc, rot, thingDef, thing, extraRotation);
		}

		// Token: 0x06002970 RID: 10608 RVA: 0x000FE45B File Offset: 0x000FC65B
		protected override void DrawMeshInt(Mesh mesh, Vector3 loc, Quaternion quat, Material mat)
		{
			Graphics.DrawMesh(mesh, loc, quat, mat, 0, null, 0, this.propertyBlock);
		}

		// Token: 0x04001CDA RID: 7386
		public MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
	}
}
