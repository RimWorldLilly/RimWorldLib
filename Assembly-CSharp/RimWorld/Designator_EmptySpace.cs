using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C46 RID: 7238
	public class Designator_EmptySpace : Designator
	{
		// Token: 0x0600B88F RID: 47247 RVA: 0x00411BC2 File Offset: 0x0040FDC2
		public override GizmoResult GizmoOnGUI(Vector2 loc, float maxWidth, GizmoRenderParms parms)
		{
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x0600B890 RID: 47248 RVA: 0x00411BCA File Offset: 0x0040FDCA
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600B891 RID: 47249 RVA: 0x00411BD1 File Offset: 0x0040FDD1
		public override void DesignateSingleCell(IntVec3 c)
		{
			throw new NotImplementedException();
		}
	}
}
