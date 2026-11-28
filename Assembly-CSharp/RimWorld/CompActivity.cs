using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020017EC RID: 6124
	public sealed class CompActivity : ThingComp
	{
		// Token: 0x170019E9 RID: 6633
		// (get) Token: 0x060098BA RID: 39098 RVA: 0x003755A0 File Offset: 0x003737A0
		public CompProperties_Activity Props
		{
			get
			{
				return (CompProperties_Activity)this.props;
			}
		}

		// Token: 0x170019EA RID: 6634
		// (get) Token: 0x060098BB RID: 39099 RVA: 0x003755AD File Offset: 0x003737AD
		public float ActivityLevel
		{
			get
			{
				return this.activityLevelPercent;
			}
		}

		// Token: 0x170019EB RID: 6635
		// (get) Token: 0x060098BC RID: 39100 RVA: 0x003755B5 File Offset: 0x003737B5
		public bool IsActive
		{
			get
			{
				return this.state == ActivityState.Active;
			}
		}

		// Token: 0x170019EC RID: 6636
		// (get) Token: 0x060098BD RID: 39101 RVA: 0x003755C0 File Offset: 0x003737C0
		public bool IsDormant
		{
			get
			{
				return this.state == ActivityState.Passive;
			}
		}

		// Token: 0x170019ED RID: 6637
		// (get) Token: 0x060098BE RID: 39102 RVA: 0x003755CB File Offset: 0x003737CB
		public bool Deactivated
		{
			get
			{
				return this.deactivated;
			}
		}

		// Token: 0x170019EE RID: 6638
		// (get) Token: 0x060098BF RID: 39103 RVA: 0x003755D3 File Offset: 0x003737D3
		public ActivityState State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x170019EF RID: 6639
		// (get) Token: 0x060098C0 RID: 39104 RVA: 0x003755DB File Offset: 0x003737DB
		public float ActivityResearchFactor
		{
			get
			{
				return this.Props.activityResearchFactorCurve.Evaluate(this.ActivityLevel);
			}
		}

		// Token: 0x170019F0 RID: 6640
		// (get) Token: 0x060098C1 RID: 39105 RVA: 0x003755F4 File Offset: 0x003737F4
		public bool CanBeSuppressed
		{
			get
			{
				using (IEnumerator<IActivity> enumerator = this.parent.GetComps<IActivity>().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.CanBeSuppressed())
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x060098C2 RID: 39106 RVA: 0x0037564C File Offset: 0x0037384C
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.SetActivity(this.Props.startingRange.RandomInRange, true);
		}

		// Token: 0x060098C3 RID: 39107 RVA: 0x0037566C File Offset: 0x0037386C
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.SuppressingEntities, OpportunityType.Important);
		}

		// Token: 0x060098C4 RID: 39108 RVA: 0x00375680 File Offset: 0x00373880
		public override void CompTick()
		{
			this.AdjustActivity(this.Props.Worker.GetChangeRatePerDay(this.parent) / 60000f);
		}

		// Token: 0x060098C5 RID: 39109 RVA: 0x003756A4 File Offset: 0x003738A4
		public override string CompInspectStringExtra()
		{
			if (this.Deactivated)
			{
				return "Deactivated".Translate() + ".";
			}
			if (this.IsActive)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			this.Props.Worker.GetInspectString(this.parent, stringBuilder);
			return stringBuilder.ToString();
		}

		// Token: 0x060098C6 RID: 39110 RVA: 0x00375700 File Offset: 0x00373900
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompActivity.<CompGetGizmosExtra>d__28 <CompGetGizmosExtra>d__ = new CompActivity.<CompGetGizmosExtra>d__28(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x060098C7 RID: 39111 RVA: 0x00375710 File Offset: 0x00373910
		public void AdjustActivity(float delta)
		{
			this.SetActivity(this.activityLevelPercent + delta, false);
		}

		// Token: 0x060098C8 RID: 39112 RVA: 0x00375724 File Offset: 0x00373924
		public void SetActivity(float activity, bool dontDirtyGraphic = false)
		{
			if (this.Deactivated)
			{
				return;
			}
			this.activityLevelPercent = Mathf.Clamp01(activity);
			if (this.activityLevelPercent >= 1f && this.state != ActivityState.Active && this.CanActivate())
			{
				this.EnterActiveState();
			}
			else if (this.activityLevelPercent <= 0f && this.state != ActivityState.Passive)
			{
				this.EnterPassiveState();
			}
			if (!dontDirtyGraphic && this.Props.dirtyGraphicsOnActivityChange)
			{
				Pawn pawn = this.parent as Pawn;
				if (pawn != null && Mathf.Abs(this.activityLevelPercent - this.lastGraphicDirtyPercent) > 0.01f)
				{
					this.lastGraphicDirtyPercent = this.activityLevelPercent;
					pawn.Drawer.renderer.SetAllGraphicsDirty();
				}
			}
		}

		// Token: 0x060098C9 RID: 39113 RVA: 0x003757DC File Offset: 0x003739DC
		private bool CanActivate()
		{
			using (IEnumerator<IActivity> enumerator = this.parent.GetComps<IActivity>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.CanActivate())
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060098CA RID: 39114 RVA: 0x00375834 File Offset: 0x00373A34
		public bool ShouldGoPassive()
		{
			if (this.Deactivated)
			{
				return false;
			}
			using (IEnumerator<IActivity> enumerator = this.parent.GetComps<IActivity>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.ShouldGoPassive())
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060098CB RID: 39115 RVA: 0x00375898 File Offset: 0x00373A98
		public void EnterActiveState()
		{
			if (this.Deactivated)
			{
				return;
			}
			this.triggeredNaturally = (this.activityLevelPercent >= 1f);
			this.activityLevelPercent = 1f;
			this.state = ActivityState.Active;
			foreach (IActivity activity in this.parent.GetComps<IActivity>())
			{
				activity.OnActivityActivated();
			}
			if ((this.triggeredNaturally || this.Props.showLetterOnManualActivation) && this.Props.showLetterOnActivated)
			{
				Find.LetterStack.ReceiveLetter(this.Props.letterTitle, this.Props.letterDesc, this.Props.letterDef, this.parent, null, null, null, null, 0, true);
			}
		}

		// Token: 0x060098CC RID: 39116 RVA: 0x00375980 File Offset: 0x00373B80
		public void EnterPassiveState()
		{
			if (this.Deactivated)
			{
				return;
			}
			this.activityLevelPercent = 0f;
			this.state = ActivityState.Passive;
			foreach (IActivity activity in this.parent.GetComps<IActivity>())
			{
				activity.OnPassive();
			}
		}

		// Token: 0x060098CD RID: 39117 RVA: 0x003759EC File Offset: 0x00373BEC
		public void Deactivate()
		{
			if (this.IsActive)
			{
				this.EnterPassiveState();
			}
			this.activityLevelPercent = 0f;
			this.deactivated = true;
			this.suppressionEnabled = false;
			this.suppressIfAbove = 0.5f;
			this.gizmo = null;
			CompStudiable comp = this.parent.GetComp<CompStudiable>();
			if (comp == null)
			{
				return;
			}
			comp.Notify_ActivityDeactivated();
		}

		// Token: 0x060098CE RID: 39118 RVA: 0x00375A48 File Offset: 0x00373C48
		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (this.Deactivated)
			{
				return;
			}
			if (totalDamageDealt > 0f && this.IsDormant && this.Props.changePerDamage > 0f)
			{
				float x = 1f;
				Pawn pawn = this.parent as Pawn;
				if (pawn != null)
				{
					x = pawn.health.summaryHealth.SummaryHealthPercent;
				}
				else if (this.parent.def.useHitPoints)
				{
					x = (float)this.parent.HitPoints / (float)this.parent.MaxHitPoints;
				}
				float num = this.Props.changePerDamage * totalDamageDealt * this.Props.damagedActivityMultiplierCurve.Evaluate(x);
				this.SetActivity(this.activityLevelPercent + num, false);
				if (this.IsActive)
				{
					return;
				}
				TaggedString taggedString = "MessageActivityRisingDamage".Translate(this.parent.LabelNoParenthesis);
				if (MessagesRepeatAvoider.MessageShowAllowed(taggedString, 15f))
				{
					Messages.Message(taggedString, this.parent, MessageTypeDefOf.CautionInput, true);
				}
			}
		}

		// Token: 0x060098CF RID: 39119 RVA: 0x00375B5E File Offset: 0x00373D5E
		public void Notify_HeldOnPlatform()
		{
			this.suppressionEnabled = true;
		}

		// Token: 0x060098D0 RID: 39120 RVA: 0x00375B68 File Offset: 0x00373D68
		public override void PostExposeData()
		{
			Scribe_Values.Look<float>(ref this.activityLevelPercent, "activityPercent", 0f, false);
			Scribe_Values.Look<ActivityState>(ref this.state, "state", ActivityState.Passive, false);
			Scribe_Values.Look<float>(ref this.suppressIfAbove, "suppressIfAbove", 0f, false);
			Scribe_Values.Look<bool>(ref this.suppressionEnabled, "suppressionEnabled", false, false);
			Scribe_Values.Look<bool>(ref this.triggeredNaturally, "triggeredNaturally", false, false);
			Scribe_Values.Look<float>(ref this.lastGraphicDirtyPercent, "lastGraphicDirtyPercent", 0f, false);
			Scribe_Values.Look<bool>(ref this.deactivated, "deactivated", false, false);
		}

		// Token: 0x04005B4F RID: 23375
		private float activityLevelPercent;

		// Token: 0x04005B50 RID: 23376
		private ActivityState state;

		// Token: 0x04005B51 RID: 23377
		private bool triggeredNaturally;

		// Token: 0x04005B52 RID: 23378
		private bool deactivated;

		// Token: 0x04005B53 RID: 23379
		public bool suppressionEnabled;

		// Token: 0x04005B54 RID: 23380
		public float suppressIfAbove = 0.4f;

		// Token: 0x04005B55 RID: 23381
		private float lastGraphicDirtyPercent;

		// Token: 0x04005B56 RID: 23382
		private Gizmo gizmo;
	}
}
