using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200018E RID: 398
	public class WorkModeDrawer
	{
		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000B4B RID: 2891 RVA: 0x0003D4A2 File Offset: 0x0003B6A2
		protected virtual bool DrawIconAtTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x0003D4A8 File Offset: 0x0003B6A8
		public virtual void DrawControlGroupMouseOverExtra(MechanitorControlGroup group)
		{
			GlobalTargetInfo targetForLine = this.GetTargetForLine(group);
			List<Pawn> mechsForReading = group.MechsForReading;
			Map currentMap = Find.CurrentMap;
			if (targetForLine.IsValid && targetForLine.Map == currentMap)
			{
				Vector3 vector = targetForLine.Cell.ToVector3ShiftedWithAltitude(AltitudeLayer.MoteOverhead);
				for (int i = 0; i < mechsForReading.Count; i++)
				{
					if (mechsForReading[i].Map == currentMap)
					{
						GenDraw.DrawLineBetween(vector, mechsForReading[i].DrawPos, SimpleColor.White, 0.1f);
						GenDraw.DrawCircleOutline(mechsForReading[i].DrawPos, 0.5f);
					}
				}
				if (this.DrawIconAtTarget)
				{
					if (this.iconMat == null)
					{
						this.iconMat = MaterialPool.MatFrom(this.def.uiIcon);
					}
					Matrix4x4 matrix = Matrix4x4.TRS(vector, Quaternion.identity, WorkModeDrawer.IconScale);
					Graphics.DrawMesh(MeshPool.plane14, matrix, this.iconMat, 0);
				}
			}
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x0003D59C File Offset: 0x0003B79C
		public virtual GlobalTargetInfo GetTargetForLine(MechanitorControlGroup group)
		{
			return group.Target;
		}

		// Token: 0x0400099C RID: 2460
		private const float MouseoverLineWidth = 0.1f;

		// Token: 0x0400099D RID: 2461
		private const float CircleOutlineRadius = 0.5f;

		// Token: 0x0400099E RID: 2462
		private static readonly Vector3 IconScale = Vector3.one * 0.5f;

		// Token: 0x0400099F RID: 2463
		public MechWorkModeDef def;

		// Token: 0x040009A0 RID: 2464
		private Material iconMat;
	}
}
