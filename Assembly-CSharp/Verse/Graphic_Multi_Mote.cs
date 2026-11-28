using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000578 RID: 1400
	public class Graphic_Multi_Mote : Graphic_Multi_AgeSecs
	{
		// Token: 0x06002974 RID: 10612 RVA: 0x000FE518 File Offset: 0x000FC718
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Mote mote = thing as Mote;
			if (mote != null)
			{
				this.propertyBlock.SetColor(ShaderPropertyIDs.Color, mote.instanceColor);
			}
			base.DrawWorker(loc, rot, thingDef, thing, extraRotation);
		}

		// Token: 0x06002975 RID: 10613 RVA: 0x000FE553 File Offset: 0x000FC753
		protected override void DrawMeshInt(Mesh mesh, Vector3 loc, Quaternion quat, Material mat)
		{
			Graphics.DrawMesh(mesh, loc, quat, mat, 0, null, 0, this.propertyBlock);
		}
	}
}
