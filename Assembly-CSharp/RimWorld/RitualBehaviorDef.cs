using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200167F RID: 5759
	public class RitualBehaviorDef : Def
	{
		// Token: 0x170018F2 RID: 6386
		// (get) Token: 0x0600905C RID: 36956 RVA: 0x0034FDC8 File Offset: 0x0034DFC8
		public RitualPosition_Lectern FirstLecternPosition
		{
			get
			{
				foreach (RitualStage ritualStage in this.stages)
				{
					if (ritualStage.roleBehaviors != null)
					{
						foreach (RitualRoleBehavior ritualRoleBehavior in ritualStage.roleBehaviors)
						{
							if (ritualRoleBehavior.CustomPositionsForReading != null)
							{
								foreach (RitualPosition ritualPosition in ritualRoleBehavior.CustomPositionsForReading)
								{
									RitualPosition_Lectern ritualPosition_Lectern = ritualPosition as RitualPosition_Lectern;
									if (ritualPosition_Lectern != null)
									{
										return ritualPosition_Lectern;
									}
								}
							}
						}
					}
				}
				return null;
			}
		}

		// Token: 0x170018F3 RID: 6387
		// (get) Token: 0x0600905D RID: 36957 RVA: 0x0034FEB8 File Offset: 0x0034E0B8
		public bool UsesLectern
		{
			get
			{
				return this.FirstLecternPosition != null;
			}
		}

		// Token: 0x0600905E RID: 36958 RVA: 0x0034FEC3 File Offset: 0x0034E0C3
		public RitualBehaviorWorker GetInstance()
		{
			return (RitualBehaviorWorker)Activator.CreateInstance(this.workerClass, new object[]
			{
				this
			});
		}

		// Token: 0x0600905F RID: 36959 RVA: 0x0034FEDF File Offset: 0x0034E0DF
		public override IEnumerable<string> ConfigErrors()
		{
			RitualBehaviorDef.<ConfigErrors>d__22 <ConfigErrors>d__ = new RitualBehaviorDef.<ConfigErrors>d__22(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x06009060 RID: 36960 RVA: 0x0034FEF0 File Offset: 0x0034E0F0
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.stages.NullOrEmpty<RitualStage>())
			{
				return;
			}
			for (int i = 0; i < this.stages.Count; i++)
			{
				this.stages[i].parent = this;
			}
		}

		// Token: 0x06009061 RID: 36961 RVA: 0x0034FF39 File Offset: 0x0034E139
		public IEnumerable<RitualRole> RequiredRoles()
		{
			RitualBehaviorDef.<RequiredRoles>d__24 <RequiredRoles>d__ = new RitualBehaviorDef.<RequiredRoles>d__24(-2);
			<RequiredRoles>d__.<>4__this = this;
			return <RequiredRoles>d__;
		}

		// Token: 0x0400581E RID: 22558
		public Type workerClass = typeof(RitualBehaviorWorker);

		// Token: 0x0400581F RID: 22559
		public List<RitualRole> roles;

		// Token: 0x04005820 RID: 22560
		public List<RitualStage> stages;

		// Token: 0x04005821 RID: 22561
		public IntRange durationTicks = new IntRange(7500, 7500);

		// Token: 0x04005822 RID: 22562
		public List<PreceptRequirement> preceptRequirements;

		// Token: 0x04005823 RID: 22563
		public List<RitualCancellationTrigger> cancellationTriggers;

		// Token: 0x04005824 RID: 22564
		public bool displayCompletedMessage = true;

		// Token: 0x04005825 RID: 22565
		[NoTranslate]
		public string useVisualEffectsFromRoleIdIdeo;

		// Token: 0x04005826 RID: 22566
		public int displayOrder;

		// Token: 0x04005827 RID: 22567
		[MustTranslate]
		public string letterTitle;

		// Token: 0x04005828 RID: 22568
		[MustTranslate]
		public string letterText;

		// Token: 0x04005829 RID: 22569
		[MustTranslate]
		public string spectatorsLabel;

		// Token: 0x0400582A RID: 22570
		[MustTranslate]
		public string spectatorGerund;

		// Token: 0x0400582B RID: 22571
		public bool spectatorsIgnoreBleeding;

		// Token: 0x0400582C RID: 22572
		public List<SoundDef> soundDefsPerEnhancerCount;

		// Token: 0x0400582D RID: 22573
		public int maxEnhancerDistance;

		// Token: 0x0400582E RID: 22574
		public RitualSpectatorFilter spectatorFilter;
	}
}
