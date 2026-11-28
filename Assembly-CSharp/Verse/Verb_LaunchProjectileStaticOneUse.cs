using System;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020000E4 RID: 228
	public class Verb_LaunchProjectileStaticOneUse : Verb_LaunchProjectileStatic
	{
		// Token: 0x06000863 RID: 2147 RVA: 0x000302EA File Offset: 0x0002E4EA
		protected override bool TryCastShot()
		{
			if (base.TryCastShot())
			{
				if (this.burstShotsLeft <= 1)
				{
					this.SelfConsume();
				}
				return true;
			}
			if (this.burstShotsLeft < base.BurstShotCount)
			{
				this.SelfConsume();
			}
			return false;
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x0003031A File Offset: 0x0002E51A
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			return base.ValidateTarget(target, true) && target.Cell.GetFirstBuilding(Find.CurrentMap) == null && target.Cell.Standable(Find.CurrentMap);
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x00030350 File Offset: 0x0002E550
		public override void OnGUI(LocalTargetInfo target)
		{
			Map currentMap = Find.CurrentMap;
			base.OnGUI(target);
			Texture2D icon;
			if (!target.Cell.InBounds(currentMap))
			{
				icon = TexCommand.CannotShoot;
			}
			else
			{
				icon = (this.ValidateTarget(target, false) ? TexCommand.Attack : TexCommand.CannotShoot);
			}
			GenUI.DrawMouseAttachment(icon);
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x0003039E File Offset: 0x0002E59E
		public override void Notify_EquipmentLost()
		{
			base.Notify_EquipmentLost();
			if (this.state == VerbState.Bursting && this.burstShotsLeft < base.BurstShotCount)
			{
				this.SelfConsume();
			}
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x000303C3 File Offset: 0x0002E5C3
		private void SelfConsume()
		{
			if (base.EquipmentSource != null && !base.EquipmentSource.Destroyed)
			{
				base.EquipmentSource.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x000303E8 File Offset: 0x0002E5E8
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			Job job = JobMaker.MakeJob(JobDefOf.UseVerbOnThingStaticReserve, target);
			job.verbToUse = this;
			this.CasterPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
		}
	}
}
