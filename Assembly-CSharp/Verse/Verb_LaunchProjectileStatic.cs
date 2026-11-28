using System;
using RimWorld;
using RimWorld.Utility;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200073C RID: 1852
	public class Verb_LaunchProjectileStatic : Verb_LaunchProjectile
	{
		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x06003AC2 RID: 15042 RVA: 0x00176AFA File Offset: 0x00174CFA
		public override bool MultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x06003AC3 RID: 15043 RVA: 0x00176AFD File Offset: 0x00174CFD
		public override Texture2D UIIcon
		{
			get
			{
				return TexCommand.Attack;
			}
		}

		// Token: 0x06003AC4 RID: 15044 RVA: 0x00176B04 File Offset: 0x00174D04
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			return base.ValidateTarget(target, true) && ReloadableUtility.CanUseConsideringQueuedJobs(this.CasterPawn, base.EquipmentSource, true);
		}

		// Token: 0x06003AC5 RID: 15045 RVA: 0x00176B2C File Offset: 0x00174D2C
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			Job job = JobMaker.MakeJob(JobDefOf.UseVerbOnThingStatic, target);
			job.verbToUse = this;
			this.CasterPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
		}
	}
}
