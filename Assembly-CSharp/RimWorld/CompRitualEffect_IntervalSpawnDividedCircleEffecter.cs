using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001741 RID: 5953
	public class CompRitualEffect_IntervalSpawnDividedCircleEffecter : CompRitualEffect_IntervalSpawnBurst
	{
		// Token: 0x1700195D RID: 6493
		// (get) Token: 0x0600943E RID: 37950 RVA: 0x00360824 File Offset: 0x0035EA24
		protected new CompProperties_RitualEffectIntervalSpawnDividedCircle Props
		{
			get
			{
				return (CompProperties_RitualEffectIntervalSpawnDividedCircle)this.props;
			}
		}

		// Token: 0x0600943F RID: 37951 RVA: 0x00360831 File Offset: 0x0035EA31
		protected override Vector3 SpawnPos(LordJob_Ritual ritual)
		{
			return Vector3.zero;
		}

		// Token: 0x06009440 RID: 37952 RVA: 0x00360838 File Offset: 0x0035EA38
		public override void OnSetup(RitualVisualEffect parent, LordJob_Ritual ritual, bool loading)
		{
			this.parent = parent;
			float num = 360f / (float)this.Props.numCopies;
			for (int i = 0; i < this.Props.numCopies; i++)
			{
				Vector3 a = Quaternion.AngleAxis(num * (float)i, Vector3.up) * Vector3.forward;
				IntVec3 cell = parent.ritual.selectedTarget.Cell;
				TargetInfo targetInfo = new TargetInfo(cell, parent.ritual.Map, false);
				Vector3 vector = (a * this.Props.radius + this.Props.offset) * base.ScaleForRoom(ritual);
				Room room = (cell + vector.ToIntVec3() + this.Props.roomCheckOffset).GetRoom(ritual.Map);
				if (!this.props.onlySpawnInSameRoom || room == ritual.GetRoom)
				{
					Effecter effecter = this.Props.effecterDef.Spawn(cell, parent.ritual.Map, vector, 1f);
					effecter.Trigger(targetInfo, TargetInfo.Invalid, -1);
					parent.AddEffecterToMaintain(targetInfo, effecter);
				}
			}
		}
	}
}
