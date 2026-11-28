using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003DA RID: 986
	public class PawnRenderNodeWorker_TurretGun : PawnRenderNodeWorker
	{
		// Token: 0x06001CF8 RID: 7416 RVA: 0x000B196C File Offset: 0x000AFB6C
		public override Quaternion RotationFor(PawnRenderNode node, PawnDrawParms parms)
		{
			Quaternion quaternion = base.RotationFor(node, parms);
			PawnRenderNode_TurretGun pawnRenderNode_TurretGun = node as PawnRenderNode_TurretGun;
			if (pawnRenderNode_TurretGun != null)
			{
				quaternion *= pawnRenderNode_TurretGun.turretComp.curRotation.ToQuat();
			}
			return quaternion;
		}
	}
}
