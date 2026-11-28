using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000577 RID: 1399
	public class Graphic_Multi_BuildingWorking : Graphic_Multi_AgeSecs
	{
		// Token: 0x06002972 RID: 10610 RVA: 0x000FE484 File Offset: 0x000FC684
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			this.MatAt(rot, thing);
			Building building = thing as Building;
			if (building != null)
			{
				this.propertyBlock.SetFloat(ShaderPropertyIDs.Working, building.IsWorking() ? 1f : 0f);
			}
			this.propertyBlock.SetFloat(ShaderPropertyIDs.Rotation, (float)rot.AsInt);
			if (thing != null)
			{
				this.propertyBlock.SetFloat(ShaderPropertyIDs.RandomPerObject, (float)thing.thingIDNumber.HashOffset());
			}
			base.DrawWorker(loc, rot, thingDef, thing, extraRotation);
		}
	}
}
