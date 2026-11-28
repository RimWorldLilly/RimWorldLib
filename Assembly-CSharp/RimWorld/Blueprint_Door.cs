using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A53 RID: 6739
	public class Blueprint_Door : Blueprint_Build
	{
		// Token: 0x17001DA2 RID: 7586
		// (get) Token: 0x0600A8FA RID: 43258 RVA: 0x003BF969 File Offset: 0x003BDB69
		public override Graphic Graphic
		{
			get
			{
				return base.DefaultGraphic;
			}
		}

		// Token: 0x0600A8FB RID: 43259 RVA: 0x003BF974 File Offset: 0x003BDB74
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			if (this.def.entityDefToBuild.Size == IntVec2.One)
			{
				base.Rotation = DoorUtility.DoorRotationAt(base.Position, base.Map, base.BuildDef.building.preferConnectingToFences);
			}
			base.DrawAt(drawLoc, flip);
		}
	}
}
